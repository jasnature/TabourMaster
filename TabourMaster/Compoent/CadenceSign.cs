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
    /// 节奏信号
    /// </summary>
    public class CadenceSign
    {
        /// <summary>
        /// 显示用图像
        /// </summary>
        public Image currentSign;
        /// <summary>
        /// 延迟时间
        /// </summary>
        public int DelayTimeMill;
        /// <summary>
        /// 需要符合类型
        /// </summary>
        public DrumType drumType;

        /// <summary>
        /// 状态 0初始化 1 未被敲击 2 被敲中
        /// </summary>
        public byte state = 0;

        ///// <summary>
        ///// 是否还需要显示在界面上
        ///// </summary>
        //public bool isDisplay = true;
    }
}
