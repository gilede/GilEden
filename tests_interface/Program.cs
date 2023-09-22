using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using BackendTesting;
using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;

class Program
{
    static void Main(string[] args)
    {
     

        UserFacade uf = new UserFacade();
        uf.LoadData();



        uf.RegisterUser("asher9485@gmail.com", "As23443");
        uf.RegisterUser("asher9685@gmail.com", "As23443");
        uf.RegisterUser("asher9885@gmail.com", "As23443");
        uf.RegisterUser("asher9085@gmail.com", "As23443");
     

        ServiceFactory service = new ServiceFactory();
        service.DeleteData();
        service.GetUserService().Register("gileden@gmail.com", "Aa1234567");
        service.GetUserService().Register("ahser4587@gmail.com", "Aa123456");
        service.GetBoardService().CreateBoard("ahser4587@gmail.com", "wow");
        service.GetBoardService().JoinBoard("gileden@gmail.com", 0);

    }
}
