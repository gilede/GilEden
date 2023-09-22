using IntroSE.Kanban.Backend.BusinessLayer;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Config;
using System.IO;
using System.Reflection;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class BoardController : DalController
    {
        private const string BoardsName = "Boards";
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public BoardController() : base(BoardsName)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting new BoardController DTO log :) ");
        }


        /// <summary>
        /// Converts a SQLite data reader object into a Board Data Transfer Object (DTO).
        /// </summary>
        /// <param name="reader">SQLiteDataReader object.</param>
        /// <returns>Returns a new Board DTO object with values from the data reader.</returns>
        /// 
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            BoardDTO result = new BoardDTO(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
            result.IsPersisted = true;
            return result;
        }


        /// <summary>
        /// Deletes a board from the database.
        /// This method is currently not implemented.
        /// </summary>
        /// <param name="board">The board to delete.</param>
        /// <returns>Returns a string message indicating that the method is not implemented.</returns>
        public string deleteBoard(BoardDTO board)
        {
            return "Method not implemented yes";

        }

        /// <summary>
        /// Inserts a new board into the database.
        /// </summary>
        /// <param name="board">The board to insert.</param>
        /// <returns>Returns a boolean indicating whether the operation was successful.</returns>
        public bool Insert(BoardDTO board)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {

                    connection.Open();
                    command.CommandText = $"INSERT INTO {BoardsName} ({BoardDTO.BoardIdColumnName} ,{BoardDTO.BoardNameColumnName} ,{BoardDTO.OwnerEmailColumnName}) " +
                        $"VALUES (@idVal,@nameVal,@ownerVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", board.BoardId);
                    SQLiteParameter titleParam = new SQLiteParameter(@"nameVal", board.BoardName);
                    SQLiteParameter ownerParam = new SQLiteParameter(@"ownerVal", board.OwnerEmail);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(ownerParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                    log.Info($"successfully insert board to DB (board id: {board.BoardId})");
                }
                catch (Exception ex)
                {
                    log.Error(ex.ToString()); ;
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
        /// Deletes a specific board from the database.
        /// </summary>
        /// <param name="board">The board to delete.</param>
        /// <returns>Returns a boolean indicating whether the operation was successful.</returns>
        internal bool Delete(BoardDTO board)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {BoardsName} where BoardId={board.BoardId}"
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
        /// Updates a specific field of a board in the database.
        /// </summary>
        /// <param name="id">The ID of the board to update.</param>
        /// <param name="Name">The name of the field to update.</param>
        /// <param name="Value">The new value for the field.</param>
        /// <returns>Returns a boolean indicating whether the operation was successful.</returns>
        public override bool Update(int id, string Name, string Value)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"UPDATE {_tableName} SET [{Name}] = @Name1 WHERE BoardId = @id"
                };
                try
                {

                    command.Parameters.Add(new SQLiteParameter($"@Name1", Value));
                    command.Parameters.Add(new SQLiteParameter("@id", id));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info("Successfully updated the database");
                }
                catch (Exception e)
                {
                    log.Debug($"couldn't update the data in table {_tableName}", e);
                    throw new Exception($"failed in updating the DTO in table {_tableName} - " + e.Message);
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
        /// Deletes all boards from the database.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the operation was successful.</returns>
        public bool DeleteAllBoards()
        {
            bool res1 = false;
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteDataReader dataReader = null;
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"Delete from {_tableName}"
                };
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();
                    if (!dataReader.HasRows)
                        res1 = true;
                    res = command.ExecuteNonQuery();
                    log.Info($"Successfully deleted all from table {_tableName}");
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
            return res1;
        }
    }
}
