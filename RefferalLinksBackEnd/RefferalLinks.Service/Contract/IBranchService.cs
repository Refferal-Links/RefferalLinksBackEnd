using MayNghien.Models.Request.Base;
using MayNghien.Models.Response.Base;
using RefferalLinks.Models.Dto;

namespace RefferalLinks.Service.Contract
{
    public interface IBranchService
    {
        AppResponse<List<BranchDto>> GetAll();
        AppResponse<BranchDto> Get(Guid id);
        AppResponse<BranchDto> Edit(BranchDto request);
        AppResponse<BranchDto> Create(BranchDto request);
        AppResponse<string> Delete(Guid Id);
        AppResponse<SearchResponse<BranchDto>> Search(SearchRequest request);
    }
}
