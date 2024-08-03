using ProjectManagerApp.Model.Base;

namespace ProjectManagerApp.Model
{
    internal class Comment : Entity
    {
        public string Text { get; set; } = string.Empty;
        public DateTime DateCreate { get; set; } = DateTime.Now;

        public int TaskProjectId { get; set; }
        public TaskProject TaskProject { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }


        public override string ToString() => $"Комментарий {Id} для задачи {TaskProject.Name}";
    }
}
