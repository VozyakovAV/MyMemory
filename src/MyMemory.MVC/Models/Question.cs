using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMemory.MVC.Models
{
    public class StudyData
    {
        public StudyStatistic Statistic { get; set; }
        public StudyQuestion CurrentQuestion { get; set; }
        public StudyAnswer PrevAnswer { get; set; }
    }

    public class StudyStatistic
    {
        public int CountQuestions { get; set; }
        public int CountCorrectAnswers { get; set; }
    }

    public class StudyQuestion
    {
        public string Text { get; set; }
    }

    public class StudyAnswer
    {
        public bool IsCorrectAnswer { get; set; }
        public string CorrectAnswer { get; set; }
    }
}