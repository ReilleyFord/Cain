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
        public List<Property> Properties { get; set; }

        public ENTable() {
            this.Rows = new List<ENTableRow>();
            this.Properties = new List<Property>();
        }

        /**
         * GetProperties function is used to built This.Properties for each individual property number
         * and associated ENTableRows in This.Rows.
        **/
        internal void GetProperties() {
            List<Property> properties = new List<Property>();
            List<string> propertyNumbers = GetDistinctPropertyNumbers();

            string pattern = @"^P\d{8}";
            Regex rgx = new Regex(pattern);
            foreach (string pNum in propertyNumbers) {
                Property property = new Property();
                List<ENTableRow> newRow = new List<ENTableRow>();
                property.Rows = newRow;
                foreach (ENTableRow row in this.Rows) {
                    string match = rgx.Match(row.EntryContent.Substring(0, 9)).ToString();
                    if (match == pNum) {
                        property.PropertyNumber = match;
                        property.Rows.Add(row);
                    }
                }
                properties.Add(property);
            }
            this.Properties = properties;
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
            foreach (ENTableRow first in this.Rows) {
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
         * Parses this.Rows and builds a FilteredProperty object.
         **/
        public Property FilterByProperty(string propertyNum) {
            Property filtered = new Property();
            filtered.Rows = new List<ENTableRow>();

            foreach (Property property in this.Properties) {
                if (property.PropertyNumber == propertyNum) {
                    filtered = property;
                }
            }
            return filtered;
        }

        /**
         * GetDistinctPropertyNumber function that will parse this.Rows
         * for each distinct property number
         * Returns a List of property numbers
         **/
        public List<string> GetDistinctPropertyNumbers() {
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
}