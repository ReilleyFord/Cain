using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text.RegularExpressions;
using System.Drawing;

namespace Cain {
    public static class CainLibrary {

        public static Case ConvertPathToCase(string @directory) {
            if (!Directory.Exists(directory))
                throw new IOException("Error: That directory does not exist");

            Regex rgx = new Regex(@"\w{2}\d{8}");

            Case newCase = new Case();
            List<CaseNotes> caseNotes = new List<CaseNotes>();
            newCase.CaseNumber = rgx.Match(directory).ToString();
            newCase.RootPath = directory;
            newCase.RootCaseDirectory = GetDirectories(directory);
            newCase.CaseNotes = GetCaseNotes(newCase.RootCaseDirectory, rgx, caseNotes);

            return newCase;
        }

        private static List<CaseNotes> GetCaseNotes(CaseDirectory caseDir, Regex rgx, List<CaseNotes> caseNotes) {
            foreach (CaseFile file in caseDir.CaseFiles) {
                if (file.Extension != ".docx")
                    continue;
                if (!rgx.IsMatch(file.FileName))
                    continue;
                CaseNotes notes = ConvertDocxToCaseNotes(file.RootPath);
                caseNotes.Add(notes);
            }

            foreach (CaseDirectory dir in caseDir.CaseDirectories) {
                GetCaseNotes(dir, rgx, caseNotes);
            }
            return caseNotes;
        }

        private static CaseDirectory GetDirectories(string path) {
            CaseDirectory item = new CaseDirectory();
            item.RootPath = path;
            item.DirectoryName = new DirectoryInfo(path).Name;
            foreach(string dir in Directory.GetDirectories(path)) {
                item.CaseDirectories.Add(GetDirectories(dir));
            }
            List<Image> images = new List<Image>();
            foreach (string file in Directory.GetFiles(path)) {
                CaseFile caseFile = new CaseFile();
                string ext = Path.GetExtension(file);
                if (ext.ToLower() == ".jpg" || ext.ToLower() == ".png") {
                    images.Add(Image.FromFile(file));
                }
                if (ext == ".docx") {
                    Regex rgx = new Regex(@"\w{2}\d{8}");
                    if (rgx.IsMatch(Path.GetFileName(file))) 
                        caseFile.CaseNotes = ConvertDocxToCaseNotes(file);
                }
                caseFile.RootPath  = file;
                caseFile.FileName  = Path.GetFileNameWithoutExtension(file);
                caseFile.Extension = ext;
                item.Images = images;
                item.CaseFiles.Add(caseFile);
            }
            return item;
        }

        /**
         * ConvertDocxToENTable function. This will take a docx file path (Generally a case notes file)
         * And open it to parse it/Break it down. First into the main document and then to the first table in the body
         * From there it will parse each row of the table and then grab values from the cells in the TableRow
         * Eventually returns a built ENTable object filled with ENTableRows. 
         **/
        public static CaseNotes ConvertDocxToCaseNotes(string @path) {
            CaseNotes caseNotes = new CaseNotes();

            using (WordprocessingDocument docx = WordprocessingDocument.Open(path, false)) {
                //Setting up required variables.
                MainDocumentPart main = docx.MainDocumentPart;
                Table table = main.Document.Body.Elements<Table>().First();
                string date = String.Empty;

                //Header parsing not required. However I might rework later.
                //IEnumerable<HeaderPart> headerParts = main.HeaderParts;
                //HeaderPart header = headerParts;

                // Refactored. Possibly requires more refactoring for efficiency and optimization.
                foreach (TableRow row in table.Elements<TableRow>()) { 
                    //Grabbing the TableCells -- Don't want to parse TableProperties etc.
                    IEnumerable<TableCell> cells = row.OfType<TableCell>();
                    //Only grabbing rows that have content.
                    if(cells.ElementAt(2).InnerText != "") {
                        CaseNotesRow newRow = new CaseNotesRow();
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
                        caseNotes.Rows.Add(newRow);
                    }
                }
            }
            caseNotes.GetProperties();
            return caseNotes;
        }
    }
}