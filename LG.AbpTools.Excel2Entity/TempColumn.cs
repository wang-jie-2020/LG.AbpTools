using System;

namespace LG.AbpTools.Excel2Entity
{
    public class TempColumn
    {
        public int OrderNumber { get; set; }

        public string ColumnDisplayName { get; set; }

        public bool IsPk { get; set; }

        public string ColumnName { get; set; }

        public string ColumnType { get; set; }

        public bool AllowNull { get; set; }

        public bool IsDefault { get; set; }

        public string DefaultValue { get; set; }

        public string ColumnRemark { get; set; }

        public string GetColumnType(out int? len)
        {
            string ret = string.Empty;
            len = null;

            //if (Isnull)
            //{
            //    return "?"
            //}

            if (ColumnType.IndexOf("bit", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                ret = "bool";
            }
            else if (ColumnType.IndexOf("tinyint", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                ret = "byte";
            }
            else if (ColumnType.IndexOf("smallint", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                ret = "short";
            }
            else if (ColumnType.IndexOf("bigint", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                ret = "long";
            }
            else if (ColumnType.IndexOf("int", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                ret = "int";
            }
            else if (ColumnType.IndexOf("real", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                ret = "float";
            }
            else if (ColumnType.IndexOf("float", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                ret = "double";
            }
            else if (ColumnType.IndexOf("money", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                ret = "decimal";
            }
            else if (ColumnType.IndexOf("decimal", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                ret = "decimal";
            }
            else if (ColumnType.IndexOf("datetime", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                ret = "DateTime";
            }
            else if (ColumnType.IndexOf("varchar", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                ret = "string";
                len = GetLen(ColumnType, "varchar");
            }
            else if (ColumnType.IndexOf("nchar", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                ret = "string";
                len = GetLen(ColumnType, "nchar");
            }
            else if (ColumnType.IndexOf("nvarchar", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                ret = "string";
                len = GetLen(ColumnType, "nvarchar");
            }
            else if (ColumnType.IndexOf("char", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                ret = "string";
                len = GetLen(ColumnType, "char");
            }
            else if (ColumnType.IndexOf("ntext", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                ret = "string";
            }
            else if (ColumnType.IndexOf("text", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                ret = "string";
            }
            else if (ColumnType.IndexOf("image", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                ret = "byte[]";
            }
            else if (ColumnType.IndexOf("binary", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                ret = "byte[]";
            }
            else if (ColumnType.IndexOf("uniqueidentifier", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                ret = "Guid";
            }

            if (AllowNull && ret.Length > 0 && ret != "string" && ret != "byte[]" && ret != "Guid")
            {
                ret += "?";
            }

            //现存情况是以int存储bool类型，按0=否1=是的逻辑存储，但是按正常思路这按逻辑类型明显更好
            if (Global.IsAutoIntToBool && ret == "int" && this.ColumnName.StartsWith("Is", StringComparison.CurrentCultureIgnoreCase))
            {
                ret = "bool";
            }

            return ret;
        }

        private int GetLen(string source, string replay)
        {
            string strlen = source.Replace(replay, "").Replace("(", "").Replace(")", "").Trim();
            int result = 4000;
            if (!int.TryParse(strlen, out result))
            {
                result = 4000;
            }
            return result;
        }

        public void GetDecimalPrecision(out int? precision, out int? scale)
        {
            precision = null;
            scale = null;

            var tmp = this.ColumnType.Replace("decimal", "").Replace("(", "").Replace(")", "").Trim();
            if (string.IsNullOrEmpty(tmp))
            {
                precision = null;
                scale = null;
            }

            int index = tmp.IndexOf(",");
            if (index == -1)
                return;

            if (int.TryParse(tmp.Substring(0, index), out int result1))
            {
                precision = result1;
            }

            if (int.TryParse(tmp.Substring(index + 1), out int result2))
            {
                scale = result2;
            }
        }
    }
}
