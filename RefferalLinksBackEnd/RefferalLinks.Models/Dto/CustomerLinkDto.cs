﻿using MayNghien.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefferalLinks.Models.Dto
{
    public class CustomerLinkDto : BaseDto
    {
        public Guid CustomerId { get; set; }
        public string Url { get; set; }

        public Guid LinkTemplateId { get; set; }
        public string? InforCustomer { get;set; }
        public string? Name { get; set; }
        public string? Passport { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Cccd { get; set; }
        public string? BankName { get; set; }
        public string? CamPainNamme { get; set; }
        public Guid? TeamId { get; set; }
        public string? UserName { get; set; }
    }
}
