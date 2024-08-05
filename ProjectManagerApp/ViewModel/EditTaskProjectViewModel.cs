using ProjectManagerApp.Infrastructure.Commands;
using ProjectManagerApp.Model;
using ProjectManagerApp.Repositories;
using ProjectManagerApp.View;
using ProjectManagerApp.Data;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using ProjectManagerApp.ViewModel.Base;

namespace ProjectManagerApp.ViewModel
{
    internal class EditTaskProjectViewModel : ViewModelBase
    {
        private string _title = "Изменить задачу";
        private string _taskName;
        private string _taskDescription;
        private DateTime _startDate;
        private DateTime _endDate;
        private User _assignedUser;
        private Project _project;
        private StatusTask _statusTask;

        private readonly TaskProjectRepository _taskProjectRepository;
        private ObservableCollection<TaskProject> _taskProjects;

        private readonly UserRepository _userRepository;
        private ObservableCollection<User> _users;

        private readonly ProjectRepository _projectRepository;
        private ObservableCollection<Project> _projects;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        public string TaskName
        {
            get => _taskName;
            set => SetProperty(ref _taskName, value);
        }
        public string TaskDescription
        {
            get => _taskDescription;
            set => SetProperty(ref _taskDescription, value);
        }
        public DateTime StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }
        public DateTime EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }
        public User AssignedUser
        {
            get => _assignedUser;
            set => SetProperty(ref _assignedUser, value);
        }
        public ObservableCollection<TaskProject> TaskProjects
        {
            get => _taskProjects;
            set => SetProperty(ref _taskProjects, value);
        }
        public Project Project
        {
            get => _project;
            set => SetProperty(ref _project, value);
        }
        public ObservableCollection<User> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }
        public StatusTask StatusTask
        {
            get => _statusTask;
            set => SetProperty(ref _statusTask, value);
        }

        public IEnumerable<StatusTask> Statuses => Enum.GetValues(typeof(StatusTask)).Cast<StatusTask>();

        public ICommand SaveTaskProjectCommand { get; }
        public event EventHandler TaskProjectSaved;

        public EditTaskProjectViewModel(TaskProject task, Project project)
        {
            _taskProjectRepository = new TaskProjectRepository(new ApplicationContext());
            _userRepository = new UserRepository(new ApplicationContext());

            Project = project;

            Task.Run(async () => await LoadTaskProject());
            Task.Run(async () => await LoadUser());

            TaskName = task.Name;
            TaskDescription = task.Description;
            StartDate = task.StartDate;
            EndDate = task.EndDate;
            StatusTask = task.Status;
            AssignedUser = task.AssignedUser;

            SaveTaskProjectCommand = new RelayCommand(async _ => await SaveTaskProject(task.Id));
        }
        public EditTaskProjectViewModel() { }

        private async Task LoadTaskProject()
        {
            var taskProjects = await _taskProjectRepository.GetByFilterProject(Project.Id);

            foreach (var taskProject in taskProjects)
            {
                taskProject.AssignedUser = await _userRepository.GetById(taskProject.AssignedUserId);
            }

            TaskProjects = new ObservableCollection<TaskProject>(taskProjects);
        }

        private async Task LoadUser()
        {
            var users = await _userRepository.GetAll();
            Users = new ObservableCollection<User>(users);
        }

        private async Task SaveTaskProject(int taskId)
        {
            if (!string.IsNullOrWhiteSpace(TaskName) &&
                !string.IsNullOrWhiteSpace(TaskDescription) &&
                StartDate < EndDate &&
                Project.StartDate < StartDate &&
                Project.EndDate > EndDate)
            {
                var task = new TaskProject()
                {
                    Id = taskId,
                    Name = TaskName,
                    Description = TaskDescription,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    Status = StatusTask,
                    ProjectId = Project.Id,
                    AssignedUserId = AssignedUser.Id
                };

                await _taskProjectRepository.Update(task);
                TaskProjectSaved?.Invoke(this, EventArgs.Empty);

                Close();

                MessageBox.Show("Задача обнавлена.",
                    "Успешно!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Asterisk);
            }
            else
                MessageBox.Show("Заполните все поля.",
                    "Ошибка!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
        }
        private void Close()
        {
            Application.Current.Windows.OfType<EditTaskProjectWindow>().FirstOrDefault()?.Close();
        }
    }
}
