using Frontend.Model;
using System;
using System.Windows;
using System.Windows.Media;

namespace Frontend.ViewModel

{
    internal class BoardControllerViewModel : NotifiableObject
    {
        private Model.BackendController controller;
        public UserModel user;
        public SolidColorBrush BackgroundColor
        {
            get
            {
                return new SolidColorBrush(user.Email.Contains("mail@mail") ? Colors.Blue : Colors.Red);
            }
        }
        public BoardControllerModel boardControllerModel { get; private set; }
        public string Title { get; private set; }
        private BoardModel _selectedBoard;

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

        public BoardModel SelectedBoard
        {
            get
            {
                return _selectedBoard;
            }
            set
            {
                _selectedBoard = value;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedBoard");
            }
        }
        private bool _enableForward = false;
        public bool EnableForward
        {
            get => _enableForward;
            private set
            {
                _enableForward = value;
                RaisePropertyChanged("EnableForward");
            }
        }


        /// <summary>
        /// Logout from system
        /// </summary>
        /// <returns></returns>
        internal void Logout()
        {
            Message = "";
            try
            {
                controller.LogOut(user.Email);
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
        }

        public BoardControllerViewModel(UserModel user)
        {
            this.controller = user.Controller;
            this.user = user;
            Title = "Boards for " + user.Email;
            boardControllerModel = user.GetBoards();
        }

        /// <summary>
        /// Choose a board
        /// </summary>
        /// <returns>BoardModel</returns>
        public BoardModel ChooseBoard()
        {

            return SelectedBoard;

        }
    }
}