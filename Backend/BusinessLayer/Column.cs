using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class Column
    {
        private int limit;
        private string ColumnName;
        private int BoardId;
        internal int ColumnOrdinal;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public Dictionary<int, Task> tasks { get; set; }

        public ColumnDTO columnDTO { get; private set; }

        // Constructors
        /// <summary>
        /// Initializes a new instance of the Column class with a given column name.
        /// </summary>
        /// <param name="ColumnName">The name of the column.</param>
        /// 

        public Column(int boardid, int columnOrdinal, string ColumnName)
        {
            this.BoardId = boardid;
            this.ColumnOrdinal = columnOrdinal;
            this.limit = int.MaxValue;
            this.ColumnName = ColumnName;
            tasks = new Dictionary<int, Task>();
            this.columnDTO = new ColumnDTO(BoardId, ColumnOrdinal, ColumnName, limit);
            this.columnDTO.Insert();
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting new Column log!");

        }

        public Column(ColumnDTO columnDTO)
        {
            tasks = new Dictionary<int, Task>();
            this.BoardId = columnDTO.BoardId;
            this.ColumnOrdinal = columnDTO.ColumnOrdinal;
            this.ColumnName = columnDTO.ColumnName;
            this.limit = columnDTO.ColumnLimit;
            this.columnDTO = columnDTO;
        }
        public void Delete()
        {
            foreach (KeyValuePair<int, Task> entry in tasks)
            {
                Task task = entry.Value;
                task.TaskDTO.Delete();
            }
            tasks.Clear();
            log.Info("Column deleted successfully");
        }
        /// <summary>
        /// Initializes a new instance of the Column class.
        /// </summary>

        // Methods
        /// <summary>
        /// Sets the limit of the number of tasks that can be added to the column.
        /// </summary>
        /// <param name="limit">The limit of the number of tasks that can be added to the column.</param>
        public void Setlimit(int limit)
        {
            this.columnDTO.ColumnLimit = limit;
            this.limit = limit;
            log.Info("limit is set successfully to column");
        }
        /// <s
        /// <summary>
        /// Gets the limit of the number of tasks that can be added to the column.
        /// </summary>
        /// <returns>The limit of the number of tasks that can be added to the column.</returns>
        public int Getlimit()
        {
            return this.limit;
        }

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        public string GetName()
        {
            return this.ColumnName;
        }
        /// <summary>
        /// Gets the dictionary of tasks in the column.
        /// </summary>
        /// <returns>The dictionary of tasks in the column.</returns>
        public Dictionary<int, Task> GetTasks()
        {
            return tasks;
        }


        /// <summary>
        /// Gets a task with a specified ID from the column.
        /// </summary>
        /// <param name="TaskId">The ID of the task to get.</param>
        /// <returns>The task with the specified ID.</returns>
        public Task GetTask(int TaskId)
        {
            return tasks[TaskId];
        }
        /// <summary>
        /// Adds a task to the column.
        /// </summary>
        /// <param name="task">The task to add.</param>
        /// <exception cref="Exception">Thrown when the number of tasks in the column has reached its limit.</exception>

        public void AddTask(Task task)
        {
            if (this.tasks.Count >= limit)
            {
                throw new Exception("too much tasks");
            }
            this.tasks.Add(task.getTaskId(), task);
            log.Info("Task added to column successfully");
        }



        /// <summary>
        /// Deletes a task from the column.
        /// </summary>
        /// <param name="task">The task to delete.</param>
        public void DeleteTask(Task task)
        {
            this.tasks.Remove(task.getTaskId());
            log.Info("Task delted from column successfully");
        }

        /// <summary>
        /// Returns a string representation of the Column object.
        /// </summary>
        /// <returns>A string representation of the Column object.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Column Name: ").AppendLine(this.ColumnName);
            sb.Append("Limit: ").AppendLine(this.limit.ToString());
            sb.AppendLine("Tasks: ");
            sb.Append(tasks.Count.ToString());
            foreach (var task in tasks.Values)
            {
                sb.Append(task.ToString()).AppendLine();
            }
            return sb.ToString();
        }
    }
}
