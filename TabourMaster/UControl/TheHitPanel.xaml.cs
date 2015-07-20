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
using System.Windows.Threading;

namespace TabourMaster.UControl
{
    /// <summary>
    /// 
    /// </summary>
    public partial class TheHitPanel : UserControl
    {

        BitmapImage[] flash = new BitmapImage[7];

        DispatcherTimer dt = new DispatcherTimer();

        public TheHitPanel()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(FlashPanel_Loaded);
        }

        void FlashPanel_Loaded(object sender, RoutedEventArgs e)
        {
            flash[1] = new BitmapImage(new Uri("/Res/Imgs/fl_1.png", UriKind.RelativeOrAbsolute));
            flash[2] = new BitmapImage(new Uri("/Res/Imgs/fl_2.png", UriKind.RelativeOrAbsolute));
            flash[3] = new BitmapImage(new Uri("/Res/Imgs/fl_3.png", UriKind.RelativeOrAbsolute));
            flash[4] = new BitmapImage(new Uri("/Res/Imgs/fl_4.png", UriKind.RelativeOrAbsolute));
            flash[5] = new BitmapImage(new Uri("/Res/Imgs/drums/explosion_lower1.png", UriKind.RelativeOrAbsolute));
            flash[6] = new BitmapImage(new Uri("/Res/Imgs/drums/explosion_lower2.png", UriKind.RelativeOrAbsolute));
            
            dt.Tick += new EventHandler(dt_Tick);
            dt.Interval = new TimeSpan(0, 0, 0, 0, 60);
        }

        byte _type = 0;

        int index = 0;

        void dt_Tick(object sender, EventArgs e)
        {
            if (index == 0)
            {
                if (_type == 0)
                {
                    flash[0] = flash[5];
                }
                else
                {
                    flash[0] = flash[6];
                }
            }
            Imgbg.Source = flash[index];
            index++;
            if (index == 5)
            {
                dt.Stop();
                index = 0;
                Imgbg.Source = null;
            }
        }

        /// <summary>
        /// 播放击中光环动画
        /// </summary>
        /// <param name="type">0 小脸 1 大脸</param>
        public void Play(byte type)
        {
            dt.Stop();
            _type = type;
            Canvas.SetZIndex(Imgbg, 0);
            dt.Start();
        }
    }
}
