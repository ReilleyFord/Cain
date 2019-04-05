using System;
using System.Collections.Generic;

namespace Cain {
    public class Case {
        public string         RootPath  { get; set; }
        public List<ENTable>  CaseNotes { get; set; }
        public CaseItem CaseItem { get; set; }

        public Case() {
            CaseNotes = new List<ENTable>();
        }

        public Case(string path, List<ENTable> caseNotes) {
            this.RootPath  = path;
            this.CaseNotes = caseNotes;
        }
    }
}
