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
    public class AdminController : Controller
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IPointRepository pointRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IKitchenItemRepository kitchenItemRepository;
        private readonly IKitchenItemCategoryRepository categoryRepository;
        private readonly IStatusRepository statusRepository;
        private readonly IEntryTypeRepository entryTypeRepository;
        string userId = string.Empty;

        public AdminController(IHttpContextAccessor httpContextAccessor, IPointRepository pointRepository, IAccountRepository accountRepository,
            IKitchenItemRepository kitchenItemRepository, IKitchenItemCategoryRepository categoryRepository, IStatusRepository statusRepository,
            IEntryTypeRepository entryTypeRepository)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.pointRepository = pointRepository;
            this.accountRepository = accountRepository;
            this.kitchenItemRepository = kitchenItemRepository;
            this.categoryRepository = categoryRepository;
            this.statusRepository = statusRepository;
            this.entryTypeRepository = entryTypeRepository;
            userId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            var users = await accountRepository.GetAllUserAsync();

            DashboardViewModel viewModel = new DashboardViewModel
            {
                CountUser = users.Count
            };
            return View(viewModel);
        }
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateKitchenItem()
        {
            KitchenItemViewModel viewModel = new KitchenItemViewModel
            {
                Categories = await categoryRepository.GetAllCategory(),
                CreatedBy = userId,
                EntryTypeId = 1
            };
            ViewBag.Code = null;
            return View(viewModel);
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateKitchenItem(KitchenItemViewModel model)
        {
            //To populate dropdown if the page reload 
            KitchenItemViewModel viewModel = new KitchenItemViewModel
            {
                Categories = await categoryRepository.GetAllCategory(),
                CreatedBy = userId,
                EntryTypeId = 1
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
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> MyKitchenItem(int code, string message)
        {
            GetKitchenItemViewModel viewModel = new GetKitchenItemViewModel
            {
                KitchenItemCategories = await categoryRepository.GetAllCategory(),
                Statuses = await statusRepository.GetAllStatus(),
                KitchenItems = await kitchenItemRepository.GetKitchenItem(0, userId, 0, 1)
            };
            ViewBag.code = code;
            ViewBag.message = message;
            return View(viewModel);
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AllKitchenItem(int code, string message)
        {
            GetKitchenItemViewModel viewModel = new GetKitchenItemViewModel
            {
                KitchenItemCategories = await categoryRepository.GetAllCategory(),
                Statuses = await statusRepository.GetAllStatus(),
                UserProfiles = await accountRepository.GetAllUserAsync(),
                EntryTypes = await entryTypeRepository.GetAllEntryType(),
                KitchenItems = await kitchenItemRepository.GetKitchenItem(0, null, 0, 0)
            };
            ViewBag.code = code;
            ViewBag.message = message;
            return View(viewModel);
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AllKitchenItem(GetKitchenItemViewModel model)
        {
            GetKitchenItemViewModel responseModel = new GetKitchenItemViewModel
            {
                KitchenItemCategories = await categoryRepository.GetAllCategory(),
                Statuses = await statusRepository.GetAllStatus(),
                UserProfiles = await accountRepository.GetAllUserAsync(),
                EntryTypes = await entryTypeRepository.GetAllEntryType(),
                KitchenItems = await kitchenItemRepository.GetKitchenItem(model.CategoryId, model.UserId, model.StatusId, model.EntryTypeId)
            };
            return View(responseModel);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteKitchenItem(long id)
        {
            var response = await kitchenItemRepository.DeleteAsync(id, 1);
            return RedirectToAction("AllKitchenItem", new { code = response.StatusCode, message = response.StatusMessage });
        }
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ApproveKitchenItem(long id)
        {
            GenericResponseModel responseModel = await kitchenItemRepository.ApproveAsync(id, userId);
            return RedirectToAction("AllKitchenItem", new { code = responseModel.StatusCode, message = responseModel.StatusMessage });
        }
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeclineKitchenItem(long id)
        {
            GenericResponseModel responseModel = await kitchenItemRepository.DeclineAsync(id);
            return RedirectToAction("AllKitchenItem", new { code = responseModel.StatusCode, message = responseModel.StatusMessage });
        }
    }
}
