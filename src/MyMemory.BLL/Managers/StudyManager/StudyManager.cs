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
            data.Step = GetNextStep();

            VerifyStudyData(data);

            return data;
        }

        public StudyData NextStep(StudyData currentData, string answer)
        {
            _mngStudyNewTasks = new StudyNewTasksManager(currentData.UserId);

            var data = new StudyData();

            data.PrevStep = GetPrevStep(currentData.Step.Question, answer);
            data.Step = GetNextStep();

            VerifyStudyData(data);

            return data;
        }

        private void VerifyStudyData(StudyData data)
        {
            if (data.Step.Question == null)
            {
                data.Message = "Нет вопросов";
            }
        }

        private StudyStep GetNextStep()
        {
            return new StudyStep()
            {
                Question = GetNextQuestion()
            };
        }

        private StudyQuestion GetNextQuestion()
        {
            MemoryTask task = _mngRepeatTasks.GetNextItem();

            if (task == null)
            {
                task = _mngStudyNewTasks.GetNextItem();
            }

            if (task != null)
            {
                return new StudyQuestion()
                {
                    ItemId = task.Item.Id,
                    TaskId = task.Id,
                    Text = task.Item.Question
                };
            }

            return null;
        }

        private StudyStep GetPrevStep(StudyQuestion question, string answer)
        {
            var prevTask = _mng.FindTask(question.TaskId);
            var prevItem = prevTask.Item;
            var isPrevCorrect = prevItem.Answer == answer;

            _mngRepeatTasks.WriteAnswer(prevTask, isPrevCorrect);

            var prevStep = new StudyStep()
            {
                Question = new StudyQuestion()
                {
                    ItemId = question.ItemId,
                    TaskId = question.TaskId,
                    Text = question.Text
                },
                Answer = new StudyAnswer()
                {
                    CorrectAnswer = prevItem.Answer,
                    IsCorrect = isPrevCorrect
                }
            };

            return prevStep;
        }
    }
}
