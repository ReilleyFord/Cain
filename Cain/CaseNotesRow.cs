using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cain {
    /**
     * Class for storing each TableRows Entry Number, Entry
     * Date Time stamp, and Entry Content.
     **/
    public class CaseNotesRow {
        public string             EntryNumber   { get; set; }
        public Nullable<DateTime> EntryDateTime { get; set; }
        public string             EntryContent  { get; set; }
    }
}
