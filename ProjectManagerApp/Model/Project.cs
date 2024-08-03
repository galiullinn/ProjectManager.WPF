using ProjectManagerApp.Model.Base;

namespace ProjectManagerApp.Model
{
    internal class Project : Entity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now.AddMonths(1);
        public StatusProject Status { get; set; }

        public List<TaskProject>? Tasks { get; set; } = [];
        public List<Report> Reports { get; set; } = [];
        public List<Comment> Comments { get; set; } = [];

        public override string ToString() => $"Проект {Name}";
    }
}
