using MyMemory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMemory.BLL
{
    public partial class MemoryManager
    {
        private readonly GroupRepository _groupRepository;
        private readonly ItemRepository _itemRepository;
        private readonly UnitOfWork _uow;

        public MemoryManager()
        {
            this._uow = new UnitOfWork();
            this._groupRepository = new GroupRepository(_uow);
            this._itemRepository = new ItemRepository(_uow);
        }
    }
}
