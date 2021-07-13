using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InYourFridge.ViewModels
{
    public class SalesInformationViewModel
    {
        [Required]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }
        [Required]
        [Display(Name = "Country")]
        public string CountryCode { get; set; }
        [Required]
        [Display(Name = "Region")]
        public string RegionCode { get; set; }
        [Required]
        [Display(Name = "City")]
        public int CityCode { get; set; }
        [Required]
        [Display(Name = "Product")]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string CreatedBy { get; set; }
    }
}
