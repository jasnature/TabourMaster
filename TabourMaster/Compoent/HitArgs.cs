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
    /// 击中参数
    /// </summary>
    public class HitArgs : EventArgs
    {
        /// <summary>
        /// 信号原始对象
        /// </summary>
        public CadenceSign SignSource
        {
            get;
            set;
        }

        /// <summary>
        /// 敲击的位置
        /// </summary>
        public DrumType DrumType
        {
            get;
            set;
        }

        /// <summary>
        /// 击中类型
        /// </summary>
        public HitType HitType
        {
            get;
            set;
        }


    }
}
