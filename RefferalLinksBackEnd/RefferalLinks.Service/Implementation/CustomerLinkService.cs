using AutoMapper;
using LinqKit;
using MayNghien.Common.Helpers;
using MayNghien.Models.Request.Base;
using MayNghien.Models.Response.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OfficeOpenXml;
using RefferalLinks.DAL.Contract;
using RefferalLinks.DAL.Implementation;
using RefferalLinks.DAL.Models.Entity;
using RefferalLinks.Models.Dto;
using RefferalLinks.Service.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static Maynghien.Common.Helpers.SearchHelper;
namespace RefferalLinks.Service.Implementation
{
    public class CustomerLinkService : ICustomerLinkService
    {
        private readonly ICustomerLinkRepository _customerLinkRepository;
        private IMapper _mapper;
        private readonly ICustomerRespository _customerRespository;
        private readonly ILinkTemplateRepository _linkTemplateRepository;
        private readonly IUserespository _userespository;
        private readonly ITeamRespository _teamRespository;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICustomerlinkImageRepository _customerlinkImageRepository;

        public CustomerLinkService(ICustomerLinkRepository customerLinkRepository, IMapper mapper , ICustomerRespository customerRespository ,
            ILinkTemplateRepository linkTemplateRepository, IUserespository userespository , ITeamRespository teamRespository , IHttpContextAccessor httpContextAccessor ,
            UserManager<ApplicationUser> userManager, ICustomerlinkImageRepository customerlinkImageRepository
            )
        {
            _customerLinkRepository = customerLinkRepository;
            _mapper = mapper;
            _customerRespository = customerRespository;
            _linkTemplateRepository = linkTemplateRepository;
            _userespository = userespository;
            _teamRespository = teamRespository;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _customerlinkImageRepository = customerlinkImageRepository;
        }

