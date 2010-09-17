namespace MvcPaging
{
    public class PagerOptions
    {
        public PagerOptions()
        {
            this.IndexParameterName = "id";
            this.MaximumPageNumbers = 5;
            this.PageNumberFormatString = "{0}";
            this.SelectedPageNumberFormatString = "{0}";
            this.ShowPrevious = true;
            this.PreviousText = "<";
            this.ShowNext = true;
            this.NextText = ">";
            this.ShowNumbers = true;
        }

        public string IndexParameterName { get; set; }
        public string PageNumberFormatString { get; set; }
        public string SelectedPageNumberFormatString { get; set; }
        public object LinkAttributes { get; set; }
        public int MaximumPageNumbers { get; set; }
        public bool ShowPrevious { get; set; }
        public string PreviousText { get; set; }
        public bool ShowNext { get; set; }
        public string NextText { get; set; }
        public bool ShowNumbers { get; set; }

    }
}