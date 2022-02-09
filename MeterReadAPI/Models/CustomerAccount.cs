using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MeterReadAPI.Models
{
    [Table("CustomerAccount")]
    public class CustomerAccount
    {
        [Key]
        [Required]
        public int AccountId { get; set; }     
        public String Fname { get; set; }
        public String Lname { get; set; }
    }
}
