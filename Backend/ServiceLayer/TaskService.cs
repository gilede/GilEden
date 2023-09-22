using IntroSE.Kanban.Backend.BusinessLayer;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace IntroSE.Kanban.Backend.ServiceLayer
{

    public class TaskService
    {

        public BoardFacade bf { get; set; }
        // i will need the user facade
        public readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskService"/> class.
        /// </summary>
        /// <param name="bf">An instance of <see cref="BoardFacade"/> used to call methods in the business layer.</param>
        public TaskService(BoardFacade bf)
        {
            this.bf = bf;
        }
        /// <summary>
        /// Adds a new task to the system.
        /// </summary>
        /// <param name="email">The email of the user who wants to add the task.</param>
        /// <param name="boardName">The name of the board in which the task will be added.</param>
        /// <param name="title">The title of the new task.</param>
        /// <param name="description">The description of the new task.</param>
        /// <param name="dueDate">The due date of the new task.</param>
        /// <returns>A JSON representation of a new <see cref="Response"/> object indicating whether the operation succeeded or not.</returns>

        public string AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            try
            {

                bf.AddTask(email, boardName, title, description, dueDate);
                log.Info($"{email} successfully add task ");
                var r = ReturnJson(new Response());
                return r;
            }
            catch (Exception ex)
            {
                log.Debug($"{email} Failed to add task ", ex);
                return ReturnJson(new Response((ex.Message), null));
            }
        }

        /// <summary>
        /// Updates the due date of a task.
        /// </summary>
        /// <param name="email">The email of the user who wants to update the task.</param>
        /// <param name="boardName">The name of the board in which the task is located.</param>
        /// <param name="columnOrdinal">The ordinal of the column in which the task is located.</param>
        /// <param name="taskId">The ID of the task to update.</param>
        /// <param name="dueDate">The new due date of the task.</param>
        /// <returns>A JSON representation of a new <see cref="Response"/> object indicating whether the operation succeeded or not.</returns>
        public string UpdateTaskDueDate(string email, string BoardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            try
            {

                bf.UpdateTaskDueDate(email, BoardName,columnOrdinal,taskId, dueDate);
                log.Info($"{email} successfully update task due date");
                var r = ReturnJson(new Response());
                return r;
            }
            catch (Exception ex)
            {
                log.Debug($"{email} Failed to update task due date", ex);
                return ReturnJson(new Response((ex.Message), null));
            }
        }
        /// <summary>
        /// Updates the title of a task on a board.
        /// </summary>
        /// <param name="email">The email of the user who is updating the task. The user must be logged in.</param>
        /// <param name="boardName">The name of the board on which the task is located.</param>
        /// <param name="columnOrdinal">The ordinal number of the column in which the task is located.</param>
        /// <param name="taskId">The ID of the task that needs to be updated.</param>
        /// <param name="title">The new title to be assigned to the task.</param>
        /// <returns>A response object in JSON format indicating whether the update was successful or not.</returns>
        public string UpdateTaskTitle(string email, string BoardName, int columnOrdinal, int taskId, string title)
        {
            try
            {
                bf.UpdateTaskTitle(email, BoardName,columnOrdinal,taskId,title);
                log.Info($"{email} successfully update task title");
                var r = ReturnJson(new Response());
                return r;
            }
            catch (Exception ex)
            {
                log.Debug($"{email} Failed to update task title", ex);
                return ReturnJson(new Response((ex.Message), null));
            }
        }

        public string GetTaskTitle(string email, string BoardName, int columnOrdinal, int taskId)
        {
            try
            {
                String Title = bf.GetTaskTitle(email, BoardName, columnOrdinal, taskId);
                log.Info($"{email} successfully update task title");
                var r = ReturnJson(new Response(Title));
                return r;
            }
            catch (Exception ex)
            {
                log.Debug($"{email} Failed to update task title", ex);
                return ReturnJson(new Response((ex.Message), null));
            }
        }
        public string getTaskDescription(string email, string BoardName, int columnOrdinal, int taskId)
        {
            try
            {
                String description = bf.GetTaskDescription(email, BoardName, columnOrdinal, taskId);
                log.Info($"{email} successfully get the column");
                return JsonSerializer.Serialize(new Response(description));
            }
            catch (Exception ex)
            {
                log.Debug($"{email} Failed to get the column", ex);
                return ReturnJson(new Response((ex.Message), null));

            }
        }

        /// <summary>
        /// Updates the description of a task on a board.
        /// </summary>
        /// <param name="email">The email of the user who is updating the task. The user must be logged in.</param>
        /// <param name="boardName">The name of the board on which the task is located.</param>
        /// <param name="columnOrdinal">The ordinal number of the column in which the task is located.</param>
        /// <param name="taskId">The ID of the task that needs to be updated.</param>
        /// <param name="description">The new description to be assigned to the task.</param>
        /// <returns>A response object in JSON format indicating whether the update was successful or not.</returns>
        public string UpdateTaskDescription(string email, string BoardName, int columnOrdinal, int taskId, string description)
        {
            try
            {
                bf.UpdateTaskDescription(email, BoardName, columnOrdinal, taskId, description);
                log.Info($"{email} successfully update task description");
                var r = ReturnJson(new Response());
                return r;
            }
            catch (Exception ex)
            {
                log.Debug($"{email} Failed to update task description", ex);
                return ReturnJson(new Response((ex.Message), null));
            }
        }

        /// <summary>
        /// Serializes a response object to JSON format and returns it as a string.
        /// </summary>
        /// <param name="res">The response object to be serialized.</param>
        /// <returns>A string containing the serialized response object in JSON format, or an error message if serialization fails.</returns>
        internal string ReturnJson(Response res)
        {
            try
            {
                var options = new JsonSerializerOptions();
                options.WriteIndented = true;
                options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                return JsonSerializer.Serialize(res, res.GetType(), options);
            }
            catch
            {
                return "Error : Failed to serialize Response object";
            }
        }
    }
}


