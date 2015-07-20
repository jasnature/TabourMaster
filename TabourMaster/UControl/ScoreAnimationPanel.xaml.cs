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
    public partial class ScoreAnimationPanel : UserControl
    {
        public ScoreAnimationPanel()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Score_Loaded);
        }

        private Timer _timer = null;//为了不影响主线程！使用其他线程更新分数

        ManualResetEvent mre = new ManualResetEvent(false);

        /// <summary>
        /// 当前总分数
        /// </summary>
        private int _nowTotalScore = 0;
        /// <summary>
        /// 当前总分数
        /// </summary>
        public int NowTotalScore
        {
            get { return _nowTotalScore + _oddScore; }
        }

        /// <summary>
        /// 剩余要加上的分数
        /// </summary>
        private int _oddScore = 0;

        /// <summary>
        /// 开始记分
        /// </summary>
        public void StartCalc()
        {
            mre.Set();
        }

        /// <summary>
        /// 暂停记分
        /// </summary>
        public void PauseCalc()
        {
            mre.Reset();
        }

        /// <summary>
        /// 增加分数
        /// </summary>
        /// <param name="score">需要增加分数</param>
        public void AddScore(int score)
        {
            Interlocked.Add(ref _oddScore, score);
            
        }

        /// <summary>
        /// 分数清零
        /// </summary>
        public void Clear()
        {
            Interlocked.Exchange(ref _oddScore, 0);
            lblScore.Text = "0000000";
            _nowTotalScore = 0;
        }

        void Score_Loaded(object sender, RoutedEventArgs e)
        {
            _timer = new Timer(OddCallback, null, 0, 10);
        }

        private void OddCallback(object state)
        {
            mre.WaitOne();
            // 如果有需要增加的分数
            if (_oddScore > 0)
            {
                // 定时器的此次回调需要增加的得分数
                //（缓冲分数为1或2位数则此次回调增加1分，3位数增加10分，4位数增加100分，以此类推）
                var i = 1;

                if (_oddScore.ToString().Length > 2)
                {
                    i = (int)Math.Pow(10, _oddScore.ToString().Length - 2);
                }

                Interlocked.Add(ref _oddScore, -i);

                this.Dispatcher.BeginInvoke(delegate
                    ()
                {
                    lblScore.Text = (_nowTotalScore + i).ToString().PadLeft(7, '0');
                    Interlocked.Add(ref _nowTotalScore, i);
                });
            }
        }
    }
}
