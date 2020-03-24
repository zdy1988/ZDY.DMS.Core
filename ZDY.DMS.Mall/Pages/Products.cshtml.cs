using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Zdy.Mall.TagHelpers;
using ZDY.DMS.Application.Untils;
using ZDY.DMS.DataObjects;
using ZDY.DMS.Models;
using ZDY.DMS.ServiceContracts;
using ZDY.DMS.Untils.Enums;

namespace Zdy.Mall.Pages
{
    public class ProductsModel : PageModel
    {
        private IProductService _productService;
        private IMapper _mapper;

        public ProductsModel(IMapper mapper,
            IProductService productService)
        {
            _mapper = mapper;
            _productService = productService;
        }

        public ProductDataObjectList Collections { get; set; }

        public PagerOption PagerOption { get; set; }

        public async Task OnGet(int i = 1, string word = "", string tag = "")
        {
            int size = 9;

            var products = await _productService.FindAsync(i, size, t => t.Desc(a => a.EnterTime), t => t.IsDisabled == false && t.ProductState == (int)ProductState.OnShelf);

            Collections = _mapper.Map<IEnumerable<Product>, ProductDataObjectList>(products.Item1);

            foreach (var item in Collections)
            {
                var url = StaticFileManager.GetFileServerUrl(item.Cover);
                if (string.IsNullOrEmpty(url))
                {
                    url = "/images/no-image.jpg";
                }
                item.CoverUrl = url;
            }

            PagerOption = new PagerOption
            {
                PageIndex = i,
                PageSize = size,
                TotalCount = products.Item2,
                RouteUrl = "/Products"
            };
        }
    }
}