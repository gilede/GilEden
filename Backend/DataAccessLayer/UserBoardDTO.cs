using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class UserBoardDTO : DTO
    {
        public const string userColumnName = "emailOwner";
        public const string BoardColumnName = "BoardId";

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly int _BoardId;
        private readonly string _ownerEmail;
        public int boardId { get => _BoardId; }
        public string OwnerEmail { get => _ownerEmail; }


        /// <summary>
        /// Constructs a new instance of the UserBoardDTO.
        /// </summary>
        /// <param name="ownerEmail">The email of the owner.</param>
        /// <param name="BoardID">The ID of the board.</param>
        internal UserBoardDTO(string ownerEmail, int BoardID) : base(new UserBoardController())
        {
            this._BoardId = BoardID;
            this._ownerEmail = ownerEmail;

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting new BoardUsersDTO log!");

        }


        /// <summary>
        /// Inserts the current UserBoardDTO into the database.
        /// Throws an exception if the operation fails.
        /// </summary>
        public void insert()
        {
            UserBoardController userBoard = new UserBoardController();
            if (!userBoard.Insert(this))
                throw new Exception("the creation of the connection between the user and the board in the DB failed");

        }

        /// <summary>
        /// Deletes the current UserBoardDTO from the database.
        /// Throws an exception if the operation fails.
        /// </summary>

        public void delete()
        {
            UserBoardController userBoard = new UserBoardController();
            if (!userBoard.Delete(this.boardId))
                throw new Exception("the creation of the connection between the user and the board in the DB failed");

        }
    }

}
