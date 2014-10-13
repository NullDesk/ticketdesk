using System.ComponentModel;

namespace TicketDesk.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TicketTag
    {
        [DisplayName("Tag Id")]
        public int TicketTagId { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Tag Name")]
        public string TagName { get; set; }

        [DisplayName("Ticket Id")]
        public int TicketId { get; set; }

        public virtual Ticket Ticket { get; set; }

        #region utility

        public static string[] GetTagsFromString(string tagString)
        {
            var returnTags = new List<string>();
            if (!string.IsNullOrEmpty(tagString))
            {
                string[] tags = tagString.Split(',');
                foreach (string t in tags)
                {
                    var formattedTag = t.ToLowerInvariant().Trim();
                    if (!string.IsNullOrEmpty(formattedTag) && !returnTags.Contains(formattedTag))
                    {
                        returnTags.Add(formattedTag);
                    }
                }
            }
            return returnTags.ToArray();
        }

        #endregion
    }
}
