using Android.Content;

namespace AndroidApp1.Event
{
    /// <summary>
    /// Dispatches random events at end-of-turn.
    /// Supports turn-indexed event lists with null gaps, probability rolling,
    /// two-dialog flow (event choice → result display), and a bottom-right
    /// button with empty callback.
    /// </summary>
    public class EventDispatcher
    {
        private readonly EventRegistry _registry;
        private readonly StudentModifier _modifier;
        private readonly Context _context;
        private static readonly Random _random = new();

        public EventDispatcher(EventRegistry registry, StudentModifier modifier, Context context)
        {
            _registry = registry;
            _modifier = modifier;
            _context = context;
        }

        /// <summary>
        /// Try to dispatch an event for the current turn. Shows a dialog.
        /// Call this at end-of-turn. Returns true if an event was shown.
        /// The onCompleted callback fires after the result dialog is closed.
        /// </summary>
        public bool Dispatch(string characterName, int turn, Action? onCompleted = null)
        {
            var turnEvent = _registry.GetEventForTurn(characterName, turn);

            // No event configured for this turn (null entry in list)
            if (turnEvent == null)
            {
                ShowNoEventDialog(onCompleted);
                return false;
            }

            // Roll probability
            if (turnEvent.TriggerProbability < 1f)
            {
                if (_random.NextDouble() >= turnEvent.TriggerProbability)
                {
                    // Probability check failed → treat as no event
                    ShowNoEventDialog(onCompleted);
                    return false;
                }
            }

            // Show the event dialog
            ShowEventDialog(turnEvent, onCompleted);
            return true;
        }

        /// <summary>
        /// Build a formatted button text: title + effects summary.
        /// </summary>
        private string BuildOptionButtonText(EventOption option)
        {
            string effectsText = BuildEffectsPreview(option.Effects);
            if (string.IsNullOrEmpty(effectsText))
                return option.Text;
            return option.Text + "\n" + effectsText;
        }

        /// <summary>
        /// Build a concise effects preview string for scroll button display.
        /// </summary>
        private string BuildEffectsPreview(List<PropertyEffect> effects)
        {
            var parts = new List<string>();
            foreach (var effect in effects)
            {
                string name = PropertyMetadata.GetDisplayName(effect.Property, _modifier.Student);
                string sign = effect.Value >= 0 ? "+" : "";
                parts.Add(name + sign + effect.Value);
            }
            return string.Join("  ", parts);
        }

        private void ShowEventDialog(RandomEvent evt, Action? onCompleted)
        {
            var dialog = new CustomDialog(_context);
            dialog.SetTitle(evt.Title);
            dialog.SetMessage(evt.Description);

            // Add each option as a scroll button
            foreach (var option in evt.Options)
            {
                string buttonText = BuildOptionButtonText(option);
                dialog.AddScrollButton(buttonText, () =>
                {
                    // Apply effects via StudentModifier
                    var result = _modifier.ApplyEffects(option.Effects);

                    // Hide the event dialog
                    dialog.Hide();

                    // Show result dialog with property changes
                    ShowResultDialog(option, result, onCompleted);
                });
            }

            // Bottom-right button with empty callback (does nothing when clicked)
            dialog.SetButtonText(evt.BottomButtonText);
            dialog.SetOnButtonClick(() =>
            {
                // Intentionally empty callback per requirements
            });

            dialog.CancelOnTouchOutside = false;
            dialog.Show();
        }

        /// <summary>
        /// Show a result dialog after the player chooses an event option.
        /// Displays the option's result text and the actual property changes.
        /// Closing this dialog triggers the onCompleted callback (→ EndTurn).
        /// </summary>
        private void ShowResultDialog(EventOption option, PropertyChangeResult result, Action? onCompleted)
        {
            var resultDialog = new CustomDialog(_context);
            resultDialog.SetTitle("事件结果");

            // Build message: result text + property changes
            string message = string.IsNullOrEmpty(option.ResultText)
                ? ActionDialog.GetEffectsString(0, option.Effects, _modifier.Student)
                : option.ResultText;

            if (result.HasChanges)
            {
                message += "\n\n【属性变化】\n" + result.GetSummaryString();
            }

            resultDialog.SetMessage(message);
            resultDialog.SetButtonText("确定");
            resultDialog.SetOnButtonClick(() =>
            {
                resultDialog.Hide();
                onCompleted?.Invoke();
            });
            resultDialog.CancelOnTouchOutside = false;
            resultDialog.Show();
        }

        private void ShowNoEventDialog(Action? onCompleted)
        {
            var dialog = new CustomDialog(_context);
            dialog.SetTitle("无事发生");
            dialog.SetMessage("这回合平静地过去了。");
            dialog.SetButtonText("关闭");
            dialog.SetOnButtonClick(() =>
            {
                dialog.Hide();
                onCompleted?.Invoke();
            });
            dialog.CancelOnTouchOutside = false;
            dialog.Show();
        }
    }
}