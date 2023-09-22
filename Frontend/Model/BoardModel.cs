using IntroSE.Kanban.Backend.BusinessLayer;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Frontend.Model
{
    public class BoardModel : NotifiableModelObject
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                this._name = value;
                RaisePropertyChanged("Name");
            }
        }
        private int _id;
        public int ID
        {
            get => _id;
            set
            {
                this._id = value;
                RaisePropertyChanged("ID");
            }
        }
        internal readonly UserModel user;
        public ObservableCollection<TaskModel> Tasks_backlog { get; set; }
        public ObservableCollection<TaskModel> Tasks_inProgress { get; set; }
        public ObservableCollection<TaskModel> Tasks_done { get; set; }
        public BoardModel(BackendController controller, string name, int id, UserModel user) : base(controller)
        {
            this.user = user;
            this._name = name;
            this._id = id;

            Tasks_backlog = new ObservableCollection<TaskModel>(controller.GetColumnIds(user.Email, name, 0).
            Select((c, i) => new TaskModel(controller, controller.GetTask(user.Email, name, 0, c))).ToList());

            Tasks_inProgress = new ObservableCollection<TaskModel>(controller.GetColumnIds(user.Email, name, 1).

            Select((d, i) => new TaskModel(controller, controller.GetTask(user.Email, name, 1,d))).ToList());

            Tasks_done = new ObservableCollection<TaskModel>(controller.GetColumnIds(user.Email, name, 2).
            Select((e, i) => new TaskModel(controller, controller.GetTask(user.Email, name, 2, e))).ToList());
        }
        public BoardModel(BackendController controller, (string name, int id) board, UserModel user) : this(controller, board.name, board.id, user) { }



    }
}
