using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using WpfApp1.Model;
using WpfApp1.View;

namespace WpfApp1.ViewModel
{
    public class ViewModelMyTaskInfo:BaseViewModel
    {
        private Task _selectedtask = Connection.selectTask;

        public Task SelectedTask
        {
            get { return _selectedtask; }
            set { _selectedtask = value; OnPropertyChanged(); }
        }

        private RealyCommand _ready;
        private RealyCommand _process;
        private RealyCommand _avilable;
        private RealyCommand _back;

        public RealyCommand Back
        {
            get
            {
                return _back ?? (_back = new RealyCommand(x =>
                {
                    Connection.selectTask = null;
                    new MenuWindow().Show();
                    Application.Current.Windows.OfType<MyTaskInfoWindow>().First().Close();

                }));
            }
        }

        public RealyCommand Ready
        {
            get
            {
                return _ready ?? (_ready = new RealyCommand(x =>
                {
                    ChangeStatus(1);

                }));
            }
        }

        public RealyCommand Process
        {
            get
            {
                return _process ?? (_process = new RealyCommand(x =>
                {
                    ChangeStatus(2);
                }));
            }
        }

        public RealyCommand Avilable
        {
            get
            {
                return _avilable ?? (_avilable = new RealyCommand(x =>
                {
                    ChangeStatus(3);
                }));
            }
        }

        private void ChangeStatus(int statusId)
        {
            if (_selectedtask!=null)
            {
                Task task = Connection.db.Tasks.FirstOrDefault(x => x.Id == _selectedtask.Id);
                task.StatusId = statusId;
                if (statusId == 3)
                {
                    task.TakeUser = null;
                }
                Connection.db.SaveChanges();
                MessageBox.Show("Статус изменен!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
