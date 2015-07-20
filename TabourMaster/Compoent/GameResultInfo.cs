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
    /// 游戏结果信息
    /// </summary>
    public class GameResultInfo
    {
        public string MusicName
        {
            get;
            set;
        }
        /// <summary>
        /// 总信号数
        /// </summary>
        public int SignCount
        {
            get;
            set;
        }

        /// <summary>
        /// 总得分
        /// </summary>
        public int ScoreSum
        {
            get;
            set;
        }
        /// <summary>
        /// 最大连击数
        /// </summary>
        public int MaxDoubleHits
        {
            get;
            set;
        }
        /// <summary>
        /// 完成敲击百分比
        /// </summary>
        public int HitPercent
        {
            get;
            set;
        }
        /// <summary>
        /// Cool敲击数量
        /// </summary>
        public int CoolHits
        {
            get;
            set;
        }
        /// <summary>
        /// Bad敲击数量
        /// </summary>
        public int BadHits
        {
            get;
            set;
        }
        /// <summary>
        /// 普通敲击数量
        /// </summary>
        public int NormalHits
        {
            get;
            set;
        }
        /// <summary>
        /// 总体评价(优，良，普通，加油)
        /// </summary>
        public string TotalEvaluate
        {
            get;
            set;
        }
    }


}
