using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Cw5.Models
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class RegisterProduct
    {
        [Required(ErrorMessage ="IdProduct is required")]
        public int IdProduct { get; set; }
        [Required(ErrorMessage = "IdWareHouse is required")]
        public int IdWareHouse { get; set; }
        [Required(ErrorMessage = "Amount is required")]
        [Range(0,Int32.MaxValue, ErrorMessage = "Value must be bigger than 0")]
        public int Amount { get; set; }
        [Required(ErrorMessage = "CreatedAt is required")]
        public DateTime CreatedAt { get; set; }

    }
}
