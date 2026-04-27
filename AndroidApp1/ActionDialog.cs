using Android.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidApp1
{
    public class ActionDialog : CustomDialog
    {

        protected ActionDialog _actionDialog;

        public ActionDialog(Context context) : base(context)
        {
            
        }

        public void BecomeActionDialog(string title, string intro, string finishText, System.Action onclick)
        {
            if (_actionDialog != null)
                return;

            _actionDialog = new ActionDialog(_context);

            SetTitle(title);
            SetMessage(intro);
            SetButtonText("执行");
            _actionDialog.SetTitle(title);
            _actionDialog.SetOnButtonClick(() =>
            {
                _actionDialog.Hide();
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
    }
}
