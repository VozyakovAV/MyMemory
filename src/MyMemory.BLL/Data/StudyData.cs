using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMemory.BLL
{
    public class StudyData
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public string Message { get; set; }
        public StudyStatistic Statistic { get; set; }
        public StudyStep Step { get; set; }
        public StudyStep PrevStep { get; set; }

        public StudyData()
        {
            this.Statistic = new StudyStatistic();
            this.Step = new StudyStep();
            this.PrevStep = new StudyStep();
        }
    }

    public class StudyStatistic
    {
        public int NumberOfCorrect { get; set; }
        public int NumberOfIncorrect { get; set; }
        
    }

    public class StudyStep
    {
        public StudyQuestion Question { get; set; }
        public StudyAnswer Answer { get; set; }
    }

    public class StudyQuestion
    {
        public int ItemId { get; set; }
        public int TaskId { get; set; }
        public string Text { get; set; }
        public int StepNumber { get; set; }
        public bool IsRepeat { get; set; }
        public string GroupName { get; set; }
        public StudyGroupVariants[] GroupVariants { get; set; }
    }

    public class StudyAnswer
    {
        public bool IsCorrect { get; set; }
        public string CorrectAnswer { get; set; }
        public string CorrectAnswerMD5 { get; set; }
    }

    public class StudyGroupVariants
    {
        public string[] Variants { get; set; }
    }
}
