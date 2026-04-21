using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace AndroidApp1
{
    internal class GameManager
    {
        private bool _gameStarted = false;
        private MainActivity _mainActivity;

        public Student Student;
        private Student _lastStudent;
        // 属性
        private int currentTurn;
        private const int MAX_TURNS = 10;
        /*private GameState currentState;
        private List<GameEvent> turnEvents;
        private List<GameEvent> randomEvents;

        // 回调事件（用于UI更新）
        public event Action<int> OnTurnChanged;
        public event Action<GameEvent> OnEventTriggered;
        public event Action<string> OnGameOver;*/

        // 方法
        public GameManager(MainActivity mainActivity)
        {
            _mainActivity = mainActivity;
        }

        public void StartGame()
        {
            if(_gameStarted)
                return;

            var dialog = new CustomDialog(_mainActivity);
            dialog.SetTitle("");
            dialog.SetMessage("");
            dialog.SetScrollButton("", () =>
            {
                var studentJson = JsonFileReader.ReadValueByKey("studentdata.json", "Student_test");
                if (!string.IsNullOrEmpty(studentJson))
                {
                    Student = JsonSerializer.Deserialize<Student>(studentJson);
                }
            });
            dialog.SetButtonText("开始");
            dialog.SetOnButtonClick(()=> { 
                _gameStarted = true;
                dialog.Hide();
            });
            dialog.Show();
        }

        public void InitializeGame()
        {

        }

        public void EndTurn()
        {

        }

        private void OnTurnStart()
        {

        }

        private void ApplyEventEffects(GameEvent gameEvent)
        {

        }

        private void EndGame()
        {

        }

        public bool IsGameStart => _gameStarted;
    }

    // 游戏事件类
    public class GameEvent
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<EventEffect> Effects { get; set; }
        public int TriggerTurn { get; set; } // 特定回合触发
        public float TriggerProbability { get; set; } // 随机触发概率
        public Dictionary<string, int> Requirements { get; set; } // 触发条件
    }

    public class EventEffect
    {
        public string Property { get; set; }
        public int Value { get; set; }
    }
}