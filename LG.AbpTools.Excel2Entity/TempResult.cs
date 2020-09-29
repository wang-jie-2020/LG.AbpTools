namespace LG.AbpTools.Excel2Entity
{
    public class TempResult
    {
        public string Namespace { get; set; }

        public string Module { get; set; }

        public string TableName { get; set; }

        public string TableDesc { get; set; }

        public string ColumnEntity { get; set; }

        public string MainKey { get; set; }

        public bool HasTenant { get; set; }
    }
}
