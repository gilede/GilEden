using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class ServiceFactory
    {
        private static readonly ILog log = LogManager.GetLogger("Activate ServiceFactory ");
        private UserFacade userFacade;
        private BoardFacade boardFacade;
        private UserService userService;
        private BoardService boardService;
        private TaskService taskService;


        public ServiceFactory()
        {
            userFacade = new UserFacade();
            boardFacade = new BoardFacade(userFacade);
            userService = new UserService(userFacade);
            boardService = new BoardService(boardFacade);
            taskService = new TaskService(boardFacade);
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

        }

        public UserFacade GetUserFacade()
        {
            return userFacade;
        }

        public BoardFacade GetBoardFacade()
        {
            return boardFacade;
        }

        public UserService GetUserService()
        {
            return userService;
        }

        public BoardService GetBoardService()
        {
            return boardService;
        }

        public TaskService GetTaskService()
        {
            return taskService;
        }
        public string LoadData()
        {
            JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };
            Response res;
            string objectAsJson;
            try
            {
                userService.LoadData();
                boardService.LoadData();
            }
            catch (Exception e)
            {
                res = new Response(e.Message, null);
                log.Error(e.Message);
                objectAsJson = JsonSerializer.Serialize(res, options1);
                return objectAsJson;
            }
            res = new Response(null, null);
            objectAsJson = JsonSerializer.Serialize(res, options1);
            return objectAsJson;
        }
        public string DeleteData()
        {
            JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };
            Response res;
            string objectAsJson;
            try
            {
                boardService.DeleteData();
            }
            catch (Exception e)
            {
                res = new Response(e.Message, null);
                log.Error(e.Message);
                objectAsJson = JsonSerializer.Serialize(res, options1);
                return objectAsJson;
            }
            res = new Response(null, null);
            objectAsJson = JsonSerializer.Serialize(res, options1);
            return objectAsJson;
        }
    }


}
