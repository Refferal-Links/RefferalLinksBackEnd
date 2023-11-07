using MayNghien.Common.Models.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefferalLinks.DAL.Models.Entity
{
  public class Team : BaseEntity
    {
        public string? name { get; set; }
        public string? RefferalCode { get; set; }
    }
}
