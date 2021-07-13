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
    public class PointRepository : IPointRepository
    {
        private readonly AppDbContext dbContext;

        public PointRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<UserPoint> GetUserPointAsync(string userId)
        {
            return await dbContext.UserPoints.Where(x => x.UserId == userId).FirstOrDefaultAsync();
        }
    }
}
