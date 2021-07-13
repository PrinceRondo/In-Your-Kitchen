using InYourFridge.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InYourFridge.Models
{
    public class KitchenItemMarket
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long ItemId { get; set; }
        public string Location { get; set; }
        public string Contact { get; set; }
        public string OtherInformation { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }


        [ForeignKey("ItemId")]
        public virtual KitchenItem FKitchenItem { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("UpdatedBy")]
        public virtual ApplicationUser ApplicationUser1 { get; set; }
    }
}
