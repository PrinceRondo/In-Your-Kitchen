using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InYourFridge.Models
{
    public class KitchenProductItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long ProductId { get; set; }
        public long ItemId { get; set; }
        public string Item { get; set; }


        [ForeignKey("ProductId")]
        public virtual KitchenProduct KitchenProduct { get; set; }
        [ForeignKey("ItemId")]
        public virtual KitchenItem KitchenItem { get; set; }
    }
}
