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
        public MemoryTask FindTask(int taskId)
        {
            return _taskRepository.GetItems()
                .Include(x => x.Item)
                .FirstOrDefault(x => x.Id == taskId);
        }

        public MemoryTask FindTask(int userId, int itemId)
        {
            return _taskRepository.GetItems()
                .Include(x => x.User)
                .Include(x => x.Item)
                .Include(x => x.Item.Group)
                .FirstOrDefault(x => x.User.Id == userId && x.Item.Id == itemId);
        }

        public MemoryTask[] GetTasks()
        {
            return _taskRepository.GetItems().ToArray();
        }

        public MemoryTask SaveTask(MemoryTask task)
        {
            var task2 = _taskRepository.Save(task);
            _uow.Commit();
            return task2;
        }

        public void DeleteTask(MemoryTask task)
        {
            _taskRepository.Delete(task);
            _uow.Commit();
        }

        public void Attach(MemoryTask task)
        {
            _uow.GetTasks().Attach(task);
        }
    }
}
