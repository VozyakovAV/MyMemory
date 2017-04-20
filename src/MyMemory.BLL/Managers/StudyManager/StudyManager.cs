using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyMemory.Domain;
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

            //var isVariants = task.StepNumber <= 3;
            var isVariants = false;
            var variants = isVariants ? GenerateVariants(task.Item).ToArray() : null;

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
                    Type = isVariants ? StudyQuestionType.TestWords : StudyQuestionType.TestLetters,
                },
                Answer = new StudyAnswer()
                {
                    CorrectAnswerMD5 = Crypto.MD5Hash(task.Item.Answer.Trim().ToLower()),
                }
            };
        }

        private StudyStep GetRepeatStep(StudyStep prevStep)
        {
            var answer = prevStep.Answer.DeepClone();
            var question = prevStep.Question.DeepClone();
            question.IsRepeat = true;

            return new StudyStep()
            {
                Question = question,
                Answer = answer,
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
                Question = question.DeepClone(),
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
            
            var otherVariants = _mng.GetItemsRandom(correctItem.Group, 20)
                .Where(x => x.Id != correctItem.Id)
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
