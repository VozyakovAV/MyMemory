using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using MyMemory.Domain;

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

        public StudyNewTasksManager(int userId)
        {
            this._uow = new UnitOfWork();
            this._itemRepository = new ItemRepository(_uow);
            this._taskRepository = new TaskRepository(_uow);
            this._stepsStudyRepository = new StepsStudyRepository(_uow);
            this._mng = new MemoryManager();
            this._userId = userId;
        }

        public MemoryTask GetNextItem()
        {
            // TODO: убрать ToList

            var itemIds = _taskRepository.GetItems()
                .Where(x => x.User.Id == _userId)
                .Select(x => x.Item.Id)
                .ToList();

            var items = _itemRepository.GetItems()
                .Include(x => x.Group)
                .Where(x => !itemIds.Contains(x.Id))
                .OrderBy(x => x.Id)
                .ToList();

            var item = items.FirstOrDefault();

            var step = _mng.GetSteps().First();
            var user = _mng.FindUser(_userId);

            var newTask = new MemoryTask(user, item, step.Number, AddPeriod(DateTime.Now, step));
            _mng.Attach(item);
            try
            {
                _mng.SaveTask(newTask);
            }
            catch (Exception ex)
            {

            }

            return newTask;
        }

        public void WriteAnswer(MemoryTask task, bool isCorrect)
        { }

        private DateTime AddPeriod(DateTime date, MemoryStepsStudy step)
        {
            switch (step.Format)
            {
                case PeriodFormat.Min:      return date.AddMinutes(step.Period);
                case PeriodFormat.Hour:     return date.AddHours(step.Period);
                case PeriodFormat.Day:      return date.AddDays(step.Period);
                case PeriodFormat.Month:    return date.AddMonths(step.Period);
                case PeriodFormat.Year:     return date.AddYears(step.Period);
            }

            return date;
        }
    }
}
