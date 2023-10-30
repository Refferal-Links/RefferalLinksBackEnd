using AutoMapper;
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
			CreateMap<Bank, BankDto>().ReverseMap();
			CreateMap<Campaign, CampaignDto>().ReverseMap();
		}
	}
}
