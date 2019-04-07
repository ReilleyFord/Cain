using System;

namespace Cain {

    public class CaseFile {
        public string    RootPath  { get; set; }
        public string    FileName  { get; set; }
        public string    Extension { get; set; }
        public CaseNotes CaseNotes { get; set; }

        public CaseFile() { }

        public CaseFile(string path, string name, string ext, CaseNotes notes) {
            this.RootPath  = path;
            this.FileName  = name;
            this.Extension = ext;
            this.CaseNotes = notes;
        }
    }
}
