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
using System.Threading;
using System.Windows.Browser;

namespace TabourMaster
{

    /// <summary>
    /// create by liujing
	/// cnblog: http://www.cnblogs.com/NatureSex/
    /// </summary>
    public partial class StartPanel : UserControl, IMyControl
    {
        //背景歌曲
        MediaElement mebg = new MediaElement();
        //按钮声音
        MediaElement meBtn = new MediaElement();
        //点击进入声音
        MediaElement btnClickSd = new MediaElement();

        UControl.UChildMessage umsg = new UControl.UChildMessage();

        public StartPanel()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(StartPanel_Loaded);
            Application.Current.CheckAndDownloadUpdateCompleted += new CheckAndDownloadUpdateCompletedEventHandler(Current_CheckAndDownloadUpdateCompleted);
            Application.Current.InstallStateChanged += new EventHandler(Current_InstallStateChanged);
            //fetchPanel1.st
        }

        void StartPanel_Loaded(object sender, RoutedEventArgs e)
        {
            mebg.Source = new Uri("/Res/Sound/jingle_select.wma", UriKind.Relative);
            meBtn.Source = new Uri("/Res/Sound/ka.wma", UriKind.Relative);
            btnClickSd.Source = new Uri("/Res/Sound/title_call.m4a", UriKind.Relative);
            mebg.AutoPlay = true;
            meBtn.AutoPlay = false;
            btnClickSd.AutoPlay = false;
            meBtn.Volume = 0.8f;
            mebg.Volume = 0.7f;
            btnClickSd.Volume = 0.9f;
            mebg.MediaEnded += new RoutedEventHandler(me_MediaEnded_Replay);
            meBtn.MediaEnded += new RoutedEventHandler(me_MediaEnded_Stop);
            btnClickSd.MediaEnded += new RoutedEventHandler(me_MediaEnded_Stop);
            LayoutRoot.Children.Add(mebg);
            LayoutRoot.Children.Add(meBtn);
            LayoutRoot.Children.Add(btnClickSd);
            PlayStartAnimation();
            //本地安装判断
            IsOOBRuning();
        }

        /// <summary>
        /// 是否是浏览器外运行
        /// </summary>
        private void IsOOBRuning()
        {
            if (Application.Current.InstallState == InstallState.Installed)
            {
                btnOOB.Content = "已经安装";
            }
            else
            {
                btnOOB.Content = "安装游戏";
            }
        }

        void me_MediaEnded_Replay(object sender, RoutedEventArgs e)
        {
            ((MediaElement)sender).Stop();
            ((MediaElement)sender).Play();
        }

        void me_MediaEnded_Stop(object sender, RoutedEventArgs e)
        {
            ((MediaElement)sender).Stop();
        }

        /// <summary>
        /// 播放开始动画和音乐
        /// </summary>
        private void PlayStartAnimation()
        {
            startAnimation.Stop();
            StartNameAni.Stop();
            startAnimation.Begin();
            StartNameAni.Begin();
            //音乐
            mebg.Play();
        }

        private void RelaseSomething()
        {
            mebg.Stop();
            startAnimation.Stop();
            StartNameAni.Stop();
        }

        /// <summary>
        /// 选择音乐界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectMusic_Click(object sender, RoutedEventArgs e)
        {
            CommHelper.BaseNavigate(this, PanelType.MusicListPanel);
        }

        #region IMyControl 调用方法

        public void ControlVisible()
        {
            PlayStartAnimation();
        }

        public void ControlCollapsed()
        {
            btnClickSd.Play();
            RelaseSomething();
        }
        #endregion

        private void btnRecordSign_Click(object sender, RoutedEventArgs e)
        {
            RelaseSomething();
            CommHelper.BaseNavigate(this, PanelType.MusicRecordSignPanel);
        }

        private void btnViewSigns_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSelectMusic_MouseEnter(object sender, MouseEventArgs e)
        {
            meBtn.Play();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDing_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.CheckAndDownloadUpdateAsync();
        }

        void Current_CheckAndDownloadUpdateCompleted(object sender, CheckAndDownloadUpdateCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.UpdateAvailable)
                {
                    umsg.Show("更新成功!请关闭程序重新运行!");
                    this.UpdateLayout();
                }
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.IsRunningOutOfBrowser)
            {
                ResourceMgr.CachePanel.Clear();
                this.LayoutRoot.Children.Clear();
                Application.Current.MainWindow.Close();
            }
            else
            {
                HtmlPage.Window.Invoke("close", null);
            }
        }

        /// <summary>
        /// 安装到本地
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.InstallState == InstallState.Installed)
            {
                umsg.Show("游戏已经安装！要卸载请右击界面后,在菜单中选择删除!");
                this.UpdateLayout();
                return;
            }
            Application.Current.Install();
        }

        void Current_InstallStateChanged(object sender, EventArgs e)
        {
            InstallState instate = Application.Current.InstallState;
            if (instate == InstallState.Installing)
            {
                btnOOB.Content = "安装中..";
            }
            if (instate == InstallState.Installed)
            {
                btnOOB.Content = "已经安装";
                Application.Current.InstallStateChanged -= new EventHandler(Current_InstallStateChanged);
            }
            if (instate == InstallState.InstallFailed || instate == InstallState.NotInstalled)
            {
                btnOOB.Content = "取消安装";
                Application.Current.InstallStateChanged -= new EventHandler(Current_InstallStateChanged);
            }
        }





    }
}
