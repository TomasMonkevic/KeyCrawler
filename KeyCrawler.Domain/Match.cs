namespace KeyCrawler.Domain
{
    public class Match
    {
        public int Id { get; set; }
        public string Keyword { get; set; }
        public int HitCount { get; set; }

        public UriReport UriReport { get; set; }
        public int UriReportId { get; set; }
    }
}
