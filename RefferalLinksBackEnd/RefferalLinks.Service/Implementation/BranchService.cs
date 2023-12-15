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
    public class BranchService : IBranchService
    {
        private readonly IMapper _mapper;
        private IBranchRepository _branchRepository;
        private IHttpContextAccessor _httpContextAccessor;

        public BranchService(IMapper mapper, IBranchRepository branchRepository, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _branchRepository = branchRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public AppResponse<BranchDto> Create(BranchDto request)
        {
            var result = new AppResponse<BranchDto>();
            try
            {
                var UserName = ClaimHelper.GetClainByName(_httpContextAccessor, "UserName");
                if (UserName == null)
                {
                    return result.BuildError("Cannot find Account by this user");
                }
                var branch =  _mapper.Map<Branch>(request);
                branch.Id = Guid.NewGuid();
                branch.CreatedBy = UserName;
                _branchRepository.Add(branch);
                request.Id = branch.Id;
                result.BuildResult(request);
            }
            catch(Exception ex)
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
                var branch = _branchRepository.Get(Id);
                branch.IsDeleted = true;
                _branchRepository.Edit(branch);
                result.BuildResult("OK");
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }

        public AppResponse<BranchDto> Edit(BranchDto request)
        {
            var result = new AppResponse<BranchDto>();
            try
            {
                var UserName = ClaimHelper.GetClainByName(_httpContextAccessor, "UserName");
                if (UserName == null)
                {
                    return result.BuildError("Cannot find Account by this user");
                }
                var branch = _branchRepository.Get(request.Id.Value);
                branch.Name = request.Name;
                branch.Modifiedby = UserName;
                _branchRepository.Edit(branch);
                result.BuildResult(request);
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }

        public AppResponse<BranchDto> Get(Guid id)
        {
            var result = new AppResponse<BranchDto>();
            try
            {
                var branch = _branchRepository.Get(id);
                var data = _mapper.Map<BranchDto>(branch);
                result.BuildResult(data);
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }

        public AppResponse<List<BranchDto>> GetAll()
        {
            var result = new AppResponse<List<BranchDto>>();
            try
            {
                var list = _branchRepository.GetAll().Select(x => new BranchDto
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToList();
                result.BuildResult(list);
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }

        public AppResponse<SearchResponse<BranchDto>> Search(SearchRequest request)
        {
            var result = new AppResponse<SearchResponse<BranchDto>>();
            try
            {
                var query = BuildFilterExpression(request.Filters);
                var numOfRecords = _branchRepository.CountRecordsByPredicate(query);
                var model = _branchRepository.FindByPredicate(query).OrderByDescending(x => x.CreatedOn);
                int pageIndex = request.PageIndex ?? 1;
                int pageSize = request.PageSize ?? 1;
                int startIndex = (pageIndex - 1) * (int)pageSize;
                var List = model.Skip(startIndex).Take(pageSize)
                    .Select(x => new BranchDto
                    {
                        Id = x.Id,
                        Name = x.Name
                    })
                    .ToList();


                var searchUserResult = new SearchResponse<BranchDto>
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

        private ExpressionStarter<Branch> BuildFilterExpression(IList<Filter> Filters)
        {
            try
            {
                var predicate = PredicateBuilder.New<Branch>(true);
                if (Filters != null)
                    foreach (var filter in Filters)
                    {
                        switch (filter.FieldName)
                        {
                            case "name":
                                predicate = predicate.And(m => m.Name.Contains(filter.Value));
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
