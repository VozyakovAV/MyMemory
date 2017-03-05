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
        private readonly RepeatTasksManager _mngRepeatTasks;
        private StudyNewTasksManager _mngStudyNewTasks;

        public StudyManager()
        {
            _mng = new MemoryManager();
            _mngRepeatTasks = new RepeatTasksManager();
            
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

            data.UserId = user.Id;
            _mngStudyNewTasks = new StudyNewTasksManager(data.UserId);
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

            _mngRepeatTasks.WriteAnswer(prevTask, isPrevCorrect);

            data.Statistic = new StudyStatistic();
            data.PrevAnswer = new StudyAnswer()
            {
                CorrectAnswer = prevItem.Answer,
                IsCorrectAnswer = isPrevCorrect
            };

            _mngStudyNewTasks = new StudyNewTasksManager(currentData.UserId);
            data.Question = GetNextQuestion(currentData.GroupId);

            if (data.Question == null)
            {
                data.Message = "Нет вопросов";
            }

            return data;
        }

        private StudyQuestion GetNextQuestion(int groupId)
        {
            var task = _mngRepeatTasks.GetNextItem();

            if (task != null)
            {
                return CreateStudyQuestion(task);
            }

            task = _mngStudyNewTasks.GetNextItem();
            if (task != null)
            {
                return CreateStudyQuestion(task);
            }

            return null;
        }

        private StudyQuestion CreateStudyQuestion(MemoryTask task)
        {
            return new StudyQuestion()
            {
                TaskId = task.Id,
                ItemId = task.Item.Id,
                Text = task.Item.Question
            };
        }
    }
}
