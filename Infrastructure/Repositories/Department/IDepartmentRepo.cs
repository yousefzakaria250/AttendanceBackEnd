﻿using Data;
using Infrastructure.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IDepartmentRepo
    {
        public Task<dynamic> Add(DepartmentDto dto);
        public Task<List<Department>> GetAll();

    }
}
