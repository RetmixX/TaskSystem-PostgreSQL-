using System;
using System.Linq;
using System.Windows;
using WpfApp1.Model;
using WpfApp1.View;

namespace WpfApp1.ViewModel
{
    public class ViewModelRegistration:BaseViewModel
    {
        private RealyCommand _reg;

        public string UserName { get; set; }
        public string UserSurName { get; set; }
        public string UserLastName { get; set; }
        public string UserLogin { get; set; }
        public string UserPassword { get; set; }
        public string UserPhone { get; set; }


        public RealyCommand Reg
        {
            get
            {
                return _reg ?? (_reg = new RealyCommand(x =>
                {
                    if (!CheckOfEmpty())
                    {
                        User? checkLogin = Connection.db.Users.FirstOrDefault(x=>x.Login ==UserLogin);
                        if (checkLogin == null)
                        {
                            User newUser = new User()
                            {
                                Name = UserName,
                                Surname = UserSurName,
                                Lastname = UserLastName,
                                Login = UserLogin,
                                Password = UserPassword,
                                Phone = UserPhone
                            };

                            Connection.db.Add(newUser);
                            Connection.db.SaveChanges();
                            Application.Current.Windows.OfType<RegistrationWindow>().First().Close();
                        }
                        else
                        {
                            MessageBox.Show("Логин занят", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Есть незаполненные поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }));
            }
        }

        private bool CheckOfEmpty() => String.IsNullOrEmpty(UserName) && String.IsNullOrEmpty(UserSurName) && String.IsNullOrEmpty(UserLastName) && String.IsNullOrEmpty(UserLogin) && String.IsNullOrEmpty(UserPassword) && String.IsNullOrEmpty(UserPhone);
    }
}
