using System;
using System.Collections.Generic;

namespace Cain {
    public class Case {
        public string          CaseNumber         { get; set; }
        public string          RootPath           { get; set; }
        public List<CaseNotes> CaseNotes          { get; set; }
        public CaseDirectory   RootCaseDirectory  { get; set; }

        public Case() {
            CaseNotes = new List<CaseNotes>();
        }

        public Case(string caseNumber, string path, List<CaseNotes> caseNotes, CaseDirectory caseDirectory) {
            this.CaseNumber        = caseNumber;
            this.RootPath          = path;
            this.CaseNotes         = caseNotes;
            this.RootCaseDirectory = caseDirectory;
        }
    }
}
