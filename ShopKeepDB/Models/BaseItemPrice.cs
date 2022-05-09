using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShopKeepDB.Models
{
    public class BaseItemPrice
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int Gold { get; set; }
        [Required]
        public int Silver { get; set; }
        [Required]
        public int Copper { get; set; }
        public BaseItemPrice(){}

        public BaseItemPrice(int gold, int silver, int copper)
        {
            this.Gold = gold;
            this.Silver = silver;
            this.Copper = copper;
        }
    }
}
