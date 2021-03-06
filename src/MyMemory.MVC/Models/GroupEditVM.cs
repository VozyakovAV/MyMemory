﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyMemory.MVC.Models
{
    public class GroupEditVM
    {
        public int Id { get; set; }
        
        [Display(Name = "Имя")]
        public string Name { get; set; }
        public ItemVM[] Items { get; set; }
    }

    public class ItemVM
    {
        public int Id { get; set; }

        [Display(Name = "Группа")]
        public string GroupName { get; set; }

        public int GroupId { get; set; }

        [Display(Name = "Вопрос")]
        public string Question { get; set; }

        [Display(Name = "Ответ")]
        public string Answer { get; set; }
    }
}