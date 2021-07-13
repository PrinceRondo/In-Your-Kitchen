using InYourFridge.Models;
using InYourFridge.ResponseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InYourFridge.ViewModels
{
    public class GetKitchenItemViewModel
    {
        public IList<KitchenItemResponseModel> KitchenItems { get; set; }
        public IEnumerable<KitchenItemCategory> KitchenItemCategories { get; set; }
        public IEnumerable<Status> Statuses { get; set; }
        public IEnumerable<EntryType> EntryTypes { get; set; }
        public IEnumerable<UserProfile> UserProfiles { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [Display(Name = "Status")]
        public int StatusId { get; set; }
        [Display(Name = "Entry Type")]
        public int EntryTypeId { get; set; }
        [Display(Name = "User")]
        public string UserId { get; set; }
    }
}
