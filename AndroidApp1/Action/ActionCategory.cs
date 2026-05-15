using Android.Content;
using AndroidApp1.UIData;

namespace AndroidApp1.Actions
{
    /// <summary>
    /// Represents a category of actions (Study, SpendMoney, DoActivity, etc.).
    /// Groups ActionButtonConfig items and their associated UI buttons.
    /// </summary>
    public class ActionCategory
    {
        public int ToggleButtonResourceId { get; }
        public string ToggleButtonText { get; }
        public List<ActionButtonConfig> Configs { get; }

        /// <summary>UI buttons created from configs. Populated by ActionCategoryManager.</summary>
        public List<ActionButton> Buttons { get; } = new();

        public ActionCategory(int toggleButtonResourceId, string toggleButtonText,
            List<ActionButtonConfig> configs)
        {
            ToggleButtonResourceId = toggleButtonResourceId;
            ToggleButtonText = toggleButtonText;
            Configs = configs;
        }
    }
}
