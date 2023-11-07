﻿using MayNghien.Common.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefferalLinks.DAL.Models.Entity
{
    public class Customer : BaseEntity
    {
      
        public string Name { get; set; }
        public string Passport { get; set; }    
        public string PhoneNumber { get; set; }
        public string Province { get;set; }
        public string Email { get; set; }
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }


    }
}
