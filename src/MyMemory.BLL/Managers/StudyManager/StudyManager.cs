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
        private readonly TaskDbManager _mngTasksDb;

        public StudyManager()
        {
            _mng = new MemoryManager();
            _mngTasksDb = new TaskDbManager();
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

            if (data.Question == null)
            {
                data.Message = "Нет вопросов";
            }

            return data;
        }

        public StudyData NextStep(StudyData currentData, string answer)
        {
            var items = _mng.GetItems();
            var data = new StudyData();

            var prevTask = _mng.FindTask(currentData.Question.TaskId);
            var prevItem = prevTask.Item;
            var isPrevCorrect = prevItem.Answer == answer;

            _mngTasksDb.WriteAnswer(prevTask, isPrevCorrect);

            data.Statistic = new StudyStatistic();
            data.PrevAnswer = new StudyAnswer()
            {
                CorrectAnswer = prevItem.Answer,
                IsCorrectAnswer = isPrevCorrect
            };

            data.Question = GetNextQuestion(currentData.GroupId);

            if (data.Question == null)
            {
                data.Message = "Нет вопросов";
            }

            return data;
        }

        private StudyQuestion GetNextQuestion(int groupId)
        {
            var task = _mngTasksDb.GetNextItem();

            if (task != null)
            {
                return new StudyQuestion()
                {
                    TaskId = task.Id,
                    ItemId = task.Item.Id,
                    Text = task.Item.Question
                };
            }

            return null;
        }
    }
}
