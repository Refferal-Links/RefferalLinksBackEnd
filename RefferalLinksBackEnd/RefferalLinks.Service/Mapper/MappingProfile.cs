using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RefferalLinks.DAL.Models.Entity;
using RefferalLinks.Models.Dto;

namespace RefferalLinks.Service.Mapper
{
	public class MappingProfile :Profile
	{
		public MappingProfile()
		{
			CreateMap();
		}

		public void CreateMap()
		{
			CreateMap<IdentityUser, UserModel>().ReverseMap();
			CreateMap<TeamManagement, TeamDto>().ReverseMap();	

		}
	}
}
