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
    internal class TaskProjectViewModel : ViewModelBase
    {
        private string _title = "Список задач";

        private readonly TaskProjectRepository _taskProjectRepository;
        private ObservableCollection<TaskProject> _taskProjects;
        private TaskProject _selectedTaskProject;
        private Project _project;
        private User _assignedUser;

        private readonly UserRepository _userRepository;
        private ObservableCollection<User> _users;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        public ObservableCollection<TaskProject> TaskProjects
        {
            get => _taskProjects;
            set => SetProperty(ref _taskProjects, value);
        }
        public TaskProject SelectedTaskProject
        {
            get => _selectedTaskProject;
            set => SetProperty(ref _selectedTaskProject, value);
        }
        public Project Project
        {
            get => _project;
            set => SetProperty(ref _project, value);
        }
        public User AssignedUser
        {
            get => _assignedUser; 
            set => SetProperty(ref _assignedUser, value);
        }
        public ObservableCollection<User> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }

        public ICommand OpenAddTaskProjectWindowCommand { get; }
        public ICommand OpenEditTaskProjectWindowCommand { get; }
        public ICommand DeleteTaskProjectCommand { get; }
        public ICommand ShowDescriptionTaskProjectCommand { get; }

        public TaskProjectViewModel(Project project)
        {
            _taskProjectRepository = new TaskProjectRepository(new ApplicationContext());
            _userRepository = new UserRepository(new ApplicationContext());
            Users = new ObservableCollection<User>();
            Project = project;

            DeleteTaskProjectCommand = new RelayCommand(async _ => await DeleteTaskProject());
            ShowDescriptionTaskProjectCommand = new RelayCommand(_ => ShowDescriptionTaskProject());
            OpenAddTaskProjectWindowCommand = new RelayCommand(_ => OpenAddTaskProjectWindow());
            OpenEditTaskProjectWindowCommand = new RelayCommand(_ => OpenEditTaskProjectWindow());

            Task.Run(async () => await LoadTaskProject());
        }

        public TaskProjectViewModel() { }

        private async Task LoadTaskProject()
        {
            var taskProjects = await _taskProjectRepository.GetByFilterProject(Project.Id);

            foreach (var taskProject in taskProjects)
            {
                taskProject.AssignedUser = await _userRepository.GetById(taskProject.AssignedUserId);
            }

            TaskProjects = new ObservableCollection<TaskProject>(taskProjects);
        }

        private async Task DeleteTaskProject()
        {
            if (SelectedTaskProject != null)
            {
                await _taskProjectRepository.Delete(SelectedTaskProject.Id);
                await LoadTaskProject();
                MessageBox.Show("Задача удалена.",
                    "Успешно!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Asterisk);
            }
            else
                MessageBox.Show("Выберите задачу для удаления.",
                    "Ошибка!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
        }

        private void ShowDescriptionTaskProject()
        {
            if (SelectedTaskProject != null)
            {
                var formattedStringDate = $"\nДата начала: {SelectedTaskProject.StartDate.ToString("dd.MM.yyyy")}\n" +
                                      $"Дата завершения: {SelectedTaskProject.EndDate.ToString("dd.MM.yyyy")}";

                MessageBox.Show($"{SelectedTaskProject.Description}\n{formattedStringDate}",
                    $"Задача \"{SelectedTaskProject.Name}\"",
                    MessageBoxButton.OK);
            }
            else
                MessageBox.Show("Выберите задачу.",
                    "Ошибка!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

        }
        private void OpenAddTaskProjectWindow()
        {
            var addTaskProjectWindow = new AddTaskProjectWindow();
            var addTaskProjectViewModel = new AddTaskProjectViewModel(Project);
            addTaskProjectViewModel.TaskProjectAdded += OnLoadTaskProject;
            addTaskProjectWindow.DataContext = addTaskProjectViewModel;
            addTaskProjectWindow.ShowDialog();
        }

        private void OpenEditTaskProjectWindow()
        {
            if (SelectedTaskProject != null)
            {
                var editTaskProjectWindow = new EditTaskProjectWindow();
                var editTaskProjectViewModel = new EditTaskProjectViewModel(SelectedTaskProject, Project);
                editTaskProjectViewModel.TaskProjectSaved += OnLoadTaskProject;
                editTaskProjectWindow.DataContext = editTaskProjectViewModel;
                editTaskProjectWindow.ShowDialog();
            }
            else
                MessageBox.Show("Выберите задачу.",
                    "Ошибка!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
        }
        private void OnLoadTaskProject(object sender, EventArgs e)
        {
            Task.Run(async () => await LoadTaskProject());
        }
    }
}
