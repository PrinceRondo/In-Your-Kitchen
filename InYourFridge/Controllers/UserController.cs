using InYourFridge.Models;
using InYourFridge.Repository;
using InYourFridge.ResponseModels;
using InYourFridge.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InYourFridge.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IPointRepository pointRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IKitchenItemRepository kitchenItemRepository;
        private readonly IKitchenItemCategoryRepository categoryRepository;
        private readonly IStatusRepository statusRepository;
        private readonly IKitchenProductRepository kitchenProductRepository;
        string userId = string.Empty;

        public UserController(IHttpContextAccessor httpContextAccessor, IPointRepository pointRepository, IAccountRepository accountRepository,
            IKitchenItemRepository kitchenItemRepository, IKitchenItemCategoryRepository categoryRepository, IStatusRepository statusRepository,
            IKitchenProductRepository kitchenProductRepository)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.pointRepository = pointRepository;
            this.accountRepository = accountRepository;
            this.kitchenItemRepository = kitchenItemRepository;
            this.categoryRepository = categoryRepository;
            this.statusRepository = statusRepository;
            this.kitchenProductRepository = kitchenProductRepository;
            userId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Index()
        {
            string firstName = string.Empty;
            var userPoint = await pointRepository.GetUserPointAsync(userId);
            var userProfile = await accountRepository.GetUserProfileByUserIdAsync(userId);
            if(userProfile != null)
            {
                firstName = userProfile.FirstName;
            }

            DashboardViewModel viewModel = new DashboardViewModel
            {
                UserPoint = userPoint.Point,
                FirstName = firstName,
            };
            return View(viewModel);
        }
        //Kitchen Item
        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> CreateKitchenItem()
        {
            KitchenItemViewModel viewModel = new KitchenItemViewModel
            {
                Categories = await categoryRepository.GetAllCategory(),
                CreatedBy = userId,
                EntryTypeId = 2
            };
            ViewBag.Code = null;
            return View(viewModel);
        }
        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> CreateKitchenItem(KitchenItemViewModel model)
        {
            //To populate dropdown if the page reload 
            KitchenItemViewModel viewModel = new KitchenItemViewModel
            {
                Categories = await categoryRepository.GetAllCategory(),
                CreatedBy = userId,
                EntryTypeId = 2
            };
            if (ModelState.IsValid)
            {
                var response = await kitchenItemRepository.SaveAsync(model);
                if (response.StatusCode == 200)
                {
                    ViewBag.Message = response.StatusMessage;
                    ViewBag.Code = response.StatusCode;
                    ModelState.Clear();
                    return View(viewModel);
                }
                else
                {
                    ViewBag.Message = response.StatusMessage;
                    ViewBag.Code = response.StatusCode;
                    return View(viewModel);
                }
            }
            ViewBag.Message = "Invalid Input";
            ViewBag.Code = 302;
            return View(viewModel);
        }
        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> MyKitchenItem(int code, string message)
        {
            GetKitchenItemViewModel viewModel = new GetKitchenItemViewModel
            {
                KitchenItemCategories = await categoryRepository.GetAllCategory(),
                Statuses = await statusRepository.GetAllStatus(),
                KitchenItems = await kitchenItemRepository.GetKitchenItem(0, userId, 0, 0)
            };
            ViewBag.code = code;
            ViewBag.message = message;
            return View(viewModel);
        }
        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> MyKitchenItem(GetKitchenItemViewModel model)
        {
            GetKitchenItemViewModel responseModel = new GetKitchenItemViewModel
            {
                KitchenItemCategories = await categoryRepository.GetAllCategory(),
                Statuses = await statusRepository.GetAllStatus(),
                KitchenItems = await kitchenItemRepository.GetKitchenItem(model.CategoryId, userId, model.StatusId, 2)
            };
            return View(responseModel);
        }
        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> AllKitchenItem(int code, string message)
        {
            GetKitchenItemViewModel viewModel = new GetKitchenItemViewModel
            {
                KitchenItemCategories = await categoryRepository.GetAllCategory(),
                Statuses = await statusRepository.GetAllStatus(),
                KitchenItems = await kitchenItemRepository.GetKitchenItem(0, null, 2, 0)
            };
            ViewBag.code = code;
            ViewBag.message = message;
            return View(viewModel);
        }
        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> AllKitchenItem(GetKitchenItemViewModel model)
        {
            GetKitchenItemViewModel responseModel = new GetKitchenItemViewModel
            {
                KitchenItemCategories = await categoryRepository.GetAllCategory(),
                Statuses = await statusRepository.GetAllStatus(),
                KitchenItems = await kitchenItemRepository.GetKitchenItem(model.CategoryId, null,2,0)
            };
            return View(responseModel);
        }
        [Authorize(Roles = "user")]
        public async Task<IActionResult> DeleteKitchenItem(long id)
        {
            var response = await kitchenItemRepository.DeleteAsync(id, 2);
            return RedirectToAction("MyKitchenItem", new { code = response.StatusCode, message = response.StatusMessage });
        }

        //Kitchen Product
        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> CreateKitchenProduct()
        {
            KitchenProductViewModel viewModel = new KitchenProductViewModel
            {
                Categories = await categoryRepository.GetAllCategory(),
                CreatedBy = userId,
                EntryTypeId = 2
            };
            ViewBag.Code = null;
            return View(viewModel);
        }
        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> CreateKitchenProduct(KitchenProductViewModel model)
        {
            //To populate dropdown if the page reload 
            KitchenProductViewModel viewModel = new KitchenProductViewModel
            {
                Categories = await categoryRepository.GetAllCategory(),
                CreatedBy = userId,
                EntryTypeId = 2
            };
            if (ModelState.IsValid)
            {
                var response = await kitchenProductRepository.SaveAsync(model);
                if (response.StatusCode == 200)
                {
                    ViewBag.Message = response.StatusMessage;
                    ViewBag.Code = response.StatusCode;
                    ModelState.Clear();
                    return View(viewModel);
                }
                else
                {
                    ViewBag.Message = response.StatusMessage;
                    ViewBag.Code = response.StatusCode;
                    return View(viewModel);
                }
            }
            ViewBag.Message = "Invalid Input";
            ViewBag.Code = 302;
            return View(viewModel);
        }
        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> MyKitchenProduct(int code, string message)
        {
            GetKitchenProductViewModel viewModel = new GetKitchenProductViewModel
            {
                Statuses = await statusRepository.GetAllStatus(),
                KitchenProducts = await kitchenProductRepository.GetKitchenProduct(userId, 0, 0)
            };
            ViewBag.code = code;
            ViewBag.message = message;
            return View(viewModel);
        }
        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> MyKitchenProduct(GetKitchenItemViewModel model)
        {
            GetKitchenProductViewModel responseModel = new GetKitchenProductViewModel
            {
                Statuses = await statusRepository.GetAllStatus(),
                KitchenProducts = await kitchenProductRepository.GetKitchenProduct(userId, model.StatusId, 0)
            };
            return View(responseModel);
        }
        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> AllKitchenProduct(int code, string message)
        {
            GetKitchenProductViewModel viewModel = new GetKitchenProductViewModel
            {
                Statuses = await statusRepository.GetAllStatus(),
                KitchenProducts = await kitchenProductRepository.GetKitchenProduct(null,2, 0)
            };
            ViewBag.code = code;
            ViewBag.message = message;
            return View(viewModel);
        }
        //[HttpPost]
        //[Authorize(Roles = "user")]
        //public async Task<IActionResult> AllKitchenProduct(GetKitchenItemViewModel model)
        //{
        //    GetKitchenItemViewModel responseModel = new GetKitchenItemViewModel
        //    {
        //        KitchenItemCategories = await categoryRepository.GetAllCategory(),
        //        Statuses = await statusRepository.GetAllStatus(),
        //        KitchenItems = await kitchenItemRepository.GetKitchenItem(model.CategoryId, null, 2, 0)
        //    };
        //    return View(responseModel);
        //}
        [Authorize(Roles = "user")]
        public async Task<IActionResult> DeleteKitchenProduct(long id)
        {
            var response = await kitchenProductRepository.DeleteAsync(id, 2);
            return RedirectToAction("MyKitchenProduct", new { code = response.StatusCode, message = response.StatusMessage });
        }

        //drop down
        [HttpPost]
        public async Task<JsonResult> GetItem(int categoryId)
        {
            IList<KitchenItemResponseModel> items = await kitchenItemRepository.GetKitchenItem(categoryId, null, 0, 0);
            return Json(items);
        }
    }
}
