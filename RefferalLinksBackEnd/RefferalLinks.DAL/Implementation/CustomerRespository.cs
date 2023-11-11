﻿using Maynghien.Common.Repository;
using RefferalLinks.DAL.Contract;
using RefferalLinks.DAL.Models.Context;
using RefferalLinks.DAL.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefferalLinks.DAL.Implementation
{
    public class CustomerRespository : GenericRepository<Customer, RefferalLinksDbContext, ApplicationUser>, ICustomerRespository
    {
        public CustomerRespository(RefferalLinksDbContext unitOfWork) : base(unitOfWork)
        {
        }
    }
}