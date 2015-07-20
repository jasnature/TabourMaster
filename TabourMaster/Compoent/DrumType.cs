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
    /// 鼓面位置类型枚举
    /// </summary>
    public enum DrumType
    {
        /// <summary>
        /// 左侧
        /// </summary>
        LeftSide = 33,
        /// <summary>
        /// 右侧
        /// </summary>
        RightSide = 40,
        /// <summary>
        /// 左正
        /// </summary>
        LeftFace = 35,
        /// <summary>
        /// 右正
        /// </summary>
        RightFace = 39,

        /// <summary>
        /// 正面全部
        /// </summary>
        FaceAll = LeftFace + RightFace,
        /// <summary>
        /// 侧面全部
        /// </summary>
        SideAll = LeftSide + RightSide,
    }
}
