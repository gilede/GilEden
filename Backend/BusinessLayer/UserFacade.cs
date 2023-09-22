using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    /// <summary>
    ///  Initializes a new instance of the <see cref="UserFacade"/> class.
    /// </summary>
    public class UserFacade
    {

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        internal Dictionary<string, User> users { get; set; }
        public UserFacade()
        {
            this.users = new Dictionary<string, User>();
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting new User log!");
        }
        /// <summary>
        ///  Registers a new user.
        /// </summary>
        /// <param name="Email">The email address of the user to register.</param>
        /// <param name="password">The password of the user to register.</param>
        public void RegisterUser(string Email, string password)
        {

            if (string.IsNullOrWhiteSpace(Email))
            {
                throw new Exception("Email is null");
            }
            Email = Email.ToLower();
            if (users.ContainsKey(Email))
            {
                throw new Exception($"Email {Email} already exists");
            }
            try
            {
                User u = new User(Email, password);
                users.Add(Email, u);
                log.Info("User registered successfully to system");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        ///  Checks whether a user with the specified email is logged in.
        /// </summary>
        /// <param name="Email">The email address of the user to check.</param>
        /// <returns><c>true</c> if the user is logged in, otherwise <c>false</c>.</returns>
        internal bool IsLogIn(string Email)
        {
            Email = Email.ToLower();
            return users[Email].status;
        }

        /// <summary>
        ///  Checks whether a user with the specified email exists and is logged in.
        /// </summary>
        /// <param name="Email">The email address of the user to check.</param>
        /// <returns><c>true</c> if the user exists and is logged in, otherwise <c>false</c>.</returns>
        internal bool CheckUser(string Email)
        {
            if (!Exists(Email))
            {
                return false;
            }
            Email = Email.ToLower();
            if (!IsLogIn(Email))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        ///  Checks whether a user with the specified email exists.
        /// </summary>
        /// <param name="Email">The email address of the user to check.</param>
        /// <returns><c>true</c> if the user exists, otherwise <c>false</c>.</returns>
        public bool Exists(string Email)
        {
            if (string.IsNullOrWhiteSpace(Email))
            {

                return false;
            }
            Email = Email.ToLower();
            return users.ContainsKey(Email);
        }
        /// <summary>
        ///  Logs in an existing user.
        /// </summary>
        /// <param name="Email">The email address of the user to login.</param>
        /// <param name="password">The password of the user to login.</param>
        /// <returns>A response with the user's email, unless an error occurs (see <see cref="GradingService"/>).</returns>
        public User loggedIn(string Email, string password)
        {
            if (Exists(Email))
            {
                var u = users[Email];
                try
                {
                    u.Login(password);
                    log.Info("user logged in system successfully");
                    return u;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }
            else
            {
                throw new Exception("no such Email:" + Email);
            }


        }
        /// <summary>
        /// Gets the user with the specified email address.
        /// </summary>
        /// <param name="email">The email address of the user to get.</param>
        /// <returns>The user with the specified email address.</returns>
        internal User GetUser(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new Exception($"Email is null");
            }
            email = email.ToLower();
            if (!users.ContainsKey(email))
            {
                throw new Exception($"Email {email} is not in users dict");
            }
            log.Info("User fatched by email successfully");
            return users[email];
        }


       

        /// <summary>
        /// Changes the password of an existing user.
        /// </summary>
        /// <param name="Email">The email address of the
        public string ChangePassword(string Email, string oldP, string newP)
        {
            if (Exists(Email))
            {
                var u = users[Email];

                try
                {
                    u.ChangePassword(oldP, newP);
                    log.Info("Password changed to the user successfully");
                    return $"Password {oldP} changed to {newP} successfully! ";

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }
            else
            {
                throw new Exception("User Email doesn't exist in dict");
            }


        }
        public List<int> GetUserBoards(string email)
        {
            if (!Exists(email))
            {
                throw new Exception($"the user does not exist.");
            }
            log.Info("All board ids of user obtained");
            return users[email].GetBoardIDs();
        }

        public List<Task> gellallCurrentCol(string Email)
        {
            if (Exists(Email))
            {
                List<Task> t = new List<Task>();
                var u = users[Email].allUserBoards;
                for (int i = 0; i < u.Count; i++)
                {
                    List<Task> tasks = new List<Task>();
                    tasks = t.Concat(u[i].GetColumn(1).tasks.Values).ToList();
                    t = tasks;
                }
                log.Info("recieved all current col");
                return t;
            }
            else
            {
                throw new Exception("Email doesn't exist");
            }
        }
        public string Logout(string Email)
        {

            if (Exists(Email))
            {
                var u = users[Email];
                try
                {
                    u.Logout();
                    log.Info("Loged out from user successfully");
                    return u.ToString();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            
            }
            else
            {
                throw new Exception("User Email doesn't exist in dict");
            }
        }

        public void LoadData()
        {
            UserController userController = new UserController();
            List<UserDTO> dtoUser = userController.LoadUsers();
            foreach (UserDTO DU in dtoUser)
            {
                User user = new User(DU);
                users[user.Email] = user;
            }
            log.Info("Data of userFacade laoded successfully");
           
        }
        public void DeleteData()
        {
            try
            {
                new UserController().DeleteAll();
                users.Clear();
                log.Info("Data of userFacade deleted successfully");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
