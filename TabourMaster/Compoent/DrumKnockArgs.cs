using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TabourMaster.Compoent
{
    /// <summary>
    /// 鼓参数
    /// </summary>
    public class DrumKnockArgs : EventArgs
    {
        /// <summary>
        /// 敲击的位置
        /// </summary>
        public DrumType DrumType
        {
            get;
            set;
        }

    }
}
