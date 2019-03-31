using System;
using System.Data;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace Cain {
    public class ENTable {
        public List<ENTableRow> Rows { get; set; }

        public ENTable() {
            Rows = new List<ENTableRow>();
        }

        public DataTable ConvertToDataTable() {
            DataTable dt = new DataTable();
            dt.Columns.Add("Entry Number", typeof(string));
            dt.Columns.Add("Entry Date Time", typeof(string));
            dt.Columns.Add("Entry Info", typeof(string));

            foreach (ENTableRow row in this.Rows) {
                DataRow dr = dt.NewRow();
                dr[0] = row.EntryNumber;
                if (row.EntryDateTime.HasValue) {
                    dr[1] = row.EntryDateTime.Value.ToString("dd MMM yyyy HH:mm");
                }
                dr[2] = row.EntryContent;
                dt.Rows.Add(dr);
            }

            return dt;
        }

        public DateTime GetStartTime() {
            DateTime start = new DateTime();
            foreach(ENTableRow first in this.Rows) {
                if (first.EntryDateTime != null) {
                    start = (DateTime)first.EntryDateTime;
                    break;
                }
            }
            return start;
        }

        public DateTime GetEndTime() {
            DateTime end = (DateTime)this.Rows.Last().EntryDateTime;
            return end;
        }

        public List<ENTableRow> FilterByProperty(string propertyNum) {
            List<ENTableRow> rows = new List<ENTableRow>();
            string pattern = @"^P\d{8}";
            Regex rgx = new Regex(pattern);

            foreach (ENTableRow row in this.Rows) {
                if (rgx.Match(row.EntryContent.Substring(0, 9)).ToString() == propertyNum) 
                    rows.Add(row);
            }

            return rows;
        }

        public List<string> GetDistinctPropertyNumber() {
            List<string> propertyNums = new List<string>();
            string pattern = @"^P\d{8}";
            Regex rgx = new Regex(pattern);
            foreach (ENTableRow row in this.Rows) {
                if (rgx.IsMatch(row.EntryContent))
                    propertyNums.Add(row.EntryContent.Substring(0, 9));
            }

            propertyNums = propertyNums.Distinct().OrderBy(p => p).ToList();
            return propertyNums;
        }

    }   

    public class ENTableRow {
        public string EntryNumber { get; set; }
        public Nullable<DateTime> EntryDateTime { get; set; }
        public string EntryContent { get; set; }
    }
}