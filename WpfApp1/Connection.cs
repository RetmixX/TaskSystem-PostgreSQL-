using WpfApp1.Model;

namespace WpfApp1
{
    public class Connection
    {
        public static TaskSystemContext db = new TaskSystemContext();

        public static User userAuth = new User();
        public static Task selectTask = new Task();
        public static User selectUser = new User();

    }
}
