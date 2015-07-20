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
using System.Windows.Threading;
using System.Threading;
using System.IO.IsolatedStorage;

namespace TabourMaster
{

/// <summary>
    /// create by liujing
	/// cnblog: http://www.cnblogs.com/NatureSex/
    /// </summary>
    public partial class MusicRecordSignPanel : UserControl, IMyControl
    {

        /// <summary>
        /// 选取的文件信息
        /// </summary>
        FileInfo musicFile = null;
        /// <summary>
        /// 播放歌曲的媒体文件
        /// </summary>
        MediaElement meMusic = new MediaElement();

        /// <summary>
        /// 对话框
        /// </summary>
        OpenFileDialog ofd = new OpenFileDialog();

        /// <summary>
        /// 秒表
        /// </summary>
        DispatcherTimer dtMusic = new DispatcherTimer();

        /// <summary>
        /// 存放信号集合
        /// </summary>
        List<string> listSigns = new List<string>(100);

        public MusicRecordSignPanel()
        {
            InitializeComponent();
            InitialCFG();
        }

        /// <summary>
        /// 初始化配置
        /// </summary>
        private void InitialCFG()
        {
            //音乐
            meMusic.Volume = 0.7f;
            meMusic.AutoPlay = true;
            meMusic.MediaEnded += new RoutedEventHandler(meMusic_MediaEnded);
            LayoutRoot.Children.Add(meMusic);
            //属性设置
            ofd.Filter = "Silverlight支持音频格式|*.wma;*.mp3;*.mp4;*.wmv;*.m4a";
            btnSave.Visibility = System.Windows.Visibility.Collapsed;
            tbHelp.Visibility = System.Windows.Visibility.Collapsed;
            //计时器
            dtMusic.Tick += new EventHandler(dt_Tick);
            dtMusic.Interval = new TimeSpan(0, 0, 0, 0, 10);

            //事件
            this.KeyUp += new KeyEventHandler(MusicRecordSignPanel_KeyUp);

            //数据源
            if (Application.Current.IsRunningOutOfBrowser) ReloadLocalMusicList();

#if !DEBUG
            this.btnCopySign.Visibility = System.Windows.Visibility.Collapsed;
#endif
        }

        /// <summary>
        /// 音乐结束后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void meMusic_MediaEnded(object sender, RoutedEventArgs e)
        {
            StopRelaPty(false);
            isStart = false;
            btnStartRecord.Content = "开始录制";
        }

        /// <summary>
        /// 监控按键信号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MusicRecordSignPanel_KeyUp(object sender, KeyEventArgs e)
        {
            if (!isStart || !CommHelper.CheckKeyLicity(e.Key)) return;
            long realLapseTime = lapseTime - PrelapseTime;
            PrelapseTime = lapseTime;
            string tempSign = string.Empty;
            switch (e.Key)
            {
                case Key.D:
                    tempSign = "LS-" + realLapseTime;
                    break;
                case Key.F:
                    tempSign = "LF-" + realLapseTime;
                    break;
                case Key.J:
                    tempSign = "RF-" + realLapseTime;
                    break;
                case Key.K:
                    tempSign = "RS-" + realLapseTime;
                    break;
                case Key.L:
                    tempSign = "FA-" + realLapseTime;
                    break;
                case Key.S:
                    tempSign = "SA-" + realLapseTime;
                    break;
            }
            listSigns.Add(tempSign);
            Interlocked.Increment(ref signCount);
            tbSignCount.Text = signCount.ToString();
            tbPreRecordTime.Text = PrelapseTime.ToString();
            //调试用
            AddPreviewSign(e);
        }

        /// <summary>
        /// 预览信号
        /// </summary>
        /// <param name="e"></param>
        private void AddPreviewSign(KeyEventArgs e)
        {
            Image tempimg = null;
            switch (e.Key)
            {
                case Key.D://蓝小
                case Key.K:
                    tempimg = new Image();
                    tempimg.Width = tempimg.Height = 15;
                    tempimg.Source = ResourceMgr.SignimgBgCache[2];
                    wpSigns.Children.Add(tempimg);
                    break;
                case Key.F://黄小
                case Key.J:
                    tempimg = new Image();
                    tempimg.Width = tempimg.Height = 15;
                    tempimg.Source = ResourceMgr.SignimgBgCache[0];
                    wpSigns.Children.Add(tempimg);
                    break;
                case Key.L://黄大
                    tempimg = new Image();
                    tempimg.Width = tempimg.Height = 20;
                    tempimg.Source = ResourceMgr.SignimgBgCache[1];
                    wpSigns.Children.Add(tempimg);
                    break;
                case Key.S://蓝大
                    tempimg = new Image();
                    tempimg.Width = tempimg.Height = 20;
                    tempimg.Source = ResourceMgr.SignimgBgCache[3];
                    wpSigns.Children.Add(tempimg);
                    break;
            }
        }

