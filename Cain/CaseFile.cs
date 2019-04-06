using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
