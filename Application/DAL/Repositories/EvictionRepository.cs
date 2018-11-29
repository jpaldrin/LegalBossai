﻿using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class EvictionRepository : Repository<Eviction>, IEvictionsRepository
    {
        public EvictionRepository(DbContext context) : base(context)
        { }




        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
    }
}
