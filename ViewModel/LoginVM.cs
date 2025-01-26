using System.Windows;
using Car_projekt.DatabaseService;
using Car_projekt.View;
using MVVM.RelayCommand;
using MVVM.VMBase;

namespace VM.Login
{
    public class LoginVM : VMBase
    {
        private string username;
        private string password;

        public string Username
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand LoginCommand => new RelayCommand(execute => Login());
        public RelayCommand RegisterCommand => new RelayCommand(execute => Register());

        public LoginVM()
        {
        }

        private void OpenDashboard()
        {
            // Öffne das Dashboard-Fenster
            var dashboardWindow = new Dashboard();
            dashboardWindow.Show();
            Application.Current.MainWindow.Close();
        }

        public void Login()
        {
            if (AuthService.LoginUser(Username, Password))
            {

                OpenDashboard();
            }
            else
            {
                MessageBox.Show("Benutzername oder Passwort ist falsch!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Register()
        {
            if (AuthService.RegisterUser(Username, Password))
            {
                MessageBox.Show("Registrierung erfolgreich!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(
                    "Registrierung fehlgeschlagen. Benutzername könnte bereits existieren.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
