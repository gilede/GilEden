using log4net;
using log4net.Config;
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
    internal class UserBoardController : DalController
    {
        private const string UserBoardTableName = "UserBoard";
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Constructs a new instance of the UserBoardController.
        /// </summary>
        internal UserBoardController() : base(UserBoardTableName) {


            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting new BoardUsersDTO log!");
        }


        /// <summary>
        /// Inserts a new UserBoardDTO into the database.
        /// </summary>
        /// <param name="userBoard">The UserBoardDTO to insert into the database.</param>
        /// <returns>True if the insertion was successful, false otherwise.</returns>
        public bool Insert(UserBoardDTO userBoard)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {UserBoardTableName} ({UserBoardDTO.userColumnName} ,{UserBoardDTO.BoardColumnName}) " +
                        $"VALUES (@userVal,@boardVal);";

                    SQLiteParameter userParam = new SQLiteParameter(@"userVal", userBoard.OwnerEmail);
                    SQLiteParameter BoardParam = new SQLiteParameter(@"boardVal", userBoard.boardId);

                    command.Parameters.Add(userParam);
                    command.Parameters.Add(BoardParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                    log.Info($"Successfully inserted new board member to the db ");
                }
                catch (Exception ex)
                {
                    log.Debug(ex.Message);
                    throw new Exception(ex.Message);
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
        /// Converts an SQLiteDataReader to a UserBoardDTO object.
        /// </summary>
        /// <param name="reader">The SQLiteDataReader to convert.</param>
        /// <returns>A UserBoardDTO object that represents the current row the reader is on.</returns>
        protected override UserBoardDTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            UserBoardDTO result = new UserBoardDTO(reader.GetString(0), reader.GetInt32(1));
            return result;
        }


        /// <summary>
        /// Retrieves all UserBoardDTOs with the given board id.
        /// </summary>
        /// <param name="BoardID">The id of the board to retrieve UserBoardDTOs for.</param>
        /// <returns>A list of UserBoardDTOs associated with the given board id.</returns>
        internal List<UserBoardDTO> GetUsers(int BoardID)
        {
            List<UserBoardDTO> results = new List<UserBoardDTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {UserBoardTableName} where BoardID = {BoardID}";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));

                    }
                }
                catch (Exception ex)
                {
                    log.Debug(ex.Message);
                    throw new Exception(ex.Message);
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

        /// <summary>
        /// Deletes a UserBoardDTO from the database with the given board id.
        /// </summary>
        /// <param name="boardid">The id of the board to delete.</param>
        /// <returns>True if the deletion was successful, false otherwise.</returns>
        internal bool Delete(int boardid)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {_tableName} where BoardId={boardid}"
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
        /// Retrieves all UserBoardDTOs with the given email.
        /// </summary>
        /// <param name="email">The email to retrieve UserBoardDTOs for.</param>
        /// <returns>A list of UserBoardDTOs associated with the given email.</returns>

        internal List<UserBoardDTO> GetBoards(string email)
        {
            List<UserBoardDTO> results = new List<UserBoardDTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {UserBoardTableName} where emailOwner = {email}";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));

                    }
                }
                catch (Exception ex)
                {
                    log.Debug(ex.Message);
                    throw new Exception(ex.Message);
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

        /// <summary>
        /// Loads all UserBoardDTOs from the database.
        /// </summary>
        /// <returns>A list of all UserBoardDTOs currently stored in the database.</returns>
        public List<UserBoardDTO> LoadBoardMembers()
        {
            List<UserBoardDTO> result = Select().Cast<UserBoardDTO>().ToList();
            log.Info("Successfully loaded all board members");
            return result;
        }


    }

}