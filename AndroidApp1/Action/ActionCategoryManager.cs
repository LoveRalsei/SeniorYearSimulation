using Android.Content;
using Android.Views;
using Android.Widget;
using AndroidApp1.UIData;

namespace AndroidApp1.Actions
{
    /// <summary>
    /// Manages all ActionCategories: creates buttons from configs,
    /// adds them to the UI container, and handles category switching.
    /// Replaces the 5× duplicated foreach loops and if-else chains in MainActivity.
    /// </summary>
    public class ActionCategoryManager
    {
        private readonly List<ActionCategory> _categories = new();
        private ActionCategory? _activeCategory;
        private StudentModifier? _modifier;

        /// <summary>All registered categories.</summary>
        public IReadOnlyList<ActionCategory> Categories => _categories;

        public void SetModifier(StudentModifier modifier)
        {
            _modifier = modifier;
        }

        /// <summary>Register a category from its config data.</summary>
        public void AddCategory(ActionCategory category)
        {
            _categories.Add(category);
        }

        /// <summary>
        /// Build all ActionButtons from all categories and add them to the given container.
        /// All buttons start hidden; SwitchTo() makes them visible.
        /// </summary>
        public void Initialize(ViewGroup container, Context context)
        {
            foreach (var category in _categories)
            {
                foreach (var config in category.Configs)
                {
                    var actionBtn = new ActionButton(context);
                    actionBtn.SetAllTexts(config.Title, config.Description, config.CostText);

                    if (_modifier != null)
                        actionBtn.SetDialog(config, _modifier);

                    actionBtn.SetOnClickListener((s, e) =>
                    {
                        actionBtn.ShowDialog();
                    });

                    category.Buttons.Add(actionBtn);
                    actionBtn.Visibility = ViewStates.Gone;
                    container.AddView(actionBtn);
                }
            }
        }

        /// <summary>Switch to display buttons for the given category resource ID.</summary>
        public void SwitchTo(int toggleButtonResourceId)
        {
            foreach (var category in _categories)
            {
                bool isActive = category.ToggleButtonResourceId == toggleButtonResourceId;
                foreach (var btn in category.Buttons)
                {
                    btn.Visibility = isActive ? ViewStates.Visible : ViewStates.Gone;
                }
                if (isActive) _activeCategory = category;
            }
        }

        /// <summary>Set up click handlers on the toggle buttons (the 5 top buttons).</summary>
        public void BindToggleButtons(Context context, ViewGroup buttonStrip)
        {
            foreach (var category in _categories)
            {
                var toggleBtn = buttonStrip.FindViewById<Button>(category.ToggleButtonResourceId);
                if (toggleBtn != null)
                {
                    toggleBtn.Click += (s, e) => SwitchTo(category.ToggleButtonResourceId);
                }
            }
        }

        public ActionCategory? ActiveCategory => _activeCategory;
    }
}
