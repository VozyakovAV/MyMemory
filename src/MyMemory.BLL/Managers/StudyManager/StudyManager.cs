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
        private RepeatTasksDbManager _mngRepeatTasks;
        private StudyNewTasksManager _mngStudyNewTasks;

        public bool IsRandom { get; set; }

        public StudyManager()
        {
            _mng = new MemoryManager();
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
            data.GroupId = groupId;

            Init(data);
            data.Step = GetNextStep();

            VerifyStudyData(data);

            return data;
        }
        
        public StudyData NextStep(StudyData currentData, string answer)
        {
            var data = new StudyData();
            data.UserId = currentData.UserId;
            data.GroupId = currentData.GroupId;

            Init(currentData);
            data.PrevStep = GetPrevStep(currentData.Step.Question, answer);

            if (data.PrevStep.Answer.IsCorrect)
            {
                data.Step = GetNextStep();
            }
            else
            {
                data.Step = GetRepeatStep(data.PrevStep.Question);
            }

            VerifyStudyData(data);

            return data;
        }

        private void Init(StudyData currentData)
        {
            _mngRepeatTasks = new RepeatTasksDbManager();
            _mngStudyNewTasks = new StudyNewTasksManager(currentData.UserId, IsRandom);
        }

        private void VerifyStudyData(StudyData data)
        {
            if (data.Step == null || data.Step.Question == null)
            {
                data.Message = "Нет вопросов";
            }
        }

        private StudyStep GetNextStep()
        {
            MemoryTask task = _mngRepeatTasks.GetNextItem();

            if (task == null)
                task = _mngStudyNewTasks.GetNextItem();

            if (task == null)
                return null;

            return new StudyStep()
            {
                Question = new StudyQuestion()
                {
                    ItemId = task.Item.Id,
                    TaskId = task.Id,
                    Text = task.Item.Question
                }
            };
        }

        private StudyStep GetRepeatStep(StudyQuestion question)
        {
            return new StudyStep()
            {
                Question = new StudyQuestion()
                {
                    ItemId = question.ItemId,
                    TaskId = question.TaskId,
                    Text = question.Text,
                    IsRepeat = true
                }
            };
        }

        private StudyStep GetPrevStep(StudyQuestion question, string answer)
        {
            var prevTask = _mng.FindTask(question.TaskId);
            var prevItem = prevTask.Item;
            var isPrevCorrect = prevItem.Answer == answer;

            if (!question.IsRepeat)
            {
                _mngRepeatTasks.WriteAnswer(prevTask, isPrevCorrect);
            }

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
