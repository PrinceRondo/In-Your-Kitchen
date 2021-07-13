using InYourFridge.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InYourFridge.Models
{
    public class KitchenProduct
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string ProductName { get; set; }
        public string ProductPreparation { get; set; }
        public string HealthBenefit { get; set; }
        public int StatusId { get; set; }
        public int EntryTypeId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovedAt { get; set; }


        [ForeignKey("StatusId")]
        public virtual Status Status { get; set; }
        [ForeignKey("EntryTypeId")]
        public virtual EntryType EntryType { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("UpdatedBy")]
        public virtual ApplicationUser ApplicationUser1 { get; set; }
        [ForeignKey("ApprovedBy")]
        public virtual ApplicationUser ApplicationUser2 { get; set; }
    }
}
