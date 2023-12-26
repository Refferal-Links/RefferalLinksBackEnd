﻿using MayNghien.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefferalLinks.Models.Dto
{
  public class UserModel : BaseDto
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public Guid? TeamId { get; set; }
        public string? RefferalCode { get; set; }
        public string? TpBank { get; set; }
        public string? LockoutEnabled { get; set; }
        public string? TeamName { get; set; }
        public Guid? BranchId { get; set; }
        public string? BranchName { get; set; }
    }
}