        public AppResponse<List<CustomerLinkDto>> GetAll()
        {
            var result = new AppResponse<List<CustomerLinkDto>>();
            try
            {
                var teamNames = _teamRespository.GetAllTeamNames();
                var list = _customerLinkRepository.GetAll() .Include(x => x.Customer) .Select(x => new CustomerLinkDto
                {
                    Id = x.Id,
                    Url = x.Url,
                    CustomerId = x.CustomerId,
                    LinkTemplateId = x.LinkTemplateId,
                    Email = x.Customer.Email,       
                    Passport = x.Customer.Passport,
                    PhoneNumber = x.Customer.PhoneNumber,
                    Name = x.Customer.Name,
                    BankId = x.LinkTemplate.BankId,
                    TeamId = x.Customer.ApplicationUser.TeamId,
                    UserName = x.Customer.ApplicationUser.UserName,
                    CampaignId = x.LinkTemplate.CampaignId,
                    BankName = x.LinkTemplate.Bank.Name,
                    TeamName = x.Customer.ApplicationUser.TeamId.HasValue && teamNames.ContainsKey(x.Customer.ApplicationUser.TeamId.Value)
        ? teamNames[x.Customer.ApplicationUser.TeamId.Value]
        : string.Empty,
                    CamPaignName = x.LinkTemplate.Campaign.Name,
                    CreatedOn = x.CreatedOn,
                    InforCustomer = String.Format("Name:{0} , Email:{1} , Cccd:{2} , PhoneNumber:{3} , PassPort:{4}  ", x.Customer.Name, x.Customer.Email, x.Customer.Passport, x.Customer.PhoneNumber, x.Customer.Passport)
                }).ToList();
                result.BuildResult(list);
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
                var customer = _customerLinkRepository.Get(Id);
                customer.IsDeleted = true;
                _customerLinkRepository.Edit(customer);
                result.BuildResult("xóa thành công");
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }

        public AppResponse<CustomerLinkDto> Edit(CustomerLinkDto request)
        {
            var result = new AppResponse<CustomerLinkDto>();
            try
            {
                var customer = _customerLinkRepository.Get((Guid)request.Id);
                customer.Url = request.Url;
                customer.CustomerId = (Guid)request.CustomerId;
                customer.LinkTemplateId = request.LinkTemplateId;
                _customerLinkRepository.Edit(customer);
                result.BuildResult(request);
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }

        public AppResponse<CustomerLinkDto> Get(Guid Id)
        {
            var result = new AppResponse<CustomerLinkDto>();
            try
            {
                var query = _customerLinkRepository.FindBy(x => x.Id == Id).Include(x => x.LinkTemplate) .Include(x => x.Customer);

                var data = query.Select(x => new CustomerLinkDto
                {
                    Id = x.Id,
                    Url = x.Url,
                    CustomerId = x.CustomerId,
                    Email = x.Customer.Email,
                    LinkTemplateId = x.LinkTemplateId,
                    Passport = x.Customer.Passport,
                    PhoneNumber = x.Customer.PhoneNumber,
                    Name = x.Customer.Name,
                    BankId = x.LinkTemplate.BankId,
                    CampaignId = x.LinkTemplate.CampaignId,
                    CreatedOn = x.CreatedOn,
                    BankName = x.LinkTemplate.Bank.Name,
                    TeamId = x.Customer.ApplicationUser.TeamId,
                    CamPaignName = x.LinkTemplate.Campaign.Name,
                    UserName = x.Customer.ApplicationUser.UserName,
                    InforCustomer = String.Format("Name:{0} , Email:{1} , Cccd:{2} , PhoneNumber:{3} , PassPort:{4}  ", x.Customer.Name, x.Customer.Email, x.Customer.Passport, x.Customer.PhoneNumber, x.Customer.Passport)
                }).First();
                result.BuildResult(data);
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }

        public async Task<AppResponse<SearchResponse<CustomerLinkDto>>> Search(SearchRequest request)
        {
            var result = new AppResponse<SearchResponse<CustomerLinkDto>>();

            try
            {
                var userRole = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
                var query = await BuildFilterExpression(request.Filters);
                var numOfRecords = _customerLinkRepository.CountRecordsByPredicate(query);
                var model = _customerLinkRepository.FindByPredicate(query).OrderByDescending(p => p.CreatedOn);
                int pageIndex = request.PageIndex ?? 1;
                int pageSize = request.PageSize ?? 1;
                int startIndex = (pageIndex - 1) * (int)pageSize;
                var teamNames = _teamRespository.GetAllTeamNames();
                var List = model.Skip(startIndex).Take(pageSize).Include(x => x.Customer).Include(x => x.LinkTemplate.Bank).Include(x => x.LinkTemplate.Campaign)
                    .Select(x => new CustomerLinkDto
                    {
                        Id = x.Id,
                        Url = x.Url,
                        CustomerId = x.CustomerId,
                        LinkTemplateId = x.LinkTemplateId,
                        Email = x.Customer.Email,
                        PhoneNumber = x.Customer.PhoneNumber,
                        Passport = x.Customer.Passport,
                        Name = x.Customer.Name,    
                        BankId = x.LinkTemplate.BankId,
                        CampaignId = x.LinkTemplate.CampaignId,
                        BankName = x.LinkTemplate.Bank.Name,
                        CamPaignName = x.LinkTemplate.Campaign.Name,
                        TeamId = x.Customer.ApplicationUser.TeamId,
                        CreatedOn = x.CreatedOn,
                        TpBank = x.Customer.ApplicationUser.TpBank,
                        RefferalCode = x.Customer.ApplicationUser.RefferalCode,
                        UserName = x.Customer.ApplicationUser.UserName,
                        TeamName = x.Customer.ApplicationUser.TeamId.HasValue && teamNames.ContainsKey(x.Customer.ApplicationUser.TeamId.Value)
        ? teamNames[x.Customer.ApplicationUser.TeamId.Value]
        : string.Empty,
                        InforCustomer = String.Format("Name:{0} , Email:{1} , Cccd:{2} , PhoneNumber:{3} , PassPort:{4}  ", x.Customer.Name, x.Customer.Email, x.Customer.Passport, x.Customer.PhoneNumber, x.Customer.Passport)
                    })
                    .ToList();
               

                var searchUserResult = new SearchResponse<CustomerLinkDto>
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
        public async Task<ApplicationUser> GetCurrentUserAsync(ClaimsPrincipal user)
        {
            var currentUser = await _userManager.GetUserAsync(user);
            return currentUser;
        }
        private async Task<ExpressionStarter<Customerlink>> BuildFilterExpression(IList<Filter> Filters)
        {
            try
            {
                var userRole = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
                var UserName = ClaimHelper.GetClainByName(_httpContextAccessor, "UserName");
                //var userid =  _userManager.GetUserId( _httpContextAccessor.HttpContext.User);
                var user = await _userManager.FindByNameAsync(UserName);
                var predicate = PredicateBuilder.New<Customerlink>(true);

                switch (userRole)
                {

                    case "teamleader":


                        predicate = predicate.And(m => m.Customer.ApplicationUser.TeamId.ToString().Contains(user.TeamId.ToString()));

                        break;

                    case "sale":
                        predicate = predicate.And(m => m.Customer.ApplicationUserId.Contains(user.Id));
                        break;
                    default:
                        break;
                }

                if (Filters != null)

                    foreach (var filter in Filters)
                    {
                        switch (filter.FieldName)
                        {

                            case "email":

                                predicate = predicate.And(m => m.Customer.Email.Contains(filter.Value));
                                
                                break;
                            case "phoneNumber":
                                predicate = predicate.And(m => m.Customer.PhoneNumber.Contains(filter.Value));
                                break;
                            case "passport":
                                predicate = predicate.And(m => m.Customer.Passport.Contains(filter.Value));
                                break;
                            case "name":
                                predicate = predicate.And(m => m.Customer.Name.Contains(filter.Value));
                                break;
                            case "bankId":
                                predicate = predicate.And(m => m.LinkTemplate.BankId.ToString().Contains(filter.Value));
                                break;
                            case "campaignId":
                                predicate = predicate.And(m => m.LinkTemplate.CampaignId.ToString().Contains(filter.Value));
                                break;
                            case "teamId":
                                if (userRole == "teamleader" || userRole == "sale") break;
                                predicate = predicate.And(m => m.Customer.ApplicationUser.TeamId.ToString().Contains(filter.Value));
                                break;
                            case "userId":
                                if (userRole == "sale") break;
                                predicate = predicate.And(m => m.Customer.ApplicationUserId.Contains(filter.Value));
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

        public AppResponse<string> StatusChange(CustomerLinkDto request)
        {
            var result = new AppResponse<string>();
            try
            {
                var UserName = ClaimHelper.GetClainByName(_httpContextAccessor, "UserName");
                if (UserName == null)
                {
                    return result.BuildError("Cannot find Account by this user");
                }
                if (request.ListCustomerlinkImage == null)
                {
                    return result.BuildError("Không để trống danh sách hình ảnh");
                }
                request.ListCustomerlinkImage.ForEach(item =>
                {
                    var customerLinkImage = _mapper.Map<CustomerLinkImage>(item);
                    customerLinkImage.Id = Guid.NewGuid();
                    customerLinkImage.CreatedOn = DateTime.UtcNow;
                    customerLinkImage.CreatedBy = UserName;
                    customerLinkImage.CustomerLinkId = request.Id.Value;
                    _customerlinkImageRepository.Add(customerLinkImage);
                });
                var customerLink = _customerLinkRepository.Get(request.Id.Value);
                customerLink.Status = request.Status.Value;
                _customerLinkRepository.Edit(customerLink);
                result.BuildResult("OK");

            }
            catch(Exception ex)
            {
                result.BuildError(ex.Message);
            }

            return result;
        }



        public async Task<byte[]> ExportToExcel(SearchRequest request)
        {
            var data = await this.Search(request);
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("SelectedRows");
           

                worksheet.Cells[1, 1].Value = "PhoneNumber";
                worksheet.Cells[1, 2].Value = "Passport";
                worksheet.Cells[1, 3].Value = "Email";
                worksheet.Cells[1, 4].Value = "Ngày đăng kí thành công";
                worksheet.Cells[1, 5].Value = "Dự án";
                worksheet.Cells[1, 6].Value = "Sản phẩm";
                worksheet.Cells[1, 7].Value = "Link Image";

                for (int i = 0; i < data.Data.Data.Count; i++)
                {
                    var dto = data.Data.Data[i];
                    var GetallImg =  _customerlinkImageRepository.GetAll().Where(x => x.CustomerLinkId == dto.Id).ToList();
                    var convertedItems = _mapper.Map<List<CustomerlinkImageDto>>(GetallImg);
                    dto.ListCustomerlinkImage = new List<CustomerlinkImageDto>();
                    dto.ListCustomerlinkImage?.AddRange( convertedItems);
                    worksheet.Cells[i + 2, 1].Value = dto.PhoneNumber;
                    worksheet.Cells[i + 2, 2].Value = dto.Passport;
                    worksheet.Cells[i + 2, 3].Value = dto.Email;
                    worksheet.Cells[i + 2, 4].Value = dto.CreatedOn;
                    worksheet.Cells[i + 2, 5].Value = dto.BankName;
                    worksheet.Cells[i + 2, 6].Value =  String.Format("Team : {0} , Reffercode : {1} , TPbank : {2} , ImageLink {3}", dto.TeamName , dto.RefferalCode , dto.TpBank,dto.ListCustomerlinkImage)  ;
                    foreach(var j in dto.ListCustomerlinkImage)
                    {
                        worksheet.Cells[i + 2, 7].Value += j + " , ";
                    }
                    
                }


                return package.GetAsByteArray();
            }
        }

    }
}
