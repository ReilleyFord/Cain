using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text.RegularExpressions;

namespace Cain {
    public static class CainLibrary {

        public static ENTable ConvertDocxToENTable(string @path) {
            ENTable enTable = new ENTable();

            using (WordprocessingDocument docx = 
                WordprocessingDocument.Open(path, false)) {

                MainDocumentPart main = docx.MainDocumentPart;

                //Header parsing not required. However I might refactor later.
                //IEnumerable<HeaderPart> headerParts = main.HeaderParts;
                //HeaderPart header = headerParts;

                Table table = main.Document.Body.Elements<Table>().First();
                string date = "";
                // Definite work in progress, REQUIRES REFACTORING.
                foreach (TableRow row in table.Elements<TableRow>()) { 
                    IEnumerable<TableCell> cells = row.OfType<TableCell>();
                    if(cells.ElementAt(2).InnerText != "") {  
                        ENTableRow newRow = new ENTableRow();
                        Regex rgx = new Regex(@"(?m)\d{3}$");

                        string entryNum = cells.ElementAt(0).InnerText;
                        newRow.EntryNumber = rgx.Match(entryNum).ToString();

                        rgx = new Regex(@"^\d{2}\w{3}\d{4}");
                        date = rgx.Match(entryNum).ToString();

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
            }
            return enTable;
        }
    }
}