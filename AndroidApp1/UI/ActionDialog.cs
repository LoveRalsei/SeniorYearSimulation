using Android.Content;
using Android.OS;
using AndroidApp1.UIData;

namespace AndroidApp1
{
    /// <summary>
    /// Confirm-only action dialog. Shows description, optional subject-choice
    /// scroll buttons, and an "execute" button. When the player clicks execute
    /// (and has enough energy), fires OnExecuteConfirmed — the caller
    /// (ActionButton) then creates a separate result dialog for typewriter +
    /// property-change display.
    /// </summary>
    public class ActionDialog : CustomDialog
    {
        private int _costEnergy;
        private string _introText = "";
        private StudentProperty? _selectedSubject;
        private StudentModifier? _studentModifier;
        private bool _scrollButtonsAdded;
        private ActionButtonConfig? _config;

        /// <summary>
        /// Fired when the player confirms execution and has enough energy.
        /// Parameters: the config and the selected subject (null if not a study action).
        /// </summary>
        public Action<ActionButtonConfig, StudentProperty?>? OnExecuteConfirmed;

        public ActionDialog(Context context) : base(context) { }

        // ── Configuration ────────────────────────────────────────────

        public void BecomeActionDialog(ActionButtonConfig config, StudentModifier modifier)
        {
            _studentModifier = modifier ?? throw new ArgumentNullException(nameof(modifier));
            _config = config;
            _costEnergy = config.CostEnergy;
            _introText = config.DialogIntro;
            _scrollButtonsAdded = false;
            RequiresSubjectChoice = config.RequiresSubjectChoice;

            SetTitle(config.DialogTitle);
            SetMessage(_introText);
            SetButtonText("执行");
            SetOnButtonClick(OnConfirmClicked);
        }

        // ── Confirm: execute clicked ─────────────────────────────────

        private void OnConfirmClicked()
        {
            if (_studentModifier == null || _config == null) return;

            if (!_studentModifier.Student.EnoughEnergy(_costEnergy))
            {
                SetMessage("精力不足！");
                var handler = new Handler(Looper.MainLooper);
                handler.PostDelayed(() =>
                {
                    SetMessage(_introText);
                }, 2000);
                return;
            }

            // Hide this dialog and notify caller to spawn the result dialog
            Hide();
            OnExecuteConfirmed?.Invoke(_config, _selectedSubject);
        }

        // ── Subject choice scroll buttons ────────────────────────────

        public bool RequiresSubjectChoice { get; set; }

        public void AddSubjectButtons()
        {
            if (_scrollButtonsAdded) return;
            if (_studentModifier == null) return;

            var student = _studentModifier.Student;
            AddScrollButton("语文", () => _selectedSubject = StudentProperty.Chinese);
            AddScrollButton("数学", () => _selectedSubject = StudentProperty.Math);
            AddScrollButton("英语", () => _selectedSubject = StudentProperty.English);
            if (!string.IsNullOrEmpty(student.crouse1Name))
                AddScrollButton(student.crouse1Name, () => _selectedSubject = StudentProperty.Crouse1Grade);
            if (!string.IsNullOrEmpty(student.crouse2Name))
                AddScrollButton(student.crouse2Name, () => _selectedSubject = StudentProperty.Crouse2Grade);
            if (!string.IsNullOrEmpty(student.crouse3Name))
                AddScrollButton(student.crouse3Name, () => _selectedSubject = StudentProperty.Crouse3Grade);

            _selectedSubject = StudentProperty.Chinese;
            _scrollButtonsAdded = true;
        }

        // ── Effect resolution (static, used by ActionButton for result dialog) ──

        public static List<PropertyEffect> ResolveEffects(
            List<PropertyEffect> effects, StudentProperty? selectedSubject)
        {
            if (selectedSubject == null) return effects;

            var resolved = new List<PropertyEffect>();
            foreach (var effect in effects)
            {
                if (IsSubjectProperty(effect.Property))
                    resolved.Add(new PropertyEffect(selectedSubject.Value, effect.Value));
                else
                    resolved.Add(effect);
            }
            return resolved;
        }

        public static bool IsSubjectProperty(StudentProperty prop)
        {
            return prop == StudentProperty.Chinese || prop == StudentProperty.Math
                || prop == StudentProperty.English
                || prop == StudentProperty.Crouse1Grade
                || prop == StudentProperty.Crouse2Grade
                || prop == StudentProperty.Crouse3Grade;
        }

        // ── Static helper for effects display ────────────────────────

        public static string GetEffectsString(int costEnergy, List<PropertyEffect> effects, Student? student = null)
        {
            string result = "";

            if (costEnergy > 0)
                result += "精力-" + costEnergy + "\n";

            foreach (var effect in effects)
            {
                string name = PropertyMetadata.GetDisplayName(effect.Property, student);
                string sign = effect.Value >= 0 ? "+" : "";
                result += name + sign + effect.Value + "\n";
            }

            return result.TrimEnd();
        }
    }
}