        /// <summary>
        /// 当前计时器逝去时间,毫秒
        /// </summary>
        long lapseTime = 0;

        /// <summary>
        /// 上次记录时间
        /// </summary>
        long PrelapseTime = 0;

        /// <summary>
        /// 是否开始
        /// </summary>
        bool isStart = false;

        /// <summary>
        /// 信号数量
        /// </summary>
        int signCount = 0;

        void dt_Tick(object sender, EventArgs e)
        {
            if (isStart)
            {
                Interlocked.Add(ref lapseTime, 10);
                tbCurrentPassTime.Text = lapseTime.ToString();
            }
        }


        /// <summary>
        /// 选择本地文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.IsRunningOutOfBrowser)
            {
                if (ofd.ShowDialog().Value)
                {
                    musicFile = ofd.File;
                    tbMusicPath.Text = musicFile.FullName;
                }
            }
            else
            {
                ucmSave.Show("该功能需要安装到本地运行!\n请在菜单界面点击安装程序!");
            }
        }

        /// <summary>
        /// 开始录制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartRecord_Click(object sender, RoutedEventArgs e)
        {

            if (!isStart)
            {
                if (musicFile != null && File.Exists(musicFile.FullName))
                {
                    isStart = true;
                    StopRelaPty(true);
                    meMusic.SetSource(musicFile.OpenRead());
                    meMusic.Play();
                    dtMusic.Start();
                    SetStartProperty();
                    btnStartRecord.Content = "停止";
                }
                else
                {
                    ucmSave.Show("请选择需要录制节拍的音频文件!");
                }
            }
            else
            {
                StopRelaPty(false);
                isStart = false;
                btnStartRecord.Content = "开始录制";
            }

        }

        /// <summary>
        /// 设置开始录制时控件属性
        /// </summary>
        private void SetStartProperty()
        {
            listSigns.Clear();
            lapseTime = 0;
            PrelapseTime = 0;
            btnSave.Visibility = System.Windows.Visibility.Visible;

        }

        /// <summary>
        /// 停止相关属性
        /// </summary>
        private void StopRelaPty(bool isClear)
        {
            meMusic.Stop();
            dtMusic.Stop();
            if (isClear)
            {
                lapseTime = 0;
                PrelapseTime = 0;
                this.wpSigns.Children.Clear();
                this.tbCurrentPassTime.Text = string.Empty;
                this.tbMusicPath.Text = string.Empty;
                this.tbPreRecordTime.Text = string.Empty;
                this.tbSignCount.Text = string.Empty;

            }
            if (listSigns.Count <= 0)
            {
                btnSave.Visibility = System.Windows.Visibility.Collapsed;
            }
        }


        #region IMyControl 方法

        public void ControlVisible()
        {

        }

        public void ControlCollapsed()
        {
            StopRelaPty(true);
            isStart = false;
        }

        #endregion

        /// <summary>
        /// 返回主菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBackMenu_Click(object sender, RoutedEventArgs e)
        {
            CommHelper.BaseNavigate(this, PanelType.StartPanel);
        }


        bool isHelp = false;

        /// <summary>
        /// 帮助
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBackMenu_Copy1_Click(object sender, RoutedEventArgs e)
        {
            if (!isHelp)
            {
                tbHelp.Visibility = System.Windows.Visibility.Visible;
                Canvas.SetZIndex(tbHelp, 999);
                sbHelp.Stop();
                sbHelp.Begin();
                isHelp = true;
            }
            else
            {
                tbHelp.Visibility = System.Windows.Visibility.Collapsed;
                isHelp = false;
            }
        }

        #region 保存到本地存储



        UControl.UChildMessage ucmSave = new UControl.UChildMessage();

