using System;
using System.Collections.Generic;
using System.Text;

namespace LG.AbpTools.Excel2Entity
{
    public class TempTable
    {
        public string TableName { get; set; }

        public string TableDesc { get; set; }

        public List<TempColumn> Columns { get; set; } = new List<TempColumn>();

        public string PkName { get; set; } = string.Empty;

        public string ToReturn()
        {
            string s0 = TableName;
            StringBuilder s1 = new StringBuilder();
            string s2 = "";
            StringBuilder s3 = new StringBuilder();
            StringBuilder s4 = new StringBuilder();

            foreach (TempColumn c in Columns)
            {
                if (c.IsPk)
                {
                    s2 = c.ColumnName;
                    PkName = s2;
                }

                s1.Append(String.Format(TempColumn, c.ColumnName, c.ColumnType, c.AllowNull ? " NULL" : " NOT NULL"));

                s3.Append(String.Format(TempRemark, c.ColumnDisplayName, TableName, c.ColumnName));

                if (c.IsDefault)
                    s4.Append(String.Format(TempDefault, TableName, c.ColumnName, c.DefaultValue));
            }

            StringBuilder sb = new StringBuilder();
            string ss1 = s1.ToString();
            string ss3 = s3.ToString();
            string ss4 = s4.ToString();

            return string.Format(TempAll, s0, ss1, s2, ss3, ss4);
        }

        private const string TempDefault = @"
        ALTER TABLE [dbo].[{0}] ADD  CONSTRAINT [DF_{0}_{1}]  DEFAULT ({2}) FOR [{1}]
        GO 
        ";

        private const string TempRemark = @"EXEC sys.sp_addextendedproperty 
        @name=N'MS_Description', 
        @value=N'{0}' , 
        @level0type=N'SCHEMA',
        @level0name=N'dbo',
        @level1type=N'TABLE',
        @level1name=N'{1}',
        @level2type=N'COLUMN',@level2name=N'{2}'
        GO 
        ";

        private const string TempColumn = @"[{0}] {1} {2},";

        private const string TempAll = @"

        SET ANSI_NULLS ON
        GO

        SET QUOTED_IDENTIFIER ON
        GO

        SET ANSI_PADDING ON
        GO

        CREATE TABLE [dbo].[{0}](
	        {1}
         CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED 
        (
	        {2} ASC
        )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
        ) ON [PRIMARY]

        GO

        SET ANSI_PADDING OFF
        GO 
        {3}

        {4}
        ";
    }
}
