using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyMemory.Domain;

namespace MyMemory.BLL
{
    public class ItemManager
    {
        private readonly ItemRepository _itemRepository;
        private readonly UnitOfWork _uow;
        public ItemManager()
        {
            this._uow = new UnitOfWork();
            this._itemRepository = new ItemRepository(_uow);
        }

        public MemoryItem[] GetItems(MemoryGroup group)
        {
            return _itemRepository.GetItems()
                .Where(x => x.Group.Id == group.Id)
                .ToArray();
        }

        public void SaveItem(MemoryItem item)
        {
            _itemRepository.Save(item);
            _uow.Commit();
        }

        public void DeleteItem(MemoryItem item)
        {
            _itemRepository.Delete(item);
            _uow.Commit();
        }
    }
}
