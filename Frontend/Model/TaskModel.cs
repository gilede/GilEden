using IntroSE.Kanban.Backend.ServiceLayer;

namespace Frontend.Model
{
    public class TaskModel : NotifiableModelObject
    {
        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                this._id = value;
                RaisePropertyChanged("Id");
            }
        }
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                this._title = value;
                RaisePropertyChanged("Title");
            }
        }
        private string _descreption;
        public string Descreption
        {
            get => _descreption;
            set
            {
                this._descreption = value;
                RaisePropertyChanged("Descreption");
            }
        }
        public TaskModel(BackendController controller, int id, string title, string descreption) : base(controller)
        {
            _id = id;
            _title = title;
            _descreption = descreption;
        }
        public TaskModel(BackendController controller, (int Id, string Title, string description) task) : this(controller, task.Id, task.Title, task.description) { }

    }
}
