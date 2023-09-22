using log4net.Config;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class TaskControllerDTO : DalController
    {
        private const string taskTableName = "Tasks";
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Constructs a new instance of the TaskControllerDTO.
        /// </summary>
        public TaskControllerDTO() : base(taskTableName)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting new TaskControllerDTO log!");
        }


        /// <summary>
        /// Converts a SQLiteDataReader to a TaskDTO object.
        /// </summary>
        /// <param name="reader">The SQLiteDataReader object to convert.</param>
        /// <returns>A TaskDTO object that represents the current row in the reader.</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            TaskDTO result = new TaskDTO(reader.GetInt32(0), DateTime.Parse(reader.GetString(1)), reader.GetString(2), reader.GetString(3), DateTime.Parse(reader.GetString(4)), reader.GetInt32(5), reader.GetInt32(6));
            log.Info("successfully convert reader to TaskDTO object");
            return result;
        }


        /// <summary>
        /// Updates a specific attribute of a Task in the Tasks table.
        /// </summary>
        /// <param name="boardId">The id of the board.</param>
        /// <param name="taskId">The id of the task.</param>
        /// <param name="attributeName">The name of the attribute to update.</param>
        /// <param name="attributeValue">The new value for the attribute.</param>
        /// <returns>true if the command executed successfully, false otherwise.</returns>

        public bool Update(int boardId, int taskId, string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {taskTableName} set [{attributeName}]=@{attributeName} where Id={taskId} AND AssignedBoard={boardId} "
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info("Successfully update task");
                }
                catch (Exception e)
                {
                    log.Error(e.ToString());
                    throw new ArgumentException(e.ToString());
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }
        public bool Update(int boardId, int taskId, string attributeName, int attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {taskTableName} set [{attributeName}]=@{attributeName} where Id={taskId} AND AssignedBoard={boardId} "
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info("Successfully update task");
                }
                catch (Exception e)
                {
                    log.Error(e.ToString());
                    throw new ArgumentException(e.ToString());
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }

        public bool Update(int boardId, int taskId, string attributeName, DateTime attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {taskTableName} set [{attributeName}]=@{attributeName} where Id={taskId} AND AssignedBoard={boardId}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info("Successfully update task");
                }
                catch (Exception e)
                {
                    log.Error(e.ToString());
                    throw new ArgumentException(e.ToString());
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }

        /// <summary>
        /// Inserts a new Task into the Tasks table.
        /// </summary>
        /// <param name="newTask">The TaskDTO object to insert.</param>
        /// <returns>true if the command executed successfully, false otherwise.</returns>
        public bool Insert(TaskDTO newTask)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {taskTableName} ({TaskDTO.TaskIdColumnName}, {TaskDTO.TaskCreationTimeColumnName}, {TaskDTO.TaskTitleColumnName}, {TaskDTO.TaskDescriptionColumnName}, {TaskDTO.TaskDueDateColumnName}, {TaskDTO.TaskBoardIDColumnName},{TaskDTO.TaskColumnOrdinalName}) " +
                        $"VALUES (@taskIdVal,@creationTimeVal,@titleVal,@descriptionVal,@dueDateVal,@assignedBoard,@columnordi);";


                    SQLiteParameter taskIdParam = new SQLiteParameter(@"taskIdVal", newTask.TaskId);
                    SQLiteParameter creationTimeParam = new SQLiteParameter(@"creationTimeVal", newTask.CreationTime);
                    SQLiteParameter titleParam = new SQLiteParameter(@"titleVal", newTask.Title);
                    SQLiteParameter descriptionParam = new SQLiteParameter(@"descriptionVal", newTask.Description);
                    SQLiteParameter dueDateParam = new SQLiteParameter(@"dueDateVal", newTask.DueDate);
                    SQLiteParameter boardIdParam = new SQLiteParameter(@"assignedBoard", newTask.AssigedBoard);
                    SQLiteParameter ColumnOrdinalParam = new SQLiteParameter(@"columnordi", newTask.ColumnOrdinal);
                    command.Parameters.Add(taskIdParam);
                    command.Parameters.Add(creationTimeParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(descriptionParam);
                    command.Parameters.Add(dueDateParam);
                    command.Parameters.Add(boardIdParam);
                    command.Parameters.Add(ColumnOrdinalParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        /// <summary>
        /// Deletes a specific Task from the Tasks table.
        /// </summary>
        /// <param name="task">The TaskDTO object to delete.</param>
        /// <returns>true if the command executed successfully, false otherwise.</returns>
        public bool Delete(TaskDTO task)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {_tableName} where BoardID={task.AssigedBoard} AND TaskID={task.TaskId}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw new ArgumentException(e.ToString());
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }
        /// <summary>
        /// Deletes all Tasks associated with a specific board from the Tasks table.
        /// </summary>
        /// <param name="boardid">The id of the board.</param>
        /// <returns>true if the command executed successfully, false otherwise.</returns>
        internal bool Delete(int boardid)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {_tableName} where AssignedBoard={boardid}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info("delete successfully the DTO object");
                }
                catch (Exception e)
                {
                    log.Debug($"couldn't delete the DTO object from table {_tableName}", e);
                    throw new Exception($"failed in deleting the DTO from table {_tableName} - " + e.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }

            return res > 0;
        }


        /// <summary>
        /// Loads all Tasks associated with a specific board from the Tasks table.
        /// </summary>
        /// <param name="boardId">The id of the board.</param>
        /// <returns>A list of TaskDTO objects representing all tasks associated with the board.</returns>
        public List<TaskDTO> LoadBoardTasks(int boardId)
        {
            List<TaskDTO> results = new List<TaskDTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {_tableName} where {TaskDTO.TaskBoardIDColumnName} = {boardId};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add((TaskDTO)ConvertReaderToObject(dataReader));

                    }
                }
                catch (Exception e)
                {
                    throw new ArgumentException(e.ToString());
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }
                    command.Dispose();
                    connection.Close();
                }
            }
            return results;
        }




    }
}