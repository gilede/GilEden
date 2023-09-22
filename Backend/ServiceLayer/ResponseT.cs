using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class Response<T>
    {
        public string ErrorMessage { get; set; }

        public T ReturnValue { get; set; }
        public Response() { }
        public bool ErrorOccured { get => ErrorMessage != null; }

        public Response(string msg, T returnValue)
        {
            ErrorMessage = msg;
            this.ReturnValue = returnValue;
        }
    }
}
