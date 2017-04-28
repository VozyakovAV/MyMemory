using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMemory.MVC.Models
{
    public class HomeVM
    {
        public List<GroupVM> Groups { get; set; }

        public HomeVM()
        {
            this.Groups = new List<GroupVM>();
        }
    }
}