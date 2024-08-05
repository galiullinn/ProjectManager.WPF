using ProjectManagerApp.Infrastructure.Commands;
using ProjectManagerApp.Model;
using ProjectManagerApp.Repositories;
using ProjectManagerApp.View;
using ProjectManagerApp.Data;
using ProjectManagerApp.ViewModel.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;

namespace ProjectManagerApp.ViewModel
{
    internal class AddTaskProjectViewModel : ViewModelBase
    {
        private string _title = "Добавить новую задачу";
        private string _taskName;
        private string _taskDescription;
        private DateTime _startDate;
        private DateTime _endDate;
        private User _assignedUser;
        private Project _project;

        private readonly TaskProjectRepository _taskProjectRepository;
        private ObservableCollection<TaskProject> _taskProjects;

        private readonly UserRepository _userRepository;
        private ObservableCollection<User> _users;

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

        public ICommand AddTaskProjectCommand { get; }
        public event EventHandler TaskProjectAdded;

        public AddTaskProjectViewModel(Project project)
        {
            Project = project;
            _taskProjectRepository = new TaskProjectRepository(new ApplicationContext());
            _userRepository = new UserRepository(new ApplicationContext());

            AddTaskProjectCommand = new RelayCommand(async _ => await AddTaskProject());

            Task.Run(async () => await LoadUser());
            Task.Run(async () => await LoadTaskPoject());
        }
        public AddTaskProjectViewModel() { }

        private async Task LoadUser()
        {
            var users = await _userRepository.GetAll();
            Users = new ObservableCollection<User>(users);
        }

        private async Task LoadTaskPoject()
        {
            var tasks = await _taskProjectRepository.GetByFilterProject(Project.Id);
            TaskProjects = new ObservableCollection<TaskProject>(tasks);
        }

        private async Task AddTaskProject()
        {
            if (!string.IsNullOrWhiteSpace(TaskName) &&
                !string.IsNullOrWhiteSpace(TaskDescription) &&
                StartDate < EndDate &&
                Project.StartDate < StartDate &&
                Project.EndDate > EndDate)
            {
                var task = new TaskProject()
                {
                    Name = TaskName,
                    Description = TaskDescription,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    Status = StatusTask.InProgress,
                    ProjectId = Project.Id,
                    AssignedUserId = AssignedUser.Id
                    
                };

                await _taskProjectRepository.Add(task);
                await LoadTaskPoject();
                TaskProjectAdded?.Invoke(this, EventArgs.Empty);

                Close();

                MessageBox.Show("Задача добавлена.",
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
            Application.Current.Windows.OfType<AddTaskProjectWindow>().FirstOrDefault()?.Close();
        }
    }
}
