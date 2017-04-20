using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using MyMemory.Domain;
using Common;

namespace MyMemory.BLL
{
    public interface IItemManager<T> where T:class
    {
        T GetNextItem();
        void WriteAnswer(T item, bool isCorrect);
    }

    public class RepeatTasksDbManager : IItemManager<MemoryTask>
    {
        private readonly UnitOfWork _uow;
        private readonly TaskRepository _taskRepository;
        private readonly MemoryManager _mng;

        private readonly int _userId;
        private readonly int _groupId;
        private readonly bool _isRandom;

        public RepeatTasksDbManager(int userId, int groupId, bool isRandom)
        {
            this._uow = new UnitOfWork();
            this._taskRepository = new TaskRepository(_uow);
            this._mng = new MemoryManager();
            this._userId = userId;
            this._groupId = groupId;
            this._isRandom = isRandom;
        }

        public MemoryTask GetNextItem()
        {
            var groupsIs = _mng.GetTreeId(_groupId);

            // находим задачи пользователя у которых подошло время
            var queryBase = _taskRepository.GetItems()
                .Include(x => x.Item)
                .Include(x => x.Item.Group)
                .Where(x => x.User.Id == _userId 
                    && groupsIs.Contains(x.Item.Group.Id)
                    && x.Deadline <= CustomDateTime.Now);

            // из найденных задач находим максимальный StepNumber
            var maxStepNumber = queryBase
                .Select(x => x.StepNumber)
                .DefaultIfEmpty(0)
                .Max();

            // отбираем задачи с максимальным StepNumber
            var query = queryBase
                .Include(x => x.Item)
                .Where(x => x.StepNumber == maxStepNumber);

            // берем одно значение из выборки (с рандом или без)
            return _isRandom
                ? query.OrderBy(x => Guid.NewGuid()).FirstOrDefault()
                : query.OrderBy(x => x.Id).FirstOrDefault();
        }

        public void WriteAnswer(MemoryTask task, bool isCorrect)
        {
            var steps = _mng.GetSteps();
            var currentStep = steps.Single(x => x.Number == task.StepNumber);

            var nextStepNumber = isCorrect
                ? currentStep.Number + 1
                : currentStep.Number - 2;

            if (nextStepNumber <= 0)
                nextStepNumber = steps.Min(x => x.Number);

            if (nextStepNumber > steps.Max(x => x.Number))
                nextStepNumber = steps.Max(x => x.Number);

            var nextStep = steps.FirstOrDefault(x => x.Number == nextStepNumber);

            task.StepNumber = nextStep.Number;
            task.Deadline = nextStep.NextDateTime();

            _mng.SaveTask(task);
        }
    }
}
