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

            if (groupId != 0)
            {
                var group = _mng.FindGroup(groupId);
                if (group == null)
                {
                    data.Message = string.Format("Не найдена группа: {0}", groupId);
                    return data;
                }
            }

            data.UserId = user.Id;
            data.GroupId = groupId;

            Init(data);
            data.Step = GetNextStep();

            VerifyStudyData(data);

            return data;
        }
        
        public StudyData NextStep(StudyData currentData, string answer)
        {
            var data = new StudyData()
            {
                UserId = currentData.UserId,
                GroupId = currentData.GroupId,

                Statistic = new StudyStatistic()
                {
                    NumberOfCorrect = currentData.Statistic.NumberOfCorrect,
                    NumberOfIncorrect = currentData.Statistic.NumberOfIncorrect,
                },
            };

            Init(currentData);
            data.PrevStep = GetPrevStep(currentData.Step.Question, answer);

            if (data.PrevStep.Answer.IsCorrect)
            {
                data.Step = GetNextStep();

                if (!currentData.Step.Question.IsRepeat)
                {
                    data.Statistic.NumberOfCorrect++;
                }
            }
            else
            {
                data.Step = GetRepeatStep(data.PrevStep);

                if (!currentData.Step.Question.IsRepeat)
                {
                    data.Statistic.NumberOfIncorrect++;
                }
            }

            VerifyStudyData(data);

            return data;
        }

        private void Init(StudyData currentData)
        {
            _mngRepeatTasks = new RepeatTasksDbManager(currentData.UserId, currentData.GroupId, IsRandom);
            _mngStudyNewTasks = new StudyNewTasksManager(currentData.UserId, currentData.GroupId, IsRandom);
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

            var variants = GenerateVariants(task.Item).ToArray();

            return new StudyStep()
            {
                Question = new StudyQuestion()
                {
                    ItemId = task.Item.Id,
                    TaskId = task.Id,
                    Text = task.Item.Question,
                    StepNumber = task.StepNumber,
                    GroupVariants = variants,
                    GroupName = task.Item.Group.Name,
                    Type = task.StepNumber < 5 ? StudyQuestionType.TestWords : StudyQuestionType.TestLetters,
                },
                Answer = new StudyAnswer()
                {
                    CorrectAnswerMD5 = Crypto.MD5Hash(task.Item.Answer),
                }
            };
        }

        private StudyStep GetRepeatStep(StudyStep prevStep)
        {
            return new StudyStep()
            {
                Question = new StudyQuestion()
                {
                    ItemId = prevStep.Question.ItemId,
                    TaskId = prevStep.Question.TaskId,
                    Text = prevStep.Question.Text,
                    StepNumber = prevStep.Question.StepNumber,
                    GroupVariants = prevStep.Question.GroupVariants, // TODO: надо скопировать
                    GroupName = prevStep.Question.GroupName,
                    Type = prevStep.Question.Type,
                    IsRepeat = true
                },
                Answer = new StudyAnswer()
                {
                    CorrectAnswerMD5 = prevStep.Answer.CorrectAnswerMD5,
                }
            };
        }

        private StudyStep GetPrevStep(StudyQuestion question, string answer)
        {
            var prevTask = _mng.FindTask(question.TaskId);
            var prevItem = prevTask.Item;
            var isPrevCorrect = IsCorrectAnswer(answer, prevItem.Answer);

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
                    StepNumber = question.StepNumber,
                    Text = question.Text,
                    GroupVariants = question.GroupVariants,
                    GroupName = question.GroupName,
                    Type = question.Type,
                },
                Answer = new StudyAnswer()
                {
                    CorrectAnswer = prevItem.Answer,
                    IsCorrect = isPrevCorrect,
                    CorrectAnswerMD5 = Crypto.MD5Hash(prevItem.Answer),
                }
            };

            return prevStep;
        }

        private bool IsCorrectAnswer(string answer, string etalon)
        {
            return answer.Trim().ToLower() == etalon.Trim().ToLower();
        }

        private List<StudyGroupVariants> GenerateVariants(MemoryItem correctItem)
        {
            var correctVariants = Split(correctItem.Answer, ' ');
            
            var otherVariants = _mng.GetItems(correctItem.Group)
                .Where(x => x.Id != correctItem.Id)
                .Take(20)
                .SelectMany(x => x.Answer.Split(' '))
                .Except(correctVariants)
                .Distinct()
                .ToArray();

            var variants = new List<StudyGroupVariants>();
            foreach (var t in correctVariants)
            {
                var group = GenerateVariantItem(t, otherVariants);
                variants.Add(group);
            }

            return variants;
        }

        private List<string> Split(string text, char ch)
        {
            var result = new List<string>();
            /*while (true)
            {
                var n = text.IndexOf(ch, 1);
                if (n == -1)
                    break;

                result.Add(text.Substring(0, n));
                text = text.Substring(n, text.Length - n);
            }*/

            result = text.Split(ch).ToList();
            for (int i = 1; i < result.Count; i++)
            {
                result[i] = ch + result[i];
            }
            return result;
        }

        private StudyGroupVariants GenerateVariantItem(string correctItem, params string[] otherItems)
        {
            var items = new List<string>();
            items.Add(correctItem);
            items.AddRange(otherItems.GetRandomizeList().Take(5));
            items = items.GetRandomizeList();

            return new StudyGroupVariants()
            {
                Variants = items.ToArray()
            };
        }
    }
}
