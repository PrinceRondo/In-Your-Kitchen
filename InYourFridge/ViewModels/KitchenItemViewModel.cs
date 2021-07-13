using InYourFridge.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InYourFridge.ViewModels
{
    public class KitchenItemViewModel
    {
        [Required]
        [Display(Name = "Item Name")]
        public string ItemName { get; set; }
        [Required]
        [Display(Name = "Health Benefit")]
        public string HealthBenefit { get; set; }
        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public int StatusId { get; set; }
        public int EntryTypeId { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        public IEnumerable<KitchenItemCategory> Categories { get; set; }
    }
}
