namespace ProjectManagerApp.Model
{
    internal class TaskProject
    {
        public int TaskProjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now.AddMonths(1);
        public StatusTask Status { get; set; }

        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        public int AssignedUserId { get; set; }
        public User? AssignedUser { get; set; }
        public List<Comment>? Comments { get; set; } = [];

        public TaskProject(string name, string description, DateTime startDate,  DateTime endDate, int projectId, int assignedUserId)
        {
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            Status = StatusTask.InProgress;
            ProjectId = projectId;
            AssignedUserId = assignedUserId;
        }
    }
}
