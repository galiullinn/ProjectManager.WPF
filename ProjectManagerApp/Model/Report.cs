using ProjectManagerApp.Model.Base;

namespace ProjectManagerApp.Model
{
    internal class Report : Entity
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime DateCreate { get; set; } = DateTime.Now;

        public int ProjectId { get; set; }
        public Project Project { get; set; } 


        public override string ToString() => $"Отчет {Title} для проекта {Project.Name}";
    }
}
