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

        public StudyData Start(string userName, int groupId)
        {
            var data = new StudyData();
            data.Statistic = new StudyStatistic();
            var user = _mng.FindUser(userName);

            if (user == null)
            {
                data.Message = string.Format("Не найден пользователь: {0}", userName);
                return data;
            }

            /*var group = _mng.FindGroup(groupId);
            if (group == null)
            {
                data.Error = string.Format("Не найдена группа: {0}", groupId);
                return data;
            }*/

            data.Statistic = new StudyStatistic();
            data.Question = GetNextQuestion(groupId);

            return data;
        }

        public StudyData NextStep(StudyData currentData, string answer)
        {
            var items = _mng.GetItems();
            var data = new StudyData();

            var prevItem = items.FirstOrDefault(x => x.Id == currentData.Question.Index);
            var nextItem = items.GetRandom();
            data.Statistic = new StudyStatistic();
            data.PrevAnswer = new StudyAnswer()
            {
                CorrectAnswer = prevItem.Answer,
                IsCorrectAnswer = prevItem.Answer == answer
            };

            data.Question = GetNextQuestion(currentData.GroupId);

            return data;
        }

        private StudyQuestion GetNextQuestion(int groupId)
        {
            var items = _mng.GetItems();
            var item = items.GetRandom();
            var res = new StudyQuestion()
            {
                Index = item.Id,
                Text = item.Question
            };
            return res;
        }
    }
}
