using System;
using System.Data;
using System.Collections.Generic;

namespace Cain
{
    public class ENTable
    {
        public List<ENTableRow> Rows { get; set; }

        public ENTable()
        {
            Rows = new List<ENTableRow>();
        }

        public DataTable ConvertToDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Entry Number", typeof(string));
            dt.Columns.Add("Entry Date Time", typeof(string));
            dt.Columns.Add("Entry Info", typeof(string));

            foreach (var row in this.Rows)
            {
                DataRow dr = dt.NewRow();
                dr[0] = row.entryNumber;
                if (row.entryDateTime.HasValue)
                {
                    dr[1] = row.entryDateTime.Value.ToString("dd MMM yyyy HH:mm");
                }
                dr[2] = row.entryContent;
                dt.Rows.Add(dr);
            }

            return dt;
        }

    }   

    public class ENTableRow
    {
        public string entryNumber { get; set; }
        public Nullable<DateTime> entryDateTime { get; set; }
        public string entryContent { get; set; }
    }
}