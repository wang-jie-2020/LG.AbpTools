using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LG.AbpTools.Excel2Entity
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void textExcel_TextChanged(object sender, EventArgs e)
        {
            textSQL.Text = "";
            textNamespace.Text = "";
            textClassEN.Text = "";
            textClassZH.Text = "";
            textModule.Text = "";
        }

        TempTable _table;

        private void button4_Click(object sender, EventArgs e)
        {
            Global.IsRemarkAsAnnotation = chkRemarkAsAnnotation.Checked;
            Global.IsAutoIntToBool = chkAutoIntToBool.Checked;
            Global.IsAutoIntToEnum = chkAutoIntToEnum.Checked;

            try
            {
                string dataInput = textExcel.Text;
                dataInput = dataInput.Replace("	", "|");
                dataInput = dataInput.Replace("\r\n", "$");
                dataInput = dataInput.Replace("：", ":");

                string[] lines = dataInput.Split('$');
                _table = new TempTable();

                //第一行为表名
                string value = lines[0].Trim().Replace("|", "");

                int index = value.IndexOf("数据表名：");
                if (index == -1)
                {
                    index = value.IndexOf("数据表名:");
                }

                if (index == -1)
                {
                    _table.TableName = value;
                    _table.TableDesc = "";
                }
                else
                {
                    _table.TableName = value.Substring(index + 5).Trim();
                    _table.TableDesc = value.Substring(0, index - 1).Trim();
                    if (_table.TableDesc.Substring(_table.TableDesc.Length - 1) == "表") //"xxx表"最后的表字不需要
                    {
                        var last = _table.TableDesc.Substring(_table.TableDesc.Length - 2);
                        if (last.Equals("主表") || last.Equals("从表") || last.Equals("子表"))
                        {

                        }
                        else
                        {
                            _table.TableDesc = _table.TableDesc.Substring(0, _table.TableDesc.Length - 1);
                        }
                    }
                }

                foreach (string line in lines)
                {
                    string[] data = line.Split('|');
                    int orderNumber = 0;
                    if (int.TryParse(data[0], out orderNumber)) //若orderNumber是数字说明是列
                    {
                        TempColumn column = new TempColumn
                        {
                            OrderNumber = orderNumber,
                            ColumnDisplayName = data[1],
                            ColumnName = data[2],
                            ColumnType = data[3],
                            AllowNull = data[4].IndexOf("Y", StringComparison.CurrentCultureIgnoreCase) >= 0,
                            IsPk = data[5].IndexOf("PK", StringComparison.CurrentCultureIgnoreCase) >= 0,   //用不到
                            IsDefault = data[5].IndexOf("默认:", StringComparison.CurrentCultureIgnoreCase) >= 0,  //用不到
                            ColumnRemark = data[5]
                        };

                        if (column.IsDefault)   //用不到
                        {
                            if (data[5].IndexOf("newid()", StringComparison.CurrentCultureIgnoreCase) >= 0)
                                column.DefaultValue = "newid()";
                            else if (data[5].IndexOf("0", StringComparison.CurrentCultureIgnoreCase) >= 0)
                                column.DefaultValue = "0";
                            else if (data[5].IndexOf("getdate()", StringComparison.CurrentCultureIgnoreCase) >= 0)
                                column.DefaultValue = "getdate()";
                            else if (data[5].IndexOf("('')", StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                                     data[5].IndexOf("''", StringComparison.CurrentCultureIgnoreCase) >= 0)
                                column.DefaultValue = "''";
                        }

                        _table.Columns.Add(column);
                    }
                }

                textSQL.Text = _table.ToReturn();
                textNamespace.Text = "LG.Platform.App";
                textClassEN.Text = _table.TableName;
                textClassZH.Text = _table.TableDesc;
                textModule.Text = _table.TableName.IndexOf("_") == -1
                    ? textNamespace.Text
                    : _table.TableName.Substring(0, _table.TableName.IndexOf("_"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                button4_Click(null, null);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textSQL.Text))
            {
                button4_Click(null, null);
            }

            try
            {
                var columnEntity = new StringBuilder();

                foreach (var c in _table.Columns)
                {
                    columnEntity.AppendLine("\t\t/// <summary>");
                    columnEntity.AppendLine($"\t\t/// { c.ColumnDisplayName + (Global.IsRemarkAsAnnotation ? " " + c.ColumnRemark : "")}");
                    columnEntity.AppendLine("\t\t/// </summary>");

                    string typeName = c.GetColumnType(out int? typelen);

                    //columnEntity.AppendLine("\t\t" + $@"[Column(""{c.ColumnName}"")]");

                    if (!string.IsNullOrEmpty(c.ColumnDisplayName))
                    {
                        columnEntity.AppendLine("\t\t" + $@"[DisplayName(""{c.ColumnDisplayName}"")]");
                    }

                    if (!c.AllowNull)
                    {
                        if (typeName == "string" && typeName == "byte[]" && typeName == "Guid")
                        {
                            columnEntity.AppendLine("\t\t" + @"[Required]");
                        }
                    }

                    if (typelen.HasValue)
                    {
                        columnEntity.AppendLine("\t\t" + $@"[StringLength({typelen.Value})]");
                    }

                    if (typeName.Equals("decimal"))
                    {
                        c.GetDecimalPrecision(out int? precision, out int? scale);

                        if (precision == null || scale == null || (precision.Value == 18 && scale.Value == 2))
                        {
                            columnEntity.AppendLine("\t\t" + @"[DecimalPrecision]");
                        }
                        else
                        {
                            columnEntity.AppendLine("\t\t" + $@"[DecimalPrecision({precision}, {scale})]");
                        }
                    }

                    if (c.IsPk)
                    {
                        columnEntity.AppendLine("\t\t" + @"[DatabaseGenerated(DatabaseGeneratedOption.None)]");
                        columnEntity.AppendLine("\t\t" + @"public override long Id { get; set; }");
                    }
                    else
                    {
                        columnEntity.AppendLine("\t\t" + $@"public {typeName} {c.ColumnName} " + "{ get; set; }");
                    }

                    columnEntity.AppendLine();
                }

                var temp = new TempResult()
                {
                    Namespace = textNamespace.Text,
                    Module = textModule.Text,
                    TableName = textClassEN.Text,
                    TableDesc = textClassZH.Text,
                    ColumnEntity = columnEntity.ToString(),
                    HasTenant = _table.Columns.Any(p => p.ColumnName.Equals("TenantId", StringComparison.OrdinalIgnoreCase))
                };

                string currentDir = Directory.GetCurrentDirectory();
                string patch1 = currentDir + "\\Code";

                //生成Entity
                MakeFile(currentDir + "\\Template\\Entity.txt",
                    patch1 + "\\Entity\\" + temp.Module,
                    patch1 + "\\Entity\\" + temp.Module + "\\" + _table.TableName + ".cs",
                    temp);

                System.Diagnostics.Process.Start(patch1 + "\\Entity\\" + temp.Module);

                ////生成DAL
                //MakeFile(patch + "\\Temp\\DAL.txt", patch1 + "\\DAL\\" + NameSpance, patch1 + "\\Kiloway.DAL\\" + NameSpance + "\\" + TableName + "DAL.cs", temp);
                ////生成Controller
                //MakeFile(patch + "\\Temp\\Controller.txt", patch1 + "\\Web\\Areas\\" + NameSpance + "\\Controllers", patch1 + "\\Web\\Areas\\" + NameSpance + "\\Controllers\\" + TableName + "Controller.cs", temp);
                ////生成View 
                //MakeFile(patch + "\\Temp\\Form.txt", patch1 + "\\Web\\Areas\\" + NameSpance + "\\Views\\" + TableName, patch1 + "\\Web\\Areas\\" + NameSpance + "\\Views\\" + TableName + "\\Form.cshtml", temp);
                //MakeFile(patch + "\\Temp\\Index.txt", patch1 + "\\Web\\Areas\\" + NameSpance + "\\Views\\" + TableName, patch1 + "\\Web\\Areas\\" + NameSpance + "\\Views\\" + TableName + "\\Index.cshtml", temp);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MakeFile(string sourceFile, string dirPath, string newFile, TempResult temp)
        {
            string file = FileHelper.ReadFile(sourceFile);

            file = file.Replace("%Namespace%", temp.Namespace);
            file = file.Replace("%TableName%", temp.TableName);
            file = file.Replace("%TableDesc%", temp.TableDesc);
            file = file.Replace("%ColumnEntity%", temp.ColumnEntity.TrimEnd());

            file = file.Replace("%IMustHaveTenant%", temp.HasTenant ? ", IMustHaveTenant" : "");


            FileHelper.WriteFile(newFile, dirPath, file);
        }
    }
}
