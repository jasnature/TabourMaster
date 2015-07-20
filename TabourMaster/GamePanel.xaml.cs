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
using TabourMaster.Compoent;
using System.IO;
using System.Xml;
using TabourMaster.UControl;
using System.Threading;


    /// <summary>
    /// create by liujing
	/// cnblog: http://www.cnblogs.com/NatureSex/
    /// </summary>
namespace TabourMaster
{

    /// <summary>
    /// 游戏主界面
    /// </summary>
    public partial class GamePanel : UserControl, IMyControl
    {
        /// <summary>
        /// 游戏完成当前信息
        /// </summary>
        private GameResultInfo currentGri = new GameResultInfo();

        private UControl.UChildMessage umsg = new UChildMessage();



        public GamePanel()
        {
            InitializeComponent();
            LoadGameInfo();
            StartGame();
        }

        /// <summary>
        /// 加载游戏信息
        /// </summary>
        private void LoadGameInfo()
        {
            this.Width = 600;
            this.Height = 300;
            tbEnd.Visibility = System.Windows.Visibility.Collapsed;
            //订阅
            this.KeyDown += new KeyEventHandler(LayoutRoot_KeyDown);
            this.KeyUp += new KeyEventHandler(LayoutRoot_KeyUp);
            drumHead.Knocked += new Action<UControl.Drumhead, DrumKnockArgs>(drumHead_Knocked);
            cpSignPanel.OnHit += new Action<UControl.CadencePanel, HitArgs>(cpSignPanel_OnHit);
            cpSignPanel.OnPlayEnd += new Action(cpSignPanel_OnPlayEnd);
        }

        Timer tr = null;

        /// <summary>
        /// 音乐播放完毕
        /// </summary>
        void cpSignPanel_OnPlayEnd()
        {
            tbEnd.Visibility = System.Windows.Visibility.Visible;
            //计算相关信息
            double percent = (double)(currentGri.CoolHits + currentGri.NormalHits) / cpSignPanel.CadenceCount;
            currentGri.HitPercent = (int)Math.Floor(percent * 100);
            CalcPingJia();
            currentGri.MusicName = CommHelper.CurrnetSelectMusic.MusicName;
            currentGri.SignCount = cpSignPanel.CadenceCount;
            //模拟计算
            tr = new Timer((state) =>
            {
                currentGri.ScoreSum = spScore.NowTotalScore;
                CommHelper.LastGameResultInfo = currentGri;
                ((GameEndPanel)ResourceMgr.CachePanel[PanelType.GameEndPanel]).DisplayGameEndInfo();
                CommHelper.BaseNavigate(this, PanelType.GameEndPanel);
                tr.Dispose();

            }, null, 2000, 999999);
        }

        /// <summary>
        /// 计算评价
        /// </summary>
        private void CalcPingJia()
        {
            if (currentGri.HitPercent < 60)
            {
                currentGri.TotalEvaluate = "加油";
            }
            else if (currentGri.HitPercent <= 70)
            {
                currentGri.TotalEvaluate = "普通";
            }
            else if (currentGri.HitPercent <= 80)
            {
                currentGri.TotalEvaluate = "良好";
            }
            else
            {
                currentGri.TotalEvaluate = "优秀";
            }

        }

        /// <summary>
        /// 敲击后如果有信号进入范围触发的方法
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        void cpSignPanel_OnHit(CadencePanel arg1, HitArgs arg2)
        {
            switch (arg2.HitType)
            {
                case HitType.Lost:
                    rlsPlayer.Play(HitType.Lost);
                    currentGri.BadHits++;
                    if (currentGri.MaxDoubleHits < dhtPanel.HitCount) currentGri.MaxDoubleHits = dhtPanel.HitCount;
                    dhtPanel.ClearHit();
                    break;
                case HitType.Normal:
                    dhtPanel.AddHit();
                    spScore.AddScore(dhtPanel.HitCount * 20);
                    fpScore.AddFetch(dhtPanel.HitCount * 3);
                    currentGri.NormalHits++;
                    PlayHitFlash(arg2);
                    rlsPlayer.Play(HitType.Normal);
                    break;
                case HitType.Nice:
                    dhtPanel.AddHit();
                    spScore.AddScore(dhtPanel.HitCount * 35);
                    fpScore.AddFetch(dhtPanel.HitCount * 6);
                    currentGri.CoolHits++;
                    PlayHitFlash(arg2);
                    rlsPlayer.Play(HitType.Nice);
                    break;
            }

        }

        /// <summary>
        /// 播放击中光环动画
        /// </summary>
        /// <param name="arg2"></param>
        private void PlayHitFlash(HitArgs arg2)
        {
            if (arg2.DrumType == DrumType.FaceAll || arg2.DrumType == DrumType.SideAll)
            {
                fpPlayer.Play(1);
            }
            else
            {
                fpPlayer.Play(0);
            }
        }

