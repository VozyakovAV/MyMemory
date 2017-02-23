using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyMemory.Domain;

namespace MyMemory.BLL
{
    public partial class MemoryManager
    {
        public MemoryUser[] GetUsers()
        {
            return _userRepository.GetItems().ToArray();
        }

        public MemoryUser FindUser(string name)
        {
            return _userRepository.GetItems().FirstOrDefault(x => x.Name == name);
        }

        public void SaveUser(MemoryUser user)
        {
            _userRepository.Save(user);
            _uow.Commit();
        }

        public void DeleteUser(MemoryUser user)
        {
            _userRepository.Delete(user);
            _uow.Commit();
        }
    }
}
