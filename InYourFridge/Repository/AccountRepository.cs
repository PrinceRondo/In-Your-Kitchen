using InYourFridge.Data;
using InYourFridge.Helper;
using InYourFridge.Models;
using InYourFridge.Repository;
using InYourFridge.ResponseModels;
using InYourFridge.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InYourFridge.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext dbContext;
        private readonly Utility utility;

        public AccountRepository(AppDbContext dbContext, Utility utility)
        {
            this.dbContext = dbContext;
            this.utility = utility;
        }

        public async Task<IList<UserProfile>> GetAllUserAsync()
        {
            return await dbContext.UserProfiles.ToListAsync();
        }

        public async Task<UserProfile> GetUserProfileByUserIdAsync(string userId)
        {
            return await dbContext.UserProfiles.Where(x=>x.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<GenericResponseModel> RegisterUserAsync(ProfileViewModel model)
        {
            try
            {
                //save user profile
                UserProfile userProfile = new UserProfile
                {
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Email = model.Email,
                    UserId = model.UserId,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };
                await dbContext.UserProfiles.AddAsync(userProfile);
                //save user point
                UserPoint userPoint = new UserPoint
                {
                    UpdatedAt = DateTime.Now,
                    Point = 100,
                    UserId = model.UserId
                };
                await dbContext.UserPoints.AddAsync(userPoint);
                await dbContext.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 200, StatusMessage = "User Created Successfully" };
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

        //public async Task<IEnumerable<object>> GetAllRoleAsync()
        //{
        //    return await dbContext.Roles.Where(x => x.Name != "admin" && x.Name != "hr").ToListAsync();
        //    //eturn new GenericResponseModel { StatusCode = 200, StatusMessage = "Success", Datas = result  };
        //}

        //public async Task<IList<UserProfileResponseModel>> GetAllUserAsync(int departmentId, string staffId, string role)
        //{
        //    var result = await dbContext.EmployeeProfiles.Select(x => new UserProfileResponseModel
        //    {
        //        CreatedAt = x.CreatedAt,
        //        CreatedBy = x.CreatedBy,
        //        DepartmentName = x.Department.DepartmentName,
        //        DepartmentId = x.DepartmentId,
        //        Email = x.Email,
        //        EmployeeRole = x.EmployeeRole,
        //        EmployeeUserId = x.EmployeeUserId,
        //        FirstName = x.FirstName,
        //        Id = x.Id,
        //        LastName = x.LastName,
        //        StaffId = x.StaffId,
        //        UpdatedAt = x.UpdatedAt
        //    }).OrderByDescending(x => x.Id).ToListAsync();
        //    if(role != null)
        //    {
        //        result = result.Where(x => x.EmployeeRole == role).ToList();
        //    }
        //    if (departmentId > 0)
        //    {
        //        result = result.Where(x => x.DepartmentId == departmentId).ToList();
        //    }
        //    if (staffId != null)
        //    {
        //        result = result.Where(x => x.StaffId == staffId).ToList();
        //    }
        //    return result;
        //}
    }
}
