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

namespace TabourMaster
{

    /// <summary>
    /// create by liujing
	/// cnblog: http://www.cnblogs.com/NatureSex/
    /// </summary>
    public partial class MusicListPanel : UserControl, IMyControl
    {
        //背景歌曲
        MediaElement mebg = new MediaElement();

        //MediaElement meMus = new MediaElement();

        bool _isStartPlay = false;

        /// <summary>
        /// 默认音乐列表,通过ReadMusicXML();方法赋值
        /// </summary>
        List<MusicInfo> MusicInfos = new List<MusicInfo>(10);

        public MusicListPanel(bool isStartPlay)
        {
            InitializeComponent();
            _isStartPlay = isStartPlay;
            this.Loaded += new RoutedEventHandler(MusicListPanel_Loaded);
        }

        void MusicListPanel_Loaded(object sender, RoutedEventArgs e)
        {
            mebg.Source = new Uri("/Res/Sound/jingle_result.wma", UriKind.Relative);
            mebg.AutoPlay = _isStartPlay;
            mebg.MediaEnded += new RoutedEventHandler(me_MediaEnded_Replay);
            LayoutRoot.Children.Add(mebg);

            //默认音乐
            ReadMusicXML();
            lbMusicList.ItemsSource = MusicInfos;
            //本地录制歌曲
            if (CommHelper.LocalMusicInfos == null)
            {
                CommHelper.GetLocalMusicList();
            }
            lbLocalMusicList.ItemsSource = CommHelper.LocalMusicInfos;
        }

        void me_MediaEnded_Replay(object sender, RoutedEventArgs e)
        {
            ((MediaElement)sender).Stop();
            ((MediaElement)sender).Play();
        }

        /// <summary>
        /// 读取音乐资源
        /// </summary>
        private void ReadMusicXML()
        {
            Stream s = Application.GetResourceStream(new Uri("Res/MusicList.xml", UriKind.Relative)).Stream;
            XmlReader xr = XmlReader.Create(s);
            //xr.ReadToNextSibling("/Musics/Music");
            while (xr.Read())
            {
                XmlNodeType xnt = xr.NodeType;
                if (xr.LocalName.Equals("Music"))
                {
                    MusicInfo mi = new MusicInfo();
                    for (int i = 0; i < xr.AttributeCount; i++)
                    {
                        xr.MoveToAttribute(i);
                        if (xr.Name.Equals("Name", StringComparison.CurrentCultureIgnoreCase))
                        {
                            mi.MusicName = xr.Value;
                        }
                        else if (xr.Name.Equals("Length", StringComparison.CurrentCultureIgnoreCase))
                        {
                            mi.MusicLengthMill = xr.ReadContentAsInt();
                        }
                        else if (xr.Name.Equals("Data", StringComparison.CurrentCultureIgnoreCase))
                        {
                            mi.MusicData = xr.Value;
                        }
                        else if (xr.Name.Equals("Key", StringComparison.CurrentCultureIgnoreCase))
                        {
                            mi.Key = xr.Value;
                        }
                        else if (xr.Name.Equals("Speed", StringComparison.CurrentCultureIgnoreCase))
                        {
                            mi.Speed = xr.ReadContentAsInt();
                        }
                    }
                    MusicInfos.Add(mi);
                }
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (lbMusicList.SelectedItem != null)
            {
                CommHelper.CurrnetSelectMusic = lbMusicList.SelectedItem as MusicInfo;
            }
            else if (lbLocalMusicList.SelectedItem != null)
            {
                CommHelper.CurrnetSelectMusic = lbLocalMusicList.SelectedItem as MusicInfo;
            }
            if (CommHelper.CurrnetSelectMusic != null)
            {
                CommHelper.BaseNavigate(this, PanelType.GamePanel);
            }
        }


        public void ControlVisible()
        {
            mebg.Play();
        }

        public void ControlCollapsed()
        {
            lbLocalMusicList.SelectedIndex = -1;
            lbMusicList.SelectedIndex = -1;
            mebg.Stop();
        }

        private void btnBackMenu_Click(object sender, RoutedEventArgs e)
        {
            mebg.Stop();
            CommHelper.BaseNavigate(this, PanelType.StartPanel);
        }

        #region 互斥

        private void lbMusicList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbMusicList.SelectedItem != null)
            {
                tbSpeed.Text = ((MusicInfo)lbMusicList.SelectedItem).Speed.ToString();
            }
            lbLocalMusicList.SelectedIndex = -1;
        }

        private void lbLocalMusicList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbLocalMusicList.SelectedItem != null)
            {
                tbSpeed.Text = ((MusicInfo)lbLocalMusicList.SelectedItem).Speed.ToString();
            }
            lbMusicList.SelectedIndex = -1;
        }

        #endregion

        private void btnUpdateList_Click(object sender, RoutedEventArgs e)
        {
            lbLocalMusicList.ItemsSource = CommHelper.GetLocalMusicList();
        }




    }
}
