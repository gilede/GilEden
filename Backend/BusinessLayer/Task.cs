using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class Task
    {

        public int columnOrdinal { get; set; }
        public int Id { get; set; }
        public DateTime CreationTime { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime DueDate { get; set; }

        public int AssignedBoard { get; set; }

    
        public string EmailAssigne { get; set; }

        private TaskDTO taskDTO;
        public TaskDTO TaskDTO { get => taskDTO; set { taskDTO = value; } }

        private static readonly ILog log1 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Initializes a new instance of the Task class with specified TaskId, CreationTime, DueTime, Title and Description.
        /// </summary>
        /// <param name="TaskId">The ID of the task.</param>
        /// <param name="CreationTime">The time the task was created.</param>
        /// <param name="DueTime">The time the task is due.</param>
        /// <param name="Title">The title of the task.</param>
        /// <param name="Description">The description of the task (optional).</param>
        /// <exception cref="Exception">Thrown when DueTime is earlier than CreationTime, or Title length is over 50, or Description length is over 300.</exception>
        public Task(string EmailAssigne, int AssignedBoard,int taskId, string title, string description, DateTime dueDate,int columnOrdinal)
        {

            this.columnOrdinal = columnOrdinal; 
            this.EmailAssigne = EmailAssigne;
            this.AssignedBoard = AssignedBoard;
            this.Id = taskId;
            this.Title = title;
            this.Description = description;
            this.DueDate = dueDate;
            this.CreationTime = DateTime.Now;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log1.Info("Starting new Task log!");


            this.TaskDTO=new TaskDTO(Id,this.CreationTime,Title,Description,DueDate,this.AssignedBoard,this.columnOrdinal); 

            if (CreationTime > dueDate)
            {
                throw new Exception("Invalid DueTime");
            }
            if (string.IsNullOrWhiteSpace(title) || Title.Length > 50)
            {
                throw new Exception("the maximum lenght of title is 50");
            }
            if (Description.Length > 300)
            {
                throw new Exception("the maximum lenght of Description is 300");
            }
            taskDTO.Insert();
            log1.Info("task object created successfully");
           
        }
        public Task(TaskDTO taskDTO)
        {

            this.AssignedBoard = taskDTO.AssigedBoard;
            this.Id = taskDTO.TaskId;
            this.DueDate = taskDTO.DueDate;
            this.Title = taskDTO.Title;
            this.Description = taskDTO.Description;
            this.CreationTime = taskDTO.CreationTime;
            this.TaskDTO = taskDTO;
            this.columnOrdinal = taskDTO.ColumnOrdinal;
        }

        /// <summary>
        /// Gets the ID of the task.
        /// </summary>
        /// <returns>The ID of the task.</returns>
        public int getTaskId() { return this.Id; }

        /// <summary>
        /// Gets the due time of the task.
        /// </summary>
        /// <returns>The due time of the task.</returns>
        public DateTime getDueTime() { return this.DueDate; }

        /// <summary>
        /// Sets the due time of the task.
        /// </summary>
        /// <param name="DueTime">The due time of the task.</param>
        /// <exception cref="Exception">Thrown when DueTime is earlier than CreationTime.</exception>


        public void SetDueTime(DateTime DueTime)
        {
            if (CreationTime > DueTime)
            {
                throw new Exception("Invalid DueTime");
            }
            this.DueDate = DueDate;
            TaskDTO.DueDate = DueTime;
            log1.Info("Set task Duetime successfully");

        }
        /// <summary>
        /// Sets the title of the task.
        /// </summary>
        /// <param name="Title">The title of the task.</param>
        /// <exception cref="Exception">Thrown when Title length is over 50.</exception>
        public void SetTitle(string Title)
        {
            if (Title.Length > 50)
            {
                throw new Exception("the maximum lenght of title is 50");
            }
            this.Title = Title;
            taskDTO.Title = Title;
            log1.Info("Set task's title successfully");

        }

        /// <summary>
        /// Sets the description of the task.
        /// </summary>
        /// <param name="Description">The description of the task.</param>
        /// <exception cref="Exception">Thrown when Description length is over 300.</exception>
        public void SetDescription(string Description)
        {
            if (Description.Length > 300)
            {
                throw new Exception("the maximum lenght of Description is 300");
            }
            this.Description = Description;
            taskDTO.Description = Description;
            log1.Info("Set task's Description successfully");
        }



    }
}
