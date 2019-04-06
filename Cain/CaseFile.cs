using System;

namespace Cain {

    public class CaseFile {
        public string RootPath  { get; set; }
        public string FileName  { get; set; }
        public string Extension { get; set; }

        public CaseFile(string path, string name, string ext) {
            this.RootPath = path;
            this.FileName = name;
            this.Extension = ext;
        }
    }
}
