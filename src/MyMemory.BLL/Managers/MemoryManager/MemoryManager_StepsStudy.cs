using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyMemory.Domain;

namespace MyMemory.BLL
{
    public partial class MemoryManager
    {
        public MemoryStepsStudy[] GetSteps()
        {
            return _stepsStudyRepository.GetItems().OrderBy(x => x.Number).ToArray();
        }

        public void SaveStep(MemoryStepsStudy step)
        {
            _stepsStudyRepository.Save(step);
            _uow.Commit();
        }

        public void DeleteStep(MemoryStepsStudy step)
        {
            _stepsStudyRepository.Delete(step);
            _uow.Commit();
        }
    }
}
