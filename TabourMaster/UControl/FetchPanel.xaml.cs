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
using TabourMaster.Compoent;

namespace TabourMaster.UControl
{
    public partial class FetchPanel : UserControl
    {
        public FetchPanel()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(FetchPanel_Loaded);
        }

        SolidColorBrush scblv1 = new SolidColorBrush(Colors.Black);
        SolidColorBrush scblv2 = new SolidColorBrush(Colors.Yellow);
        SolidColorBrush scblv3 = new SolidColorBrush(Colors.Green);
        SolidColorBrush scblv4 = new SolidColorBrush(Colors.Red);
        SolidColorBrush scblv5 = new SolidColorBrush(Colors.Purple);

        void FetchPanel_Loaded(object sender, RoutedEventArgs e)
        {
            int h = 10;
            for (int i = 0; i < rectMax; i++)
            {
                Rectangle re = new Rectangle();
                re.StrokeThickness = 1;
                re.Stroke = new SolidColorBrush(Colors.Transparent);
                re.Width = 3;
                if (i < 30) { h = 8; re.Fill = scblv1; }
                else if (i < 40) { h = 11; re.Fill = scblv2; }
                else if (i < 55) { h = 14; re.Fill = scblv3; }
                else if (i < 80) { h = 17; re.Fill = scblv4; }
                else { h = 20; re.Fill = scblv5; }
                re.Height = h;
                re.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                rectFetch[i] = re;
            }

        }

        Timer timerFetch = null;

        /// <summary>
        /// 预先初始化
        /// </summary>
        Rectangle[] rectFetch = new Rectangle[rectMax];

        /// <summary>
        /// 魂数量
        /// </summary>
        int fetchSum = 0;

        /// <summary>
        /// 缓存用魂
        /// </summary>
        int buffFecth = 0;

        /// <summary>
        /// 多少魂一个方格子
        /// </summary>
        static int rectScore = 100;

        /// <summary>
        /// 方格子最大数量
        /// </summary>
        static int rectMax = 93;

        /// <summary>
        /// 当前方格子数量
        /// </summary>
        int rectCount = 0;

        /// <summary>
        /// 添加对应的魂数量
        /// </summary>
        /// <param name="fetchCount"></param>
        public void AddFetch(int fetchCount)
        {
            Interlocked.Add(ref buffFecth, fetchCount);
            if (buffFecth >= rectScore)
            {
                Interlocked.Exchange(ref buffFecth, buffFecth - rectScore);
                Interlocked.Add(ref fetchSum, rectScore);
                if (rectCount <= rectMax - 1)
                {
                    wpFetchContainer.Dispatcher.BeginInvoke(delegate()
                    {
                        if (fetchIndex <= rectMax - 1)
                        {
                            wpFetchContainer.Children.Add(rectFetch[fetchIndex]);
                        }
                    });
                    Interlocked.Increment(ref rectCount);
                    Interlocked.Increment(ref fetchIndex);
                }
            }
            tbFetchNum.Dispatcher.BeginInvoke(delegate()
            {
                tbFetchNum.Text = fetchSum.ToString();
            });
        }

        /// <summary>
        /// 开始播放魂动画
        /// </summary>
        /// <param name="delayTime">播放间隔</param>
        public void PlayFetch(int delayTime)
        {
            timerFetch = new Timer(timerFetch_Tick, null, delayTime, 100);
        }

        int fetchIndex = 0;//魂

        int picIndex = 0;//图片

        private void timerFetch_Tick(object ustate)
        {
            imgFetch.Dispatcher.BeginInvoke(delegate()
            {
                imgFetch.Source = ResourceMgr.FetchimgCache[picIndex];
            });
            picIndex++;
            if (picIndex >= 9)
            {
                picIndex = 0;
            }
        }


        public void StopFetch()
        {
            fetchIndex = 0;
            picIndex = 0;
            fetchSum = 0;
            buffFecth = 0;
            rectScore = 100;
            rectMax = 93;
            rectCount = 0;
            this.tbFetchNum.Text = "";
            if (timerFetch != null)
            {
                timerFetch.Dispose();
                timerFetch = null;
            }
        }

    }
}
