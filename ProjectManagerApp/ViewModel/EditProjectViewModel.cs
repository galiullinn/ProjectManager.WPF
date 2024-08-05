using ProjectManagerApp.Infrastructure.Commands;
using ProjectManagerApp.Model;
using ProjectManagerApp.Data;
using ProjectManagerApp.Repositories;
using ProjectManagerApp.ViewModel.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using ProjectManagerApp.View;

namespace ProjectManagerApp.ViewModel
{
    internal class EditProjectViewModel : ViewModelBase
    {
        private string _title = "Редактировать проект";
        private string _projectName;
        private string _projectDescription;
        private DateTime _startDate;
        private DateTime _endDate;
        private StatusProject _statusProject;

        private readonly ProjectRepository _projectRepository;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        public string ProjectName
        {
            get => _projectName;
            set => SetProperty(ref _projectName, value);
        }
        public string ProjectDescription
        {
            get => _projectDescription;
            set => SetProperty(ref _projectDescription, value);
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
        public StatusProject StatusProject
        {
            get => _statusProject;
            set => SetProperty(ref _statusProject, value);
        }

        public IEnumerable<StatusProject> Statuses => Enum.GetValues(typeof(StatusProject)).Cast<StatusProject>();

        public ICommand SaveProjectCommand { get; }
        public event EventHandler ProjectSaved;

        public EditProjectViewModel(Project project)
        {
            _projectRepository = new ProjectRepository(new ApplicationContext());
            ProjectName = project.Name;
            ProjectDescription = project.Description;
            StartDate = project.StartDate;
            EndDate = project.EndDate;
            StatusProject = project.Status;

            SaveProjectCommand = new RelayCommand(async _ => await SaveProject(project.Id));
        }

        public EditProjectViewModel()
        {

        }

        private async Task SaveProject(int projectId)
        {
            if (!string.IsNullOrWhiteSpace(ProjectName) &&
                !string.IsNullOrWhiteSpace(ProjectDescription) &&
                StartDate < EndDate)
            {
                var project = new Project
                {
                    Id = projectId,
                    Name = ProjectName,
                    Description = ProjectDescription,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    Status = StatusProject
                };

                await _projectRepository.Update(project);
                ProjectSaved?.Invoke(this, EventArgs.Empty);

                Close();

                MessageBox.Show("Проект обновлен.",
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
            Application.Current.Windows.OfType<EditProjectWindow>().FirstOrDefault()?.Close();
        }
    }
}
