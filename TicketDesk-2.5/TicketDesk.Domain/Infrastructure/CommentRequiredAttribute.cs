using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketDesk.Domain.Infrastructure
{
    /// <summary>
    /// Marker attribute indicating if comments are required for TicketActivityEnum
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class CommentRequiredAttribute : Attribute
    {

    }
}
