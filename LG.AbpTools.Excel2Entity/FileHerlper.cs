using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace LG.AbpTools.Excel2Entity
{
    public static class FileHelper
    {
        /// <summary>
        /// 当前文件路径
        /// </summary>
        private static string FilePath = Application.StartupPath;

        #region File

        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static long FileSize(string filename)
        {
            FileInfo fi = new FileInfo(FilePath + filename);
            return fi.Length;
        }

        /// <summary>
        /// 是否存在文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool HaveFile(string fileName)
        {
            return File.Exists(FilePath + fileName);
        }

        /// <summary>
        /// 获取路径下的文件名
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<string> GetFileNames(string pattern, string filePath)
        {
            List<string> fileNames = new List<string>();
            string[] filePaths = Directory.GetFiles(FilePath + filePath, pattern);
            foreach (string fp in filePaths)
            {
                string name = GetFileName(fp);
                if (name.IndexOf('-') > 0)
                {
                    fileNames.Add(name);
                }
            }
            return fileNames;
        }

        /// <summary>
        /// 获取文件名称
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileName(string path)
        {
            if (path.Contains("\\"))
            {
                string[] arr = path.Split('\\');
                return arr[arr.Length - 1];
            }
            else
            {
                string[] arr = path.Split('/');
                return arr[arr.Length - 1];
            }
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="fileName"></param>
        private static void CreateFile(string fileName)
        {
            FileStream fs = File.Create(FilePath + fileName);
            fs.Close();
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="copyFileName"></param>
        /// <param name="copyedFileName"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public static bool Copy(string copyFileName, string copyedFileName, bool overwrite)
        {
            try
            {
                File.Copy(FilePath + copyFileName, FilePath + copyedFileName, overwrite);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool Delete(string fileName)
        {
            try
            {
                File.Delete(FilePath + fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Stream

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="registerstring"></param>
        public static string ReadFile(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    return "";
                }

                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader sr = new StreamReader(fs, Encoding.Default))
                    {
                        string s = sr.ReadToEnd();
                        if (string.IsNullOrEmpty(s))
                        {
                            return "";
                        }
                        else
                        {
                            return s;
                        }
                    }
                }
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 读取文件的所有行
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> Read(string path)
        {
            return Read(path, false);
        }

        /// <summary>
        /// 读取文件的所有行
        /// </summary>
        /// <param name="path"></param>
        ///  <param name="fillType"></param>
        /// <returns></returns>
        public static List<string> Read(string path, bool fillType)
        {
            List<string> ls = new List<string>();
            if (!fillType)
            {
                path = FilePath + path;
            }

            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.Default))
                {
                    while (!sr.EndOfStream)
                    {
                        ls.Add(sr.ReadLine());
                    }
                }
            }
            return ls;
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="dirPath"></param>
        /// <param name="registerStr"></param>
        /// <returns></returns>
        public static bool WriteFile(string path, string dirPath, string registerStr)
        {
            try
            {
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                using (StreamWriter fs = new StreamWriter(path, false, Encoding.Default))
                {
                    //写入数据 
                    fs.Write(registerStr);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="line"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool WriteLine(string line, string fileName)
        {
            try
            {
                //如果不存在，则创建
                if (!HaveFile(fileName))
                {
                    CreateFile(fileName);
                }

                using (StreamWriter fs = new StreamWriter(FilePath + fileName, true, Encoding.Default))
                {
                    //写入数据 
                    fs.Write(line);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 写入行
        /// </summary>
        /// <param name="dataColumnCollection"></param>
        /// <param name="fileNamePath"></param>
        public static void WriteColumns(DataColumnCollection dataColumnCollection, string fileNamePath)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataColumn dc in dataColumnCollection)
            {
                sb.Append(dc.ColumnName + "￥");
            }
            sb.Append("\r\n");

            //如果不存在，则创建
            if (!HaveFile(fileNamePath))
            {
                CreateFile(fileNamePath);
            }

            using (StreamWriter fs = new StreamWriter(FilePath + fileNamePath, true, Encoding.Default))
            {
                //写入数据 
                fs.Write(sb.ToString());
            }
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="errorLines"></param>
        /// <param name="p"></param>
        public static void Write(List<string> errorLines, string fileName)
        {
            File.WriteAllLines(FilePath + fileName, errorLines.ToArray(), Encoding.Default);
        }

        /// <summary>
        /// 删除一行
        /// </summary>
        /// <param name="line"></param>
        /// <param name="fileName"></param>
        public static void DeleteLine(string line, string fileName)
        {
            List<string> lines = new List<string>(File.ReadAllLines(FilePath + fileName, Encoding.Default));
            lines.Remove(line);//删除第3行
            File.WriteAllLines(FilePath + fileName, lines.ToArray());
        }

        #endregion

        #region log

        /// <summary>
        /// 内部代码执行发生异常的处理.
        /// </summary>
        /// <param name="exp"></param>
        public static void WriteLog(string msg, string pathName)
        {
            try
            {
                // 文件名基准
                // 年月日时分秒
                string baseFileName = DateTime.Now.ToString("yyyyMMdd");

                // 取得当前应用程序的目录.
                string path = System.AppDomain.CurrentDomain.BaseDirectory + pathName;

                // 如果日志目录不存在，创建.
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                // 写日志文件.
                Log(msg, path + "\\" + baseFileName + ".txt");
            }
            catch
            {
            }
        }

        /// <summary>
        /// 向日志文件写入异常信息。
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="fileName"></param>
        public static void Log(string msg, string fileName)
        {
            StreamWriter sw = null;
            try
            {
                // 注意第2个参数：
                // 确定是否将数据追加到文件。如果该文件存在，并且 append 为 false，则该文件被覆盖。
                // 如果该文件存在，并且 append 为 true，则数据被追加到该文件中。否则，将创建新文件。
                // 也就是说，如果第2个参数 是 false， 可以不用写前面的 判断文件存在则删除的代码.

                // 第3个参数为编码方式， 读取和写入，尽可能使用统一的编码
                sw = new StreamWriter(fileName, true, Encoding.UTF8);

                // 写入异常信息.
                sw.WriteLine(msg);

                // 关闭文件.
                sw.Close();

                sw = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("在写入文件的过程中，发生了异常！");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                if (sw != null)
                {
                    try
                    {
                        sw.Close();
                    }
                    catch
                    {
                        // 最后关闭文件，无视 关闭是否会发生错误了.
                    }
                }
            }
        }

        #endregion

        #region DateTable

        /// <summary>
        /// TXT转Table
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static DataTable FileLogToTable(string path)
        {
            DataTable dt = new DataTable();
            DataColumn dc1 = new DataColumn("type");
            DataColumn dc2 = new DataColumn("time");
            DataColumn dc3 = new DataColumn("filename");
            DataColumn dc4 = new DataColumn("remark");
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);
            dt.Columns.Add(dc4);
            List<string> lines = Read(path, true);
            foreach (string line in lines)
            {
                try
                {
                    string[] drData = line.Split(new char[] { '|' }, StringSplitOptions.None);
                    DataRow dr = dt.NewRow();
                    dr[0] = drData[0];
                    dr[1] = drData[1];
                    dr[2] = drData[2];
                    dr[3] = drData[3];
                    dt.Rows.Add(dr);
                }
                catch (Exception ex)
                { }
            }
            return dt;
        }

        /// <summary>
        /// TXT转Table
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static DataTable SQLLogToTable(string path)
        {
            DataTable dt = new DataTable();
            DataColumn dc1 = new DataColumn("time");
            DataColumn dc2 = new DataColumn("fileNamePath");
            DataColumn dc3 = new DataColumn("keys");
            DataColumn dc4 = new DataColumn("remark");
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);
            dt.Columns.Add(dc4);
            List<string> lines = Read(path, true);
            foreach (string line in lines)
            {
                try
                {
                    string[] drData = line.Split(new char[] { '|' }, StringSplitOptions.None);
                    DataRow dr = dt.NewRow();
                    dr[0] = drData[0];
                    dr[1] = drData[1];
                    dr[2] = drData[2];
                    dr[3] = drData[3];
                    dt.Rows.Add(dr);
                }
                catch (Exception ex)
                { }
            }
            return dt;
        }

        public static object FileUpDownLogToTable(string path)
        {
            DataTable dt = new DataTable();
            DataColumn dc1 = new DataColumn("time");
            DataColumn dc2 = new DataColumn("filename");
            DataColumn dc3 = new DataColumn("remark");
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);

            List<string> lines = Read(path, true);
            foreach (string line in lines)
            {
                try
                {
                    string[] drData = line.Split(new char[] { '|' }, StringSplitOptions.None);
                    DataRow dr = dt.NewRow();
                    dr[0] = drData[0];
                    dr[1] = drData[1];
                    dr[2] = drData[2];
                    dt.Rows.Add(dr);
                }
                catch (Exception ex)
                { }
            }
            return dt;
        }

        #endregion
    }
}
