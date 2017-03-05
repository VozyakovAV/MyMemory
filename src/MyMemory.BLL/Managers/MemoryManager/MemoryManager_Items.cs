using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyMemory.Domain;

namespace MyMemory.BLL
{
    public partial class MemoryManager
    {
        public MemoryItem[] GetItems()
        {
            return _itemRepository.GetItems()
                .ToArray();
        }

        public MemoryItem[] GetItems(MemoryGroup group)
        {
            return _itemRepository.GetItems()
                .Where(x => x.Group.Id == group.Id)
                .ToArray();
        }

        public MemoryItem SaveItem(MemoryItem item)
        {
            var item2 = _itemRepository.Save(item);
            _uow.Commit();
            return item2;
        }

        public void DeleteItem(MemoryItem item)
        {
            _itemRepository.Delete(item);
            _uow.Commit();
        }

        public void Attach(MemoryItem item)
        {
            _uow.GetItems().Attach(item);
        }
    }
}
