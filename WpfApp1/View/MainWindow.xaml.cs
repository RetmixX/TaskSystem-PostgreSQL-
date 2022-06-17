using System.Windows;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Scaffold-DbContext 'Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TaskSystemBase' Microsoft.EntityFrameworkCore.SqlServer
            //Scaffold-DbContext "Host=localhost;Port=5432;Database=TaskSystem;Username=postgres;Password=12345"
        }
    }
}
