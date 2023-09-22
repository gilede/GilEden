using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Frontend.Model
{
    public class BoardControllerModel : NotifiableModelObject
    {
        private readonly UserModel user;
        public ObservableCollection<BoardModel> Boards { get; set; }

        public BoardControllerModel(BackendController controller, UserModel user) : base(controller)
        {
            this.user = user;
            Boards = new ObservableCollection<BoardModel>(controller.GetAllBoardsIds(user.Email).
                Select((c, i) => new BoardModel(controller, controller.GetBoard(i), user)).ToList());

        }
    }
}
