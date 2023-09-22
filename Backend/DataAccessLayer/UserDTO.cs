using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class UserDTO : DTO
    {
        public static readonly string EMAIL_COLUMN = "Email";
        public static readonly string PASSWORD_COLUMN = "Password";
        public static readonly string LOGIN_STATUS_COLUMN = "Status";

        private string Email;
        public string _Email { get =>Email; }

        private string Password;
        public string _Password { get => Password; }

        private bool loginStatus;
        public bool _loginStatus
        {
            get => loginStatus;
            set
            {
                loginStatus = value;
                ((UserController)_Controller).Update(_Email, LOGIN_STATUS_COLUMN, value);
            }
        }

        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);



        /// <summary>
        /// Constructs a new instance of the UserDTO.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="password">The user's password.</param>
        /// <param name="loginStatus">The user's login status.</param>
        /// 
        public UserDTO(string email, string password, bool loginStatus) : base(new UserController())
        {
            Email = email;
            Password = password;
            this.loginStatus = loginStatus;
        }
        /// <summary>
        /// Inserts the user into the database.
        /// Returns true if the operation is successful.
        /// </summary>
        /// 
        public bool InsertUser()
        {
            if (((UserController)_Controller).InsertUser(this, EMAIL_COLUMN, PASSWORD_COLUMN, LOGIN_STATUS_COLUMN))
            {
                logger.Info("User added to database successfully.");
                return true;
            }

            throw new Exception($"Issue while adding user {Email} to database.");
        }
    }
}
