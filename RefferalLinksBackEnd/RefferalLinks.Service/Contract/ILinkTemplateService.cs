using MayNghien.Models.Request.Base;
using MayNghien.Models.Response.Base;
using RefferalLinks.DAL.Models.Entity;
using RefferalLinks.Models.Dto;

namespace RefferalLinks.Service.Contract
{
    public interface ILinkTemplateService
    {
        AppResponse<LinkTemplateDto> Create(LinkTemplateDto request);
        AppResponse<LinkTemplateDto> Edit(LinkTemplateDto request);
        AppResponse<string> Delete(Guid Id);
        AppResponse<LinkTemplateDto> Get(Guid Id);
        AppResponse<List<LinkTemplateDto>> GetAll();
        AppResponse<SearchResponse<LinkTemplateDto>> Search(SearchRequest request);
        AppResponse<string> StatusChange(LinkTemplateDto Id);
    }
}
