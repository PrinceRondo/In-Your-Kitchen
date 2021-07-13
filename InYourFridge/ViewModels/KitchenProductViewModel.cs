using InYourFridge.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InYourFridge.ViewModels
{
    public class KitchenProductViewModel
    {
        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Required]
        [Display(Name = "Product Preparation")]
        public string ProductPreparation { get; set; }
        [Required]
        [Display(Name = "Health Benefit")]
        public string HealthBenefit { get; set; }
        [Display(Name = "First Category")]
        public int CategoryId1 { get; set; }
        [Display(Name = "Second Category")]
        public int CategoryId2 { get; set; }
        [Display(Name = "Third Category")]
        public int CategoryId3 { get; set; }
        public int StatusId { get; set; }
        public int EntryTypeId { get; set; }
        public string CreatedBy { get; set; }

        //It will later be list of items
        [Display(Name = "First Item")]
        public long Item1 { get; set; }
        [Display(Name = "Second Item")]
        public int Item2 { get; set; }
        [Display(Name = "Third Item")]
        public int Item3 { get; set; }

        public IEnumerable<KitchenItemCategory> Categories { get; set; }
    }
}
