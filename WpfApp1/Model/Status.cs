using System.Collections.Generic;

namespace WpfApp1.Model
{
    public partial class Status : BaseViewModel
    {
        public Status()
        {
            Tasks = new HashSet<Task>();
        }

        private int _id;
        private string _title;

        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("Id"); }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("Title"); }
        }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
