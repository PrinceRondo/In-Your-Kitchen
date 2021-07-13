using InYourFridge.Data;
using InYourFridge.Models;
using InYourFridge.ResponseModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InYourFridge.Repository
{
    public class KitchenItemCategoryRepository : IKitchenItemCategoryRepository
    {
        private readonly AppDbContext dbContext;

        public KitchenItemCategoryRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<KitchenItemCategory> FindByNameAsync(string name)
        {
            return await dbContext.KitchenItemCategories.Where(x => x.CategoryName.ToLower() == name.ToLower()).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<KitchenItemCategory>> GetAllCategory()
        {
            return await dbContext.KitchenItemCategories.ToListAsync();
        }

        public async Task<GenericResponseModel> SaveAsync(KitchenItemCategory model)
        {
            GenericResponseModel response = new GenericResponseModel();
            var newCategory = new KitchenItemCategory()
            {
                CategoryName = model.CategoryName
            };
            if (model.CategoryName.Any())
            {
                dbContext.KitchenItemCategories.Add(newCategory);
                try
                {
                    dbContext.SaveChanges();
                    response.StatusMessage = "Saved Successfully";
                    response.StatusCode = 200;
                }
                catch (Exception ex)
                {
                }
            }
            return response;
        }
    }
}
