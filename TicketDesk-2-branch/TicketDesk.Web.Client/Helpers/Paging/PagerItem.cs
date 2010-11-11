using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcPaging
{
    public class PagerItem
    {
        public PagerItem(string text, string url, bool isSelected, int pageIndex)
        {
            this.Text = text;
            this.Url = url;
            this.IsSelected = isSelected;
            this.PageIndex = pageIndex;
        }

        public string Text { get; set; }
        public string Url { get; set; }
        public bool IsSelected { get; set; }
        public int PageIndex { get; set; }
    }
}
