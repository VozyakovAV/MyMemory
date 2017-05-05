using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyMemory.MVC.Models
{
    public class ItemEditVM
    {
        public int Id { get; set; }

        [Display(Name = "Группа")]
        public string GroupName { get; set; }

        public int GroupId { get; set; }

        [Display(Name = "Вопрос")]
        public string Question { get; set; }

        [Display(Name = "Ответ")]
        public string Answer { get; set; }

        public SelectList ListGroups { get; set; }
    }
}