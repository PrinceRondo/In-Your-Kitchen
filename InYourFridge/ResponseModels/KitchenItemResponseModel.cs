using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InYourFridge.ResponseModels
{
    public class KitchenItemResponseModel
    {
        public long Id { get; set; }
        public string ItemName { get; set; }
        public string HealthBenefit { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
        public string EntryType { get; set; }
        public int EntryTypeId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovedAt { get; set; }
    }
}
