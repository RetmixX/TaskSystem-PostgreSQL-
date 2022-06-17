using System.Linq;
using WpfApp1.Model;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using WpfApp1.View;
using System.Windows;

namespace WpfApp1.ViewModel
{
    public class ViewModelMenuWindow:BaseViewModel
    {
        private User _user = Connection.userAuth;

        private Task _selectedTask;
        private User _selectedUser;

        public ObservableCollection<Task> MyTask { get; set; }
        public ObservableCollection<Task> AvalibaleTask { get; set; }
        public ObservableCollection<User> Users { get; set; }

        public ViewModelMenuWindow()
        {
            MyTask = new ObservableCollection<Task>(Connection.db.Tasks.Include(x => x.PubUserNavigation).Include(x => x.TakeUserNavigation).Include(x => x.Status).Where(x => x.PubUser == _user.Id));
            AvalibaleTask = new ObservableCollection<Task>(Connection.db.Tasks.Include(x => x.PubUserNavigation).Include(x => x.TakeUserNavigation).Include(x => x.Status).Where(x => x.StatusId == 3 && x.PubUser != _user.Id));
            Users = new ObservableCollection<User>(Connection.db.Users);
        }
        public User User
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged("User"); }
        }

        private RealyCommand _exit;
        private RealyCommand _inputMyTask;
        private RealyCommand _inputAvalibaleTask;
        private RealyCommand _showUserInfo;
        private RealyCommand _findTaskOrLogin;

        public Task SelectedTask
        {
            get
            {
                return _selectedTask;
            }

            set
            {
                _selectedTask = value; 
                OnPropertyChanged();
            }
        }

        public User SelectedUser
        {
            get
            {
                return _selectedUser;
            }

            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }

        public RealyCommand InputMyTask
        {
            get
            {
                return _inputMyTask ?? (_inputMyTask = new RealyCommand(x =>
                {
                    if (SelectedTask!=null)
                    {
                        Connection.selectTask = null;
                        Connection.selectTask = SelectedTask;
                        new MyTaskInfoWindow().Show();
                        CloseWindow();
                    }
                }));
            }
        }

        public RealyCommand InputAvalibaleTask
        {
            get
            {
                return _inputAvalibaleTask ?? (_inputAvalibaleTask = new RealyCommand(x =>
                {
                    if (SelectedTask!=null)
                    {
                        Connection.selectTask = null;
                        Connection.selectTask = SelectedTask;
                        new TakeTaskWIndow().Show();
                        CloseWindow();
                    }
                }));
            }


        }

        public RealyCommand ShowUserInfo
        {
            get
            {
                return _showUserInfo ?? (_showUserInfo = new RealyCommand(x =>
                {
                    if (SelectedUser!=null)
                    {
                        Connection.selectUser = SelectedUser;
                        new UserInfoWindow().ShowDialog();
                    }
                }));
            }
        }

        public RealyCommand Exit
        {
            get
            {
                return _exit ?? (_exit = new RealyCommand(x =>
                {
                    new MainWindow().Show();
                    CloseWindow();
                    Connection.userAuth = null;
                }));
            }
        }

        public RealyCommand FindTaskOrUser
        {
            get
            {
                return _findTaskOrLogin ?? (_findTaskOrLogin = new RealyCommand(x =>
                {
                    new FindTaskUserWindow().ShowDialog();
                }));
            }
        }

        private void CloseWindow()
        {
            Application.Current.Windows.OfType<MenuWindow>().First().Close();
        }
    }
}
