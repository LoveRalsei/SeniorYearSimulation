using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace AndroidApp1
{
    public class GameManager
    {
        private bool _gameStarted = false;
        public static MainActivity MainActivity;

        public static Student StudentData;
        private Student _lastStudentData;
        private bool _choosedStudent = false;
        // 属性
        private int currentTurn = 0;
        private const int MAX_TURNS = 10;
        private CustomDialog _endTurnDialog;
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
            MainActivity = mainActivity;
            // 初始化测试学生数据
            StudentData = new Student();
            var studentJson = JsonFileReader.GetValueByKey("studentdata.json", "Student_test");
            if (!string.IsNullOrEmpty(studentJson))
            {
                StudentData = JsonSerializer.Deserialize<Student>(studentJson);
                _lastStudentData = new Student(StudentData);
            }
        }

        public void StartGame()
        {
            if(_gameStarted)
                return;

            var dialog = new CustomDialog(MainActivity);
            dialog.SetTitle(MainActivity.GetString(Resource.String.dialog_startgame_title));
            dialog.SetMessage(MainActivity.GetString(Resource.String.dialog_startgame_intro));
            dialog.AddScrollButton(MainActivity.GetString(Resource.String.dialog_startgame_option1), () =>
            {
                var studentJson = JsonFileReader.GetValueByKey("studentdata.json", "Student_test");
                if (!string.IsNullOrEmpty(studentJson))
                {
                    StudentData = JsonSerializer.Deserialize<Student>(studentJson);
                    MainActivity.RefreshPropertiesTextView();
                    _lastStudentData = new Student(StudentData);
                }
            });
            dialog.SetButtonText(MainActivity.GetString(Resource.String.dialog_startgame_startbutton));
            dialog.SetOnButtonClick(()=> { 
                if(StudentData==null)
                {
                    dialog.SetMessage(MainActivity.GetString(Resource.String.dialog_startgame_introwarning));
                    return;
                }
                _gameStarted = true;
                dialog.Hide();
            });
            dialog.CancelOnTouchOutside=false; // 取消点击外部区域关闭弹窗
            dialog.Show();

        }

        public void InitializeGame()
        {

        }

        public void EndTurn()
        {
            Toast.MakeText(MainActivity, "回合结束", ToastLength.Short).Show();
            if(currentTurn >= MAX_TURNS)
                EndGame();
            currentTurn++;

            _endTurnDialog = new CustomDialog(MainActivity);
            _endTurnDialog.SetTitle("回合结束");
            _endTurnDialog.SetMessage("属性变化：\n" +
                $"金钱：{_lastStudentData.money}->{StudentData.money}\n" +
                $"健康：{_lastStudentData.health}->{StudentData.health}\n" +
                $"体力：{_lastStudentData.energy}->{StudentData.energy}\n" +
                $"快乐：{_lastStudentData.happiness}->{StudentData.happiness}\n" +
                $"魅力：{_lastStudentData.charm}->{StudentData.charm}\n" +
                $"懒惰：{_lastStudentData.laziness}->{StudentData.laziness}\n" +
                $"迷茫：{_lastStudentData.confusion}->{StudentData.confusion}\n" +
                $"语文：{_lastStudentData.chinese}->{StudentData.chinese}\n" +
                $"数学：{_lastStudentData.math}->{StudentData.math}\n" +
                $"英语：{_lastStudentData.english}->{StudentData.english}\n" +
                $"课程1成绩：{_lastStudentData.crouse1Grade}->{StudentData.crouse1Grade}\n" +
                $"课程2成绩：{_lastStudentData.crouse2Grade}->{StudentData.crouse2Grade}\n" +
                $"课程3成绩：{_lastStudentData.crouse3Grade}->{StudentData.crouse3Grade}\n");
            _endTurnDialog.SetButtonText("确定");
            _endTurnDialog.SetOnButtonClick(()=> { 
                _endTurnDialog.Hide();
            });
            _endTurnDialog.Show();
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