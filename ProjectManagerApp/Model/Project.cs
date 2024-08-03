namespace ProjectManagerApp.Model
{
    internal class Project
    {
        public int ProjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now.AddMonths(1);
        public StatusProject Status { get; set; }

        public List<TaskProject>? Tasks { get; set; } = [];
        public List<Report> Reports { get; set; } = [];
        public List<Comment> Comments { get; set; } = [];

        public Project(string name, string description, DateTime startDate, DateTime endDate)
        {
            Name = name; 
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            Status = StatusProject.InProgress;
        }
    }
}
