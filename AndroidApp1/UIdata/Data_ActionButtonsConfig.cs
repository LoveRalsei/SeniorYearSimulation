using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidApp1.UIData
{
    internal class Data_ActionButtonsConfig
    {
        protected MainActivity _mainActivity;
        protected Student _student;

        public List<ActionButtonConfig> ActionButtons;

        public Data_ActionButtonsConfig(MainActivity mainActivity)
        {
            _mainActivity = mainActivity;
            _student = GameManager.StudentData;
        }
    }
}
