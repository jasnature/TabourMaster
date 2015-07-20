using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;

namespace TabourMaster.UControl
{
    /// <summary>
    /// 连击值显示控件
    /// </summary>
    public partial class DoubleHitTime : UserControl
    {
        public DoubleHitTime()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 当前连击数是否在显示
        /// </summary>
        private bool HitVisbleState = false;

        private int hitCount;

        /// <summary>
        /// 获取连击数
        /// </summary>
        public int HitCount
        {
            get { return hitCount; }
        }

        /// <summary>
        /// 增加击中连击数
        /// </summary>
        public void AddHit()
        {
            Interlocked.Increment(ref hitCount);
            if (!HitVisbleState && hitCount >= 5)
            {
                tbHitCount.Visibility = System.Windows.Visibility.Visible;
                HitVisbleState = true;
            }
            this.tbHitCount.Text = hitCount.ToString().PadLeft(3, '0');
        }

        /// <summary>
        /// 清零连击数
        /// </summary>
        public void ClearHit()
        {
            hitCount = 0;
            HitVisbleState = false;
            tbHitCount.Visibility = System.Windows.Visibility.Collapsed;
        }


    }
}
