using System;
using System.Data;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace Cain {

    /**
     * Custom ENTable class built from docx conversion. ENTable is just a list of ENTableRows
     **/
    public class ENTable {
        public List<ENTableRow> Rows { get; set; }

        public ENTable() {
            this.Rows = new List<ENTableRow>();
        }

        /**
         * Converting the ENTable to a DataTable. Currently unused and not need but left in
         * for possible future use.
         **/
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

        /**
         * Parses this.Rows for the first start time of the ENTable
         * Returns value as a DateTime.
         **/ 
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

        /**
         * Parses this.Rows for the last EntryDateTime
         * Returns value as a DateTime
         **/
        public DateTime GetEndTime() {
            DateTime end = (DateTime)this.Rows.Last().EntryDateTime;
            return end;
        }

        /**
         * 
         **/
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

        public List<FilteredRow> FilterByProperty(List<string> propertyNums) {
            List<FilteredRow> filteredRows = new List<FilteredRow>();

            string pattern = @"^P\d{8}";
            Regex rgx = new Regex(pattern);
            foreach(string pNum in propertyNums) {
                FilteredRow filteredRow = new FilteredRow();
                List<ENTableRow> newRow = new List<ENTableRow>();
                filteredRow.Rows = newRow;
                foreach (ENTableRow row in this.Rows) {
                    string match = rgx.Match(row.EntryContent.Substring(0, 9)).ToString();
                    if (match == pNum) {
                        filteredRow.PropertyNumber = match;
                        filteredRow.Rows.Add(row);
                    }
                }
                filteredRows.Add(filteredRow);
            }
            return filteredRows;
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

    public class FilteredRow
    {
        public string PropertyNumber { get; set; }
        public List<ENTableRow> Rows { get; set; }
    }
}