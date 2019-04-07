using System;
using System.Collections.Generic;
using System.Drawing;

namespace Cain {
    public class CaseDirectory {
        public string              RootPath        { get; set; }
        public string              DirectoryName   { get; set; }
        public List<CaseFile>      CaseFiles       { get; set; }
        public List<CaseDirectory> CaseDirectories { get; set; }
        public List<Image>         Images          { get; set; }

        public CaseDirectory() {
            CaseFiles       = new List<CaseFile>();
            CaseDirectories = new List<CaseDirectory>();
            Images          = new List<Image>();
        }

        public CaseDirectory(string path, List<CaseFile> files, List<CaseDirectory> items, List<Image> images) {
            this.RootPath        = path;
            this.CaseFiles       = files;
            this.CaseDirectories = items;
            this.Images          = images;
        }
    }
}