        /// <summary>
        /// 结束后保存到本地存储
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!isStart)
            {
                ucmSave.Closed += new EventHandler(ucm_Closed);
                ucmSave.Show("保存？", string.Format("确认保存该音乐文件和节拍文件吗?\n信息:\n名称:{0}\n播放速度:{1}", string.Format("{0}_Sign.sgn", musicFile.Name), ((ComboBoxItem)nuSpeed.SelectedItem).Content), MessageBoxButton.OKCancel);
            }
            else
            {
                ucmSave.Show("请先停止后在保存!");
            }
            this.UpdateLayout();
        }

        void ucm_Closed(object sender, EventArgs e)
        {
            if (ucmSave.DialogResult.Value)
            {
                IsolatedStorageFile isf = CommHelper.currentISF;

                CommHelper.CheckDiskCapcity();

                if (!isf.DirectoryExists("Musics"))
                {
                    isf.CreateDirectory("Musics");
                }

                IsolatedStorageFileStream isfs = new IsolatedStorageFileStream(string.Format("Musics/{0}_Sign.sgn", musicFile.Name), FileMode.Create, isf);
                IsolatedStorageFileStream realisfs = new IsolatedStorageFileStream(string.Format("Musics/{0}", musicFile.Name), FileMode.Create, isf);
                WriterMusicSignInfo(isfs, System.IO.Path.GetFileNameWithoutExtension(musicFile.Name));
                WriterRealMusic(realisfs);
                isfs.Close();
                realisfs.Close();
                ucmSave.Show("保存？", "保存成功!");
                listSigns.Clear();
                StopRelaPty(true);
                ucmSave.Closed -= new EventHandler(ucm_Closed);
                ReloadLocalMusicList();
                this.UpdateLayout();
            }
        }

        /// <summary>
        /// 写入一份音乐文件到本地存储
        /// </summary>
        /// <param name="isfs"></param>
        private void WriterRealMusic(IsolatedStorageFileStream isfs)
        {
            using (BinaryWriter sw = new BinaryWriter(isfs))
            {
                using (BinaryReader br = new BinaryReader(musicFile.OpenRead()))
                {
                    byte[] buff = new byte[1024 * 1024];
                    int count = 0;
                    while ((count = br.Read(buff, 0, 1024 * 1024)) > 0)
                    {
                        sw.Write(buff, 0, count);
                        Array.Clear(buff, 0, count);
                    }
                }
            }
        }


        /// <summary>
        /// 写入音乐节拍信息
        /// </summary>
        /// <param name="isfs"></param>
        /// <param name="musicName"></param>
        private void WriterMusicSignInfo(IsolatedStorageFileStream isfs, string musicName)
        {
            using (StreamWriter sw = new StreamWriter(isfs))
            {
                sw.WriteLine(musicName);//name
                sw.WriteLine("0");
                sw.WriteLine(musicFile.Name);//key
                sw.WriteLine(lapseTime);//musicLengthMill
                sw.WriteLine(Convert.ToInt32(((ComboBoxItem)nuSpeed.SelectedItem).Content));//speed
                sw.WriteLine(string.Join(",", listSigns));//data
                sw.Close();
            }
        }

        #endregion

        /// <summary>
        /// 刷新数据源并加载到控件
        /// </summary>
        private void ReloadLocalMusicList()
        {
            if (Application.Current.IsRunningOutOfBrowser)
            {
                CommHelper.GetLocalMusicList();
                lbLocalMusicList.ItemsSource = CommHelper.LocalMusicInfos;
            }
            else
            {
                ucmDel.Show("请安装后再刷新!谢谢!");
                this.UpdateLayout();
            }
            
        }


        private void lbLocalMusicList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                btnDel.IsEnabled = true;
            }
        }

        UControl.UChildMessage ucmDel = new UControl.UChildMessage();

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (lbLocalMusicList.SelectedItem != null)
            {
                ucmDel.Closed += new EventHandler(ucm_DeleteClosed);
                ucmDel.Show(null, "确认删除该节拍文件和相关联的音乐文件吗?", MessageBoxButton.OKCancel);
            }
        }

        void ucm_DeleteClosed(object sender, EventArgs e)
        {
            if (ucmDel.DialogResult.Value)
            {
                MusicInfo mi = lbLocalMusicList.SelectedItem as MusicInfo;
                CommHelper.DeleteLocalSignAndMusic(mi);
                ucmDel.Show("删除成功!");
                StopRelaPty(true);
                ReloadLocalMusicList();
                ucmDel.Closed -= new EventHandler(ucm_DeleteClosed);
            }
            this.UpdateLayout();
        }


        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(string.Join(",", listSigns));
            MessageBox.Show("复制到剪贴板成功!");
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            ReloadLocalMusicList();
        }


    }
}
