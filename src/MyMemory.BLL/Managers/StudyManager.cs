using MyMemory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace MyMemory.BLL
{
    public class StudyManager
    {
        private readonly MemoryManager _mng;

        public StudyManager()
        {
            _mng = new MemoryManager();
        }

        public StudyData NextStep(StudyData currentData, string answer)
        {
            var items = _mng.GetItems();
            var data = new StudyData();

            if (currentData == null)
            {
                var nextItem = items.GetRandom();

                data.Statistic = new StudyStatistic();
                data.Question = new StudyQuestion()
                {
                    Index = nextItem.Id,
                    Text = nextItem.Question
                };
                
            }
            else
            {
                var prevItem = items.FirstOrDefault(x => x.Id == data.Question.Index);
                var nextItem = items.GetRandom();
                data.Statistic = new StudyStatistic();
                data.PrevAnswer = new StudyAnswer()
                {
                    CorrectAnswer = prevItem.Answer,
                    IsCorrectAnswer = prevItem.Answer == answer
                };

                data.Question = new StudyQuestion()
                {
                    Index = nextItem.Id,
                    Text = nextItem.Question
                };
            }

            return data;
        }
    }
}
