using System.Windows;
using VM.MainWindow;

namespace Car_projekt
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowVM();
        }
    }
}