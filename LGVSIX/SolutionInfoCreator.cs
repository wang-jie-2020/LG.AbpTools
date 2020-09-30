using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LG.AbpDtoGenerator;
using LG.AbpDtoGenerator.CodeAnalysis;
using LG.AbpDtoGenerator.Enums;
using LG.AbpDtoGenerator.GeneratorModels;
using LG.AbpDtoGenerator.Models;
using Microsoft.CodeAnalysis;

namespace DtoGenerator.Vsix
{
    public static class SolutionInfoCreator
    {
        public static SolutionInfoModel Create(Document currentSelectedDocument)
        {
            bool isAbpZero = false;
            Solution solution = currentSelectedDocument.Project.Solution;
            currentSelectedDocument.GetPossibleProjects();
            List<Project> allProject = solution.Projects.ToList<Project>();
            string currentProjectName = currentSelectedDocument.Project.Name;
            if (currentProjectName.Contains("("))
            {
                currentProjectName = currentProjectName.Substring(0, currentProjectName.IndexOf("("));
            }
            string projectStartName = currentSelectedDocument.Project.Name.Substring(0, currentProjectName.LastIndexOf("."));
            if (allProject.Exists((Project p) => p.Name.Contains(".Shared")))
            {
                isAbpZero = true;
            }
            Project application_SharedProject = null;
            Project core_SharedProject = null;
            Project applicationProject;
            Project coreProject;
            Project efProject;
            Project mvcProject;
            Project portalProject;
            Project hostProject;
            Project webCoreProject;
            if (isAbpZero)
            {
                applicationProject = allProject.Find((Project item) => item.Name.StartsWith(projectStartName + ".Application") && !item.Name.Contains(".Shared"));
                application_SharedProject = allProject.Find((Project item) => item.Name.StartsWith(projectStartName + ".Application.Shared"));
                coreProject = allProject.Find((Project item) => item.Name.StartsWith(projectStartName + ".Core") && !item.Name.Contains(".Shared"));
                core_SharedProject = allProject.Find((Project item) => item.Name.StartsWith(projectStartName + ".Core.Shared"));
                efProject = allProject.Find((Project item) => item.Name.StartsWith(projectStartName + ".EntityFrameworkCore") && !item.Name.Contains(".Shared"));
                mvcProject = allProject.Find((Project item) => item.Name.StartsWith(projectStartName + ".Web.Mvc"));
                portalProject = allProject.Find((Project item) => item.Name.StartsWith(projectStartName + ".Web.Portal"));
                hostProject = allProject.Find((Project item) => item.Name.StartsWith(projectStartName + ".Web.Host"));
                webCoreProject = allProject.Find((Project item) => item.Name.StartsWith(projectStartName + ".Web.Core"));
            }
            else
            {
                applicationProject = allProject.Find((Project item) => item.Name.StartsWith(projectStartName + ".Application"));
                coreProject = allProject.Find((Project item) => item.Name.StartsWith(projectStartName + ".Core"));
                efProject = allProject.Find((Project item) => item.Name.StartsWith(projectStartName + ".EntityFrameworkCore"));
                mvcProject = allProject.Find((Project item) => item.Name.StartsWith(projectStartName + ".Web.Mvc"));
                portalProject = allProject.Find((Project item) => item.Name.StartsWith(projectStartName + ".Web.Portal"));
                hostProject = allProject.Find((Project item) => item.Name.StartsWith(projectStartName + ".Web.Host"));
                webCoreProject = allProject.Find((Project item) => item.Name.StartsWith(projectStartName + ".Web.Core"));
                if (efProject == null)
                {
                    "生成器只支持.net core/standard版本！不支持.net framework版本！".ErrorMsg();
                    return null;
                }
            }
            Project xUnitTestsProject = allProject.Find((Project item) => item.Name.StartsWith(projectStartName + ".Tests"));
            if (applicationProject == null || coreProject == null || efProject == null)
            {
                "当前项目不是标准的ABP框架结构！请检查框架是否正确！".ErrorMsg();
                return null;
            }
            string solutionNamespace = string.Empty;
            string companyNamespace = string.Empty;
            ProjectPathInfo application = new ProjectPathInfo
            {
                PType = ProjectType.Application,
                BasePath = applicationProject.GetProjectPath(),
                ProjectName = applicationProject.Name
            };
            ProjectPathInfo core = new ProjectPathInfo
            {
                PType = ProjectType.Core,
                BasePath = coreProject.GetProjectPath(),
                ProjectName = coreProject.Name
            };
            ProjectPathInfo ef = new ProjectPathInfo
            {
                PType = ProjectType.Ef,
                BasePath = efProject.GetProjectPath(),
                ProjectName = efProject.Name
            };
            string[] dtoSplitStrings = currentProjectName.Split(new char[]
            {
                '.'
            });
            if (dtoSplitStrings.Length < 3)
            {
                solutionNamespace = dtoSplitStrings[0];
                companyNamespace = string.Empty;
            }
            else
            {
                if (dtoSplitStrings.Length <= 2)
                {
                    throw new Exception("解决方案项目的多层架构不符合52ABP规范，建议名称为Company.Project的形式。");
                }
                solutionNamespace = dtoSplitStrings[1];
                companyNamespace = dtoSplitStrings[0] + ".";
            }
            SolutionInfoModel solutionInfoModel = new SolutionInfoModel();
            solutionInfoModel.IsAbpZero = isAbpZero;
            solutionInfoModel.Application = application;
            solutionInfoModel.Core = core;
            solutionInfoModel.EF = ef;
            solutionInfoModel.SolutionNamespace = solutionNamespace;
            solutionInfoModel.CompanyNamespace = companyNamespace;
            solutionInfoModel.CurrentProjectName = currentProjectName;
            solutionInfoModel.CurrentSelectFilePath = currentSelectedDocument.FilePath;
            solutionInfoModel.SolutionPath = Path.GetDirectoryName(solution.FilePath);
            if (hostProject != null)
            {
                ProjectPathInfo host = new ProjectPathInfo
                {
                    PType = ProjectType.Host,
                    BasePath = hostProject.GetProjectPath(),
                    ProjectName = hostProject.Name
                };
                solutionInfoModel.Host = host;
            }
            if (webCoreProject != null)
            {
                ProjectPathInfo webCore = new ProjectPathInfo
                {
                    PType = ProjectType.WebCore,
                    BasePath = webCoreProject.GetProjectPath(),
                    ProjectName = webCoreProject.Name
                };
                solutionInfoModel.WebCore = webCore;
            }
            if (mvcProject != null)
            {
                ProjectPathInfo mvc = new ProjectPathInfo
                {
                    PType = ProjectType.Mvc,
                    BasePath = mvcProject.GetProjectPath(),
                    ProjectName = mvcProject.Name
                };
                solutionInfoModel.MVC = mvc;
            }
            if (xUnitTestsProject != null)
            {
                ProjectPathInfo xUnitTests = new ProjectPathInfo
                {
                    PType = ProjectType.XUnitTests,
                    BasePath = xUnitTestsProject.GetProjectPath(),
                    ProjectName = xUnitTestsProject.Name
                };
                solutionInfoModel.Tests = xUnitTests;
            }
            if (portalProject != null)
            {
                ProjectPathInfo portal = new ProjectPathInfo
                {
                    PType = ProjectType.Portal,
                    BasePath = portalProject.GetProjectPath(),
                    ProjectName = portalProject.Name
                };
                solutionInfoModel.Portal = portal;
            }
            if (isAbpZero)
            {
                ProjectPathInfo application_Shared = new ProjectPathInfo
                {
                    PType = ProjectType.Application_Shared,
                    BasePath = application_SharedProject.GetProjectPath(),
                    ProjectName = application_SharedProject.Name
                };
                solutionInfoModel.Application_Shared = application_Shared;
                ProjectPathInfo projectPathInfo = new ProjectPathInfo();
                projectPathInfo.PType = ProjectType.Core_Shared;
                projectPathInfo.BasePath = core_SharedProject.GetProjectPath();
                projectPathInfo.ProjectName = core_SharedProject.Name;
                solutionInfoModel.Core_Shared = application_Shared;
            }
            return solutionInfoModel;
        }
    }
}
