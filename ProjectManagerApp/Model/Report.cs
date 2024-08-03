namespace ProjectManagerApp.Model
{
    internal class Report
    {
        public int ReportId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime DateCreate { get; set; } = DateTime.Now;

        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        public Report(string title, string content, int projectId)
        {
            Title = title;
            Content = content;
            DateCreate = DateTime.Now;
            ProjectId = projectId;
        }
    }
}
