using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Util;

namespace AndroidApp1
{
    /// <summary>
    /// 自定义弹窗控件，包含标题、介绍文本和右下角按钮
    /// </summary>
    public class CustomDialog
    {
        private readonly Dialog _dialog;
        private readonly TextView _titleTextView;
        private readonly TextView _messageTextView;
        private readonly Button _confirmButton;
        private readonly Context _context;
        private ScrollView? _scrollView;
        private List<Button> _scrollButtons = new List<Button>();
        private CustomDialog _actionDialog;

        private Java.Util.Timer? _typewriterTimer;
        private string _fullMessage = "";
        private int _currentCharIndex;
        private bool _isTypewriterEnabled = false;
        private System.Action? _onTypewriterComplete;
        private bool _cancelOnTouchOutside = true;

        /// <summary>
        /// 创建自定义弹窗
        /// </summary>
        /// <param name="context">上下文（Activity）</param>
        public CustomDialog(Context context)
        {
            _context = context;

            // 创建Dialog，使用透明主题（无标题、背景透明，消除灰色矩形）
            _dialog = new Dialog(context, Android.Resource.Style.ThemeTranslucentNoTitleBar);

            // 设置外部点击取消行为
            _dialog.SetCancelable(true);
            _dialog.SetCanceledOnTouchOutside(_cancelOnTouchOutside);

            // 设置布局
            var layout = new LinearLayout(context)
            {
                Orientation = Orientation.Vertical
            };

            // 设置弹窗背景（圆角白色背景）
            var background = new GradientDrawable();
            background.SetColor(Android.Graphics.Color.White);
            background.SetCornerRadius(16 * context.Resources.DisplayMetrics.Density);
            layout.Background = background;

            // 设置内边距
            int padding = (int)(24 * context.Resources.DisplayMetrics.Density);
            layout.SetPadding(padding, padding, padding, padding);

            // 创建标题TextView
            _titleTextView = new TextView(context)
            {
                TextSize = 18,
                TextAlignment = TextAlignment.ViewStart
            };
            _titleTextView.SetTextColor(Android.Graphics.Color.Black);
            _titleTextView.SetTypeface(null, Android.Graphics.TypefaceStyle.Bold);
            layout.AddView(_titleTextView, new LinearLayout.LayoutParams(
                ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.WrapContent));

            // 创建介绍TextView
            _messageTextView = new TextView(context)
            {
                TextSize = 14,
                TextAlignment = TextAlignment.ViewStart
            };
            _messageTextView.SetTextColor(Android.Graphics.Color.DarkGray);
            var messageLayoutParams = new LinearLayout.LayoutParams(
                ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.WrapContent);
            messageLayoutParams.TopMargin = (int)(16 * context.Resources.DisplayMetrics.Density);
            layout.AddView(_messageTextView, messageLayoutParams);

            // 创建按钮容器（用于右对齐）
            var buttonContainer = new LinearLayout(context)
            {
                Orientation = Orientation.Horizontal,
                LayoutParameters = new LinearLayout.LayoutParams(
                    ViewGroup.LayoutParams.MatchParent,
                    ViewGroup.LayoutParams.WrapContent)
            };
            buttonContainer.SetGravity(GravityFlags.Right);
            buttonContainer.SetPadding(0, (int)(24 * context.Resources.DisplayMetrics.Density), 0, 0);

            // 创建右下角按钮
            _confirmButton = new Button(context)
            {
                Text = "确定"
            };
            _confirmButton.SetTextColor(Android.Graphics.Color.White);
            _confirmButton.SetBackgroundColor(Android.Graphics.Color.ParseColor("#2196F3"));

            // 设置按钮圆角背景
            var buttonBackground = new GradientDrawable();
            buttonBackground.SetColor(Android.Graphics.Color.ParseColor("#2196F3"));
            buttonBackground.SetCornerRadius(8 * context.Resources.DisplayMetrics.Density);
            _confirmButton.Background = buttonBackground;

            var buttonLayoutParams = new LinearLayout.LayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent);
            buttonContainer.AddView(_confirmButton, buttonLayoutParams);

