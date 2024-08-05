using ProjectManagerApp.Infrastructure.Commands;
using ProjectManagerApp.Model;
using ProjectManagerApp.Repositories;
using ProjectManagerApp.View;
using ProjectManagerApp.Data;
using System.Windows.Input;
using System.Windows;
using ProjectManagerApp.ViewModel.Base;

namespace ProjectManagerApp.ViewModel
{
    internal class EditUserViewModel : ViewModelBase
    {
        private string _title = "Редактировать информацию о сотруднике";
        private string _firstName;
        private string _lastName;
        private string _phone;
        private string _email;
        private Position _position;

        private readonly UserRepository _userRepository;

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

        public IEnumerable<Position> Positions => Enum.GetValues(typeof(Position)).Cast<Position>();

        public ICommand SaveUserCommand { get; }
        public event EventHandler UserSaved;

        public EditUserViewModel(User user)
        {
            _userRepository = new UserRepository(new ApplicationContext());
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Phone = user.Phone;
            Position = user.Position;

            SaveUserCommand = new RelayCommand(async _ => await SaveUser(user.Id));
        }

        public EditUserViewModel()
        {

        }

        private async Task SaveUser(int userId)
        {
            if (!string.IsNullOrWhiteSpace(FirstName) &&
                !string.IsNullOrWhiteSpace(LastName) &&
                !string.IsNullOrWhiteSpace(Email) &&
                !string.IsNullOrWhiteSpace(Phone))
            {
                var user = new User()
                {
                    Id = userId,
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    Phone = Phone,
                    Position = Position
                };

                await _userRepository.Update(user);
                UserSaved?.Invoke(this, EventArgs.Empty);

                Close();

                MessageBox.Show("Информация сотрудника обновлена.",
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
            Application.Current.Windows.OfType<EditUserWindow>().FirstOrDefault()?.Close();
        }
    }
}
