using Android.Content;
using Android.Widget;
using Android.Views;
using AndroidApp1.UIData;

namespace AndroidApp1
{
    /// <summary>
    /// Custom action button (LinearLayout) that shows an ActionDialog on click.
    /// Handles both regular actions and subject-choice (study) actions
    /// via ActionButtonConfig.RequiresSubjectChoice. No separate StudyActionButton needed.
    ///
    /// When the player confirms execution in the ActionDialog, this button
    /// spawns a separate plain result dialog with typewriter animation and
    /// property-change display.
    /// </summary>
    public class ActionButton : LinearLayout
    {
        protected TextView titleText = null!;
        protected TextView descText = null!;
        protected TextView costText = null!;

        private ActionDialog? _dialog;
        private StudentModifier? _studentModifier;
        private ActionButtonConfig? _config;

        /// <summary>Called after an action completes (effects applied). Use to refresh UI.</summary>
        public Action? OnActionCompleted;

        public ActionButton(Context context) : base(context)
        {
            Initialize(context);
        }

        private void Initialize(Context context)
        {
            this.Orientation = Orientation.Vertical;
            this.SetPadding(20, 20, 20, 20);
            this.SetBackgroundColor(Android.Graphics.Color.ParseColor("#F5F5F5"));

            // Header row: title + cost
            var headerLayout = new LinearLayout(context)
            {
                Orientation = Orientation.Horizontal,
                LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent)
            };

            titleText = new TextView(context)
            {
                TextSize = 16
            };
            titleText.SetTypeface(null, Android.Graphics.TypefaceStyle.Bold);
            titleText.LayoutParameters = new LayoutParams(0, LayoutParams.WrapContent, 1);
            headerLayout.AddView(titleText);

            costText = new TextView(context)
            {
                TextSize = 14,
                LayoutParameters = new LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent)
            };
            headerLayout.AddView(costText);

            // Description
            descText = new TextView(context)
            {
                TextSize = 14,
                LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent)
            };

            this.AddView(headerLayout);
            this.AddView(descText);
        }

        public void SetTitle(string title) => titleText.Text = title;
        public void SetDescription(string description) => descText.Text = description;
        public void SetCost(string cost) => costText.Text = cost;

        public void SetAllTexts(string title, string description, string cost)
        {
            titleText.Text = title;
            descText.Text = description;
            costText.Text = cost;
        }

        /// <summary>
        /// Configure the confirm dialog for this action button.
        /// </summary>
        public void SetDialog(ActionButtonConfig config, StudentModifier modifier)
        {
            _studentModifier = modifier;
            _config = config;

            _dialog = new ActionDialog(Context);
            _dialog.BecomeActionDialog(config, modifier);
            _dialog.CancelOnTouchOutside = true;

            // When the player confirms execution, spawn the result dialog
            _dialog.OnExecuteConfirmed += OnExecuteConfirmed;
        }

        public void ShowDialog()
        {
            if (_dialog == null) return;

            // Add subject-choice scroll buttons if needed
            if (_dialog.RequiresSubjectChoice && _studentModifier != null)
            {
                _dialog.AddSubjectButtons();
            }

            _dialog.Show();
        }

        /// <summary>Set the click listener for this button.</summary>
        public void SetOnClickListener(EventHandler onClick)
        {
            this.Click += onClick;
        }

        // ── Result dialog (spawned when player clicks "执行") ─────────

        private void OnExecuteConfirmed(ActionButtonConfig config, StudentProperty? selectedSubject)
        {
            if (_studentModifier == null) return;

            // Resolve effects (redirect subject-type effects to chosen subject)
            var resolvedEffects = ActionDialog.ResolveEffects(config.Effects, selectedSubject);

            // ── Create the result dialog (plain, no scroll buttons) ──
            var resultDialog = new CustomDialog(Context);
            resultDialog.SetTitle(config.DialogTitle);

            // Button: visible but does nothing initially
            resultDialog.SetButtonText("");
            resultDialog.SetAllowButtonClick(false);
            resultDialog.SetDisableButtonClickFunction(() => { });

            // Typewriter effect for finish text
            resultDialog.EnableTypewriterEffect(true);
            resultDialog.SetOnTypewriterComplete(() =>
            {
                // Apply effects to student
                _studentModifier.ApplyEffects(resolvedEffects);
                int consumed = _studentModifier.ConsumeEnergy(config.CostEnergy);

                // Notify UI to refresh
                OnActionCompleted?.Invoke();

                // Build and display result text
                string resultText = ActionDialog.GetEffectsString(consumed, resolvedEffects,
                    _studentModifier.Student);
                resultDialog.EnableTypewriterEffect(false);
                resultDialog.SetMessage(resultText);

                // Activate close button
                resultDialog.SetButtonText("关闭");
                resultDialog.SetAllowButtonClick(true);
                resultDialog.SetOnButtonClick(() => resultDialog.Hide());
            });

            // Trigger typewriter by setting message
            resultDialog.SetMessage(config.DialogFinish);
            resultDialog.CancelOnTouchOutside = true;
            resultDialog.Show();
        }
    }
}
