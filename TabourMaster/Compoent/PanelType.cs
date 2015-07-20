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
    /// 容器类型
    /// </summary>
    public enum PanelType
    {
        /// <summary>
        /// 游戏界面
        /// </summary>
        GamePanel,
        /// <summary>
        /// 游戏结束画面
        /// </summary>
        GameEndPanel,
        /// <summary>
        /// 开始欢迎界面
        /// </summary>
        StartPanel,
        /// <summary>
        /// 音乐选择界面
        /// </summary>
        MusicListPanel,
        /// <summary>
        /// 音乐节拍录制界面
        /// </summary>
        MusicRecordSignPanel
    }
}
