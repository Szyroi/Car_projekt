using System.Windows;
using VM.MainWindow;

namespace Car_projekt.View
{
    public partial class Dashboard : Window
    {
        public Dashboard()
        {
            InitializeComponent();
            DataContext = new DashboardVM();
        }
    }
}
