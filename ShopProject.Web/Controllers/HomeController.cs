using AutoMapper;
using ShopProject.Common;
using ShopProject.Model.Models;
using ShopProject.Service;
using ShopProject.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopProject.Web.Controllers
{
    public class HomeController : Controller
    {
        IProductCategoryService _productCategoryService;
        ICommonService _commonService;
        IProductService _productService;
        IGroupService _groupService;
        public HomeController(IProductCategoryService productCategoryService, IProductService productService,ICommonService commonService, IGroupService groupService)
        {
            _productCategoryService = productCategoryService;
            _commonService = commonService;
            _productService = productService;
            _groupService = groupService;
        }

        [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            var slideModel = _commonService.GetSlide();
            var slideView = Mapper.Map<IEnumerable<Slide>, IEnumerable<SlideViewModel>>(slideModel);
            var homeViewModel = new HomeViewModel();
            homeViewModel.Slides = slideView;
            var lastestProductModel = _productService.GetLastest(6);
            var lstGroupActice = _groupService.GetGroupActive();
            var lstGroupActiceViewModel = Mapper.Map<IEnumerable<Group>, IEnumerable<GroupViewModel>>(lstGroupActice);
            foreach (var group in lstGroupActiceViewModel)
            {
                group.Products = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(_groupService.GetListProductByGroupActive(group.ID, 6));

            }
            var lastestProductViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(lastestProductModel);
            homeViewModel.LastestProducts = lastestProductViewModel;
            homeViewModel.GroupActice = lstGroupActiceViewModel;


            try
            {
                homeViewModel.Title = _commonService.GetSystemConfig(CommonConstants.HomeTitle).ValueString;
                homeViewModel.MetaKeyword = _commonService.GetSystemConfig(CommonConstants.HomeMetaKeyword).ValueString;
                homeViewModel.MetaDescription = _commonService.GetSystemConfig(CommonConstants.HomeMetaDescription).ValueString;
            }
            catch
            {

            }

            return View(homeViewModel);
        }


        [ChildActionOnly]
        [OutputCache(Duration = 3600)]
        public ActionResult Footer()
        {
            var footerModel = _commonService.GetFooter();
            var footerViewModel = Mapper.Map<Footer, FooterViewModel>(footerModel);
            return PartialView(footerViewModel);
        }

        [ChildActionOnly]
        public ActionResult Header()
        {
            return PartialView();
        }

        [ChildActionOnly]
        [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Category()
        {
            var lstGroup= _groupService.GetGroupActive();
            var lstGroupViewModel = Mapper.Map<IEnumerable<Group>, IEnumerable<GroupViewModel>>(lstGroup);
            foreach (var item in lstGroupViewModel)
            {
                item.Products = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(_groupService.GetListProductByGroupActive(item.ID, 6));

            }

            var model = _productCategoryService.GetAllByStatus();
            var lstProductCategoryVm = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(model);

            var categoryHomeVm = new CategoryHomeViewModel();
            categoryHomeVm.MenuCategory = lstProductCategoryVm;
            categoryHomeVm.Groups = lstGroupViewModel;
            return PartialView(categoryHomeVm);
        }
    }
}