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
    public class EntryTypeRepository : IEntryTypeRepository
    {
        private readonly AppDbContext dbContext;

        public EntryTypeRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<EntryType> FindByNameAsync(string name)
        {
            return await dbContext.EntryTypes.Where(x => x.Type.ToLower() == name.ToLower()).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<EntryType>> GetAllEntryType()
        {
            return await dbContext.EntryTypes.ToListAsync();
        }

        public async Task<GenericResponseModel> SaveAsync(EntryType model)
        {
            GenericResponseModel response = new GenericResponseModel();
            var newType = new EntryType()
            {
                Type = model.Type
            };
            if (model.Type.Any())
            {
                dbContext.EntryTypes.Add(newType);
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
