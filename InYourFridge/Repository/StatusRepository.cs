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
    public class StatusRepository : IStatusRepository
    {
        private readonly AppDbContext dbContext;

        public StatusRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Status> FindByNameAsync(string name)
        {
            return await dbContext.Statuses.Where(x => x.StatusName.ToLower() == name.ToLower()).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Status>> GetAllStatus()
        {
            return await dbContext.Statuses.ToListAsync();
        }

        public async Task<GenericResponseModel> SaveAsync(Status model)
        {
            GenericResponseModel response = new GenericResponseModel();
            var newStatus = new Status()
            {
                StatusName = model.StatusName
            };
            if (model.StatusName.Any())
            {
                dbContext.Statuses.Add(newStatus);
                try
                {
                    dbContext.SaveChanges();
                    response.StatusMessage = "Saved Successfully";
                    response.StatusCode = 200;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return response;
        }
    }
}
