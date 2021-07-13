using InYourFridge.Models;
using InYourFridge.Repository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InYourFridge.Data
{
    public class SeedData
    {
        private readonly AppDbContext dbContext;

        public SeedData(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public static void SeedDatas(
            RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IEntryTypeRepository entryTypeRepository,
            IKitchenItemCategoryRepository categoryRepository, IStatusRepository statusRepository
            )
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
            SeedEntryType(entryTypeRepository);
            SeedItemCategory(categoryRepository);
            SeedStatus(statusRepository);
        }
        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("admin").Result)
            {
                IdentityRole adminRole = new IdentityRole();

                adminRole.Name = "admin";
                adminRole.NormalizedName = "ADMIN";
                IdentityResult adminRoleResult = roleManager.CreateAsync(adminRole).Result;
            }
            if (!roleManager.RoleExistsAsync("user").Result)
            {
                IdentityRole userRole = new IdentityRole();

                userRole.Name = "user";
                userRole.NormalizedName = "USER";
                IdentityResult adminRoleResult = roleManager.CreateAsync(userRole).Result;
            }
        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager)
        {

            if (userManager.FindByNameAsync("admin@admin.com").Result == null)
            {

                ApplicationUser adminUser = new ApplicationUser();
                adminUser.UserName = "admin@admin.com";
                adminUser.Email = "admin@admin.com";
                adminUser.FirstName = "Admin";
                adminUser.LastName = "Admin";
                adminUser.EmailConfirmed = true;

                try
                {
                    IdentityResult adminUserResult = userManager.CreateAsync(adminUser, "Password123$").Result;
                    if (adminUserResult.Succeeded)
                    {

                        userManager.AddToRoleAsync(adminUser, "admin").Wait();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

            }
        }
        public static void SeedEntryType(IEntryTypeRepository entryTypeRepository)
        {
            string[] entryList = new string[] { "Internal", "External"};
            foreach (string entry in entryList)
            {
                if (entryTypeRepository.FindByNameAsync(entry).Result == null)
                {
                    EntryType entryType = new EntryType()
                    {
                        Type = entry,
                    };
                    try
                    {
                        entryTypeRepository.SaveAsync(entryType);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
        }
        public static void SeedStatus(IStatusRepository statusRepository)
        {
            string[] statusList = new string[] { "Pending", "Approved", "Queried", "Declined" };
            foreach (string status in statusList)
            {
                if (statusRepository.FindByNameAsync(status).Result == null)
                {
                    Status newStatus  = new Status()
                    {
                        StatusName = status,
                    };
                    try
                    {
                        statusRepository.SaveAsync(newStatus);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
        }
        public static void SeedItemCategory(IKitchenItemCategoryRepository categoryRepository)
        {
            string[] categoryList = new string[] { "Breakfast & Cereals", "Canned, Jarred, & Pouched", "Grains, Pasta & Sides", "Produce", "Snacks",
                "Baking & Cooking Supplies", "Condiments & Salad Dressings", "In the Refrigerator","In the Freezer" };
            foreach (string category in categoryList)
            {
                if (categoryRepository.FindByNameAsync(category).Result == null)
                {
                    KitchenItemCategory itemCategory = new KitchenItemCategory()
                    {
                        CategoryName = category,
                    };
                    try
                    {
                        categoryRepository.SaveAsync(itemCategory);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
        }
    }
}
