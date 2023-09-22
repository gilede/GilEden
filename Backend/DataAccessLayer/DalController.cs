using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using log4net.Config;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public abstract class DalController
    {
        protected readonly string _connectionString;
        protected readonly string _tableName;
        protected readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);



        /// <summary>
        /// Constructs a new instance of the DalController.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        public DalController(string tableName)
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = tableName;

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("start a new DalController log :)");


        }
        /// <summary>
        /// Selects all entries from the table specified in the constructor.
        /// </summary>
        /// <returns>List of DTO objects representing all rows in the table.</returns>
        /// <exception cref="System.Exception">Thrown when there's a problem executing the SQL command.</exception>

        public List<DTO> Select()
        {

            List<DTO> results = new List<DTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {_tableName};";
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
                catch (Exception ex)
                {
                    log.Error(ex.Message);
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
        /// Deletes all entries from the table specified in the constructor.
        /// </summary>
        /// <returns>true if the command executed successfully, false otherwise.</returns>
        /// <exception cref="System.ArgumentException">Thrown when there's a problem executing the SQL command.</exception>
        public bool DeleteAll()
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteDataReader dataReader = null;
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {_tableName}"
                };
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();
                    if (!dataReader.HasRows)
                        return true;
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
            return res > 0;
        }

        /// <summary>
        /// Updates a specific entry in the table specified in the constructor.
        /// </summary>
        /// <param name="id">The id of the entry to update.</param>
        /// <param name="Name">The column name of the value to update.</param>
        /// <param name="Value">The new value to set.</param>
        /// <returns>true if the command executed successfully, false otherwise.</returns>
        /// <exception cref="System.Exception">Thrown when there's a problem executing the SQL command.</exception>
        public virtual bool Update(int id, string Name, string Value)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{Name}]=@{Value} where id={id}"
                };
                try
                {

                    command.Parameters.Add(new SQLiteParameter(Name, Value));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info("update successfully the data base");
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
        /// Converts a SQLiteDataReader to a DTO object.
        /// </summary>
        /// <param name="reader">The SQLiteDataReader object to convert.</param>
        /// <returns>A DTO object that represents the current row in the reader.</returns>
        protected abstract DTO ConvertReaderToObject(SQLiteDataReader reader);

       

    }
}
