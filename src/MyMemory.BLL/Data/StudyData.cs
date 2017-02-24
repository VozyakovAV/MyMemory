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
        public StudyQuestion Question { get; set; }
        public StudyAnswer PrevAnswer { get; set; }
    }

    public class StudyStatistic
    {
        public int CountQuestions { get; set; }
        public int CountCorrectAnswers { get; set; }
    }

    public class StudyQuestion
    {
        public int Index { get; set; }
        public string Text { get; set; }
    }

    public class StudyAnswer
    {
        public bool IsCorrectAnswer { get; set; }
        public string CorrectAnswer { get; set; }
    }
}
