using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyMemory.Domain;

namespace MyMemory.BLL
{
    public partial class MemoryManager
    {
        public MemoryTask[] GetTasks()
        {
            return _taskRepository.GetItems().ToArray();
        }

        public void SaveTask(MemoryTask task)
        {
            _taskRepository.Save(task);
            _uow.Commit();
        }

        public void DeleteTask(MemoryTask task)
        {
            _taskRepository.Delete(task);
            _uow.Commit();
        }
    }
}
