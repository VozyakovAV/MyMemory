using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyMemory.Domain;

namespace MyMemory.BLL
{
    public partial class MemoryManager
    {
        public MemoryGroup FindGroup(int groupId)
        {
            return _groupRepository.GetItems().FirstOrDefault(x => x.Id == groupId);
        }

        public MemoryGroup[] GetGroups()
        {
            return _groupRepository.GetItems().ToArray();
        }

        public MemoryGroup SaveGroup(MemoryGroup group)
        {
            var group2 = _groupRepository.Save(group);
            _uow.Commit();
            return group2;
        }

        public void DeleteGroup(MemoryGroup group)
        {
            _groupRepository.Delete(group);
            _uow.Commit();
        }
    }
}
