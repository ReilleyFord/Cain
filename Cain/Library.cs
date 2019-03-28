using System;
using System.Collections.Generic;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;


namespace Cain
{
    public class CainLibrary {

        public void ConvertDocxToENTable(string @path) {

            using (WordprocessingDocument docx = 
                WordprocessingDocument.Open(path, false)) {

                MainDocumentPart main = docx.MainDocumentPart;
                IEnumerable<HeaderPart> headerParts = main.HeaderParts;

                //HeaderPart header = headerParts;

                //}

            }

            
        }

        
    }
}
