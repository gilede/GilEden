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
    public class ColumnDTO : DTO
    {

        internal const string BoardIdColumnName = "BoardId";
        internal const string ColumnOrdinalColumnName = "ColumnOrdinal";
        internal const string ColumnNameColumnName = "ColumnName";
        internal const string ColumnLimitColumnName = "ColumnLimit";
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private int boardId;
        public int BoardId { get => boardId; }
        private int columnOrdinal;
        public int ColumnOrdinal { get => columnOrdinal; }

        private string columnName;
        public string ColumnName { get => columnName; }
        private int columnLimit;
        public int ColumnLimit { get => columnLimit; set { if (IsPersisted) { ((ColumnController)_controller).Update(BoardId, ColumnOrdinal, value); } columnLimit = value; } }

        /// <summary>
        /// Constructs a new instance of the ColumnDTO.
        /// </summary>
        /// <param name="boardId">The id of the board.</param>
        /// <param name="columnOrdinal">The ordinal of the column.</param>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="columnLimit">The limit of the column.</param>
        public ColumnDTO(int boardId, int columnOrdinal, string columnName, int columnLimit) : base(new ColumnController())
        {
            this.boardId = boardId;
            this.columnOrdinal = columnOrdinal;
            this.columnName = columnName;
            this.columnLimit = columnLimit;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting new ColumnDTO log");
        }

        /// <summary>
        /// Inserts the ColumnDTO instance into the database.
        /// </summary>
        /// <exception cref="System.Exception">Thrown when the insertion to the database fails.</exception>
        public void Insert()
        {

            if (!((ColumnController)_controller).Insert(this))
            {
                throw new Exception("Insertion to DataBase failed");
            }
            IsPersisted = true;
            log.Info("Column dto inserted successfully");
        }

        /// <summary>
        /// Deletes the ColumnDTO instance from the database.
        /// </summary>
        /// <exception cref="System.Exception">Thrown when the deletion from the database fails.</exception>
        public void Delete()
        {

            if (!((ColumnController)_controller).Delete(this))
            {
                throw new Exception("Insertion to DataBase failed");
            }
            IsPersisted = true;
            log.Info("Column dto deleted successfully");
        }
    }
}
