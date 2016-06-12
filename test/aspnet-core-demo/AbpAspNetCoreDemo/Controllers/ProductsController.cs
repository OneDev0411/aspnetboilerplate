﻿using System.Collections.Generic;
using Abp.AspNetCore.Mvc.Controllers;
using Abp.Auditing;
using Abp.Dependency;
using Abp.UI;
using AbpAspNetCoreDemo.Core.Application;
using AbpAspNetCoreDemo.Core.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbpAspNetCoreDemo.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : AbpController
    {
        private readonly IProductAppService _productAppService;

        public ProductsController(IProductAppService productAppService, IAuditInfoProvider auditInfoProvider, IIocManager iocManager)
        {
            _productAppService = productAppService;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<ProductDto> Get()
        {
            return _productAppService.GetAll();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            throw new UserFriendlyException("A test exception message");
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
