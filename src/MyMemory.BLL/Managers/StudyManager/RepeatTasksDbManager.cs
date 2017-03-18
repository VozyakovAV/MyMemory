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
        private readonly StepsStudyRepository _stepsStudyRepository;
        private readonly MemoryManager _mng;

        private readonly int _userId;
        private readonly int _groupId;
        private readonly bool _isRandom;

        public RepeatTasksDbManager(int userId, int groupId, bool isRandom)
        {
            this._uow = new UnitOfWork();
            this._taskRepository = new TaskRepository(_uow);
            this._stepsStudyRepository = new StepsStudyRepository(_uow);
            this._mng = new MemoryManager();
            this._userId = userId;
            this._groupId = groupId;
            this._isRandom = isRandom;
        }

        public MemoryTask GetNextItem()
        {
            var groupsIs = _mng.GetTreeId(_groupId);

            var queryBase = _taskRepository.GetItems()
                .Include(x => x.Item)
                .Include(x => x.Item.Group)
                .Where(x => x.User.Id == _userId 
                    && groupsIs.Contains(x.Item.Group.Id)
                    && x.Deadline <= CustomDateTime.Now);

            var maxStepNumber = queryBase
                .Select(x => x.StepNumber)
                .DefaultIfEmpty(0)
                .Max();

            var query = queryBase
                .Include(x => x.Item)
                .Where(x => x.StepNumber == maxStepNumber);

            return _isRandom
                ? query.OrderBy(x => Guid.NewGuid()).FirstOrDefault()
                : query.OrderBy(x => x.Id).FirstOrDefault();
        }

        public void WriteAnswer(MemoryTask task, bool isCorrect)
        {
            var steps = _mng.GetSteps();
            var currentStep = steps.Single(x => x.Number == task.StepNumber);

            MemoryStepsStudy nextStep = isCorrect
                ? steps.FirstOrDefault(x => x.Number == currentStep.Number + 1)
                : steps.FirstOrDefault(x => x.Number == currentStep.Number - 1);

            if (nextStep == null)
                nextStep = currentStep;

            task.StepNumber = nextStep.Number;
            task.Deadline = nextStep.NextDateTime();

            _mng.SaveTask(task);
        }
    }
}
