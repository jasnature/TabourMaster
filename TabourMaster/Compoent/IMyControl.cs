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
    /// I我的控件哈哈
    /// </summary>
    public interface IMyControl
    {
        /// <summary>
        /// 控件显示的时候
        /// </summary>
        void ControlVisible();
        /// <summary>
        /// 控件隐藏的时候
        /// </summary>
        void ControlCollapsed();
    }
}
