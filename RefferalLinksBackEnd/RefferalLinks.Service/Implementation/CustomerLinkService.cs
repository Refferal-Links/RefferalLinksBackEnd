﻿using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using LinqKit;
using MayNghien.Common.Helpers;
using MayNghien.Models.Request.Base;
using MayNghien.Models.Response.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OfficeOpenXml;
using RefferalLinks.Common.Enum;
using RefferalLinks.DAL.Contract;
using RefferalLinks.DAL.Implementation;
using RefferalLinks.DAL.Models.Entity;
using RefferalLinks.Models.Dto;
using RefferalLinks.Service.Contract;
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

        public CustomerLinkService(ICustomerLinkRepository customerLinkRepository, IMapper mapper, ICustomerRespository customerRespository,
            ILinkTemplateRepository linkTemplateRepository, IUserespository userespository, ITeamRespository teamRespository, IHttpContextAccessor httpContextAccessor,
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
                var list = _customerLinkRepository.GetAll().Include(x => x.Customer).Select(x => new CustomerLinkDto
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
                var query = _customerLinkRepository.FindBy(x => x.Id == Id).Include(x => x.LinkTemplate).Include(x => x.Customer);

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
                    InforCustomer = String.Format("Name:{0} , Email:{1} , Cccd:{2} , PhoneNumber:{3} , PassPort:{4}  ", x.Customer.Name, x.Customer.Email, x.Customer.Passport, x.Customer.PhoneNumber, x.Customer.Passport),
                    ListCustomerlinkImage = _customerlinkImageRepository.GetAll().Where(m => m.CustomerLinkId == x.Id).Select(
                        cti => new CustomerlinkImageDto
                        {
                            CustomerLinkId = cti.CustomerLinkId,
                            Id = cti.Id,
                            LinkImage = cti.LinkImage,
                        }
                        ).ToList(),
                    Status = x.Status,
                    CreateOn = x.CreatedOn.Value.ToString("dd/MM/yyyy"),
                    ModifiedOn = x.ModifiedOn.Value.ToString("dd/MM/yyyy"),
                    Note = x.Note,

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
                var model = _customerLinkRepository.FindByPredicate(query);

                if(request.SortBy != null)
                {
                    model = AddSort(model, request.SortBy);
                }
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
                        Iduser = x.Customer.ApplicationUser.Id,
                        TpBank = x.Customer.ApplicationUser.TpBank,

                        TeamName = x.Customer.ApplicationUser.TeamId.HasValue && teamNames.ContainsKey(x.Customer.ApplicationUser.TeamId.Value)
                                       ? teamNames[x.Customer.ApplicationUser.TeamId.Value] : string.Empty,
                        //InforCustomer = String.Format("Tên: {0}; Email: {1}; CCCD: {2}; phone: {3}  ", x.Customer.Name, x.Customer.Email, x.Customer.Passport, x.Customer.PhoneNumber),
                        Status = x.Status,
                        StatusText = x.Status == StatusCustomerLink.Pending ? "Pending" : x.Status == StatusCustomerLink.Approved ? "Approved" : "Rejected",
                        CreateOn = x.CreatedOn.Value.ToString("dd/MM/yyyy-HH:mm:ss"),
                        ModifiedOn = x.ModifiedOn.Value.ToString("dd/MM/yyyy-HH:mm:ss"),
                        Note = x.Note,
                        UserName = x.Customer.ApplicationUser.UserName,
                        RefferalCode = x.Customer.ApplicationUser.RefferalCode,
                        CodeNVCSKH = "",
                        NvCSKH = "",

                    })
                    .ToList();
                foreach (var item in List)
                {
                    var listImage = _customerlinkImageRepository.FindBy(x => x.CustomerLinkId == item.Id).ToList();
                    var count = listImage.Count();
                    item.Image1 = count >= 1 ? listImage[0].LinkImage : "";
                    item.Image2 = count >= 2 ? listImage[1].LinkImage : "";
                    item.Image3 = count >= 3 ? listImage[2].LinkImage : "";
                    item.Image4 = count >= 4 ? listImage[3].LinkImage : "";
                    var user = await _userManager.FindByNameAsync(item.UserName);
                    var role = (await _userManager.GetRolesAsync(user)).First();
                    var phoneNumber = item.PhoneNumber;
                    phoneNumber = phoneNumber.Length < 6 ? phoneNumber: phoneNumber.Substring(0, 3) + "XXX" + phoneNumber.Substring(6);
                    item.InforCustomer = String.Format("Tên: {0}; Email: {1}; CCCD: {2}; phone: {3}  ", item.Name, item.Email, item.Passport, phoneNumber);
                    if (role == "CSKH")
                    {
                        item.NvCSKH = item.UserName;
                        item.CodeNVCSKH = item.RefferalCode;
                        item.RefferalCode = "";
                        item.UserName = "";
                    }
                }

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

        private IQueryable<Customerlink> AddSort(IQueryable<Customerlink> input, SortByInfo sortByInfo)
        {
            var result = input.AsQueryable();
            switch (sortByInfo.FieldName)
            {

                case "bankName":
                    {
                        if (sortByInfo.Ascending != null && sortByInfo.Ascending.Value)
                        {
                            result = result.OrderBy(m => m.LinkTemplate.Bank.Name);

                        }
                        else
                        {
                            result = result.OrderByDescending(m => m.LinkTemplate.Bank.Name);
                        }
                    }
                    break;
                case "camPaignName":
                    {
                        if (sortByInfo.Ascending != null && sortByInfo.Ascending.Value)
                        {
                            result = result.OrderBy(m => m.LinkTemplate.Campaign.Name);

                        }
                        else
                        {
                            result = result.OrderByDescending(m => m.LinkTemplate.Campaign.Name);
                        }
                    }
                    break;
                case "statusText":
                    {
                        if (sortByInfo.Ascending != null && sortByInfo.Ascending.Value)
                        {
                            result = result.OrderBy(m => m.Status);

                        }
                        else
                        {
                            result = result.OrderByDescending(m => m.Status);
                        }
                    }
                    break;
                case "createOn":
                    {
                        if (sortByInfo.Ascending != null && sortByInfo.Ascending.Value)
                        {
                            result = result.OrderBy(m => m.CreatedOn.Value);

                        }
                        else
                        {
                            result = result.OrderByDescending(x => x.CreatedOn.Value);
                        }
                    }
                    break;
                case "modifiedOn":
                    {
                        if (sortByInfo.Ascending != null && sortByInfo.Ascending.Value)
                        {
                            result = result.OrderBy(m => m.ModifiedOn.Value);

                        }
                        else
                        {
                            result = result.OrderByDescending(m => m.ModifiedOn.Value);
                        }
                    }
                    break;
                default:
                    break;
            }

            //if (sortByInfo.FieldName == "bankName")
            //{
            //    if (sortByInfo.Ascending != null && sortByInfo.Ascending.Value)
            //    {
            //        result = result.OrderBy(m => m.LinkTemplate.Bank.Name);

            //    }
            //    else
            //    {
            //        result = result.OrderByDescending(m => m.LinkTemplate.Bank.Name);
            //    }
            //}
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

                    case "Teamleader":


                        predicate = predicate.And(m => m.Customer.ApplicationUser.TeamId.ToString().Contains(user.TeamId.ToString()));

                        break;

                    case "Sale":
                        predicate = predicate.And(m => m.Customer.ApplicationUserId.Contains(user.Id));
                        break;
                    case "CSKH":
                        predicate = predicate.And(m => m.Customer.ApplicationUserId.Contains(user.Id));
                        break;
                    case "SUP":
                        {
                            var listTeam = _teamRespository.FindBy(x => x.Branch.Id == user.BranchId).ToList();
                            foreach (var team in listTeam)
                            {
                                predicate = predicate.Or(x => x.Customer.ApplicationUser.TeamId == team.Id);
                            }
                        }
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
                                if (userRole == "Teamleader" || userRole == "Sale") break;
                                predicate = predicate.And(m => m.Customer.ApplicationUser.TeamId.ToString().Contains(filter.Value));
                                break;
                            case "userName":
                                if (userRole == "Sale") break;
                                predicate = predicate.And(m => m.Customer.ApplicationUserId.Contains(filter.Value));
                                break;
                            case "refferalCode":
                                predicate = predicate.And(m => m.Customer.ApplicationUser.RefferalCode.Contains(filter.Value));
                                break;
                            case "createOn":
                                {
                                    var day = DateTime.ParseExact(filter.Value, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    //if (filter.Value != "")
                                    predicate = predicate.And(m => m.CreatedOn.Value.Day.Equals(day.Day) && m.CreatedOn.Value.Month.Equals(day.Month) && m.CreatedOn.Value.Year.Equals(day.Year));
                                }
                                break;
                            case "modifiedOn":
                                {
                                    var day = DateTime.ParseExact(filter.Value, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    //if (filter.Value != "")
                                    predicate = predicate.And(m => m.CreatedOn.Value.Day.Equals(day.Day) && m.CreatedOn.Value.Month.Equals(day.Month) && m.CreatedOn.Value.Year.Equals(day.Year));
                                }
                                break;
                            case "statusText":
                                {
                                    var value = int.Parse(filter.Value);
                                    StatusCustomerLink status = (StatusCustomerLink)Enum.ToObject(typeof(StatusCustomerLink), value);
                                    predicate = predicate.And(m => m.Status == status);
                                }
                                break;
                            case "nvCSKH":
                                {
                                    predicate = predicate.And(m => m.Customer.ApplicationUserId.Equals(filter.Value));
                                }
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
                var listCustomerLinkImage = _customerlinkImageRepository.GetAll().Where(x => x.CustomerLinkId == request.Id).ToList();
                if (listCustomerLinkImage != null)
                {
                    _customerlinkImageRepository.DeleteRange(listCustomerLinkImage);
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
                customerLink.ModifiedOn = DateTime.UtcNow;
                customerLink.Note = request.Note;
                _customerLinkRepository.Edit(customerLink);

                result.BuildResult("OK");

            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }

            return result;
        }



        public async Task<byte[]> ExportToExcel(SearchRequest request)
        {
            request.Filters = request.Filters.Where(x=>x.Value != "" && x.Value != null).ToList();
            var data = await Export(request);
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("SelectedRows");
                var UserName = ClaimHelper.GetClainByName(_httpContextAccessor, "UserName");
                worksheet.Cells[1, 1].Value = "Họ tên khách hàng";
                worksheet.Cells[1, 2].Value = "PhoneNumber";
                worksheet.Cells[1, 3].Value = "Căn cước công dân";
                worksheet.Cells[1, 4].Value = "Email";
                worksheet.Cells[1, 5].Value = "Ngày đăng kí thành công";
                worksheet.Cells[1, 6].Value = "Dự án";
                worksheet.Cells[1, 7].Value = "Sản phẩm";
                worksheet.Cells[1, 8].Value = "Code Sale";
                worksheet.Cells[1, 9].Value = "Tên Team";
                worksheet.Cells[1, 10].Value = "Tên Quản Lý";
                worksheet.Cells[1, 11].Value = "Tên Sale ";

                for (int i = 1; i <= 4; i++)
                {
                    worksheet.Cells[1, 11 + i].Value = $"Ảnh {i}";
                }

                for (int i = 0; i < data.Data.Data.Count; i++)
                {
                    var dto = data.Data.Data[i];
                    var GetallImg = _customerlinkImageRepository.GetAll().Where(x => x.CustomerLinkId == dto.Id).ToList();
                    var getsale = _userespository.FindById(dto.Iduser);
                    var getleader = _userespository.FindByPredicate(x => x.TeamId == dto.TeamId).ToList();
                    var convertedItems = _mapper.Map<List<CustomerlinkImageDto>>(GetallImg);
                    dto.ListCustomerlinkImage = new List<CustomerlinkImageDto>();
                    dto.ListCustomerlinkImage?.AddRange(convertedItems);
                    worksheet.Cells[i + 2, 1].Value = dto.Name;
                    worksheet.Cells[i + 2, 2].Value = dto.PhoneNumber;
                    worksheet.Cells[i + 2, 3].Value = dto.Passport;
                    worksheet.Cells[i + 2, 4].Value = dto.Email;
                    worksheet.Cells[i + 2, 5].Value = dto.CreatedOn.Value.ToString("dd/MM/yyyy-HH:mm:ss");
                    worksheet.Cells[i + 2, 6].Value = dto.BankName;
                    worksheet.Cells[i + 2, 7].Value = dto.CamPaignName;
                    worksheet.Cells[i + 2, 8].Value = dto.RefferalCode;
                    worksheet.Cells[i + 2, 9].Value = dto.TeamName;
                    var leader = "";


                    foreach (var t in getleader)
                    {
                        var roles = await _userManager.GetRolesAsync(t);
                        bool isTeamLeader = roles.Contains("Teamleader");
                        if (isTeamLeader)
                        {
                            leader = t.UserName;
                            break;
                        }
                    }
                    worksheet.Cells[i + 2, 10].Value = (getsale.RefferalCode != null) ? leader : "";
                    worksheet.Cells[i + 2, 11].Value = (getsale.RefferalCode != null) ? getsale.UserName : "";
                    for (int j = 0; j < GetallImg.Count; j++)
                    {
                        worksheet.Cells[i + 2, 11 + j + 1].Value = dto.ListCustomerlinkImage[j].LinkImage;
                    }



                }


                return package.GetAsByteArray();
            }
        }
        private async Task<AppResponse<SearchResponse<CustomerLinkDto>>> Export(SearchRequest request)
        {
            var result = new AppResponse<SearchResponse<CustomerLinkDto>>();

            try
            {
                var userRole = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
                var query = await BuildFilterExpression(request.Filters);
                var numOfRecords = _customerLinkRepository.CountRecordsByPredicate(query);
                var model = _customerLinkRepository.FindByPredicate(query).OrderByDescending(p => p.CreatedOn);
                var teamNames = _teamRespository.GetAllTeamNames();
                var List = model.Include(x => x.Customer).Include(x => x.LinkTemplate.Bank).Include(x => x.LinkTemplate.Campaign)
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
                        Iduser = x.Customer.ApplicationUser.Id,
                        TpBank = x.Customer.ApplicationUser.TpBank,
                        RefferalCode = x.Customer.ApplicationUser.RefferalCode,
                        UserName = x.Customer.ApplicationUser.UserName,
                        TeamName = x.Customer.ApplicationUser.TeamId.HasValue && teamNames.ContainsKey(x.Customer.ApplicationUser.TeamId.Value)
        ? teamNames[x.Customer.ApplicationUser.TeamId.Value]
        : string.Empty,
                        InforCustomer = String.Format("Tên:{0}; Email:{1}; CCCD:{2}; phone:{3}  ", x.Customer.Name, x.Customer.Email, x.Customer.Passport, x.Customer.PhoneNumber)
                    })
                    .ToList();


                var searchUserResult = new SearchResponse<CustomerLinkDto>
                {
                    TotalRows = numOfRecords,
                    TotalPages = 1,
                    CurrentPage = 1,
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

        public AppResponse<CustomerLinkDto> Create(CustomerLinkDto request)
        {
            var result = new AppResponse<CustomerLinkDto>();
            try
            {
                if(request.CustomerId == null)
                {
                    return result.BuildError("Không bỏ trống khách hàng");
                }
                var customer = _customerRespository.FindBy(x => x.Id == request.CustomerId);
                if (customer.Count() == 0)
                {
                    return result.BuildError("Không tìm thấy khách hàng");
                }
                if (request.LinkTemplateId == null)
                {
                    return result.BuildError("Không bỏ trống liên kết");
                }
                var linkTemplate = _linkTemplateRepository.FindBy(x => x.Id == request.LinkTemplateId);
                if (linkTemplate.Count() == 0)
                {
                    return result.BuildError("Không tìm thấy liên kết");
                }
                var CheckCustomerLink =_customerLinkRepository.FindBy(x=>x.CustomerId == request.CustomerId && x.LinkTemplateId == request.LinkTemplateId);
                if(CheckCustomerLink.Count() != 0)
                {
                    var checkIsDelete = CheckCustomerLink.First();
                    if (CheckCustomerLink.First().IsDeleted == true)
                    {
                        checkIsDelete.IsDeleted = false;
                        _customerLinkRepository.Edit(checkIsDelete);
                    }
                    return result.BuildResult(request);
                }
                
                var customerLink = _mapper.Map<Customerlink>(request);
                customerLink.Id = Guid.NewGuid();

                _customerLinkRepository.Add(customerLink);

                request.Id = customerLink.Id;
                result.BuildResult(request);
            }
            catch(Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }
    }
}
