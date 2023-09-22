using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    internal class StratViewModel:NotifiableObject
    {
        public BackendController Controller { get; private set; }

        private string _Email;
        public string Email
        {
            get => _Email;
            set
            {
                this._Email = value;
                RaisePropertyChanged("Email");
            }
        }
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                this._password = value;
                RaisePropertyChanged("Password");
            }
        }
        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                this._message = value;
                RaisePropertyChanged("Message");
            }
        }


        /// <summary>
        /// Login a user
        /// </summary>
        /// <returns>UserModel</returns>
        public UserModel Login()
        {
            Message = "";
            try
            {
                return Controller.Login(Email, Password);
            }
            catch (Exception e)
            {
                Message = e.Message;
                return null;
            }
        }

        /// <summary>
        /// Register a user
        /// </summary>
        /// <returns></returns>
        public void Register()
        {
            Message = "";
            try
            {
                Controller.Register(Email, Password);
                Message = "Registered successfully";
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
        }

        public StratViewModel()
        {
            this.Controller = new BackendController();
            this.Email = "mail@mail.com";
            this.Password = "Password1";
        }
    }
}
