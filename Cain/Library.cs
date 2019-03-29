using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;


namespace Cain
{
    public class CainLibrary {

        public void ConvertDocxToENTable(string @path) {
            ENTable enTable = new ENTable();

            using (WordprocessingDocument docx = 
                WordprocessingDocument.Open(path, false)) {

                MainDocumentPart main = docx.MainDocumentPart;
                IEnumerable<HeaderPart> headerParts = main.HeaderParts;

                //HeaderPart header = headerParts;

                //}

                Table table = main.Document.Body.Elements<Table>().First();

                string date = "";
                foreach(var row in table) {
                    Console.WriteLine("Row");
                    ENTableRow enTableRow = new ENTableRow();
                    string entryNum = row.FirstChild.InnerText;
                    if (entryNum.Trim().Length > 4)
                        date = entryNum.Substring(0, 9);
                    else
                        enTableRow.entryNumber = entryNum;

                    string time = row.ElementAt(1).InnerText;
                    Console.WriteLine(time);
                    enTableRow.entryDateTime = DateTime.ParseExact(date + time, "DDMMyyyy", null);
                    Console.WriteLine(date+"\n"+entryNum + "\n" + time);
                    enTableRow.entryContent = row.ElementAt(2).InnerText;

                    enTable.Rows.Add(enTableRow);
                }

                Console.WriteLine();

            }

            
        }

        
    }
}