        /// <summary>
        /// 敲击右侧鼓后处理
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        void drumHead_Knocked(Drumhead arg1, DrumKnockArgs arg2)
        {

            //tbDoubleKits.Text = dbkKit.ToString();
        }



        //bool D_idDown = false;
        //bool F_idDown = false;
        //bool J_idDown = false;
        //bool K_idDown = false;

        /// <summary>
        /// 键盘按下事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayoutRoot_KeyDown(object sender, KeyEventArgs e)
        {
            // MessageBox.Show("ad");
            if (!CommHelper.CheckKeyLicity(e.Key)) return;
            ResponseSign(e);
            switch (e.Key)
            {
                case Key.D:
                    drumHead.KnockPlay(DrumType.LeftSide);
                    break;
                case Key.F:
                    drumHead.KnockPlay(DrumType.LeftFace);
                    break;
                case Key.J:
                    drumHead.KnockPlay(DrumType.RightFace);
                    break;
                case Key.K:
                    drumHead.KnockPlay(DrumType.RightSide);
                    break;
                case Key.L:
                    drumHead.KnockPlay(DrumType.FaceAll);
                    break;
                case Key.S:
                    drumHead.KnockPlay(DrumType.SideAll);
                    break;
            }
        }

        /// <summary>
        /// 响应按键处理到信号处理控件
        /// </summary>
        /// <param name="e"></param>
        private void ResponseSign(KeyEventArgs e)
        {
            int nowkey = (int)e.Key;

            if (e.Key == Key.S)
            {
                nowkey = 73;
            }
            if (e.Key == Key.L)
            {
                nowkey = 74;
            }
            //textBox1.Text = nowkey.ToString();
            cpSignPanel.NotifyKeyDown(CommHelper.KeyCodeToDrumType(nowkey));
        }

        /// <summary>
        /// 键盘抬起事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayoutRoot_KeyUp(object sender, KeyEventArgs e)
        {
            if (!CommHelper.CheckKeyLicity(e.Key)) return;

            switch (e.Key)
            {
                case Key.D:
                    drumHead.RelaseKnockPlay(DrumType.LeftSide);
                    break;
                case Key.F:
                    drumHead.RelaseKnockPlay(DrumType.LeftFace);
                    break;
                case Key.J:
                    drumHead.RelaseKnockPlay(DrumType.RightFace);
                    break;
                case Key.K:
                    drumHead.RelaseKnockPlay(DrumType.RightSide);
                    break;
                case Key.L:
                    drumHead.RelaseKnockPlay(DrumType.FaceAll);
                    break;
                case Key.S:
                    drumHead.RelaseKnockPlay(DrumType.SideAll);
                    break;
            }
        }

        /// <summary>
        /// 开始游戏
        /// </summary>
        /// <param name="MusicInfo">音乐</param>
        public void StartGame()
        {
            if (CommHelper.CurrnetSelectMusic != null)
            {
                spScore.StartCalc();
                fpScore.PlayFetch(100);
                //fpFetch.PlayFetch(1000);
                tbSpeedValue.Text = CommHelper.CurrnetSelectMusic.Speed.ToString();
                cpSignPanel.Play(CommHelper.CurrnetSelectMusic);
                Canvas.SetZIndex(border1, 9999);
                Canvas.SetZIndex(fpPlayer, 8888);
            }
        }

        /// <summary>
        /// 停止游戏
        /// </summary>
        public void StopGame()
        {
            cpSignPanel.Stop();
            dhtPanel.ClearHit();
            fpScore.StopFetch();
            drumHead.ComebackSet();
            spScore.Clear(); spScore.PauseCalc();
            tbEnd.Visibility = System.Windows.Visibility.Collapsed;
        }

        /// <summary>
        /// 选择音乐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            umsg.Closed += new EventHandler(umsg_Closed);
            umsg.Show("返回提示", "你确认重新选择音乐?这样将导致游戏结束!", MessageBoxButton.OKCancel);
        }

        void umsg_Closed(object sender, EventArgs e)
        {
            if (umsg.DialogResult.Value == true)
            {
                CommHelper.BaseNavigate(this, PanelType.MusicListPanel);
                umsg.Closed -= new EventHandler(umsg_Closed);
                this.UpdateLayout();
            }
        }

        /// <summary>
        /// 导航回主界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBackMenu_Click(object sender, RoutedEventArgs e)
        {
            umsg.Closed += new EventHandler(umsg_Closed1);
            umsg.Show("返回提示", "你确认返回主菜单?这样将导致游戏结束!", MessageBoxButton.OKCancel);
        }

        void umsg_Closed1(object sender, EventArgs e)
        {
            if (umsg.DialogResult.Value == true)
            {
                CommHelper.BaseNavigate(this, PanelType.StartPanel);
                umsg.Closed -= new EventHandler(umsg_Closed1);
                this.UpdateLayout();
            }
        }


        public void ControlVisible()
        {
            StartGame();
        }

        public void ControlCollapsed()
        {
            StopGame();
        }


    }
}
