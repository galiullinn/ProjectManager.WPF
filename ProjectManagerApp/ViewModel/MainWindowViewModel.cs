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
        private readonly ProjectRepository _projectRepository;
        private ObservableCollection<Project> _projects;
        private Project _selectedProject;

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

        public ICommand OpenAddProjectWindowCommand { get; }
        public ICommand DeleteProjectCommand { get; }

        public MainWindowViewModel()
        {
            _projectRepository = new ProjectRepository(new ApplicationContext());
            Projects = new ObservableCollection<Project>();

            DeleteProjectCommand = new RelayCommand(async _ => await DeleteProject());

            Task.Run(async () => await LoadPoject());
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
                await _projectRepository.Delete(SelectedProject.ProjectId);
                await LoadPoject();
                MessageBox.Show("Проект удален!",
                    "Успешно",
                    MessageBoxButton.OK,
                    MessageBoxImage.Asterisk);
            }
            else
                MessageBox.Show("Выберите проект для удаления!",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
        }
    }
}
