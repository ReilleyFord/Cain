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
         * FilterByProperty function accepts a property number string
         * Parses this.Rows and builds a FilteredRow object.
         **/
        public FilteredRow FilterByProperty(string propertyNum) {
            FilteredRow filtered = new FilteredRow();
            filtered.Rows = new List<ENTableRow>();
            string pattern = @"^P\d{8}";
            Regex rgx = new Regex(pattern);

            foreach (ENTableRow row in this.Rows) {
                string match = rgx.Match(row.EntryContent.Substring(0, 9)).ToString();
                if (match == propertyNum) {
                    filtered.PropertyNumber = match;
                    filtered.Rows.Add(row);
                }
            }

            return filtered;
        }

        /**
         * FilterByProperty overloaded function that accepts a list of property numbers
         * parses through this.Rows and builds a FilteredRow object for each property
         * number supplied.
         * Returns a List of FilteredRow objects.
         **/
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

        /**
         * GetDistinctPropertyNumber function that will parse this.Rows
         * for each distinct property number
         * Returns a List of property numbers
         **/
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

    /**
     * Class for storing each TableRows Entry Number, Entry
     * Date Time stamp, and Entry Content.
     **/
    public class ENTableRow {
        public string EntryNumber { get; set; }
        public Nullable<DateTime> EntryDateTime { get; set; }
        public string EntryContent { get; set; }
    }

    /**
     * Class for storing a property number and a list of ENTableRows that are
     * associated with that PropertyNumber
     **/
    public class FilteredRow
    {
        public string PropertyNumber { get; set; }
        public List<ENTableRow> Rows { get; set; }
    }
}