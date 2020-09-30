using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using LG.AbpDtoGenerator;
using LG.AbpDtoGenerator.CodeAnalysis;
using LG.AbpDtoGenerator.Models;
using EnvDTE;
using EnvDTE80;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.Shell;
using Newtonsoft.Json;
using Task = System.Threading.Tasks.Task;
using Microsoft.VisualStudio.Shell.Interop;
using System.Globalization;
using DtoGenerator.Vsix;
using Document = Microsoft.CodeAnalysis.Document;
using Process = System.Diagnostics.Process;
using Project = Microsoft.CodeAnalysis.Project;

namespace LGVSIX
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class AbpGeneratorCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("da72badb-d4e5-42b8-99f7-d12df3bb7cd8");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbpGeneratorCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private AbpGeneratorCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static AbpGeneratorCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in AbpGeneratorCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new AbpGeneratorCommand(package, commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            try
            {
                Workspace workspace = ((IComponentModel)Package.GetGlobalService(typeof(SComponentModel))).GetService<Workspace>();

                ProjectItem selectedItem = this.GetSelectedSolutionExplorerItem();
                if (selectedItem != null && selectedItem.Name != null && !selectedItem.Name.EndsWith(".cs"))
                {
                    "52ABP代码生成器(官网：52abp.com),仅支持C#文件！".ErrorMsg();
                }
                else
                {
                    Document currentSelectedDocument;
                    if (((selectedItem != null) ? selectedItem.Document : null) != null)
                    {
                        currentSelectedDocument = workspace.CurrentSolution.GetDocumentByFilePath(selectedItem.Document.FullName);
                    }
                    else
                    {
                        if (selectedItem == null)
                        {
                            "致命异常-无法获得当前选择的解决方案：请重新打开这个解决方案，然后重试本工具。(官网：52abp.com)".ErrorMsg();
                            return;
                        }
                        string file = selectedItem.Name;
                        string projectName = selectedItem.ContainingProject.Name;
                        List<Project> allProjects = workspace.CurrentSolution.Projects.ToList<Project>();
                        List<Project> tmpProjects = (from o in allProjects
                                                     where o.Name.StartsWith(projectName + "(net")
                                                     select o).ToList<Project>();
                        List<Document> docs;
                        if (allProjects.Exists((Project p) => p.Name.Contains(".Shared")))
                        {
                            docs = (from d in (from p in allProjects
                                               where p.Name.Contains(projectName) && !p.Name.Contains(".Shared")
                                               select p).FirstOrDefault<Project>().Documents
                                    where d.Name == file
                                    select d).ToList<Document>();
                        }
                        else if (tmpProjects.Count > 1)
                        {
                            docs = (from d in tmpProjects.FirstOrDefault<Project>().Documents
                                    where d.Name == file
                                    select d).ToList<Document>();
                        }
                        else
                        {
                            docs = (from d in (from p in allProjects
                                               where p.Name == projectName
                                               select p).SelectMany((Project p) => p.Documents)
                                    where d.Name == file
                                    select d).ToList<Document>();
                        }
                        if (docs.Count == 0)
                        {
                            "致命异常-无法获得当前选择的解决方案。请尝试重新打开解决方案，然后再使用本工具(官网：52abp.com)".ErrorMsg();
                            return;
                        }
                        if (docs.Count > 1)
                        {
                            "当前解决方案中，有多个同名的类文件，请处理之后再使用本工具，比如删除一个^_^(官网：52abp.com)。".ErrorMsg();
                            return;
                        }
                        currentSelectedDocument = docs.FirstOrDefault<Document>();
                    }
                    SolutionInfoModel solutionInfo = SolutionInfoCreator.Create(currentSelectedDocument);
                    string path = Path.Combine(solutionInfo.SolutionPath, "solutionInfo.json");
                    string solutionInfoJson = JsonConvert.SerializeObject(solutionInfo);
                    path.CreateFile(solutionInfoJson);
                    ProcessStartInfo processStartInfo = new ProcessStartInfo();
                    string exePath = Path.Combine(Path.GetDirectoryName(typeof(CommonConsts).Assembly.Location), "YoyoAbpCodePowerProject.WPF.exe");
                    processStartInfo.FileName = exePath;
                    processStartInfo.Arguments = " \"" + solutionInfo.SolutionPath + "\"";
                    Process.Start(processStartInfo);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    "发生了一个异常。请复制 c/p 堆栈信息，然后打开项目网站(https://github.com/52ABP/52ABP.CodeGenerator),补充问题和增加简短描述。".ErrorMsg();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<html><head></head><body><a hrf='https://github.com/52ABP/52ABP.CodeGenerator/issues/new'>baidu</a><br>");
                    sb.Append(ex.ToString());
                    sb.Append("</body></html>");
                    string tmpFile = Path.GetTempFileName();
                    File.WriteAllText(tmpFile, sb.ToString());
                    VsShellUtilities.OpenBrowser("file:///" + tmpFile);
                }
                catch (Exception)
                {
                    "发生了一个异常。 无法将堆栈跟踪写入临时目录.".ErrorMsg();
                }
            }
        }

        private ProjectItem GetSelectedSolutionExplorerItem()
        {
            DTE2 ide = ServiceProvider.GetServiceAsync(typeof(DTE)).Result as DTE2;

            object[] items = ide.ToolWindows.SolutionExplorer.SelectedItems as object[];
            if (items.Length != 1)
            {
                return null;
            }
            UIHierarchyItem hierarchyItem = items[0] as UIHierarchyItem;
            if (hierarchyItem == null)
            {
                return null;
            }
            ProjectItem projectItem = ide.Solution.FindProjectItem(hierarchyItem.Name);
            if (projectItem == null)
            {
                return null;
            }
            return projectItem;
        }
    }
}
