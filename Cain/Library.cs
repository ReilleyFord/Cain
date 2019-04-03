using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text.RegularExpressions;

namespace Cain {
    public static class CainLibrary {


        /**
         * ConvertDocxToENTable function. This will take a docx file path (Generally a case notes file)
         * And open it to parse it/Break it down. First into the main document and then to the first table in the body
         * From there it will parse each row of the table and then grab values from the cells in the TableRow
         * Eventually returns a built ENTable object filled with ENTableRows. 
         **/
        public static ENTable ConvertDocxToENTable(string @path) {
            ENTable enTable = new ENTable();

            using (WordprocessingDocument docx = WordprocessingDocument.Open(path, false)) {
                //Setting up required variables.
                MainDocumentPart main = docx.MainDocumentPart;
                Table table = main.Document.Body.Elements<Table>().First();
                string date = String.Empty;

                //Header parsing not required. However I might refactor later.
                //IEnumerable<HeaderPart> headerParts = main.HeaderParts;
                //HeaderPart header = headerParts;

                // Refactored. Possibly requires more refactoring for efficiency and optimization.
                foreach (TableRow row in table.Elements<TableRow>()) { 
                    //Grabbing the TableCells -- Don't want to parse TableProperties etc.
                    IEnumerable<TableCell> cells = row.OfType<TableCell>();
                    //Only grabbing rows that have content.
                    if(cells.ElementAt(2).InnerText != "") {
                        ENTableRow newRow = new ENTableRow();
                        //Regex value for grabbing the entry number
                        Regex rgx = new Regex(@"(?m)\d{3}$");

                        string entryNum = cells.ElementAt(0).InnerText.Trim();
                        newRow.EntryNumber = rgx.Match(entryNum).ToString();

                        //Regex value for grabbing the date stamp and checking if there is a new date.
                        rgx = new Regex(@"(?m)^\d{2}\w{3}\d{4}");
                        if (rgx.IsMatch(entryNum))
                            date = rgx.Match(entryNum).ToString();

                        //Regex value for grabbing the timestamp.
                        rgx = new Regex(@"^\d{4}");
                        string timeStamp = cells.ElementAt(1).InnerText;
                        if (rgx.IsMatch(timeStamp)) {
                            string dateTime = date + " " + timeStamp.Insert(2, ":");
                            newRow.EntryDateTime = Convert.ToDateTime(dateTime.Trim());
                        } else
                            newRow.EntryDateTime = null;

                        newRow.EntryContent = cells.ElementAt(2).InnerText;
                        enTable.Rows.Add(newRow);
                    }
                }
                enTable.GetProperties();
            }
            return enTable;
        }
    }
}