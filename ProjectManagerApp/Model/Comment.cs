namespace ProjectManagerApp.Model
{
    internal class Comment
    {
        public int CommentId { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime DateCreate { get; set; } = DateTime.Now;

        public int TaskProjectId { get; set; }
        public TaskProject? TaskProject { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }

        public Comment(string text, int taskProjectId, int userId)
        {
            Text = text;
            DateCreate = DateTime.Now;
            TaskProjectId = taskProjectId;
            UserId = userId;
        }
    }
}
