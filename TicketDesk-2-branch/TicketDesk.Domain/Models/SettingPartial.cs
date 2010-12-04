using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using TicketDesk.Domain.Models.DataAnnotations;

namespace TicketDesk.Domain.Models
{
    [MetadataType(typeof(SettingMeta))]
    public partial class Setting
    {
    }
}
