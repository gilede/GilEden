using IntroSE.Kanban.Backend.BusinessLayer;
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

    public class BoardDTO : DTO
    {
        internal const string OwnerEmailColumnName = "OwnerEmail";
        internal const string BoardNameColumnName = "BoardName";
        internal const string BoardIdColumnName = "BoardId";
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private int boardId;
        public int BoardId { get => boardId; }
        private string boardName;
        public string BoardName { get => boardName; }
        private string ownerEmail;
        public string OwnerEmail { get => ownerEmail; set { if (IsPersisted) { _controller.Update(BoardId, OwnerEmailColumnName, value); } ownerEmail = value; } }

        /// <summary>
        /// Constructor for BoardDTO.
        /// </summary>
        /// <param name="boardId">The id of the board.</param>
        /// <param name="boardName">The name of the board.</param>
        /// <param name="OwnerEmail">The owner's email of the board.</param>
        public BoardDTO(int boardId, string boardName, string OwnerEmail) : base(new BoardController())
        {
            this.boardId = boardId;
            this.boardName = boardName;
            this.ownerEmail = OwnerEmail;

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting new BoardDTO log");
        }

        /// <summary>
        /// Inserts a new BoardDTO to the database.
        /// </summary>
        /// <exception cref="Exception">Thrown when the insertion operation to the database fails.</exception>
        public void Insert()
        {

            if (!((BoardController)_controller).Insert(this))
            {
                throw new Exception("Insertion to DataBase failed");
            }
            IsPersisted = true;
            log.Info("Successfully inserted the board to the db");
        }


        /// <summary>
        /// Deletes the BoardDTO from the database.
        /// </summary>
        /// <exception cref="Exception">Thrown when the deletion operation from the database fails.</exception>
        internal void Delete()
        {
            if (!new UserBoardController().Delete(this.boardId))
            {
                throw new Exception("Delete to DataBase failed");
            }
            if (!((BoardController)_controller).Delete(this))
            {
                throw new Exception("Delete to DataBase failed");
            }

            log.Info($"Successfully deleted board from db");

        }


        /// <summary>
        /// Retrieves the columns associated with the board from the database.
        /// </summary>
        /// <returns>Returns a list of DTO objects representing the columns of the board.</returns>
        /// 
        internal List<DTO> SelectBoardColumns()
        {
            return new ColumnController().SelectByBoardId(this.BoardId);
        }

    }
}
