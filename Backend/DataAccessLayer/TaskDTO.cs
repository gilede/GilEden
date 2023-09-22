using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class TaskDTO:DTO
    {
        private int taskId;
        public int TaskId { get => taskId; }

        private int columnOrdinal;
        public int ColumnOrdinal { get => columnOrdinal;set { columnOrdinal = value; ((TaskControllerDTO)_controller).Update(assignedBoard, TaskId, TaskColumnOrdinalName, value); } }

        private DateTime creationTime;
        public DateTime CreationTime { get => creationTime; }

        private string title;
        public string Title { get => title; set { title = value; ((TaskControllerDTO)_controller).Update(assignedBoard,TaskId, TaskTitleColumnName, value); } }
        

        private string description;
        public string Description { get => description; set { description = value; ((TaskControllerDTO)_controller).Update(assignedBoard, TaskId, TaskDescriptionColumnName, value); } }

        private DateTime dueDate;
        public DateTime DueDate { get => dueDate; set { dueDate = value; ((TaskControllerDTO)_controller).Update(assignedBoard, TaskId, TaskDueDateColumnName, value); } }
        private int assignedBoard;
        public int AssigedBoard { get => assignedBoard; }



        public TaskControllerDTO Controller;
        public const string TaskBoardIDColumnName = "AssignedBoard";
        public const string TaskIdColumnName = "Id";
        public const string TaskCreationTimeColumnName = "CreationTime";
        public const string TaskTitleColumnName = "Title";
        public const string TaskDescriptionColumnName = "Description";
        public const string TaskDueDateColumnName = "DueDate";
        public const string TaskColumnOrdinalName = "ColumnOrdinal";


        /// <summary>
        /// Constructs a new instance of the TaskDTO.
        /// </summary>
        public TaskDTO(int Id, DateTime creationTime, string title, string description, DateTime dueDate, int AssignedBoard, int ColumnOrdinal) : base(new TaskControllerDTO())
        {

            this.assignedBoard = AssignedBoard;
            this.taskId = Id;
            this.creationTime = creationTime;
            this.title = title;
            this.description = description;
            this.dueDate = dueDate;
            this.columnOrdinal = ColumnOrdinal;
        }

        /// <summary>
        /// Inserts the current task into the database.
        /// </summary>
        /// <returns>True if the task was successfully inserted, false otherwise.</returns>
        public bool Insert()
        {
            if (((TaskControllerDTO)_controller).Insert(this))
            {
                return true;
            }
            throw new AggregateException("Can't insert task into DataBase");
        }
        /// <summary>
        /// Deletes the current task from the database.
        /// </summary>
        /// <returns>True if the task was successfully deleted, false otherwise.</returns>
        public bool Delete()
        {
            if (((TaskControllerDTO)_controller).Delete(this))
            {
                return true;
            }

            throw new AggregateException("Unable to delete this task to the DB");
        }




    }
}