            // 创建滚动按钮（像ScrollView一样可以滚动）
            _scrollView = new ScrollView(context)
            {
                VerticalScrollBarEnabled = true
            };
            _scrollView.SetBackgroundColor(Android.Graphics.Color.Transparent);

            var scrollLayoutParams = new LinearLayout.LayoutParams(
                ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.WrapContent);
            scrollLayoutParams.TopMargin = (int)(16 * context.Resources.DisplayMetrics.Density);


            // 设置按钮宽度为弹窗宽度的80%
            var displayMetrics = context.Resources.DisplayMetrics;
            int dialogWidth = (int)(displayMetrics.WidthPixels * 0.85);
            int buttonWidth = (int)(dialogWidth * 0.8);

            var scrollButtonLayoutParams = new LinearLayout.LayoutParams(
                buttonWidth,
                ViewGroup.LayoutParams.WrapContent);
            scrollButtonLayoutParams.Gravity = GravityFlags.CenterHorizontal;
            scrollButtonLayoutParams.TopMargin = (int)(8 * context.Resources.DisplayMetrics.Density);

            //_scrollView.AddView(_scrollButton, scrollButtonLayoutParams);
            layout.AddView(_scrollView, scrollLayoutParams);

            layout.AddView(buttonContainer);

            // 设置Dialog内容
            _dialog.SetContentView(layout);

            // 设置Dialog窗口属性
            var window = _dialog.Window;
            if (window != null)
            {
                // 设置背景变暗
                window.AddFlags(WindowManagerFlags.DimBehind);
                window.SetDimAmount(0.6f); // 设置变暗程度（0.0 - 1.0）

                // 设置弹窗宽度
                displayMetrics = context.Resources.DisplayMetrics;
                int width = (int)(displayMetrics.WidthPixels * 0.85); // 占屏幕宽度的85%
                window.SetLayout(width, ViewGroup.LayoutParams.WrapContent);
            }
        }

        /// <summary>
        /// 设置标题文本
        /// </summary>
        public void SetTitle(string title)
        {
            _titleTextView.Text = title;
        }

        /// <summary>
        /// 设置介绍文本
        /// </summary>
        public void SetMessage(string message)
        {
            _fullMessage = message;
            if (_isTypewriterEnabled)
            {
                StartTypewriterEffect();
            }
            else
            {
                _messageTextView.Text = message;
            }
        }

        /// <summary>
        /// 开启或关闭逐字显示效果
        /// </summary>
        /// <param name="enable">true开启逐字显示，false直接显示全部文本</param>
        public void EnableTypewriterEffect(bool enable)
        {
            _isTypewriterEnabled = enable;
        }

        /// <summary>
        /// 设置逐字显示完成时的回调函数
        /// </summary>
        /// <param name="onComplete">文本全部显示完毕时触发的回调</param>
        public void SetOnTypewriterComplete(System.Action onComplete)
        {
            _onTypewriterComplete = null;
            _onTypewriterComplete = onComplete;
        }

        /// <summary>
        /// 开始逐字显示效果
        /// </summary>
        private void StartTypewriterEffect()
        {
            // 停止之前的定时器
            StopTypewriterEffect();

            _currentCharIndex = 0;
            _messageTextView.Text = "";

            if (string.IsNullOrEmpty(_fullMessage))
            {
                _onTypewriterComplete?.Invoke();
                return;
            }

            _typewriterTimer = new Java.Util.Timer();
            _typewriterTimer.ScheduleAtFixedRate(new TypewriterTimerTask(this), 0, 200);
        }

        /// <summary>
        /// 停止逐字显示效果
        /// </summary>
        private void StopTypewriterEffect()
        {
            _typewriterTimer?.Cancel();
            _typewriterTimer = null;
        }

