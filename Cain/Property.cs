using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cain {
    /**
     * Class for storing a property number and a list of ENTableRows that are
     * associated with that PropertyNumber
    **/
    public class Property {
        public string           PropertyNumber { get; set; }
        public List<ENTableRow> Rows           { get; set; }

        public Property() {
            this.Rows = new List<ENTableRow>();
        }

        /**
         * Parses this.Rows for the first start time of the ENTable
         * Returns value as a DateTime.
        **/
        public DateTime GetStartTime() {
            DateTime start = new DateTime();
            foreach (ENTableRow first in this.Rows) {
                if (first.EntryDateTime == null) 
                    continue;
                start = (DateTime)first.EntryDateTime;
                break;
            }
            return start;
        }

        /**
         * Parses this.Rows for the last EntryDateTime
         * Returns value as a DateTime
        **/
        public DateTime GetEndTime() {
            DateTime end = new DateTime();
            for (int i = this.Rows.Count; i > 0; i--) {
                if (Rows[i].EntryDateTime == null) 
                    continue;
                end = (DateTime)Rows[i].EntryDateTime;
                break;
                
            }
            return end;
        }
    }
}
