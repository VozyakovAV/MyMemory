using MyMemory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMemory.BLL
{
    public partial class MemoryManager
    {
        private readonly UnitOfWork _uow;
        private readonly GroupRepository _groupRepository;
        private readonly ItemRepository _itemRepository;
        private readonly UserRepository _userRepository;
        private readonly TaskRepository _taskRepository;

        public MemoryManager()
        {
            this._uow = new UnitOfWork();
            this._groupRepository = new GroupRepository(_uow);
            this._itemRepository = new ItemRepository(_uow);
            this._userRepository = new UserRepository(_uow);
            this._taskRepository = new TaskRepository(_uow);
        }
    }
}
