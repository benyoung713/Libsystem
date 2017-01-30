using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibSystem.Models
{
    public class MemberBookVM
    {
        public IEnumerable<SelectListItem> MemberTable { get; set; }
        
        //public int SelectedMemberID { get; set; }
        public string memberName { get; set; }

        public BookTable bookTable { get; set; }
        
        public int OnLoan { get; set; }

      

        


    }
    
}