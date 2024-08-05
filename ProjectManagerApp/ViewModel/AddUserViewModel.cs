using ProjectManagerApp.Infrastructure.Commands;
using ProjectManagerApp.Model;
using ProjectManagerApp.Repositories;
using ProjectManagerApp.View;
using ProjectManagerApp.Data;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using ProjectManagerApp.ViewModel.Base;

namespace ProjectManagerApp.ViewModel
{
    internal class AddUserViewModel : ViewModelBase
    {
        private string _title = "Добавить нового сотрудника";
        private string _firstName;
        private string _lastName;
        private string _phone;
        private string _email;
        private Position _position;

        private readonly UserRepository _userRepository;
        private ObservableCollection<User> _users;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);

        }
        public Position Position
        {
            get => _position;
            set => SetProperty(ref _position, value);
        }
        public ObservableCollection<User> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }

        public IEnumerable<Position> Positions => Enum.GetValues(typeof(Position)).Cast<Position>();

        public ICommand AddUserCommand { get; }
        public event EventHandler UserAdded;

        public AddUserViewModel()
        {
            _userRepository = new UserRepository(new ApplicationContext());
            AddUserCommand = new RelayCommand(async _ => await AddUser());
            Task.Run(async () => await LoadUser());
        }

        private async Task LoadUser()
        {
            var users = await _userRepository.GetAll();
            Users = new ObservableCollection<User>(users);
        }

        private async Task AddUser()
        {
            if (!string.IsNullOrWhiteSpace(FirstName) &&
                !string.IsNullOrWhiteSpace(LastName) &&
                !string.IsNullOrWhiteSpace(Email) &&
                !string.IsNullOrWhiteSpace(Phone))
            {
                var user = new User()
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    Phone = Phone,
                    Position = Position
                };

                await _userRepository.Add(user);
                await LoadUser();
                UserAdded?.Invoke(this, EventArgs.Empty);

                Close();

                MessageBox.Show("Пользователь добавлен.",
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
            Application.Current.Windows.OfType<AddUserWindow>().FirstOrDefault()?.Close();
        }
    }
}
