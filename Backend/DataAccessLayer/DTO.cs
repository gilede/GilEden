using System;


namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public abstract class DTO
    {
        protected DalController _controller;
        public DalController _Controller { get => _controller; set { _controller = value; } }
        internal bool IsPersisted { get; set; } = false;

        public DTO(DalController controller)
        {
            _Controller = controller;
        }
    }
}