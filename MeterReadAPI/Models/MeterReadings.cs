using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MeterReadAPI.Models
{
    [Table("MeterReadings")]
    public class MeterReadings
    {
       
        [Required]
        public int AccountId { get; set; }      
        public String MeterReadValue { get; set; }
        public DateTime MeterReadingDateTime { get; set; }

        [Key]
        public int Id { get; set; }
    }
}
