using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using log4net;
using System.Reflection;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class BoardService
    {
        public BoardFacade bf { get; set; }
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        // Initializes a new instance of the BoardService class with the specified BoardFacade object.
        public BoardService(BoardFacade bf)
        {
            this.bf = bf;
        }
        /// <summary>
        /// Creates a new board for a user.
        /// </summary>
        /// <param name="email">The email address of the user creating the board.</param>
        /// <param name="boardName">The name of the new board.</param>
        /// <param name="description">The description of the new board (optional).</param>
        /// <returns>Returns a Response object with an error message if an error has occurred. Otherwise, the response indicates that the board was created successfully.</returns>
        public string CreateBoard(string email, string BoardName)
        {
            try
            {
                bf.CreateBoard(email, BoardName);
                log.Info($"{email} successfully add board ");
                var r = ReturnJson(new Response());
                return r;
            }
            catch (Exception ex)
            {
                log.Debug($"{email} Failed to add board ", ex);
                return ReturnJson(new Response((ex.Message), null));
            }


        }

        public string JoinBoard(string email, int boardID)
        {
            try
            {
                bf.JoinBoard(email, boardID);
                log.Info($"{email} successfully add board ");
                var r = ReturnJson(new Response());
                return r;

            }
            catch (Exception ex)
            {
                log.Debug($"{email} Failed to add board ", ex);
                return ReturnJson(new Response((ex.Message), null));
            }
        }



        /// <summary>
        /// Retrieves a number of tasks from a board belonging to a user.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <param name="boardName">The name of the board.</param>
        /// <param name="columnOrdinal">The index of the column to limit (0-based).</param>
        /// <param name="limit">The maximum number of tasks to return.</param>
        /// <returns>Returns a Response object with an error message if an error has occurred. Otherwise, the response contains the requested tasks.</returns>
        public string LimitColumn(string email, string BoardName, int columnOrdinal, int limit)
        {
            try
            {
                bf.LimitColumn(email, BoardName, columnOrdinal, limit);
                log.Info($"{email} successfully limit column ");
                var r = ReturnJson(new Response());
                return r;
            }
            catch (Exception ex)
            {
                log.Debug($"{email} Failed to limit column ", ex);
                return ReturnJson(new Response((ex.Message), null));

            }
        }

        /// <summary>
        /// Gets the limit for a column belonging to a user's board.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <param name="boardName">The name of the board.</param>
        /// <param name="columnOrdinal">The index of the column to get the limit for (0-based).</param>
        /// <returns>Returns a Response object with an error message if an error has occurred. Otherwise, the response contains the column's limit.</returns>
        public string GetColumnLimit(string email, string BoardName, int columnOrdinal)
        {
            try
            {
                int limit = bf.GetColumnLimit(email, BoardName, columnOrdinal);
                log.Info($"{email} successfully get the column limit");
                var r = ReturnJson(new Response(limit));
                return r;
            }
            catch (Exception ex)
            {
                log.Debug($"{email} Failed to get the column limit ", ex);
                return ReturnJson(new Response((ex.Message), null));

            }
        }
        // <summary>
        /// Get the name of a column based on the email of the user, the name of the board, and the column ordinal.
        /// </summary>
        /// <param name="email">Email of the user</param>
        /// <param name="BoardName">Name of the board</param>
        /// <param name="columnOrdinal">Ordinal of the column</param>
        /// <returns>Returns the name of the column as a JSON string</returns>
        public string GetColumnName(string email, string BoardName, int columnOrdinal)
        {
            try
            {
                String name = bf.GetColumnName(email, BoardName, columnOrdinal);
                log.Info($"{email} successfully get the column name");
                var r = ReturnJson(new Response(null, name));
                return r;
            }
            catch (Exception ex)
            {
                log.Debug($"{email} Failed to get the column name ", ex);
                return ReturnJson(new Response((ex.Message), null));

            }
        }
        /// <summary>
        /// Get all tasks in a column of a board based on the email of the user, the name of the board, and the column ordinal.
        /// </summary>
        /// <param name="email">Email of the user</param>
        /// <param name="BoardName">Name of the board</param>
        /// <param name="columnOrdinal">Ordinal of the column</param>
        /// <returns>Returns the list of tasks in the column as a JSON string</returns>

        public string GetColumn(string email, string BoardName, int columnOrdinal)
        {
            try
            {
                List<Task> Tasks = bf.GetColumn(email, BoardName, columnOrdinal);
                log.Info($"{email} successfully get the column");
                return JsonSerializer.Serialize(new Response(Tasks));
            }
            catch (Exception ex)
            {
                log.Debug($"{email} Failed to get the column", ex);
                return ReturnJson(new Response((ex.Message), null));

            }
        }
        /// <summary>
        /// Get all tasks in a column of a board based on the email of the user, the name of the board, and the column ordinal.
        /// </summary>
        /// <param name="email">Email of the user</param>
        /// <param name="BoardName">Name of the board</param>
        /// <param name="columnOrdinal">Ordinal of the column</param>
        /// <returns>Returns the list of tasks in the column as a JSON string</returns>

        public string GetColumnIds(string email, string BoardName, int columnOrdinal)
        {
            try
            {
                List<int> Tasksids = bf.GetColumnIds(email, BoardName, columnOrdinal);
                log.Info($"{email} successfully get the column");
                return JsonSerializer.Serialize(new Response(Tasksids));
            }
            catch (Exception ex)
            {
                log.Debug($"{email} Failed to get the column", ex);
                return ReturnJson(new Response((ex.Message), null));

            }
        }
        /// <summary>
        /// Advances a task to the next column on a board.
        /// </summary>
        /// <param name="email">The email of the user performing the action.</param>
        /// <param name="BoardName">The name of the board containing the task.</param>
        /// <param name="columnOrdinal">The ordinal of the column containing the task.</param>
        /// <param name="taskId">The ID of the task to advance.</param>
        /// <returns>A JSON-formatted response indicating whether the task was advanced successfully or if an error occurred.</returns>
        public string AdvanceTask(string email, string BoardName, int columnOrdinal, int taskId)
        {
            try
            {
                bf.AdvanceTask(email, BoardName,columnOrdinal,taskId);
                log.Info($"{email} successfully advance the task");
                var r = ReturnJson(new Response());
                return r;
            }
            catch (Exception ex)
            {
                log.Debug($"{email} Failed to advance the task", ex);
                return ReturnJson(new Response((ex.Message), null));

            }
        }
        /// <summary>
        /// This method deletes a board belonging to a user.
        /// </summary>
        /// <param name="Email">The email address of the user.</param>
        /// <param name="BoardName">The name of the board to delete.</param>
        /// <returns>Returns a Response object with an error message if an error has occurred. Otherwise, the response indicates that the board was deleted successfully.</returns>
        public string DeleteBoard(string email, string BoardName)
        {
            try
            {
                bf.DeleteBoard(email, BoardName);
                log.Info($"{email} successfully delete the board");
                var r = ReturnJson(new Response());
                return r;
            }
            catch (Exception ex)
            {
                log.Debug($"{email} Failed to delete the board", ex);
                return ReturnJson(new Response((ex.Message), null));

            }


        }

        /// <summary>
        /// This method retrieves a board belonging to a user.
        /// </summary>
        /// <param name="Email">The email address of the user.</param>
        /// <param name="BoardName">The name of the board to retrieve.</param>
        /// <returns>Returns a Response object with an error message if an error has occurred. Otherwise, the response contains the requested board.</returns>
        public string GetBoard(string email, string BoardName)
        {
            try
            {
                Board board = bf.GetBoard(email, BoardName);
                log.Info($"{email} successfully get the board");
                Response res = new Response(null, board);
                return ReturnJson(res);
            }
            catch (Exception ex)
            {
                log.Debug($"{email} Failed to get the board", ex);
                return ReturnJson(new Response((ex.Message), null));
            }
        }
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

        public string TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            try
            {
                bf.TransferOwnership(currentOwnerEmail, newOwnerEmail, boardName);
                log.Info("successfully load the boards");
                var r = ReturnJson(new Response());
                return r;
            }
            catch (Exception ex)
            {
                log.Debug($"Failed load the boards", ex);
                return ReturnJson(new Response((ex.Message), null));

            }
        } 

        public string LoadData()
        {
            try
            {
                bf.LoadLData();
                log.Info("successfully load the boards");
                var r = ReturnJson(new Response());
                return r;
            }
            catch (Exception ex)
            {
                log.Debug($"Failed load the boards", ex);
                return ReturnJson(new Response((ex.Message), null));

            }
        }

        public string DeleteData()
        {
            try
            {
                bf.DeleteData();
                log.Info("successfully delete the boards");
                var r = ReturnJson(new Response());
                return r;
            }
            catch (Exception ex)
            {
                log.Debug($"Failed delete the boards", ex);
                return ReturnJson(new Response((ex.Message), null));

            }
        }


        public string GetBoardName(int boardId)
        {
            try
            {
                string name = bf.GetBoardName(boardId);
                log.Info("successfully got board name");
                var r = ReturnJson(new Response(null,name));
                return r;
            }
            catch (Exception ex)
            {
                log.Debug($"Failed to fatch board name ", ex);
                return ReturnJson(new Response((ex.Message), null));

            }
        }


        public string AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            try
            {
                bf.AssignTask(email, boardName,columnOrdinal,taskID,emailAssignee);
                log.Info("successfully got board name");
                var r = ReturnJson(new Response());
                return r;
            }
            catch (Exception ex)
            {
                log.Debug($"Failed to fatch board name ", ex);
                return ReturnJson(new Response((ex.Message), null));

            }

        }


       
        public string LeaveBoard(string email, int boardID)
        {
            try
            {
                bf.LeaveBoard(email,boardID);
                log.Info("successfully got board name");
                var r = ReturnJson(new Response());
                return r;
            }
            catch (Exception ex)
            {
                log.Debug($"Failed to fatch board name ", ex);
                return ReturnJson(new Response((ex.Message), null));

            }
        }

    }
}
