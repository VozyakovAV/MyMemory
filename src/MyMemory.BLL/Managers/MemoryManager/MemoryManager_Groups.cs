using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyMemory.Domain;

namespace MyMemory.BLL
{
    public partial class MemoryManager
    {
        public MemoryGroup[] GetGroups()
        {
            return _groupRepository.GetItems().ToArray();
        }

        public void SaveGroup(MemoryGroup group)
        {
            _groupRepository.Save(group);
            _uow.Commit();
        }

        public void DeleteGroup(MemoryGroup group)
        {
            _groupRepository.Delete(group);
            _uow.Commit();
        }
    }
}
