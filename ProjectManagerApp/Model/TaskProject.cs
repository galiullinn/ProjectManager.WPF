using ProjectManagerApp.Model.Base;

namespace ProjectManagerApp.Model
{
    internal class TaskProject : Entity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now.AddMonths(1);
        public StatusTask Status { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public int AssignedUserId { get; set; }
        public User AssignedUser { get; set; }
        public List<Comment>? Comments { get; set; } = [];

        public override string ToString() => $"Задача {Name} для проекта {Project.Name}";
    }
}
