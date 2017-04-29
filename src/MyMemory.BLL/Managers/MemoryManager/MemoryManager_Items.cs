using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
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
            var groupsId = GetTreeId(group.Id);

            return _itemRepository.GetItems()
                .Include(x => x.Group)
                .Where(x => groupsId.Contains(x.Group.Id))
                .ToArray();
        }

        public MemoryItem GetItem(int id)
        {
            return _itemRepository.GetItems().FirstOrDefault(x => x.Id == id);
        }

        public MemoryItem[] GetItemsRandom(MemoryGroup group, int count)
        {
            var groupsId = GetTreeId(group.Id);

            return _itemRepository.GetItems()
                .Include(x => x.Group)
                .Where(x => groupsId.Contains(x.Group.Id))
                .OrderBy(x => Guid.NewGuid())
                .Take(count)
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
