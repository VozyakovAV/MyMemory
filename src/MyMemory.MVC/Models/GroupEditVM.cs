using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMemory.MVC.Models
{
    public class GroupEditVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ItemVM[] Items { get; set; }
    }

    public class ItemVM
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}