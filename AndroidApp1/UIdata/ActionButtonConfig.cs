using Android.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidApp1.UIData
{
    internal class ActionButtonConfig
    {
        public int ResourceId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CostText { get; set; }
        public int CostEnergy { get; set; }
        public List<KeyValuePair<StudentProperty, int>> Effects {  get; set; }
        public string DialogTitle { get; set; }
        public string DialogIntro { get; set; }
        public string DialogFinish { get; set; }

    }
}
