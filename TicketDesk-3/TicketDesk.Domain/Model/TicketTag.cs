
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace TicketDesk.Domain.Model
{

    public class TicketTag
    {
        [DisplayName("Ticket Tag Id")]
        [Required]
        public int TicketTagId { get; set; }

        [DisplayName("Tag Name")]
        [Required]
        [StringLength(100)]
        public string TagName { get; set; }

        [DisplayName("Ticket Id")]
        [Required]
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
