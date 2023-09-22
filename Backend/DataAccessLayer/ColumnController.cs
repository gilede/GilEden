using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class ColumnController : DalController
    {
        private const string ColumnName = "Columns";
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Constructs a new instance of the ColumnController.
        /// </summary>
        public ColumnController() : base(ColumnName)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting new ColumnController log");
        }

        /// <summary>
        /// Converts a SQLiteDataReader instance to a DTO.
        /// </summary>
        /// <param name="reader">The SQLiteDataReader instance to convert.</param>
        /// <returns>A DTO that represents the SQLiteDataReader instance.</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            ColumnDTO result = new ColumnDTO(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetInt32(3));
            result.IsPersisted = true;
            return result;
        }

        /// <summary>
        /// Inserts a ColumnDTO instance into the database.
        /// </summary>
        /// <param name="column">The ColumnDTO instance to insert.</param>
        /// <returns>True if the operation is successful; otherwise, false.</returns>
        public bool Insert(ColumnDTO column)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {

                    connection.Open();
                    command.CommandText = $"INSERT INTO {ColumnName} ({ColumnDTO.BoardIdColumnName} ,{ColumnDTO.ColumnOrdinalColumnName} ,{ColumnDTO.ColumnNameColumnName} ,{ColumnDTO.ColumnLimitColumnName}) " +
                        $"VALUES (@idVal,@ordinalVal,@nameVal,@limitval);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", column.BoardId);
                    SQLiteParameter ordinalParam = new SQLiteParameter(@"ordinalVal", column.ColumnOrdinal);
                    SQLiteParameter nameVal = new SQLiteParameter(@"nameVal", column.ColumnName);
                    SQLiteParameter limitval = new SQLiteParameter(@"limitval", column.ColumnLimit);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(ordinalParam);
                    command.Parameters.Add(nameVal);
                    command.Parameters.Add(limitval);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                    log.Info("Inserted column successfully");
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
        /// Updates a specific column's limit in the database.
        /// </summary>
        /// <param name="boardId">The id of the board to update.</param>
        /// <param name="columnOrdinal">The ordinal of the column to update.</param>
        /// <param name="newColumnLimit">The new limit for the column.</param>
        /// <returns>True if the operation is successful; otherwise, false.</returns>
        public bool Update(int boardId, int columnOrdinal, int newColumnLimit)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"UPDATE {_tableName} SET {ColumnDTO.ColumnLimitColumnName} = @newColumnLimit WHERE {ColumnDTO.BoardIdColumnName} = @boardId AND {ColumnDTO.ColumnOrdinalColumnName} = @columnOrdinal"
                };

                try
                {

                    command.Parameters.AddWithValue("@newColumnLimit", newColumnLimit);
                    command.Parameters.AddWithValue("@boardId", boardId);
                    command.Parameters.AddWithValue("@columnOrdinal", columnOrdinal);
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
        /// Selects columns by a board id from the database.
        /// </summary>
        /// <param name="boardid">The id of the board to select columns for.</param>
        /// <returns>A list of DTOs that represents the selected columns.</returns>
        public List<DTO> SelectByBoardId(int boardid)
        {
            List<DTO> results = new List<DTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {_tableName} where BoardId ={boardid};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));

                    }
                    log.Info($"Successfully selected all from table {_tableName}");
                }
                catch (Exception e)
                {
                    log.Error(e.ToString());
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
        /// <summary>
        /// Deletes a ColumnDTO instance from the database.
        /// </summary>
        /// <param name="column">The ColumnDTO instance to delete.</param>
        /// <returns>True if the operation is successful; otherwise, false.</returns>
        public bool Delete(ColumnDTO column)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {_tableName} where BoardID={column.BoardId} AND ColumnOrdinal={column.ColumnOrdinal}"
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
        /// Deletes a board by its id from the database.
        /// </summary>
        /// <param name="boardid">The id of the board to delete.</param>
        /// <returns>True if the operation is successful; otherwise, false.</returns>
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
    }
}

