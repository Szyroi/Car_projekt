using System.Windows;
using VM.Login;

namespace Car_projekt.View
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            DataContext = new LoginVM();
        }
    }
}
