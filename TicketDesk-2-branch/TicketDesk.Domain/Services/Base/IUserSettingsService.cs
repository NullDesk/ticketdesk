using TicketDesk.Domain.Models;

namespace TicketDesk.Domain.Services
{
    public interface IUserSettingsService
    {
        /// <summary>
        /// Gets or sets a value indicating whether to open editor with preview visible.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if open editor with preview; otherwise, <c>false</c>.
        /// </value>
        bool OpenEditorWithPreview { get; set; }

        /// <summary>
        /// Gets or sets the user's preferred editor mode.
        /// </summary>
        /// <value>The editor mode.</value>
        EditorModes EditorMode { get; set; }


        /// <summary>
        /// Gets the profile repository.
        /// </summary>
        /// <value>The repository.</value>
        IProfile Repository { get; }


        /// <summary>
        /// Gets the display preferences for the current user
        /// </summary>
        /// <returns></returns>
        UserDisplayPreferences GetDisplayPreferences();

        /// <summary>
        /// Saves the display preferences for the current user.
        /// </summary>
        /// <param name="prefrences">The preferences.</param>
        void SaveDisplayPreferences(UserDisplayPreferences prefrences);
    }
}
