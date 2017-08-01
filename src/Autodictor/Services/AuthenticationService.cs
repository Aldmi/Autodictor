using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entitys.Authentication;

namespace MainExample.Services
{
    public class AuthenticationService
    {
        #region prop

        public bool IsAuthentication { get; private set; }
        public User CurrentUser { get; private set; }

        #endregion





        #region Methode

        /// <summary>
        /// Инициализация NoSql БД
        /// </summary>
        public async Task UsersDbInitialize()
        {
            await Task.Factory.StartNew(() =>
            {
                string adminLogin = "Админ";
                string adminPassword = "123456";

                var admin = Program.UsersDbRepository.List(user => (user.Role == Role.Администратор) &&
                                                                   (user.Login == adminLogin)).FirstOrDefault();
                if (admin == null)
                {
                    Program.UsersDbRepository.Add(new User { Login = adminLogin, Password = adminPassword, Role = Role.Администратор });
                }

                //--DEBUG------------------------------------------------------------------------
                //var user1 = new User { Login = "User1", Password = "User1", Role = Role.Диктор };
                //var userDb1 = Program.UsersDbRepository.List(user => (user.Role == user1.Role) &&
                //                                             (user.Login == user1.Login) &&
                //                                             (user.Password == user1.Password)).FirstOrDefault();
                //if (userDb1 == null)
                //{
                //    Program.UsersDbRepository.Add(user1);
                //}


                //var user2 = new User { Login = "User2", Password = "User2", Role = Role.Диктор };
                //var userDb2 = Program.UsersDbRepository.List(user => (user.Role == user2.Role) &&
                //                                             (user.Login == user2.Login) &&
                //                                             (user.Password == user2.Password)).FirstOrDefault();
                //if (userDb2 == null)
                //{
                //    Program.UsersDbRepository.Add(user2);
                //}


                //var user3 = new User { Login = "User3", Password = "User3", Role = Role.Диктор };
                //var userDb3 = Program.UsersDbRepository.List(user => (user.Role == user3.Role) &&
                //                                             (user.Login == user3.Login) &&
                //                                             (user.Password == user3.Password)).FirstOrDefault();
                //if (userDb3 == null)
                //{
                //    Program.UsersDbRepository.Add(user3);
                //}
                //-------------------------------------------------------
            }
             );
        }


        /// <summary>
        /// Вход пользователя
        /// </summary>
        public bool LogIn(User user)
        {
            if (user.Role == Role.Наблюдатель)
            {
                SetObserver();
                return true;
            }

            var existUser = Program.UsersDbRepository.List(u => (u.Role == user.Role) &&
                                                                (u.Login == user.Login) &&
                                                                (u.Password == user.Password)).FirstOrDefault();
            if (existUser == null)
            {
                LogOut();
                return false;
            }
                  
            CurrentUser = existUser;
            IsAuthentication = true;  
            return true;
        }



        /// <summary>
        /// Выход пользователя
        /// </summary>
        public void LogOut()
        {
            CurrentUser = null;
            IsAuthentication = false;
        }



        /// <summary>
        /// Установить пользователя с правами НАБЛЮДАТЕЛЬ
        /// </summary>
        public void SetObserver()
        {
            IsAuthentication = true;
            CurrentUser = new User {Login = "Наблюдатель", Role = Role.Наблюдатель};
        }



        /// <summary>
        /// Проверка доступа по ролям
        /// </summary>
        public bool CheckRoleAcsess(IEnumerable<Role> roles)
        {
            return roles.Contains(CurrentUser.Role);
        }


        #endregion
    }
}
