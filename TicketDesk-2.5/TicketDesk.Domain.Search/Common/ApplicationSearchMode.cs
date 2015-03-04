using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketDesk.Domain.Search
{
    public enum ApplicationSearchMode
    {
        [Description("Auto Detect")]
        Auto,
        [Description("Azure Search")]
        AzureSearch,
        [Description("Local Lucene Search")]
        LocalLucene
    }
}
