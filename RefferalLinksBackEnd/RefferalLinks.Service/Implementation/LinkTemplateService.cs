using System.Data.Entity;
using AutoMapper;
using LinqKit;
using MayNghien.Common.Helpers;
using MayNghien.Models.Request.Base;
using MayNghien.Models.Response.Base;
using Microsoft.AspNetCore.Http;
using RefferalLinks.DAL.Contract;
using RefferalLinks.DAL.Models.Entity;
using RefferalLinks.Models.Dto;
using RefferalLinks.Service.Contract;
using static Maynghien.Common.Helpers.SearchHelper;

namespace RefferalLinks.Service.Implementation
{
    public class LinkTemplateService : ILinkTemplateService
    {
        private ILinkTemplateRepository _linkTemplateRepository;
        private IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;
        private IBankRepository _bankRepository;
        private ICampaignRepository _campaignRepository;
        public LinkTemplateService(ILinkTemplateRepository linkTemplateRepository, IMapper mapper, IHttpContextAccessor contextAccessor,
            IBankRepository bankRepository, ICampaignRepository campaignRepository)
        {
            _linkTemplateRepository = linkTemplateRepository;
            _mapper = mapper;
            _httpContextAccessor = contextAccessor;
            _bankRepository = bankRepository;
            _campaignRepository = campaignRepository;
        }

