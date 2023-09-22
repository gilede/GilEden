using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading;
using log4net;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class UserService
    {
        private static readonly ILog log = LogManager.GetLogger("UserService");
        private UserFacade uf;

        /// <summary>
        /// Constructor for the UserService class.
        /// </summary>
        /// <param name="uf">The UserFacade object that the UserService operates on.</param>
        public UserService(UserFacade uf)
        {
            this.uf = uf;
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
                log.Error("Failed to serialize object");
                return "Error : serializaztion Failed ";
            }
        }

        /// <summary>
        /// Registers a new user to the system.
        /// </summary>
        /// <param name="Email">The user email address, used as the user name for logging into the system.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>Returns a Response object with an error message if an error has occurred. Otherwise, the response indicates that the registration was successful.</returns>
        public string Register(string Email, string password)
        {
            Response res1 = new Response();

            try
            {
                uf.RegisterUser(Email, password);
                log.Info("Register done succesfully");

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res1.ErrorMessage = ex.Message;
            }
            return ReturnJson(res1);


        }
        /// <summary>
        /// Logs a user into the system.
        /// </summary>
        /// <param name="Email">The user email address.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>Returns a Response object with an error message if an error has occurred. Otherwise, the response indicates that the login was successful.</returns>
        public string Login(string Email, string password)
        {
            Response res2 = new Response();
            try
            {
                User user = uf.GetUser(Email);
                user.Login(password);
                log.Info("Logged in succesfully");
                res2.ReturnValue = Email;

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res2.ErrorMessage = ex.Message;

            }
            return ReturnJson(res2);

        }
        /// <summary>
        /// Logs a user out of the system.
        /// </summary>
        /// <param name="Email">The user email address.</param>
        /// <returns>Returns a Response object with an error message if an error has occurred. Otherwise, the response indicates that the logout was successful.</returns>
        public string Logout(string Email)
        {
            Response res3 = new Response();

            try
            {
                if (!uf.CheckUser(Email))
                {
                    log.Warn("The user doesn't exist or not logged in");
                    throw new Exception("The user doesn't exist or not logged in");
                }


                Email = Email.ToLower();
                uf.GetUser(Email).Logout();
                log.Info("Lougout-succesfully");
            }

            catch (Exception ex)
            {
                log.Error(ex.Message);
                res3.ErrorMessage = ex.Message;
            }

            return ReturnJson(res3);

        }
        /// <summary>
        /// This method changes the user's password.
        /// </summary>
        /// <param name="Email">Email of the user. Must be logged in</param>
        /// <param name="oldP">The user's current password</param>
        /// <param name="newP">The user's new password</param>
        /// <returns>A response with an error message (if an error occurs) or null (if successful)</returns>
        public string ChangePassword(string Email, string oldP, string newP)
        {
            Response res4 = new Response();
            try
            {
                if (!uf.CheckUser(Email))
                {

                    throw new Exception("The user doesn't exist or not logged in the system");
                }
                uf.GetUser(Email).ChangePassword(oldP, newP);
                log.Info("password changed succesfully");
                
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res4.ErrorMessage = ex.Message;
            }

            return ReturnJson(res4);
        }

        /// <summary>
        /// This method returns all in-progress tasks of a user.
        /// </summary>
        /// <param name="Email">Email of the user. Must be logged in</param>
        /// <returns>A response with a list of the in-progress tasks of the user, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string gellallCurrentCol(string Email)
        {
            Response<List<Task>> res5 = new Response<List<Task>>();
            try
            {
                if (!uf.CheckUser(Email))
                {
                    throw new Exception("The user doesn't exist or not logged in the system");
                }
                res5.ReturnValue = uf.gellallCurrentCol(Email);
                log.Info("fatched active cols successfully ");
                var options = new JsonSerializerOptions();
                options.WriteIndented = true;
                options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                return JsonSerializer.Serialize(res5, res5.GetType(), options);


            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return ReturnJson(new Response((ex.Message), null));
            }
        }

        public string GetUserBoards(string email)
        {
            Response<List<int>> res5 = new Response<List<int>>();
            try
            {
                if (!uf.CheckUser(email))
                {
                    throw new Exception("The user doesn't exist or not logged in the system");
                }
                res5.ReturnValue = uf.GetUserBoards(email);
                log.Info("fatched active cols successfully ");
                var options = new JsonSerializerOptions();
                options.WriteIndented = true;
                options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                return JsonSerializer.Serialize(res5, res5.GetType(), options);


            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return ReturnJson(new Response((ex.Message), null));
            }
        }

        

        public string LoadData()
        {
            Response res6 = new Response();
            try
            {
                uf.LoadData();
                log.Debug("Data loaded successfully");

            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                res6.ErrorMessage= ex.Message;  

            }
            return ReturnJson(res6);
        }
        public string DeleteData()
        {
            try
            {
                uf.DeleteData();
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


    }
}
