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
    public class KitchenProductRepository : IKitchenProductRepository
    {
        private readonly AppDbContext dbContext;
        private readonly Utility utility;

        public KitchenProductRepository(AppDbContext dbContext, Utility utility)
        {
            this.dbContext = dbContext;
            this.utility = utility;
        }

        public async Task<GenericResponseModel> ApproveAsync(long id, string userId)
        {
            try
            {
                var record = await dbContext.KitchenProducts.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (record != null)
                {
                    record.StatusId = 2;
                    record.ApprovedBy = userId;
                    record.ApprovedAt = DateTime.Now;
                    await dbContext.SaveChangesAsync();
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Kitchen Product Approved Successfully" };
                }
                return new GenericResponseModel { StatusCode = 404, StatusMessage = "Kitchen Product not found" };
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
                var record = await dbContext.KitchenProducts.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (record != null)
                {
                    record.StatusId = 4;
                    await dbContext.SaveChangesAsync();
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Kitchen Product Declined Successfully" };
                }
                return new GenericResponseModel { StatusCode = 404, StatusMessage = "Kitchen Product not found" };
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
                var record = await dbContext.KitchenProducts.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (record != null)
                {
                    //check if it's user or admin that wanty to delete
                    if (record.StatusId == 2 && entryTypeId == 2)
                    {
                        return new GenericResponseModel { StatusCode = 300, StatusMessage = "Your Kitchen Product has been processed, it can't be deleted" };
                    }
                    dbContext.KitchenProducts.Remove(record);
                    await dbContext.SaveChangesAsync();
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Kitchen Product Deleted Successfully" };
                }
                return new GenericResponseModel { StatusCode = 404, StatusMessage = "Kitchen Product not found" };
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

        public async Task<IList<SuperKitchenProduct>> GetKitchenProduct(string userId, int? statusId, int entryTypeId)
        {
            IList<KitchenProductResponseModel> productItems = new List<KitchenProductResponseModel>();
            var record = await dbContext.KitchenProducts.Select(x => new KitchenProductResponseModel
            {
                ApprovedAt = x.ApprovedAt,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                CreatedBy = x.CreatedBy,
                CreatedByName = x.ApplicationUser.FirstName + " " + x.ApplicationUser.LastName,
                EntryTypeId = x.EntryTypeId,
                EntryType = x.EntryType.Type,
                HealthBenefit = x.HealthBenefit,
                Id = x.Id,
                ProductName = x.ProductName,
                ProductPreparation = x.ProductPreparation,
                StatusId = x.StatusId,
                Status = x.Status.StatusName,
                UpdatedBy = x.ApplicationUser1.FirstName + " " + x.ApplicationUser1.LastName,
                //KitchenProductItems = x
            }).OrderByDescending(x => x.Id).ToListAsync();
            //Filter
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


            IList<SuperKitchenProduct> superKitchenProducts = new List<SuperKitchenProduct>();
            foreach (var product in record)
            {
                //Get all items for product
                SuperKitchenProduct superKitchenProduct = new SuperKitchenProduct
                {
                    getKitchenProduct = product,
                    productItems = await dbContext.KitchenProductItems.Where(x => x.ProductId == product.Id).ToListAsync(),
                };
                superKitchenProducts.Add(superKitchenProduct);
            }
            return superKitchenProducts;
        }

        public async Task<GenericResponseModel> SaveAsync(KitchenProductViewModel model)
        {
            try
            {
                //This will be implemented later
                //check if the record exists
                //var checkRecord = await dbContext.KitchenProducts.Where(x => x.ProductName == model.ProductName && x.CategoryId == model.CategoryId && x.StatusId != 4).FirstOrDefaultAsync();
                //if (checkRecord != null)
                //{
                //    return new GenericResponseModel { StatusCode = 300, StatusMessage = "Item is already on the pipeline" };
                //}
                if (model.Item1 == 0 && model.Item2 == 0 && model.Item3 == 0)
                {
                    return new GenericResponseModel { StatusCode = 300, StatusMessage = "Select minimum of 1 item" };
                }
                //save new item
                KitchenProduct kitchenProduct = new KitchenProduct
                {
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = model.CreatedBy,
                    EntryTypeId = model.EntryTypeId,
                    HealthBenefit = model.HealthBenefit,
                    ProductName = model.ProductName,
                    StatusId = 1,
                    UpdatedBy = model.CreatedBy,
                    ProductPreparation = model.ProductPreparation
                };
                await dbContext.KitchenProducts.AddAsync(kitchenProduct);
                await dbContext.SaveChangesAsync();

                //This will later be implemented as loop
                if (model.Item1 > 0)
                {
                    KitchenProductItem kitchenProductItem = new KitchenProductItem
                    {
                        Item = dbContext.KitchenItems.FirstOrDefault(x=>x.Id == model.Item1).ItemName,
                        ItemId = model.Item1,
                        ProductId = kitchenProduct.Id
                    };
                    
                    await dbContext.KitchenProductItems.AddAsync(kitchenProductItem);
                    await dbContext.SaveChangesAsync();
                }
                if (model.Item2 > 0)
                {
                    KitchenProductItem kitchenProductItem = new KitchenProductItem
                    {
                        Item = dbContext.KitchenItems.FirstOrDefault(x => x.Id == model.Item2).ItemName,
                        ItemId = model.Item2,
                        ProductId = kitchenProduct.Id
                    };
                    await dbContext.KitchenProductItems.AddAsync(kitchenProductItem);
                    await dbContext.SaveChangesAsync();
                }
                if (model.Item3 > 0)
                {
                    KitchenProductItem kitchenProductItem = new KitchenProductItem
                    {
                        Item = dbContext.KitchenItems.FirstOrDefault(x => x.Id == model.Item3).ItemName,
                        ItemId = model.Item3,
                        ProductId = kitchenProduct.Id
                    };
                    await dbContext.KitchenProductItems.AddAsync(kitchenProductItem);
                    await dbContext.SaveChangesAsync();
                }
                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Product Created Successfully" };
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

        public Task<GenericResponseModel> UpdateAsync(long id, KitchenProductViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
