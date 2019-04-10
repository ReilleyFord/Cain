using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Cain {
    public class CaseDirectory {
        public string RootPath { get; set; }
        public string DirectoryName { get; set; }
        public List<CaseNotes> CaseNotes { get; set; }
        public List<CaseFile> CaseFiles { get; set; }
        public List<CaseDirectory> CaseDirectories { get; set; }
        public List<BitmapImage> Images { get; set; }

        public CaseDirectory() {
            CaseFiles = new List<CaseFile>();
            CaseDirectories = new List<CaseDirectory>();
            Images = new List<BitmapImage>();
        }

        public CaseDirectory(string path, List<CaseFile> files, List<CaseDirectory> items, List<BitmapImage> images) {
            this.RootPath = path;
            this.CaseFiles = files;
            this.CaseDirectories = items;
            this.Images = images;
        }
    }
}
