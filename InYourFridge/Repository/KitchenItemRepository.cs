using InYourFridge.Data;
using InYourFridge.Helper;
using InYourFridge.Models;
using InYourFridge.ResponseModels;
using InYourFridge.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InYourFridge.Repository
{
    public class KitchenItemRepository : IKitchenItemRepository
    {
        private readonly AppDbContext dbContext;
        private readonly Utility utility;

        public KitchenItemRepository(AppDbContext dbContext, Utility utility)
        {
            this.dbContext = dbContext;
            this.utility = utility;
        }

        public async Task<GenericResponseModel> ApproveAsync(long id, string userId)
        {
            try
            {
                var record = await dbContext.KitchenItems.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (record != null)
                {
                    record.StatusId = 2;
                    record.ApprovedBy = userId;
                    record.ApprovedAt = DateTime.Now;
                    //Add point for user for adding item
                    var userPoint = await dbContext.UserPoints.Where(x => x.UserId == record.CreatedBy).FirstOrDefaultAsync();
                    userPoint.Point += 1;

                    await dbContext.SaveChangesAsync();
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Kitchen Item Approved Successfully" };
                }
                return new GenericResponseModel { StatusCode = 404, StatusMessage = "Kitchen Item not found" };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new GenericResponseModel { StatusCode = 500, StatusMessage = ex.Message };
            }
        }

        public async Task<GenericResponseModel> DeclineAsync(long id)
        {
            try
            {
                var record = await dbContext.KitchenItems.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (record != null)
                {
                    record.StatusId = 4;
                    await dbContext.SaveChangesAsync();
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Kitchen Item Declined Successfully" };
                }
                return new GenericResponseModel { StatusCode = 404, StatusMessage = "Kitchen Item not found" };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new GenericResponseModel { StatusCode = 500, StatusMessage = ex.Message };
            }
        }

        public async Task<GenericResponseModel> DeleteAsync(long id, int entryTypeId)
        {
            try
            {
                var record = await dbContext.KitchenItems.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (record != null)
                {
                    //check if it's user or admin that wanty to delete
                    if (record.StatusId == 2 && entryTypeId == 2)
                    {
                        return new GenericResponseModel { StatusCode = 300, StatusMessage = "Your Kitchen Item has been processed, it can't be deleted" };
                    }
                    dbContext.KitchenItems.Remove(record);
                    await dbContext.SaveChangesAsync();
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Kitchen Item Deleted Successfully" };
                }
                return new GenericResponseModel { StatusCode = 404, StatusMessage = "Kitchen Item not found" };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new GenericResponseModel { StatusCode = 500, StatusMessage = ex.Message };
            }
        }

        public Task<GenericResponseModel> GetItemByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<KitchenItemResponseModel>> GetKitchenItem(int? categoryId, string userId, int? statusId, int entryTypeId)
        {
            var record = await dbContext.KitchenItems.Select(x => new KitchenItemResponseModel
            {
                ApprovedAt = x.ApprovedAt,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                CategoryId = x.CategoryId,
                Category = x.KitchenItemCategory.CategoryName,
                CreatedBy = x.CreatedBy,
                CreatedByName = x.ApplicationUser.FirstName + " " + x.ApplicationUser.LastName,
                EntryTypeId = x.EntryTypeId,
                EntryType = x.EntryType.Type,
                HealthBenefit = x.HealthBenefit,
                Id = x.Id,
                ItemName = x.ItemName,
                StatusId = x.StatusId,
                Status = x.Status.StatusName,
                UpdatedBy = x.ApplicationUser1.FirstName + " " + x.ApplicationUser1.LastName
            }).OrderByDescending(x => x.Id).ToListAsync();
            //Filter
            if (categoryId > 0)
            {
                record = record.Where(x => x.CategoryId == categoryId).ToList();
            }
            if (userId != null && userId != "0")
            {
                record = record.Where(x => x.CreatedBy == userId).ToList();
            }
            if (statusId > 0)
            {
                record = record.Where(x => x.StatusId == statusId).ToList();
            }
            if (entryTypeId > 0)
            {
                record = record.Where(x => x.EntryTypeId == entryTypeId).ToList();
            }
            return record;
        }

        public async Task<GenericResponseModel> SaveAsync(KitchenItemViewModel model)
        {
            try
            {
                //check if the record exists
                var checkRecord = await dbContext.KitchenItems.Where(x => x.ItemName == model.ItemName && x.CategoryId == model.CategoryId && x.StatusId != 4).FirstOrDefaultAsync();
                if (checkRecord != null)
                {
                    return new GenericResponseModel { StatusCode = 300, StatusMessage = "Item is already on the pipeline" };
                }
                //save new item
                KitchenItem kitchenItem = new KitchenItem
                {
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CategoryId = model.CategoryId,
                    CreatedBy = model.CreatedBy,
                    EntryTypeId = model.EntryTypeId,
                    HealthBenefit = model.HealthBenefit,
                    ItemName = model.ItemName,
                    StatusId = 1,
                    UpdatedBy = model.CreatedBy
                };
                await dbContext.KitchenItems.AddAsync(kitchenItem);
                await dbContext.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Item Created Successfully" };
            }
            catch (Exception ex)
            {
                //save error to db
                ErrorLog errorLog = new ErrorLog
                {
                    ErrorDate = DateTime.Now,
                    ErrorMessage = ex.Message,
                    ErrorSource = ex.Source,
                    ErrorStackTrace = ex.StackTrace
                };
                dbContext.ErrorLogs.Add(errorLog);
                dbContext.SaveChanges();
                //save error to file
                utility.SaveErrorMessage(ex);
                return new GenericResponseModel { StatusCode = 500, StatusMessage = ex.Message };
            }
        }

        public Task<GenericResponseModel> UpdateAsync(long id, KitchenItemViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
