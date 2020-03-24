using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDY.DMS.Application.Untils;
using ZDY.DMS.DataObjects;
using ZDY.DMS.Models;
using ZDY.DMS.ServiceContracts;
using ZDY.DMS.Untils.Enums;

namespace Zdy.Mall.Pages
{
    public class IndexModel : PageModel
    {
        private IProductService _productService;
        private IMapper _mapper;

        public IndexModel(IMapper mapper,
            IProductService productService)
        {
            _mapper = mapper;
            _productService = productService;
        }

        public ProductDataObjectList NewCollections { get; set; }

        public async Task OnGet()
        {
            var products = (await _productService.FindAsync(1, 8, t => t.Desc(a => a.EnterTime), t => t.IsDisabled == false && t.ProductState == (int)ProductState.OnShelf)).Item1;

            NewCollections = _mapper.Map<IEnumerable<Product>, ProductDataObjectList>(products);

            foreach (var item in NewCollections)
            {
                var url = StaticFileManager.GetFileServerUrl(item.Cover);
                if (string.IsNullOrEmpty(url))
                {
                    url = "/images/no-image.jpg";
                }
                item.CoverUrl = url;
            }
        }
    }
}