        public AppResponse<string> StatusChange(LinkTemplateDto request)
        {
            var result = new AppResponse<string>();
            try
            {
                var linkTemplate = _linkTemplateRepository.Get(request.Id.Value);
                linkTemplate.IsActive = !linkTemplate.IsActive;
                _linkTemplateRepository.Edit(linkTemplate);
                if(linkTemplate.IsActive)
                {
                    result.BuildResult("đã kích hoạt");
                }
                else
                {
                    result.BuildResult("đã tắt");
                }
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }

        public AppResponse<LinkTemplateDto> Create(LinkTemplateDto request)
        {
            var result = new AppResponse<LinkTemplateDto>();
            try
            {
                var UserName = ClaimHelper.GetClainByName(_httpContextAccessor, "UserName");
                if(request.BankId == null)
                {
                    return result.BuildError("Bank cannot null");
                }
                var bank = _bankRepository.Get(request.BankId);
                if (bank == null)
                {
                    return result.BuildError("Cannot find Bank");
                }
                if (request.CampaignId == null)
                {
                    return result.BuildError("Bank cannot null");
                }
                var campaign = _campaignRepository.Get(request.CampaignId);
                if (campaign == null)
                {
                    return result.BuildError("Cannot find Bank");
                }
                var linkTemplate = _mapper.Map<LinkTemplate>(request);
                linkTemplate.Id = Guid.NewGuid();
                linkTemplate.IsActive = true;
                linkTemplate.CreatedOn = DateTime.UtcNow;
                linkTemplate.CreatedBy = UserName;
                linkTemplate.Bank = null;
                linkTemplate.Campaign = null;
                _linkTemplateRepository.Add(linkTemplate);
                request.Id = linkTemplate.Id;

                result.BuildResult(request);
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }

        public AppResponse<string> Delete(Guid Id)
        {
            var result = new AppResponse<string>();
            try
            {
                var linkTemplate = _linkTemplateRepository.Get(Id);
                linkTemplate.IsDeleted = true;
                _linkTemplateRepository.Edit(linkTemplate);

                result.BuildResult("xóa thành công");
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }

        public AppResponse<LinkTemplateDto> Edit(LinkTemplateDto request)
        {
            var result = new AppResponse<LinkTemplateDto>();
            try
            {
                if(request.Id != null)
                {
                    var linkTemplate = _linkTemplateRepository.Get(request.Id.Value);
                    linkTemplate.Url = request.Url;
                    linkTemplate.BankId = request.BankId;
                    linkTemplate.IsActive = request.IsActive == "đang hoạt động" ? true:false;
                    linkTemplate.CampaignId = request.CampaignId;
                    linkTemplate.ModifiedOn = DateTime.UtcNow;
                    var UserName = ClaimHelper.GetClainByName(_httpContextAccessor, "UserName");
                    linkTemplate.Modifiedby = UserName;

                    _linkTemplateRepository.Edit(linkTemplate);
                    result.BuildResult(request);
                }

            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }

        public AppResponse<LinkTemplateDto> Get(Guid Id)
        {
            var result = new AppResponse<LinkTemplateDto>();
            try
            {
                var linkTemplate = _linkTemplateRepository.Get(Id);
                var data = _mapper.Map<LinkTemplateDto>(linkTemplate);
                result.BuildResult(data);
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }

        public AppResponse<List<LinkTemplateDto>> GetAll()
        {
            var result = new AppResponse<List<LinkTemplateDto>>();
            try
            {
                var list = _linkTemplateRepository.GetAll().Where(x=>x.IsDeleted != true).Include(x=>x.Bank).Include(x=>x.Campaign);
                var data = list.Select(x => new LinkTemplateDto
                {
                    Id = x.Id,
                    BankId = x.BankId,
                    CampaignId = x.CampaignId,
                    Url = x.Url,
                    IsActive = x.IsActive ? "đang hoạt động" : "đã tắt",
                    BankName = x.Bank.Name,
                    CampaignName = x.Campaign.Name,
                }).ToList();

                result.BuildResult(data);
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }

        public AppResponse<SearchResponse<LinkTemplateDto>> Search(SearchRequest request)
        {
            var result = new AppResponse<SearchResponse<LinkTemplateDto>>();
            try
            {
                var query = BuildFilterExpression(request.Filters);
                var numOfRecords = _linkTemplateRepository.CountRecordsByPredicate(query);
                var model = _linkTemplateRepository.FindByPredicate(query).Include(x=>x.Bank).Include(x=>x.Campaign);
                if (request.SortBy != null)
                {
                    model = AddSort(model, request.SortBy);
                }
                int pageIndex = request.PageIndex ?? 1;
                int pageSize = request.PageSize ?? 1;
                int startIndex = (pageIndex - 1) * (int)pageSize;
                var List = model.Skip(startIndex).Take(pageSize)
                    .Select(x => new LinkTemplateDto
                    {
                        Id = x.Id,
                        BankId= x.BankId,
                        CampaignId= x.CampaignId,
                        Url= x.Url,
                        IsActive = x.IsActive ? "đang hoạt động" : "đã tắt",
                        BankName = x.Bank.Name,
                        CampaignName = x.Campaign.Name,
                    })
                    .ToList();
                var role = ClaimHelper.GetClainByName(_httpContextAccessor, "Roles");
                var refferalCode = ClaimHelper.GetClainByName(_httpContextAccessor, "RefferalCode");
                if(role == "Sale")
                    foreach (var item in List)
                    {
                        item.Url = item.Url.Replace("{{sale}}", refferalCode);
                    }

                var searchUserResult = new SearchResponse<LinkTemplateDto>
                {
                    TotalRows = numOfRecords,
                    TotalPages = CalculateNumOfPages(numOfRecords, pageSize),
                    CurrentPage = pageIndex,
                    Data = List,
                };
                result.BuildResult(searchUserResult);
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }

        private IQueryable<LinkTemplate> AddSort(IQueryable<LinkTemplate> input, SortByInfo sortByInfo)
        {
            var result = input.AsQueryable();
            switch (sortByInfo.FieldName)
            {

                case "bankName":
                    {
                        if (sortByInfo.Ascending != null && sortByInfo.Ascending.Value)
                        {
                            result = result.OrderBy(m => m.Bank.Name);

                        }
                        else
                        {
                            result = result.OrderByDescending(m => m.Bank.Name);
                        }
                    }
                    break;
                case "campaignName":
                    {
                        if (sortByInfo.Ascending != null && sortByInfo.Ascending.Value)
                        {
                            result = result.OrderBy(m => m.Campaign.Name);

                        }
                        else
                        {
                            result = result.OrderByDescending(m => m.Campaign.Name);
                        }
                    }
                    break;
                case "isActive":
                    {
                        if (sortByInfo.Ascending != null && sortByInfo.Ascending.Value)
                        {
                            result = result.OrderBy(m => m.IsActive);

                        }
                        else
                        {
                            result = result.OrderByDescending(m => m.IsActive);
                        }
                    }
                    break;
                default:
                    break;
            }
            return result;
        }

        private ExpressionStarter<LinkTemplate> BuildFilterExpression(IList<Filter> Filters)
        {
            try
            {
                var predicate = PredicateBuilder.New<LinkTemplate>(true);
                if (Filters != null)
                    foreach (var filter in Filters)
                    {
                        switch (filter.FieldName)
                        {
                            case "bankId":
                                predicate = predicate.And(m => m.Bank.Id.Equals(Guid.Parse(filter.Value)));
                                break;
                            case "campaignId":
                                predicate = predicate.And(m => m.Campaign.Id.Equals(Guid.Parse(filter.Value)));
                                break;
                            default:
                                break;
                        }
                    }
                predicate = predicate.And(m => m.IsDeleted == false);
                return predicate;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
