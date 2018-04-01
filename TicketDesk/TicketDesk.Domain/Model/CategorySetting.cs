using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketDesk.Domain.Model
{
    public class CategorySetting
    {
        [Key]
        public string SubCategory { get; set; }
        public string Category { get; set; }
    }
}
