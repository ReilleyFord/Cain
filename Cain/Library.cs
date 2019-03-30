using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;


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
                foreach(TableRow row in table.Elements<TableRow>()) {
                    ENTableRow newRow = new ENTableRow();
                    string entryNum = row.ElementAt(0).InnerText;
                    if (entryNum.Trim().Length > 3) {
                        date = entryNum.Substring(0, 9);
                        newRow.entryNumber = entryNum.Substring(entryNum.Length - 3);
                    } else
                        newRow.entryNumber = entryNum;

                    string timeStamp = row.ElementAt(1).InnerText;
                    if (timeStamp != "") {
                        string dateTime = date + " " + timeStamp.Insert(2, ":");
                        newRow.entryDateTime = Convert.ToDateTime(dateTime.Trim());
                    } else
                        newRow.entryDateTime = null;

                    newRow.entryContent = row.ElementAt(2).InnerText;

                    enTable.Rows.Add(newRow);
                }
            }

            return enTable;
        }

        
    }
}