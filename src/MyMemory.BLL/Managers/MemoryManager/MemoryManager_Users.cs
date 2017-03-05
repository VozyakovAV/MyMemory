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

        public MemoryUser FindUser(int userId)
        {
            return _userRepository.GetItems().FirstOrDefault(x => x.Id == userId);
        }

        public MemoryUser FindUser(string name)
        {
            return _userRepository.GetItems()
                .FirstOrDefault(x => String.Compare(x.Name, name.Trim(), true) == 0);
        }

        public MemoryUser SaveUser(MemoryUser user)
        {
            var user2 = _userRepository.Save(user);
            _uow.Commit();
            return user2;
        }

        public void DeleteUser(MemoryUser user)
        {
            _userRepository.Delete(user);
            _uow.Commit();
        }
    }
}
