using log4net;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using System.Net.Http.Headers;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class UserController : DalController
    {
        private const string UserTableName = "Users";
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Constructs a new instance of the UserController.
        /// </summary>
        public UserController() : base(UserTableName)
        {

           

        }


        /// <summary>
        /// Updates the user's login status in the database.
        /// Returns true if the operation is successful.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="loginStatusCol">The column name of the login status.</param>
        /// <param name="value">The new login status.</param>

        public bool Update(string email,string loginStatusCol, bool value) 
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"UPDATE {UserTableName} SET {loginStatusCol}=@loginStatusVal WHERE {UserDTO.EMAIL_COLUMN}=@emailVal";
                try
                {
                    SQLiteParameter emailVal = new SQLiteParameter("@emailVal", email);
                    SQLiteParameter loginStatusVal = new SQLiteParameter("@loginStatusVal", value ? 1 : 0);
                    command.Parameters.Add(emailVal);
                    command.Parameters.Add(loginStatusVal);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                    log.Info("field is updated successfully");
                }
                catch (Exception e)
                {
                    log.Error(e.ToString());
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
        /// Inserts a new user into the database.
        /// Returns true if the operation is successful.
        /// </summary>
        /// <param name="userDTO">The data transfer object of the user.</param>
        /// <param name="emailCol">The column name of the email.</param>
        /// <param name="passwordCol">The column name of the password.</param>
        /// <param name="loginStatusCol">The column name of the login status.</param>
        public bool InsertUser(UserDTO userDTO, string emailCol, string passwordCol, string loginStatusCol)
        {
            using( var connection=new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {

                    connection.Open();
                    command.CommandText = $"INSERT INTO {_tableName} ({emailCol} , {passwordCol}, {loginStatusCol}) " +
                        $"VALUES (@emailVal,@passwordVal,{userDTO._loginStatus});";
                    SQLiteParameter emailVal = new SQLiteParameter("@emailVal", userDTO._Email);
                    SQLiteParameter passwordVal = new SQLiteParameter("@passwordVal", userDTO._Password);

                    command.Parameters.Add(emailVal);
                    command.Parameters.Add(passwordVal);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                    log.Info("Updated field successfully");


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
                return res > 0;
            }
        }





        /// <summary>
        /// Loads all users from the database.
        /// Returns a list of data transfer objects of the users.
        /// </summary>
        public List<UserDTO> LoadUsers()
        {
            List<UserDTO> result = Select().Cast<UserDTO>().ToList();
            return result;
        }


        /// <summary>
        /// Converts a SQLiteDataReader to a data transfer object of a user.
        /// Returns the data transfer object.
        /// </summary>
        /// <param name="reader">The SQLiteDataReader.</param>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            // Check if reader at index 2 is DBNull, if so, assign a default value
            bool readerValue = !reader.IsDBNull(2) && reader.GetInt32(2) == 1;

            // Check if reader at index 0 and 1 are DBNull, if so, assign default values
            string email = !reader.IsDBNull(0) ? reader.GetString(0) : string.Empty;
            string password = !reader.IsDBNull(1) ? reader.GetString(1) : string.Empty;

            UserDTO result = new UserDTO(email, password, readerValue);
            log.Debug("Pulled info successfully");
            return result;
        }
    }

}
