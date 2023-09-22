using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using log4net.Config;
using System.IO;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    /// <summary>
    /// Represents a user in the system
    /// </summary>

    public class User
    {

        public List<Board> allUserBoards { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool status { get; set; }

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private UserDTO _userDTO;
        public UserDTO userDTO { get => _userDTO; }





        /// <summary>
        /// Initializes a new instance of the User class with the specified email and password
        /// </summary>
        /// <param name="Email">The email of the user</param>
        /// <param name="password">The password of the user</param>
        public User(string Email, string password)
        {

            if (validmail(Email) == false)
            {

                throw new Exception("invalid email");
            }
            if (validPass(password) == false)
            {

                throw new Exception("invalid password");
            }


            allUserBoards = new List<Board>();
            this.Email = Email;
            this.Password = password;
            this.status = true;

            try
            {

                this._userDTO = new UserDTO(Email, Password, status);
                userDTO.InsertUser();
            }
            catch(Exception ex)
            { 
            throw new Exception(ex.Message);    
            }

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting new User log!");



        }

        internal List<int> GetBoardIDs()
        {
            List<int> boardIds = new List<int>();

            // Iterate over the list of boards
            foreach (Board board in allUserBoards)
            {
                // Retrieve the board ID and add it to the list
                int boardId = board.BoardId;
                boardIds.Add(boardId);
            }
            log.Info("Got all board id's");
            return boardIds;
        }



        public User(UserDTO user)
        {

            allUserBoards=new List<Board>();
            this.Email = user._Email;
            this.Password = user._Password;
            this.status = user._loginStatus;
            this._userDTO = user;
            
           
        }

        // <summary>
        /// Adds the specified board to the list of all boards owned by the user
        /// </summary>
        /// <param name="board">The board to add</param>

        public void addBoard(Board board)
        {
            if (board == null)
                throw new ArgumentNullException(nameof(board));

            if (allUserBoards == null)
                allUserBoards = new List<Board>();

            allUserBoards.Add(board);
            log.Info("added board to user successfully");

        }


        /// <summary>
        /// Removes the specified board from the list of all boards owned by the user
        /// </summary>
        /// <param name="board">The board to remove</param>
        public void removeBoard(Board board)
        {
            if (this.allUserBoards.Contains(board))
                this.allUserBoards.Remove(board);
            log.Info("removed board from user successfully");    
        }

        /// <summary>
        /// Logs in the user with the specified password
        /// </summary>
        /// <param name="password">The password to use for login</param>
        /// <returns>true if login is successful, false otherwise</returns>
        public bool Login(string password)
        {
            if (this.Password != password)
            {

                throw new Exception($"Password {password} is wrong");
            }
            this.status = true;
            userDTO._loginStatus = true;
            log.Info("logged in system successfully");
            return status;
        }


        /// <summary>
        /// Logs out the user
        /// </summary>

        internal void Logout()
        {
            if (status == false)
            {
                throw new Exception("USER IS NOT LOGGED IN THE SYSTEM");

            }
            this.status = false;
            userDTO._loginStatus = false;
            log.Info("Logedout from user successfully");

        }

        /// <summary>
        /// Validates the specified email
        /// </summary>
        /// <param name="Email">The email to validate</param>
        /// <returns>true if the email is valid, false otherwise</returns>
        /// 
        internal bool validmail(string Email)
        {
            if (Email is null)
                throw new Exception($"Email cannot be null");
            Regex rx1 = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Regex rx2 = new Regex(@"^\w+([.-]?\w+)@\w+([.-]?\w+)(.\w{2,3})+$");
            Match match1 = rx1.Match(Email);
            Match match2 = rx2.Match(Email);

            if (!match1.Success || !match2.Success)
            {
                throw new Exception("Email is invalid");

            }
            else
            {
                log.Info("Valid email enter successfully");
                return true;
            }

        }


        /// <summary>
        /// Validates a password to ensure it meets the following criteria:
        /// - Not null or whitespace
        /// - Length between 6 and 20 characters
        /// - Contains at least one uppercase letter, one lowercase letter, and one digit
        /// </summary>
        /// <param name="password">The password to validate</param>
        /// <returns>True if the password is valid, false otherwise</returns>
        /// 
        internal bool validPass(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) //if password is null
            {
                return false;
            }


            if (password.Length < 6 || password.Length > 20) //Length check
            {
                return false;
            }
            bool containsUppercase = false;
            bool containsLowercase = false;
            bool containsDigit = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c))
                {
                    containsUppercase = true;
                }
                else if (char.IsLower(c))
                {
                    containsLowercase = true;
                }
                else if (char.IsDigit(c))
                {
                    containsDigit = true;
                }
            }

            // Check if all required characters are present
            if (!containsUppercase || !containsLowercase || !containsDigit)
            {
                return false;
            }
            log.Info("Valid password entered successfully");
            return true;
        }


        /// <summary>
        /// Changes the password of the current user to the specified new password, after verifying the old password.
        /// Throws an exception if the user is not logged in, the new password is invalid, or the old password is incorrect.
        /// </summary>
        /// <param name="oldP">The current password of the user</param>
        /// <param name="newP">The new password to set</param>
        /// <exception cref="Exception">Thrown if the user is not logged in, the new password is invalid, or the old password is incorrect.</exception>

        internal void ChangePassword(string oldP, string newP)
        {
            if (status == false)
            {
                throw new Exception("User is not logged in");
            }
            if (!validPass(newP))
            {
                throw new Exception("User with invalid new password attempted to change password");
            }
            if (oldP != Password)
            {
                throw new Exception($"Password {oldP} is not matching to the current one");
            }
            this.Password = newP; // change password successfully
            log.Info("Password changed successfully ");
        }

      

    }
}