        /// <summary>
        /// 更新显示的字符（在主线程执行）
        /// </summary>
        private void UpdateTypewriterText()
        {
            if (_currentCharIndex < _fullMessage.Length)
            {
                _currentCharIndex++;
                _messageTextView.Text = _fullMessage.Substring(0, _currentCharIndex);
            }
            else
            {
                StopTypewriterEffect();
                _onTypewriterComplete?.Invoke();
            }
        }

        /// <summary>
        /// 逐字显示的定时器任务
        /// </summary>
        private class TypewriterTimerTask : Java.Util.TimerTask
        {
            private readonly CustomDialog _dialog;

            public TypewriterTimerTask(CustomDialog dialog)
            {
                _dialog = dialog;
            }

            public override void Run()
            {
                (_dialog._context as Activity)?.RunOnUiThread(() =>
                {
                    _dialog.UpdateTypewriterText();
                });
            }
        }

        /// <summary>
        /// 设置按钮文本
        /// </summary>
        public void SetButtonText(string text)
        {
            _confirmButton.Text = text;
        }

        /// <summary>
        /// 设置按钮点击事件
        /// </summary>
        public void SetOnButtonClick(System.Action onClick)
        {
            _confirmButton.Click += (s, e) =>
            {
                onClick?.Invoke();
            };
        }

        /// <summary>
        /// 设置滚动按钮（像ScrollView一样可以上下滚动查看超出屏幕的按钮）
        /// </summary>
        /// <param name="text">按钮文本</param>
        /// <param name="onClick">按钮点击响应函数</param>
        public void AddScrollButton(string text, System.Action onClick)
        {
            // 设置按钮背景（淡灰色圆角）
            var scrollButtonBackground = new GradientDrawable();
            scrollButtonBackground.SetColor(Android.Graphics.Color.ParseColor("#D3D3D3"));
            scrollButtonBackground.SetCornerRadius(8 * _context.Resources.DisplayMetrics.Density);
            Button newScrollButton = new Button(_context)
            {
                Text = text,
                TextSize = 14,
                Background = scrollButtonBackground
            };
            newScrollButton.SetTextColor(Android.Graphics.Color.Black);
            newScrollButton.Click += (s, e) =>
            {
                onClick?.Invoke();
            };

            _scrollButtons.Add(newScrollButton);
            _scrollView?.AddView(newScrollButton);
        }

        private void NewScrollButton_Click(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void BecomeActionDialog(string title, string intro, string finishText, System.Action onclick)
        {
            _actionDialog = new CustomDialog(_context);

            SetTitle(title);
            SetMessage(intro);
            SetButtonText("执行");
            _actionDialog.SetOnButtonClick(() =>
            {
                _actionDialog.Hide();
                _actionDialog.ResetActionDialog(finishText);
            });
            _actionDialog.SetOnTypewriterComplete(() =>
            {
                onclick?.Invoke();
            });
            SetOnButtonClick(() =>
            {
                Hide();
                _actionDialog.Show();
                _actionDialog.EnableTypewriterEffect(true);
                _actionDialog.SetMessage(finishText);
                _actionDialog.SetButtonText("关闭");

            });
        }

        public void ResetActionDialog(string finishText)
        {
            SetMessage(finishText);
        }

        /// <summary>
        /// 显示弹窗，屏幕背景变暗，阻止后面的组件交互
        /// </summary>
        public void Show()
        {
            if (!_dialog.IsShowing)
            {
                _dialog.Show();
            }
        }

        /// <summary>
        /// 隐藏弹窗，屏幕变暗失效，恢复后面组件的交互
        /// </summary>
        public void Hide()
        {
            if (_dialog.IsShowing)
            {
                _dialog.Dismiss();
            }
        }

        /// <summary>
        /// 判断弹窗是否正在显示
        /// </summary>
        public bool IsShowing => _dialog.IsShowing;

        /// <summary>
        /// 设置或获取是否允许点击弹窗外部关闭弹窗
        /// </summary>
        public bool CancelOnTouchOutside
        {
            get => _cancelOnTouchOutside;
            set
            {
                _cancelOnTouchOutside = value;
                _dialog.SetCanceledOnTouchOutside(value);
            }
        }
    }
}
