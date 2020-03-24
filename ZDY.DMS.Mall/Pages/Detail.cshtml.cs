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

namespace Zdy.Mall.Pages
{
    public class DetailModel : PageModel
    {
        private IProductService _productService;
        private IFileService _fileService;
        private IMapper _mapper;

        public DetailModel(IMapper mapper,
            IProductService productService,
            IFileService fileService)
        {
            _mapper = mapper;
            _productService = productService;
            _fileService = fileService;
        }

        public ProductDataObject Product { get; set; }

        public async Task OnGet(int id)
        {
            if (id != 0)
            {
                var product = await _productService.GetAsync(t => t.ID == id && t.IsDisabled == false);

                Product = _mapper.Map<Product, ProductDataObject>(product);

                Product.CoverUrl = StaticFileManager.GetFileServerUrl(Product.Cover);
                var images = await _fileService.FindAsync(t => t.BusinessID == Product.ID && t.Type == "ProductImages");
                var imageDataObjectList = _mapper.Map<IEnumerable<File>, FileDataObjectList>(images);
                foreach (var item in imageDataObjectList)
                {
                    item.Url = item.GetFileServerUrl();
                }
                Product.Images = imageDataObjectList;
            }
        }
    }
}