using MayNghien.Common.Models.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefferalLinks.DAL.Models.Entity
{
  public class TeamManagement : BaseEntity
    {
        public Guid id {  get; set; }
        public string name { get; set; }    
    }
}
