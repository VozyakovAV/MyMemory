using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyMemory.Domain;

namespace MyMemory.BLL
{
    public class GroupManager
    {
        private readonly GroupRepository _groupRepository;
        private readonly UnitOfWork _uow;
        public GroupManager()
        {
            this._uow = new UnitOfWork();
            this._groupRepository = new GroupRepository(_uow);
        }

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
