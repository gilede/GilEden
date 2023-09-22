using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class BoardFacade
    {
        public Dictionary<string, Board> userboards { get; set; }
        
        public static int boardId = 0;


        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// UserFacade object to retrieve the user.
        /// </summary>
        private UserFacade uf;
        /// <summary>
        /// Constructor that initializes the BoardFacade object with a UserFacade object.
        /// </summary>
        /// <param name="uf">UserFacade object</param>
        public BoardFacade(UserFacade uf)
        {
            this.uf = uf;
            userboards = new Dictionary<string, Board>();
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting new BoardFacade log!");
        }

        /// <summary>
        /// Creates a new board for a user with the given email and board name.
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="BoardName">Board name</param>
        /// <param name="Description">Board description</param>
        public void CreateBoard(string email, string BoardName)
        {
            string key = email + "," + BoardName;
            if (BoardName == null)
            {
                throw new Exception("BoardName can't be null");
            }
            if (!userboards.ContainsKey(key))
            {

                Board new_board = new Board(boardId,BoardName,email);
                UserBoardDTO user_BoardDTO = new UserBoardDTO(email, boardId);
                user_BoardDTO.insert();
                userboards.Add(key, new_board);
                User user = uf.GetUser(email);
                if (user != null)
                {
                    user.addBoard(new_board);
                    boardId++;
                    log.Info(" board created successfully");
                }
                else
                {
                    throw new Exception("User not found");
                }
            }
            else
            {
                throw new Exception("cannot make 2 board with the same name");
            }
        }


        
        public void AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            Board board = GetBoard(email, boardName);

            board.AddTask(email,title, description, dueDate);
            log.Info("task added successfully to specific board");

        }

        public void AdvanceTask(string email, string boardName, int columnOrdinal, int TaskId)
        {
            Board board = this.GetBoard(email, boardName);

         
            if (!board.Columns[columnOrdinal].tasks.ContainsKey(TaskId))
            {
                throw new Exception("Task not found in specified column");
                log.Error("Task not found in specified column");
            }

            // Ensure the task is assigned to the user trying to move it
            Task task = board.Columns[columnOrdinal].tasks[TaskId];
            if (!task.EmailAssigne.Equals(email))
            {
                throw new Exception("Only the assignee can advance the task");
            }

            try
            {
                board.AdvanceTask(columnOrdinal, TaskId);
                log.Info("Advanced task successfully");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }


        public void UpdateTaskDescription(string email, string boardName, int columnOrdinal, int TaskId, string description)
        {
            Board board = this.GetBoard(email, boardName);

            if (!board.Columns[columnOrdinal].tasks.ContainsKey(TaskId))
            {
                throw new Exception("Task not found in specified column");
            }

            Task task = board.Columns[columnOrdinal].tasks[TaskId];

            if (!task.EmailAssigne.Equals(email))
            {
                throw new Exception("Only the assignee can update the task");
            }

            try
            {
                board.UpdateTaskDescription(columnOrdinal, TaskId, description);
                log.Info("updated task Description successfully");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }


       
        public void AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {

            Task task;
            User user = uf.GetUser(email);
            Board board = null;
            foreach (KeyValuePair<string, Board> entry in userboards)
            {
                if (entry.Value.BoardName == boardName)
                {
                    board = entry.Value;
                    break;
                }
            }
            if (!user.status)
            {
                throw new Exception($"User must be logged in in reder to assign task");
            }

            if (board == null)
            {
                throw new Exception($"No board found with the name {boardName}.");
            }

            if (columnOrdinal < 0 || columnOrdinal >= board.Columns.Count)
            {
                throw new Exception($"Invalid column ordinal {columnOrdinal}.");
            }

            if (!board.members.Any(user => user.Email.Equals(emailAssignee)) && !board.OwnerEmail.Equals(emailAssignee))
            {
                throw new Exception($"The user {emailAssignee} is neither a member of the board nor its owner.");
            }

            task = board.Columns[columnOrdinal].GetTask(taskID);
            if (task == null)
            {
                throw new Exception($"No task found with the ID {taskID} in the column {columnOrdinal}");
            }

            task.EmailAssigne = emailAssignee;



        }


        public void UpdateTaskTitle(string email, string boardName, int columnOrdinal, int TaskId, string title)
        {
            Board board = this.GetBoard(email, boardName);

            if (!board.Columns[columnOrdinal].tasks.ContainsKey(TaskId))
            {
                throw new Exception("Task not found in specified column");
            }

            Task task = board.Columns[columnOrdinal].tasks[TaskId];

            if (!task.EmailAssigne.Equals(email))
            {
                throw new Exception("Only the assignee can update the task");
            }

            try
            {
                board.UpdateTaskTitle(columnOrdinal, TaskId, title);
                log.Info("updated task title successfully");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

 

        public string GetBoardName(int boardId)
        {
            foreach (var boardEntry in userboards)
            {
                Board board = boardEntry.Value;

                if (board.BoardId == boardId)
                {
                    log.Info("BoardName recieved successfully");
                    return board.BoardName;

                }
            }

           
            throw new ArgumentException($"No board with id {boardId} exists");
        }

        public void UpdateTaskDueDate(string Email,string BoardName,int columnOrdinal,int TaskId,DateTime DueDate)
        {
            Board board = this.GetBoard(Email, BoardName);


            if (!board.Columns[columnOrdinal].tasks.ContainsKey(TaskId))
            {
                throw new Exception("Task not found in specified column");
            }

            Task task = board.Columns[columnOrdinal].tasks[TaskId];


            if (!task.EmailAssigne.Equals(Email))
            {
                throw new Exception("Only the assignee can update the task");
            }

            try
            {
                board.UpdateTaskDueDate(columnOrdinal, TaskId, DueDate);
                log.Info("task due date updated successfully");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public Dictionary<int, Board> GetUserBoards(string email)
        {
            if (!uf.Exists(email))
            {
                throw new Exception($"the user does not exist.");
            }

            Dictionary<int, Board> user_boards = new Dictionary<int, Board>();

            foreach (KeyValuePair<string, Board> entry in userboards)
            {
                Board board = entry.Value;
                if (board.OwnerEmail.Equals(email))
                    user_boards.Add(board.BoardId, board);
            }

            log.Info("userBoards returned successfully");
            return user_boards;
        }


        internal bool BoardAssignedToUser(string email, int boardID)
        {
            User user = uf.GetUser(email);
            if (user != null && user.allUserBoards.Any(board => board.BoardId == boardID))
                return true;
            return false;
        }

    
        /// <summary>
        /// Retrieves a board for a user with the given email and board name.
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="BoardName">Board name</param>
        /// <returns>Board object</returns>
        public Board GetBoard(string email, string BoardName)
        {
            string key = email + "," + BoardName;
            if (BoardName == null)
            {
                throw new Exception("BoardName can't be null");
            }
            if (!userboards.ContainsKey(key))
            {
                throw new Exception("Invalid BoardName or email");
            }
            if (!uf.IsLogIn(email))
                throw new Exception("the user have to be logged in");

            log.Info("Fatched board successfully");
            return userboards[key];
        }

        /// <summary>
        /// Limits the number of tasks that can be added to a column for a user with the given email, board name, and column ordinal.
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="BoardName">Board name</param>
        /// <param name="columnOrdinal">Column ordinal</param>
        /// <param name="limit">Limit to be set for the column</param>
        public void LimitColumn(string email, string BoardName, int columnOrdinal, int limit)
        {
            string key = email + "," + BoardName;
            if (!userboards.ContainsKey(key))
            {
                throw new Exception("Invalid BoardName or email");
            }
            if (columnOrdinal < 0 || columnOrdinal > 2 || limit < 0)
            {
                throw new Exception("Invalid columnOrdinal or limit");
            }
            userboards[key].Columns[columnOrdinal].Setlimit(limit);
        }

        // <summary>
        // Retrieves the limit for a specific column in the board associated with the given email and name.
        // </summary>
        // <param name="email">The email associated with the user who owns the board.</param>
        // <param name="BoardName">The name of the board to retrieve the column limit from.</param>
        // <param name="columnOrdinal">The index of the column to retrieve the limit from.</param>
        // <returns>The limit of the column specified by columnOrdinal in the board associated with the given email and name.</returns>
        // <exception cref="Exception">Thrown when BoardName or email are invalid.</exception>
        public int GetColumnLimit(string email, string BoardName, int columnOrdinal)
        {
            string key = email + "," + BoardName;
            if (!userboards.ContainsKey(key))
            {
                throw new Exception("Invalid BoardName or email");
            }
            if (columnOrdinal < 0 || columnOrdinal > 2)
            {
                throw new Exception("Invalid columnOrdinal");
            }

            if (userboards[key].Columns[columnOrdinal].Getlimit() == int.MaxValue)
                return -1;
            else
            {
                log.Info("got limit board successfully");
                return userboards[key].Columns[columnOrdinal].Getlimit();
            }

        }

        // <param name="email">The email associated with the user who owns the board.</param>
        // <param name="BoardName">The name of the board to retrieve the column name from.</param>
        // <param name="columnOrdinal">The index of the column to retrieve the name from.</param>
        // <returns>The name of the column specified by columnOrdinal in the board associated with the given email and name.</returns>
        // <exception cref="Exception">Thrown when BoardName or email are invalid, or when columnOrdinal is out of bounds.</exception>
        public String GetColumnName(string email, string BoardName, int columnOrdinal)
        {
            string key = email + "," + BoardName;
            if (!userboards.ContainsKey(key))
            {
                throw new Exception("Invalid BoardName or email");
            }
            if (columnOrdinal < 0 || columnOrdinal > 2)
            {
                throw new Exception("Invalid columnOrdinal");
            }
            log.Info("got Column name successfully");
            return userboards[key].Columns[columnOrdinal].GetName();
        }

        /// <summary>
        /// Returns the Column object at the specified column ordinal for the specified board and user email.
        /// </summary>
        /// <param name="email">The email of the user who owns the board.</param>
        /// <param name="BoardName">The name of the board.</param>
        /// <param name="columnOrdinal">The column ordinal of the Column object to retrieve.</param>
        /// <returns>The Column object at the specified column ordinal.</returns>
        /// <exception cref="Exception">Thrown when the specified BoardName or email is invalid or the columnOrdinal is out of range.</exception>
        /// 
        public List<Task> GetColumn(string email, string BoardName, int columnOrdinal)
        {
            string key = email + "," + BoardName;
            if (!userboards.ContainsKey(key))
            {
                throw new Exception("Invalid BoardName or email");
            }
            if (columnOrdinal < 0 || columnOrdinal > 2)
            {
                throw new Exception("Invalid columnOrdinal");
            }
            List<Task> tasks = new List<Task>();
            foreach (Task t in userboards[key].GetColumn(columnOrdinal).tasks.Values)
            {
                tasks.Add(t);
            }
            log.Info("got Column successfully");
            return tasks;
        }

        /// <summary>
        /// This method assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column number. The first column is 0, the number increases by 1 for each column</param>
        /// <param name="taskID">The task to be updated identified a task ID</param>        
        /// <param name="emailAssignee">Email of the asignee user</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>

     



        public void DeleteData()
        {
            try
            {
                new TaskControllerDTO().DeleteAll();
                new BoardController().DeleteAll();
                new ColumnController().DeleteAll();
                new UserBoardController().DeleteAll();
                new UserController().DeleteAll();
                userboards.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public void LeaveBoard(string email, int boardID)
        {
            var key = userboards.FirstOrDefault(board => board.Value.BoardId == boardID).Key;

            if (key == null)
            {
                throw new Exception("Board does not exist");
            }

            User user = uf.GetUser(email);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            if (!user.status)
            {
                throw new Exception("User must be logged in in order to leave board");
            }

            if (userboards[key].OwnerEmail == user.Email)
            {
                throw new Exception("Owner cannot leave the board");
            }

            if (!userboards[key].members.Any(member => member.Email == email))
            {
                throw new Exception("User is not a member of the board");
            }

            userboards[key].members.RemoveAll(member => member.Email == email);
            user.removeBoard(userboards[key]);
            log.Info("User left board successfully");

        }



        /// <summary>
        /// Deletes a board for a user with the given email and board name.
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="BoardName">Board name</param>
        public void DeleteBoard(string email, string BoardName)
        {
            string key = email + "," + BoardName;
            if (!userboards.ContainsKey(key))
            {
                throw new Exception("Board does not exist");
            }
            User user = uf.GetUser(email);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            if (!user.allUserBoards.Any(b => b.BoardId == userboards[key].BoardId))
            {
                throw new Exception("User does not own the board");
            }

            if (!(userboards[key].OwnerEmail == email))
            {
                throw new Exception("Only Owner of board can remove board");
            }

            uf.GetUser(email).removeBoard(userboards[key]);
            userboards[key].Delete();
            userboards.Remove(key);
            log.Info("Board deleted successfully");

        }

        public void LoadTasks()
        {
            foreach (KeyValuePair<string, Board> board in userboards)
            {
                board.Value.LoadBoardTasks();
            }

        }


        public void LoadLData()
        {
            try
            {
                List<DTO> DTOs = new BoardController().Select();
                foreach (BoardDTO board in DTOs)
                {
                    string key = board.OwnerEmail + "," + board.BoardName;
                    Board new_board = new Board(board);
                    userboards.Add(key, new_board);
                    boardId++;
                }


                LoadBoardMembers();
                LoadTasks();
                log.Info("Data loaded successfully");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




        private void LoadBoardMembers()
        {
            List<UserBoardDTO> membersDTO = new UserBoardController().LoadBoardMembers();
            foreach (UserBoardDTO memberDTO in membersDTO)
            {
                foreach (KeyValuePair<string, Board> entry in userboards)
                {
                    string[] keyParts = entry.Key.Split(",");
                    string boardEmail = keyParts[0];
                    string boardName = keyParts[1];

                    if (entry.Value.BoardId == memberDTO.boardId)
                    {
                   
                        User member = uf.GetUser(memberDTO.OwnerEmail);
                        if (member != null)
                        {
                            entry.Value.members.Add(member);
                            member.allUserBoards.Add(userboards[entry.Key]);
                        }
                        else
                        { 
                            Console.WriteLine($"Warning: User with email {memberDTO.OwnerEmail} not found");
                        }
                        break;
                    }
                }
            }
           
        }


        public string JoinBoard(string email, int boardID)
        {
            Board boardToJoin = null;
            foreach (KeyValuePair<string, Board> entry in userboards)
            {
                if (entry.Value.BoardId == boardID)
                {
                    boardToJoin = entry.Value;
                    break;
                }
            }

            if (boardToJoin == null)
            {
                throw new Exception($"The board with the specified ID does not exist.");
               
            }
            User user = uf.GetUser(email);
            if (user == null)
            {
                return "The user with the specified email does not exist.";
            }

            if (user.allUserBoards.Any(b => b.BoardId == boardID))
            {
                return "The user is already a part of this board.";
            }

            user.addBoard(boardToJoin);
            boardToJoin.members.Add(user);
            UserBoardDTO member = new UserBoardDTO(email, boardID);
            member.insert();
        
            return "The user has been successfully added to the board.";
        }

        public void TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            string key = currentOwnerEmail + "," + boardName;
            if (!userboards.ContainsKey(key))
            {
                throw new Exception("Invalid BoardName or email");
            }
            Board board = userboards[key];

            // verify if the currentOwnerEmail is actually the current owner of the board
            if (!board.OwnerEmail.Equals(currentOwnerEmail))
            {
                throw new Exception("Provided currentOwnerEmail is not the owner of the board");
            }

            // check if the new owner email is valid (the actual user exists)
            User newOwner = uf.GetUser(newOwnerEmail);
            if (newOwner == null)
            {
                throw new Exception("New owner email is invalid");
            }

            string new_key = newOwnerEmail + "," + boardName;

            // check if the new key already exists in the dictionary
            if (userboards.ContainsKey(new_key))
            {
                throw new Exception("A board with the same name already exists for the new owner");
            }

            userboards.Remove(key);
            userboards.Add(new_key, board);
            board.updateOwnerEmail(newOwnerEmail);
        }

        public String GetTaskDescription(string email, string BoardName, int columnOrdinal, int TaskId)
        {
            Board board = this.GetBoard(email, BoardName);

            if (!board.Columns[columnOrdinal].tasks.ContainsKey(TaskId))
            {
                throw new Exception("Task not found in specified column");
            }

            Task task = board.Columns[columnOrdinal].tasks[TaskId];
            return task.Description;

        }
        public String GetTaskTitle(string email, string BoardName, int columnOrdinal, int TaskId)
        {
            Board board = this.GetBoard(email, BoardName);

            if (!board.Columns[columnOrdinal].tasks.ContainsKey(TaskId))
            {
                throw new Exception("Task not found in specified column");
            }

            Task task = board.Columns[columnOrdinal].tasks[TaskId];
            return task.Title;

        }
        public List<int> GetColumnIds(string email, string BoardName, int columnOrdinal)
        {
            string key = email + "," + BoardName;
            if (!userboards.ContainsKey(key))
            {
                throw new Exception("Invalid BoardName or email");
            }
            if (columnOrdinal < 0 || columnOrdinal > 2)
            {
                throw new Exception("Invalid columnOrdinal");
            }
            List<int> tasksid = new List<int>();
            foreach (Task t in userboards[key].GetColumn(columnOrdinal).tasks.Values)
            {
                tasksid.Add(t.Id);
            }
            return tasksid;
        }



    }
}
