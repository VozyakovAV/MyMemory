using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using MyMemory.Domain;
using Common;

namespace MyMemory.BLL
{
    public class StudyNewTasksManager : IItemManager<MemoryTask>
    {
        private readonly UnitOfWork _uow;
        private readonly ItemRepository _itemRepository;
        private readonly TaskRepository _taskRepository;
        private readonly StepsStudyRepository _stepsStudyRepository;
        private readonly MemoryManager _mng;

        private readonly int _userId;
        private readonly bool _isRandom;

        public StudyNewTasksManager(int userId, bool isRandom)
        {
            this._uow = new UnitOfWork();
            this._itemRepository = new ItemRepository(_uow);
            this._taskRepository = new TaskRepository(_uow);
            this._stepsStudyRepository = new StepsStudyRepository(_uow);
            this._mng = new MemoryManager();
            this._userId = userId;
            this._isRandom = isRandom;
        }

        public MemoryTask GetNextItem()
        {
            // TODO: убрать ToList

            var itemIds = _taskRepository.GetItems()
                .Where(x => x.User.Id == _userId)
                .Select(x => x.Item.Id)
                .ToList();

            var query = _itemRepository.GetItems()
                .Include(x => x.Group)
                .Where(x => !itemIds.Contains(x.Id));

            MemoryItem item = _isRandom
                ? query.OrderBy(x => Guid.NewGuid()).FirstOrDefault()
                : query.OrderBy(x => x.Id).FirstOrDefault();

            if (item == null)
            {
                return null;
            }

            var step = _mng.GetSteps().First();
            var user = _mng.FindUser(_userId);

            var newTask = new MemoryTask(user, item, step.Number, step.NextDateTime());
            _mng.Attach(item);
            _mng.SaveTask(newTask);

            return newTask;
        }

        public void WriteAnswer(MemoryTask task, bool isCorrect)
        { }
    }
}
