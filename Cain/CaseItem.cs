using System;
using System.Collections.Generic;
using System.Drawing;

namespace Cain {
    public class CaseItem {
        public string         RootPath  { get; set; }
        public Type           ItemType  { get; set; }
        public List<CaseItem> CaseItems { get; set; }
        public List<Image>    Images    { get; set; }

        public CaseItem() {
            CaseItems = new List<CaseItem>();
            Images    = new List<Image>();
        }

        public CaseItem(string path, Type type, List<CaseItem> items, List<Image> images) {
            this.RootPath  = path;
            this.ItemType  = type;
            this.CaseItems = items;
            this.Images    = images;
        }

        public enum Type { file, directory }

    }
}
