using System;
using System.Collections.Generic;

namespace Cain {
    public class Case {
        public string          RootPath  { get; set; }
        public List<CaseNotes> CaseNotes { get; set; }
        public CaseDirectory   CaseDirectory  { get; set; }

        public Case() {
            CaseNotes = new List<CaseNotes>();
        }

        public Case(string path, List<CaseNotes> caseNotes, CaseDirectory caseDirectory) {
            this.RootPath      = path;
            this.CaseNotes     = caseNotes;
            this.CaseDirectory = caseDirectory;
        }
    }
}
