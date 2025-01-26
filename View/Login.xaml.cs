using System.Windows;
using VM.Login;

namespace Car_projekt.View
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            DataContext = new LoginVM();
        }
    }
}
