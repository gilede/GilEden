namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class Response
    {
        /// <summary>
        /// The error message if an error occurred, otherwise null.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// The return value of the function, if any.
        /// </summary>
        public object ReturnValue { get; set; }

        /// <summary>
        /// Returns the string representation of the return value.
        /// </summary>


        /// <summary>
        /// Returns true if an error occurred.
        /// </summary>
        public bool ErrorOccured { get => ErrorMessage != null; }

        /// <summary>
        /// Constructs a new instance of the Response class when everything is ok.
        /// </summary>
        public Response()
        {
            // no action needed
        }

        public Response(string msg)
        {
            ReturnValue = msg;
            ErrorMessage = null;
            // no action needed
        }
        /// <summary>
        /// Constructs a new instance of the Response class when an error occurred.
        /// </summary>
        /// <param name="msg">The error message.</param>
        public Response(object res)
        {
            ReturnValue = res;
            ErrorMessage = null;
        }

        /// <summary>
        /// Constructs a new instance of the Response class when an error occurred.
        /// </summary>
        /// <param name="msg">The error message.</param>
        /// <param name="res">The return value of the function.</param>
        public Response(string msg, object res)
        {
            ErrorMessage = msg;
            ReturnValue = res;
        }
    }
}