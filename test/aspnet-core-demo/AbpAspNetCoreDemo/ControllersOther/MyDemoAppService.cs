﻿using System.Collections.Generic;
using System.Linq;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using AbpAspNetCoreDemo.Core.Application.Dtos;
using AbpAspNetCoreDemo.Core.Domain;
using Abp.AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AbpAspNetCoreDemo.ControllersOther
{
    [Controller]
    [Route("api/demo-products")]
    public class MyDemoAppService : ApplicationService
    {
        private readonly IRepository<Product> _productRepository;

        public MyDemoAppService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public List<ProductDto> Get()
        {
            //throw new UserFriendlyException("just testing...");
            return _productRepository.GetAll().ToList().MapTo<List<ProductDto>>();
        }
    }
}
