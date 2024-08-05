using System.Collections.ObjectModel;
using ProjectManagerApp.Model;
using ProjectManagerApp.Repositories;
using ProjectManagerApp.ViewModel.Base;
using ProjectManagerApp.Data;
using System.Windows.Input;
using ProjectManagerApp.View;
using ProjectManagerApp.Infrastructure.Commands;
using System.Windows;

namespace ProjectManagerApp.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private string _title = "Менеджер проектов";

        private readonly ProjectRepository _projectRepository;
        private ObservableCollection<Project> _projects;
        private Project _selectedProject;

        private readonly UserRepository _userRepository;
        private ObservableCollection<User> _users;
        private User _selectedUser;

        private readonly TaskProjectRepository _taskRepository;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        public ObservableCollection<Project> Projects
        {
            get => _projects;
            set => SetProperty(ref _projects, value);
        }
        public Project SelectedProject
        {
            get => _selectedProject;
            set => SetProperty(ref _selectedProject, value);
        }
        public ObservableCollection <User> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }
        public User SelectedUser
        {
            get => _selectedUser;
            set => SetProperty<User>(ref _selectedUser, value);
        }

        public ICommand OpenAddProjectWindowCommand { get; }
        public ICommand OpenEditProjectWindowCommand { get; }
        public ICommand DeleteProjectCommand { get; }
        public ICommand ShowDescriptionProjectCommand { get; }

        public ICommand OpenAddUserWindowCommand { get; }
        public ICommand OpenEditUserWindowCommand { get; }
        public ICommand DeleteUserCommand { get; }  

        public ICommand OpenTaskProjectWindowCommand { get; }

        public MainWindowViewModel()
        {
            _projectRepository = new ProjectRepository(new ApplicationContext());
            _userRepository = new UserRepository(new ApplicationContext());

            DeleteProjectCommand = new RelayCommand(async _ => await DeleteProject());
            ShowDescriptionProjectCommand = new RelayCommand(_ => ShowDescriptionProject());
            OpenAddProjectWindowCommand = new RelayCommand(_ => OpenAddProjectWindow());
            OpenEditProjectWindowCommand = new RelayCommand(_ => OpenEditProjectWindow());

            OpenAddUserWindowCommand = new RelayCommand(_ => OpenAddUserWindow());
            OpenEditUserWindowCommand = new RelayCommand(_ => OpenEditUserWindow());
            DeleteUserCommand = new RelayCommand(async _ => await DeleteUser());

            OpenTaskProjectWindowCommand = new RelayCommand(_ => OpenTaskProjectWindow());

            Task.Run(async () => await LoadPoject());
            Task.Run(async () => await LoadUser());
        }

        private async Task LoadPoject()
        {
            var projects = await _projectRepository.GetAll();
            Projects = new ObservableCollection<Project>(projects);
        }

        private async Task DeleteProject()
        {
            if (SelectedProject != null)
            {
                await _projectRepository.Delete(SelectedProject.Id);
                await LoadPoject();
                MessageBox.Show("Проект удален.",
                    "Успешно!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Asterisk);
            }
            else
                MessageBox.Show("Выберите проект для удаления.",
                    "Ошибка!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
        }

        private void ShowDescriptionProject()
        {
            if (SelectedProject != null)
            {
                var formattedStringDate = $"\nДата начала: {SelectedProject.StartDate.ToString("dd.MM.yyyy")}\n" +
                                      $"Дата завершения: {SelectedProject.EndDate.ToString("dd.MM.yyyy")}";

                MessageBox.Show($"{SelectedProject.Description}\n{formattedStringDate}",
                    $"Проект \"{SelectedProject.Name}\"",
                    MessageBoxButton.OK);
            }
            else
                MessageBox.Show("Выберите проект.",
                    "Ошибка!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
        
        }
        private void OpenAddProjectWindow()
        {
            var addProjectWindow = new AddProjectWindow();
            var addProjectViewModel = new AddProjectViewModel();
            addProjectViewModel.ProjectAdded += OnLoadProject;
            addProjectWindow.DataContext = addProjectViewModel;
            addProjectWindow.ShowDialog();
        }

        private void OpenEditProjectWindow()
        {
            if (SelectedProject != null)
            {
                var editProjectWindow = new EditProjectWindow();
                var editProjectViewModel = new EditProjectViewModel(SelectedProject);
                editProjectViewModel.ProjectSaved += OnLoadProject;
                editProjectWindow.DataContext = editProjectViewModel;
                editProjectWindow.ShowDialog();
            }
            else
                MessageBox.Show("Выберите проект.",
                    "Ошибка!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
        }

        private void OnLoadProject(object sender, EventArgs e)
        {
            Task.Run(async () => await LoadPoject());
        }

        private async Task LoadUser()
        {
            var users = await _userRepository.GetAll();
            Users = new ObservableCollection<User>(users);
        }

        private async Task DeleteUser()
        {
            if (SelectedUser != null)
            {
                await _userRepository.Delete(SelectedUser.Id);
                await LoadUser();
                MessageBox.Show("Сотрудник удален.",
                    "Успешно!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Asterisk);
            }
            else
                MessageBox.Show("Выберите сотрудника для удаления.",
                    "Ошибка!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
        }
        private void OpenAddUserWindow()
        {
            var addUserWindow = new AddUserWindow();
            var addUserViewModel = new AddUserViewModel();
            addUserViewModel.UserAdded += OnLoadUser;
            addUserWindow.DataContext = addUserViewModel;
            addUserWindow.ShowDialog();
        }

        private void OpenEditUserWindow()
        {
            if (SelectedUser != null)
            {
                var editUserWindow = new EditUserWindow();
                var editUserViewModel = new EditUserViewModel(SelectedUser);
                editUserViewModel.UserSaved += OnLoadUser;
                editUserWindow.DataContext = editUserViewModel;
                editUserWindow.ShowDialog();
            }
            else
                MessageBox.Show("Выберите сотрудника.",
                    "Ошибка!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
        }

        private void OnLoadUser(object sender, EventArgs e)
        {
            Task.Run(async () => await LoadUser());
        }

        private void OpenTaskProjectWindow()
        {
            if (SelectedProject != null)
            {
                var taskProjectWindow = new TaskProjectWindow();
                var taskProjectViewModel = new TaskProjectViewModel(SelectedProject);
                taskProjectWindow.DataContext = taskProjectViewModel;
                taskProjectWindow.ShowDialog();
            }
            else
                MessageBox.Show("Выберите проект.",
                    "Ошибка!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
        }
    }
}
