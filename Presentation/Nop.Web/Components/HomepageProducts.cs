using System.Collections.Generic;
using System.Linq;
using LinqToDB.Tools;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Services.Caching;
using Nop.Services.Catalog;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;
using Nop.Web.Infrastructure.Cache;

namespace Nop.Web.Components
{
    public class HomepageProductsViewComponent : NopViewComponent
    {
        private readonly CatalogSettings _catalogSettings;
        private readonly IAclService _aclService;
        private readonly ICacheKeyService _cacheKeyService;
        private readonly IProductModelFactory _productModelFactory;
        private readonly IProductService _productService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IStoreContext _storeContext;
        private readonly IOrderReportService _orderReportService;
        private readonly IStaticCacheManager _staticCacheManager;

        public HomepageProductsViewComponent(CatalogSettings catalogSettings,
            IAclService aclService,
              ICacheKeyService cacheKeyService,
                  IOrderReportService orderReportService,
            IProductModelFactory productModelFactory,
            IProductService productService,
            IStoreMappingService storeMappingService,
             IStoreContext storeContext, 
             IStaticCacheManager staticCacheManager)
        {
            _catalogSettings = catalogSettings;
            _aclService = aclService;
            _cacheKeyService = cacheKeyService;
            _orderReportService = orderReportService;
            _productModelFactory = productModelFactory;
            _productService = productService;
            _storeMappingService = storeMappingService;
            _storeContext = storeContext;
            _staticCacheManager = staticCacheManager;
        }

        public IViewComponentResult Invoke(int? productThumbPictureSize)
        {
           
            var products = _productService.GetAllProductsDisplayedOnHomepage();
            if (!products.Any() && _catalogSettings.NumberOfFeaturedProductsOnHomepage == 0)
                return Content("");
            //ACL and store mapping
                products = products.Where(p => _aclService.Authorize(p) && _storeMappingService.Authorize(p)).ToList();
            //availability dates
            products = products.Where(p => _productService.ProductIsAvailable(p)).ToList();

            products = products.Where(p => p.VisibleIndividually).ToList();
            products = products.Take(_catalogSettings.NumberOfFeaturedProductsOnHomepage).ToList();

            var model = _productModelFactory.PrepareProductOverviewModels(products, true, true, productThumbPictureSize).ToList();

            return View(model);
           
        }
    }
}