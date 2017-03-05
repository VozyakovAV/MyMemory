using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using MyMemory.Domain;

namespace MyMemory.BLL
{
    public interface IItemManager<T> where T:class
    {
        T GetNextItem();
        void WriteAnswer(T item, bool isCorrect);
    }

    public class RepeatTasksManager : IItemManager<MemoryTask>
    {
        private readonly UnitOfWork _uow;
        private readonly TaskRepository _taskRepository;
        private readonly StepsStudyRepository _stepsStudyRepository;
        private readonly MemoryManager _mng;

        public RepeatTasksManager()
        {
            this._uow = new UnitOfWork();
            this._taskRepository = new TaskRepository(_uow);
            this._stepsStudyRepository = new StepsStudyRepository(_uow);
            this._mng = new MemoryManager();
        }

        public MemoryTask GetNextItem()
        {
            var tasks = _taskRepository.GetItems()
                .Include(x => x.Item)
                .Where(x => x.Deadline <= DateTime.Now)
                .OrderByDescending(x => x.StepNumber);

            return tasks.FirstOrDefault();
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
            task.Deadline = AddPeriod(DateTime.Now, nextStep);

            _mng.SaveTask(task);
        }

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
