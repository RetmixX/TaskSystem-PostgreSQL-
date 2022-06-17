using System.Linq;
using System.Windows;
using WpfApp1.Model;
using WpfApp1.View;

namespace WpfApp1.ViewModel
{
    public class TakeTaskViewModel:BaseViewModel
    {
        private Task _task = Connection.selectTask;

        public Task Task
        {
            get
            {
                return _task;
            }
        }

        private RealyCommand _takeTask;
        private RealyCommand _exit;

        public RealyCommand TakeTask
        {
            get
            {
                return _takeTask ?? (_takeTask = new RealyCommand(x =>
                {
                    Task task = Connection.db.Tasks.FirstOrDefault(x => x.Id == _task.Id);
                    task.StatusId = 2;
                    task.TakeUser = Connection.userAuth.Id;
                    Connection.db.SaveChanges();
                    MessageBox.Show("Задача взята!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }));
            }
        }

        public RealyCommand Exit
        {
            get
            {
                return _exit ?? (_exit = new RealyCommand(x =>
                {
                    new MenuWindow().Show();
                    Connection.selectTask = null;
                    Application.Current.Windows.OfType<TakeTaskWIndow>().First().Close();

                }));
            }
        }
    }
}
