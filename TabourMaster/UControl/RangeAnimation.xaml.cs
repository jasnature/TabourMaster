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
using System.Windows.Media.Imaging;
using TabourMaster.Compoent;

namespace TabourMaster.UControl
{
    public partial class RangeAnimation : UserControl
    {
        BitmapImage good = null;
        BitmapImage normal = null;
        BitmapImage bad = null;

        public RangeAnimation()
        {
            // 为初始化变量所必需
            InitializeComponent();

            good = new BitmapImage(new Uri("/Res/Imgs/nice.png", UriKind.RelativeOrAbsolute));
            bad = new BitmapImage(new Uri("/Res/Imgs/bad.png", UriKind.RelativeOrAbsolute));
            normal = new BitmapImage(new Uri("/Res/Imgs/normal.png", UriKind.RelativeOrAbsolute));
            sbLevel.Completed += new EventHandler(sbLevel_Completed);
        }

        void sbLevel_Completed(object sender, EventArgs e)
        {
            image.Source = null;
        }

        /// <summary>
        /// 播放指定图片动画
        /// </summary>
        /// <param name="ht"></param>
        public void Play(HitType ht)
        {
            sbLevel.Stop();
            if (ht == HitType.Nice)
            {
                image.Source = good;
            }
            else if (ht == HitType.Normal)
            {
                image.Source = normal;
            }
            else
            {
                image.Source = bad;
            }
            sbLevel.Begin();
        }


    }
}
