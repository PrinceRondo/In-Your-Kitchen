using InYourFridge.Models;
using InYourFridge.ResponseModels;
using InYourFridge.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InYourFridge.Repository
{
    public interface IAccountRepository
    {
        Task<GenericResponseModel> RegisterUserAsync(ProfileViewModel model);
        Task<IList<UserProfile>> GetAllUserAsync();
        Task<UserProfile> GetUserProfileByUserIdAsync(string userId);
    }
    public interface IPointRepository
    {
        Task<UserPoint> GetUserPointAsync(string userId);
    }
    public interface IEntryTypeRepository
    {
        Task<IEnumerable<EntryType>> GetAllEntryType();
        Task<GenericResponseModel> SaveAsync(EntryType model);
        Task<EntryType> FindByNameAsync(string name);
    }
    public interface IStatusRepository
    {
        Task<IEnumerable<Status>> GetAllStatus();
        Task<GenericResponseModel> SaveAsync(Status model);
        Task<Status> FindByNameAsync(string name);
    }
    public interface IKitchenItemCategoryRepository
    {
        Task<IEnumerable<KitchenItemCategory>> GetAllCategory();
        Task<GenericResponseModel> SaveAsync(KitchenItemCategory model);
        Task<KitchenItemCategory> FindByNameAsync(string name);
    }
    public interface IKitchenItemRepository
    {
        Task<IList<KitchenItemResponseModel>> GetKitchenItem(int? categoryId, string? userId, int? statusId, int entryTypeId);
        Task<GenericResponseModel> GetItemByIdAsync(long id);
        Task<GenericResponseModel> SaveAsync(KitchenItemViewModel model);
        Task<GenericResponseModel> UpdateAsync(long id, KitchenItemViewModel model);
        Task<GenericResponseModel> DeleteAsync(long id, int entryTypeId);
        Task<GenericResponseModel> ApproveAsync(long id, string userId);
        Task<GenericResponseModel> DeclineAsync(long id);
    }

    public interface IKitchenProductRepository
    {
        Task<IList<SuperKitchenProduct>> GetKitchenProduct(string? userId, int? statusId, int entryTypeId);
        Task<GenericResponseModel> GetItemByIdAsync(long id);
        Task<GenericResponseModel> SaveAsync(KitchenProductViewModel model);
        Task<GenericResponseModel> UpdateAsync(long id, KitchenProductViewModel model);
        Task<GenericResponseModel> DeleteAsync(long id, int entryTypeId);
        Task<GenericResponseModel> ApproveAsync(long id, string userId);
        Task<GenericResponseModel> DeclineAsync(long id);
    }
}
