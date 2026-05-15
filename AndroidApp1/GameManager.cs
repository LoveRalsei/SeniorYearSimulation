using System.Text.Json;
using Android.Content;
using AndroidApp1.Event;
using AndroidApp1.UIData;

namespace AndroidApp1
{
    /// <summary>
    /// Game lifecycle coordinator. Orchestrates TurnManager, StudentModifier,
    /// EventDispatcher, and UI dialogs. No static global state — everything
    /// is instance-scoped and injected.
    /// </summary>
    public class GameManager
    {
        private readonly Context _context;
        private readonly TurnManager _turnManager;
        private readonly StudentModifier _studentModifier;
        private readonly EventRegistry _eventRegistry;
        private readonly EventDispatcher _eventDispatcher;
        private bool _gameStarted;

        // ── Public read-only access ──────────────────────────────────

        public Student StudentData => _studentModifier.Student;
        public StudentModifier Modifier => _studentModifier;
        public bool IsGameStarted => _gameStarted;

        // ── Events for UI binding ────────────────────────────────────

        /// <summary>Fired when StudentData changes (so UI can refresh).</summary>
        public event Action? OnStudentChanged;

        /// <summary>Fired at end of game with final result description.</summary>
        public event Action<string>? OnGameOver;

        // ── Constructor ──────────────────────────────────────────────

        public GameManager(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _turnManager = new TurnManager();

            var student = new Student();
            _studentModifier = new StudentModifier(student);

            _eventRegistry = Data_Events.BuildRegistry();
            _eventDispatcher = new EventDispatcher(_eventRegistry, _studentModifier, _context);
        }

        // ── Game flow ────────────────────────────────────────────────

        public void StartGame()
        {
            if (_gameStarted) return;

            var availableCharacters = Data_Events.GetAvailableCharacters();
            string? selectedKey = null;

            var dialog = new CustomDialog(_context);
            dialog.SetTitle(_context.GetString(Resource.String.dialog_startgame_title));
            dialog.SetMessage(_context.GetString(Resource.String.dialog_startgame_intro));

            // Auto-discover: create a scroll button for each character in studentdata.json
            foreach (var character in availableCharacters)
            {
                dialog.AddScrollButton(character.Name, () =>
                {
                    selectedKey = character.Key;
                });
            }

            dialog.SetButtonText(_context.GetString(Resource.String.dialog_startgame_startbutton));
            dialog.SetOnButtonClick(() =>
            {
                if (string.IsNullOrEmpty(selectedKey))
                {
                    dialog.SetMessage(_context.GetString(Resource.String.dialog_startgame_introwarning));
                    return;
                }

                var studentJson = JsonFileReader.GetValueByKey("studentdata.json", selectedKey);
                if (!string.IsNullOrEmpty(studentJson))
                {
                    var loaded = JsonSerializer.Deserialize<Student>(studentJson);
                    if (loaded != null)
                        ReplaceStudent(loaded);
                }

                _gameStarted = true;
                _turnManager.Reset();
                dialog.Hide();
            });
            dialog.CancelOnTouchOutside = false;
            dialog.Show();
        }

        public void EndTurn()
        {
            Toast.MakeText(_context, "回合结束", ToastLength.Short)?.Show();

            if (_turnManager.IsLastTurn)
            {
                EndGame();
                return;
            }

            _turnManager.Advance();

            // Apply natural decay + energy recovery
            var decayResult = _studentModifier.ApplyTurnDecay();
            int recoveryAmount = _turnManager.CalculateEnergyRecovery(StudentData);
            var recoveryResult = _studentModifier.RestoreEnergy(recoveryAmount);

            var combined = PropertyChangeResult.Merge(decayResult, recoveryResult);
            NotifyStudentChanged();

            // Show turn-end dialog, then chain to event dispatch
            ShowEndTurnDialog(combined, () =>
            {
                _eventDispatcher.Dispatch(StudentData.name, _turnManager.CurrentTurn,
                    onCompleted: NotifyStudentChanged);
            });
        }

        private void ShowEndTurnDialog(PropertyChangeResult result, Action onClosed)
        {
            var dialog = new CustomDialog(_context);
            dialog.SetTitle("回合结束");
            dialog.SetMessage(result.HasChanges
                ? result.GetDetailedSummaryString()
                : "本回合无属性变化。");
            dialog.SetButtonText("确定");
            dialog.SetOnButtonClick(() =>
            {
                dialog.Hide();
                onClosed();
            });
            dialog.Show();
        }

        private void EndGame()
        {
            _gameStarted = false;
            string summary = BuildEndingSummary();
            OnGameOver?.Invoke(summary);

            var dialog = new CustomDialog(_context);
            dialog.SetTitle("游戏结束");
            dialog.SetMessage(summary);
            dialog.SetButtonText("确定");
            dialog.SetOnButtonClick(() => dialog.Hide());
            dialog.Show();
        }

        private string BuildEndingSummary()
        {
            int totalScore = StudentData.chinese + StudentData.math + StudentData.english
                           + StudentData.crouse1Grade + StudentData.crouse2Grade + StudentData.crouse3Grade;
            return $"游戏结束！\n总得分：{totalScore}\n"
                 + $"语文：{StudentData.chinese}  数学：{StudentData.math}  英语：{StudentData.english}\n"
                 + $"{StudentData.crouse1Name}：{StudentData.crouse1Grade}  "
                 + $"{StudentData.crouse2Name}：{StudentData.crouse2Grade}  "
                 + $"{StudentData.crouse3Name}：{StudentData.crouse3Grade}";
        }

        // ── Internal helpers ─────────────────────────────────────────

        private void ReplaceStudent(Student newStudent)
        {
            _studentModifier.ReplaceStudent(newStudent);
            NotifyStudentChanged();
        }

        public void NotifyStudentChanged()
        {
            OnStudentChanged?.Invoke();
        }
    }
}
