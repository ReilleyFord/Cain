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
                //IEnumerable<HeaderPart> headerParts = main.HeaderParts;
                //HeaderPart header = headerParts;

                Table table = main.Document.Body.Elements<Table>().First();
                string date = "";
                int num, time, content;

                // Definite work in progress, REQUIRES REWORK.
                foreach (TableRow row in table.Elements<TableRow>()) { 
                    if(row.Count() == 4) {
                        num = 1;
                        time = 2;
                        content = 3;
                    } else {
                        num = 0;
                        time = 1;
                        content = 2;
                    }
                    if(row.ElementAt(2).InnerText != "") {  
                        ENTableRow newRow = new ENTableRow();
                        Regex rgx = new Regex(@"(?m)\d{3}$");

                        string entryNum = row.ElementAt(num).InnerText;
                        newRow.EntryNumber = rgx.Match(entryNum).ToString();

                        rgx = new Regex(@"^\d{2}\w{3}\d{4}");
                        date = rgx.Match(entryNum).ToString();

                        rgx = new Regex(@"^\d{4}");
                        string timeStamp = row.ElementAt(time).InnerText;
                        if (rgx.IsMatch(timeStamp)) {
                            string dateTime = date + " " + timeStamp.Insert(2, ":");
                            newRow.EntryDateTime = Convert.ToDateTime(dateTime.Trim());
                        } else
                            newRow.EntryDateTime = null;

                        newRow.EntryContent = row.ElementAt(content).InnerText;
                        enTable.Rows.Add(newRow);
                    }
                }
            }
            return enTable;
        }

        
    }
}