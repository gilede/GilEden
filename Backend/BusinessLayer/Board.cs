using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;
using log4net.Config;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IntroSE.Kanban.Backend.BusinessLayer
{

    public class Board
    {


        public int BoardId { get; set; }
        public string BoardName { get; set; }
        public string OwnerEmail { get; set; }

        private List<User> _members;
        internal List<User> members { get { return _members; } set { _members = value; } }

        public Dictionary<int, Column> Columns { get; set; }


        public static int TaskId = 0;
        public BoardDTO boardDTO { get; private set; }

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="Board"/> class with the specified name and description.
        /// </summary>
        /// <param name="BoardName">The name of the board.</param>
        /// <param name="Description">The description of the board.</param>
        public Board(int boardid, string BoardName, string OwnerEmail)
        {
            this.BoardId = boardid;
            this.BoardName = BoardName;
            this.OwnerEmail = OwnerEmail;
            Columns = new Dictionary<int, Column>();
            Columns.Add(0, new Column(boardid, 0, "backlog"));
            Columns.Add(1, new Column(boardid, 1, "in progress"));
            Columns.Add(2, new Column(boardid, 2, "done"));
            TaskId = 0;
            this.boardDTO = new BoardDTO(BoardId, BoardName, OwnerEmail);
            _members = new List<User>();
            this.boardDTO.Insert();

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting new board log!");

        }
        public Board(BoardDTO boardDTO)
        { //maybe change

            _members = new List<User>();
            this.BoardId = boardDTO.BoardId;
            this.BoardName = boardDTO.BoardName;
            this.OwnerEmail = boardDTO.OwnerEmail;
            Columns = new Dictionary<int, Column>();
            this.boardDTO = boardDTO;
            LoadColumn();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Board"/> class.
        /// </summary>
        public Board() { }
        public void Delete()
        {
            foreach (KeyValuePair<int, Column> entry in Columns)
            {
                Column column = entry.Value;
                column.Delete();
            }
            foreach (User member in _members)
            {
                UserBoardDTO members = new UserBoardDTO(member.Email,BoardId);
                members.delete();
            }

            Columns.Clear();
            members.Clear();
            _members.Clear();
            this.boardDTO.Delete();

            log.Info("board deleted succefully");
        }


        /// <summary>
        /// Adds a task to the "Backlog" column of the board.
        /// </summary>
        /// <param name="CreationTime">The creation time of the task.</param>
        /// <param name="DueTime">The due time of the task.</param>
        /// <param name="Title">The title of the task.</param>
        /// <param name="Description">The description of the task.</param>
        public void AddTask(string email, string title, string description, DateTime dueDate)
        {
            if (!OwnerEmail.Equals(email) && !_members.Any(member => member.Email.Equals(email)))
            {
                log.Error("Only a board member or owner can add new task");
                throw new Exception("Only the board owner or members can add a task");
            }

            Task task = new Task(OwnerEmail, this.BoardId, TaskId, title, description, dueDate, 0);
            this.Columns[0].AddTask(task);
            TaskId++;
        }
        /// <summary>
        /// Updates the due date of a task in a specified column.
        /// </summary>
        /// <param name="columnOrdinal">The index of the column in which the task is located.</param>
        /// <param name="TaskId">The ID of the task to be updated.</param>
        /// <param name="dueDate">The new due date of the task.</param>
        public void UpdateTaskDueDate(int columnOrdinal, int TaskId, DateTime dueDate)
        {
            Task task = this.Columns[columnOrdinal].GetTask(TaskId);
            task.SetDueTime(dueDate);
            log.Info("Task due date updated successfully");
        }

        /// <summary>
        /// Updates the title of a task in a specified column.
        /// </summary>
        /// <param name="columnOrdinal">The index of the column in which the task is located.</param>
        /// <param name="TaskId">The ID of the task to be updated.</param>
        /// <param name="title">The new title of the task.</param>
        public void UpdateTaskTitle(int columnOrdinal, int TaskId, string title)
        {
            Task task = this.Columns[columnOrdinal].GetTask(TaskId);
            task.SetTitle(title);
            log.Info("Task title updated successfully");
        }

        /// <summary>
        /// Updates the description of a task in a specified column.
        /// </summary>
        /// <param name="columnOrdinal">The index of the column in which the task is located.</param>
        /// <param name="TaskId">The ID of the task to be updated.</param>
        /// <param name="description">The new description of the task.</param>
        public void UpdateTaskDescription(int columnOrdinal, int TaskId, string description)
        {
            Task task = this.Columns[columnOrdinal].GetTask(TaskId);
            task.SetDescription(description);
            log.Info("Task description updated successfully");
        }

        /// <summary>
        /// Advances the task with the given ID to the next column.
        /// Throws an exception if the given column ordinal is invalid or the task ID is not found in the column.
        /// </summary>
        /// <param name="columnOrdinal">The ordinal of the current column of the task.</param>
        /// <param name="TaskId">The ID of the task to advance.</param>
        public void AdvanceTask(int columnOrdinal, int TaskId)
        {
            if (columnOrdinal > 1 || columnOrdinal < 0)
            {
                throw new Exception("invalid column or last column");
                log.Error("ivalid column or last column");
            }
            if (!this.Columns[columnOrdinal].tasks.ContainsKey(TaskId))
            {
                log.Error("ivalid taskid , task id not found in column ordinal ");
                throw new Exception("invalid TaskId");
            }


            Task task = this.Columns[columnOrdinal].GetTask(TaskId);
            this.Columns[columnOrdinal].DeleteTask(task);

            this.Columns[columnOrdinal + 1].AddTask(task);
            task.columnOrdinal = columnOrdinal + 1;
            task.TaskDTO.ColumnOrdinal++;
            log.Info("Task advanced successfully");


        }





        /// <summary>
        /// Returns the due time of the task with the given ID.
        /// </summary>
        /// <param name="taskid">The ID of the task to retrieve the due time for.</param>
        /// <returns>The due time of the task.</returns>
        public DateTime getTaskDueTime(int taskid)
        {
            return this.Columns[0].tasks[taskid].getDueTime();

        }
        /// <summary>
        /// Returns the column with the given ordinal.
        /// </summary>
        /// <param name="columnOrdinal">The ordinal of the column to retrieve.</param>
        /// <returns>The column with the given ordinal.</returns>

        public Column GetColumn(int columnOrdinal)
        {
            return this.Columns[columnOrdinal];
        }
        public void updateOwnerEmail(string newEmailOwner)
        {
            this.boardDTO.OwnerEmail = newEmailOwner;
            this.OwnerEmail = newEmailOwner;
            log.Info("Owner of board updated successfully");
        }

        public void LoadColumn()
        {
            try
            {
                List<DTO> DTOs = this.boardDTO.SelectBoardColumns();
                foreach (ColumnDTO column in DTOs)
                {
                    Column new_column = new Column(column);
                    Columns.Add(new_column.ColumnOrdinal, new_column);
                }
                log.Info("Columns loaded to board successfully");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                log.Error(ex);
            }
        }


        public void LoadBoardTasks()
        {
            TaskControllerDTO TDC = new TaskControllerDTO();
            List<TaskDTO> TaskDTOList = new List<TaskDTO>();
            TaskDTOList = TDC.LoadBoardTasks(BoardId);
            foreach (TaskDTO taskDTO in TaskDTOList)
            {
                Task task = new Task(taskDTO);
                int columnOrdinal = task.columnOrdinal;
                if (!Columns.ContainsKey(columnOrdinal))
                {
                    throw new Exception($"Invalid ColumnOrdinal {columnOrdinal} for task with ID {task.getTaskId}");
                }

                Columns[columnOrdinal].AddTask(task);

            }
            log.Info("Board tasks loaded successfully");
         
        }

    }

}