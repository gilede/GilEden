using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Frontend.Model
{
    public class BackendController
    {
      
        private ServiceFactory service { get; set; }
        public BackendController(ServiceFactory service)
        {
            this.service = service;
        }

        public BackendController()
        {
            this.service = new ServiceFactory();
            service.LoadData();
        }


        internal List<int> GetColumnIds(string email, string boardName, int columnOrdinal)
        {
            string jsonTasksId = service.GetBoardService().GetColumnIds(email, boardName, columnOrdinal);
            Response<List<int>> res0 = JsonSerializer.Deserialize<Response<List<int>>>(jsonTasksId);

            if (res0.ErrorOccured)
            {
                throw new Exception(res0.ErrorMessage);
            }
            List<int> taskIds = (List<int>)res0.ReturnValue;

            return taskIds;
        }
        internal (int Id, string Title, string Descreption) GetTask(string email, string _boardName, int columnOrdinal, int taskId)
        {
            string ansTitle = service.GetTaskService().GetTaskTitle(email, _boardName, columnOrdinal, taskId);
            string ansDescription = service.GetTaskService().getTaskDescription(email, _boardName, columnOrdinal, taskId);

            Response<string> res2 = JsonSerializer.Deserialize<Response<string>>(ansTitle);
            if (res2.ErrorOccured)
            {
                throw new Exception(res2.ErrorMessage);
            }
            Response<string> res3 = JsonSerializer.Deserialize<Response<string>>(ansDescription);
            if (res3.ErrorOccured)
            {
                throw new Exception(res3.ErrorMessage);
            }

            int taskId1 = taskId;
            string taskTitle = (string)res2.ReturnValue;
            string taskDesc = (string)res3.ReturnValue;

            return (taskId1, taskTitle, taskDesc);
        }




        /// <summary>
        /// Logs in a user to the system
        /// </summary>
        /// <param name="username">User's email</param>
        /// <param name="password">User's password</param>
        /// <returns>UserModel</returns>
        public UserModel Login(string username, string password)
        {
            string user = service.GetUserService().Login(username, password);   

            Response<string> res1 = JsonSerializer.Deserialize<Response<string>>(user);

            if (res1.ErrorOccured)
            {
                throw new Exception(res1.ErrorMessage);
            }
            return new UserModel(this, username);
        }

        /// <summary>
        /// Logs out a user from the system
        /// </summary>
        /// <param name="Email">User's email</param>
        /// <param name="password">User's password</param>
        /// <returns></returns>
        public void LogOut(string Email)
        {
            string user = service.GetUserService().Logout(Email);

            Response<string> res1 = JsonSerializer.Deserialize<Response<string>>(user);

            if (res1.ErrorOccured)
            {
                throw new Exception(res1.ErrorMessage);
            }
        }

        /// <summary>
        /// Registers a user to the system
        /// </summary>
        /// <param name="username">User's email</param>
        /// <param name="password">User's password</param>
        /// <returns></returns>
        internal void Register(string Email, string password)
        {
            string user = service.GetUserService().Register(Email,password);
            Response res = JsonSerializer.Deserialize<Response>(user);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        /// <summary>
        /// Returns all user's board to the system
        /// </summary>
        /// <param name="email">User's email</param>
        /// <returns>List<int></returns>
        internal List<int> GetAllBoardsIds(string email)
        {
            string jsonBoardsIds = service.GetUserService().GetUserBoards(email);
            Response<List<int>> res1 = JsonSerializer.Deserialize<Response<List<int>>>(jsonBoardsIds);
            if (res1.ErrorOccured)
            {
                throw new Exception(res1.ErrorMessage);
            }

            List<int> boardsIds = (List<int>)res1.ReturnValue;
            return boardsIds;
        }


        /// <summary>
        /// Returns a user's board to the system
        /// </summary>
        /// <param name="boardId">User's boardID</param>
        /// <returns>(int Id, string BoardName)</returns>
        internal (string Name, int id) GetBoard(int boardId)
        {
            string ans = service.GetBoardService().GetBoardName(boardId);
            Response<string> res1 = JsonSerializer.Deserialize<Response<string>>(ans);
            if (res1.ErrorOccured)
            {
                throw new Exception(res1.ErrorMessage);
            }

            string Name = (string)res1.ReturnValue;
            return (Name, boardId);
        }


    }
}
