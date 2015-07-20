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
using System.Windows.Threading;
using System.Threading;

namespace TabourMaster.UControl
{
    /// <summary>
    /// 节奏面板
    /// </summary>
    public partial class CadencePanel : UserControl
    {
        #region 字段

        MediaElement meMusic = new MediaElement();

        /// <summary>
        /// 信号图与按键集合
        /// </summary>
        private static CadenceSign[] CadenceSigns;

        /// <summary>
        /// 合法按下去的键值集合
        /// </summary>
        private volatile Queue<int> KeyDownCode = new Queue<int>();

        /// <summary>
        /// 游戏运行器之界面刷新
        /// </summary>
        DispatcherTimer dtWindows = new DispatcherTimer();

        /// <summary>
        /// 游戏运行器之辅助线程
        /// </summary>
        Timer logicThread;

        /// <summary>
        /// 当前计时器逝去时间,毫秒
        /// </summary>
        long lapseTime = 0;

        /// <summary>
        /// 达到可敲击返回移动时间
        /// </summary>
        int arriveEndPointOffset = 0;

        /// <summary>
        /// 是否开始
        /// </summary>
        bool isStart = false;

        /// <summary>
        /// 是否已经播放歌曲
        /// </summary>
        bool isPlaySound = false;

        /// <summary>
        /// 速度,默认移动一个像素
        /// </summary>
        double speed = 1;

        /// <summary>
        /// 信号数量
        /// </summary>
        int signCount = 0;

        /// <summary>
        /// 信号初始出现X轴
        /// </summary>
        double signLeftLocation = 550;

        /// <summary>
        /// 当前信号有效索引偏移
        /// </summary>
        int indexActual = 0;

        /// <summary>
        /// 第一个信号出现的时间
        /// </summary>
        int firstlongTime = 0;

        #endregion

        #region 属性

        /// <summary>
        /// 当前逝去时间,毫秒
        /// </summary>
        public long LapseTime
        {
            get { return lapseTime; }
        }

        /// <summary>
        /// 信号总数
        /// </summary>
        public int CadenceCount
        {
            get { return signCount; }
        }

        /// <summary>
        /// 游戏速度
        /// </summary>
        public double Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        #endregion

        #region 事件

        /// <summary>
        /// 在有效返回内敲击结束后引发的事件
        /// </summary>
        public event Action<CadencePanel, HitArgs> OnHit;

        /// <summary>
        /// 节奏播放完毕
        /// </summary>
        public event Action OnPlayEnd;

        #endregion

        /// <summary>
        /// 构造对象
        /// </summary>
        public CadencePanel()
        {
            InitializeComponent();
            InitDataAndCfg();
        }

        /// <summary>
        /// 初始化基础数据
        /// </summary>
        private void InitDataAndCfg()
        {
            //音乐属性初始化
            meMusic.Volume = 0.7f;
            //meMusic.AutoPlay = false;
            LayoutRoot.Children.Add(meMusic);
        }

        #region NotifyKeyDown 通知有按键按下

        double normalArrage = (500 / 3) * 10;//正常按下
        double niceArrage = (500 / 10) * 10;//漂亮
        double lostArrage = 500 * 10;//按错与位置出入太大

        /// <summary>
        /// 通知有按键按下
        /// </summary>
        /// <param name="key"></param>
        public void NotifyKeyDown(DrumType dt)
        {
            if (SignEnd()) return;
            for (int i = indexActual; i < signCount; )
            {
                if (null == CadenceSigns) return;
                CadenceSign cs = CadenceSigns[i];
                double atime = (lapseTime - arriveEndPointOffset);// / (1 * speed);//当前实际流逝时间

                if (CommHelper.Abs(cs.DelayTimeMill - atime) <= normalArrage)
                {
                    //double lv = Canvas.GetLeft(cs.currentSign);
                    cs.currentSign.Visibility = Visibility.Collapsed;
                    LayoutRoot.Children.Remove(cs.currentSign);
                    indexActual++;
                    cs.state = 2;
                    HitArgs ha = new HitArgs();
                    ha.DrumType = dt;
                    ha.SignSource = cs;
                    ha.HitType = HitType.Normal;
                    if (cs.drumType != DrumType.FaceAll && cs.drumType != DrumType.SideAll)
                    {
                        cs.DelayTimeMill -= (int)(10 * speed);
                    }

                    if (CommHelper.Abs(cs.DelayTimeMill - atime) <= niceArrage)
                    {
                        ha.HitType = HitType.Nice;
                    }

                    //得分算出来后计算按键是否对应
                    if (!KnockKeyIsCodex(cs, dt))
                    {
                        ha.HitType = HitType.Lost;
                    }

                    OnHit(this, ha);
                    return;
                }
                else if (CommHelper.Abs(cs.DelayTimeMill - atime) <= lostArrage)
                {
                    RemoveAndSendMsg(cs, 1, HitType.Lost);
                    return;
                }
                else
                {
                    return;//没有满足的直接跳出
                }
            }
        }

        #endregion


        /// <summary>
        /// 停止播放并释放相关内容
        /// </summary>
        public void Stop()
        {
            if (CadenceSigns == null) return;

            indexActual = 0;
            lapseTime = 0;
            //
            meMusic.Stop();
            //
            dtWindows.Stop();
            dtWindows.Tick -= new EventHandler(dtWindows_Tick);
            logicThread.Dispose();
            logicThread = null;
            //
            foreach (CadenceSign cs in CadenceSigns)
            {
                LayoutRoot.Children.Remove(cs.currentSign);
            }
            Array.Clear(CadenceSigns, 0, signCount);
            CadenceSigns = null;
            isStart = false;
            isPlaySound = false;
        }

        /// <summary>
        /// 播放解析后的信号
        /// </summary>
        public void Play(MusicInfo mi)
        {
            if (isStart) return;

            if (CadenceSigns == null || CadenceSigns.Length <= 0)
            {
                Load(mi);
            }
            ///Res/Musics/xiaji.wma
            //meMusic.Stop();
            meMusic.AutoPlay = false;
            if (mi.IsDefault)
            {
                meMusic.Source = new Uri(CommHelper.MusicBasePath + mi.Key, UriKind.Relative);
            }
            else
            {
                meMusic.SetSource(CommHelper.GetMusicStream(mi.Key));
            }

            //主线程界面刷新器
            dtWindows.Interval = new TimeSpan(0, 0, 0, 0, 10);
            dtWindows.Tick += new EventHandler(dtWindows_Tick);
            dtWindows.Start();
            //辅助线程
            logicThread = new Timer(dtWindows_Tick_BG, null, 0, 500);

            isStart = true;
        }

        #region 界面刷新线程回调函数

        double leftVal = 0;//当前移动控件的位置

        /// <summary>
        /// 主刷新前台线程
        /// 优化后的回调函数，只移动可见范围内的信号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtWindows_Tick(object sender, EventArgs e)
        {

            Interlocked.Add(ref lapseTime, 10);
#if DEBUG
            tbTimeLine.Text = (lapseTime - arriveEndPointOffset).ToString();
            //            tbTimeLine.Text = ((lapseTime - arriveEndPointOffset) > 0 ? (lapseTime - arriveEndPointOffset).ToString() : "0") + "当前索引:" + indexActual;
#else
            //            tbTimeLine.Text = (lapseTime - arriveEndPointOffset) > 0 ? (lapseTime - arriveEndPointOffset).ToString() : "0";
#endif
            for (int i = indexActual; i < signCount; i++)
            {
                CadenceSign cs = CadenceSigns[i];
                if (cs.DelayTimeMill <= lapseTime)
                {
                    leftVal = Canvas.GetLeft(cs.currentSign);
                    if (cs.state == 0 && leftVal <= -10)//提示未击中
                    {
                        RemoveAndSendMsg(cs, 1, HitType.Lost);
                    }
                    //TranslateTransform ttf = cs.currentSign.RenderTransform as TranslateTransform;
                    //ttf.X = -(Math.Abs(ttf.X) + (1 * speed));

                    Canvas.SetLeft(cs.currentSign, leftVal - speed);
                }
                else
                {
                    return;//没有满足的直接跳出
                }
            }

            //游戏结束判断
            SignEnd();
        }

        int flashIndex = 0;//笑脸播放索引

        //逻辑线程
        private void dtWindows_Tick_BG(object ustate)
        {
            if (!isPlaySound && (lapseTime + firstlongTime) >= arriveEndPointOffset)
            {
                meMusic.Dispatcher.BeginInvoke(delegate { meMusic.Play(); });
                isPlaySound = true;
            }
            for (int i = indexActual; i < signCount; i++)
            {
                CadenceSign cs = CadenceSigns[i];
                if (cs.DelayTimeMill <= lapseTime)
                {
                    if (flashIndex == 0)
                    {
                        DisplayDrumImg(cs, ResourceMgr.SignimgBgCache_smile);
                    }
                    else
                    {
                        DisplayDrumImg(cs, ResourceMgr.SignimgBgCache);
                    }

                }
                else
                {
                    flashIndex++;
                    if (flashIndex > 1)
                    {
                        flashIndex = 0;
                    }
                    return;//没有满足的直接跳出
                }
            }
            flashIndex++;
            if (flashIndex > 1)
            {
                flashIndex = 0;
            }

        }

        private void DisplayDrumImg(CadenceSign cs, BitmapImage[] bi)
        {
            if (isStart && cs.currentSign != null)
            {
                if (cs.drumType == DrumType.FaceAll)
                {
                    cs.currentSign.Dispatcher.BeginInvoke(delegate()
                    {
                        cs.currentSign.Source = bi[1];
                    });
                }
                else if (cs.drumType == DrumType.SideAll)
                {
                    cs.currentSign.Dispatcher.BeginInvoke(delegate()
                    {
                        cs.currentSign.Source = bi[3];
                    });
                }
                else if (cs.drumType == DrumType.LeftFace || cs.drumType == DrumType.RightFace)
                {
                    cs.currentSign.Dispatcher.BeginInvoke(delegate()
                       {
                           cs.currentSign.Source = bi[0];
                       });
                }
                else
                {
                    cs.currentSign.Dispatcher.BeginInvoke(delegate()
                       {
                           cs.currentSign.Source = bi[2];
                       });
                }
            }
        }

        #endregion

        /// <summary>
        /// 信号播放结束
        /// </summary>
        private bool SignEnd()
        {
            if (indexActual >= signCount)
            {
                Stop();
                OnPlayEnd();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 加载一个歌曲数据并解析
        /// </summary>
        /// <param name="sc"></param>
        private void Load(MusicInfo mi)
        {
            speed = mi.Speed;
            //计算到敲击点延迟
            CalcDelayOffset();
            //解析实体类
            string[] datas = mi.MusicData.Split(',');
            CadenceSigns = new CadenceSign[datas.Length];
            string temp = string.Empty;
            int delaytime = 0;
            int ttime = 0;
            int i = 0;
            foreach (string t in datas)
            {
                temp = t.Substring(0, 2);
                ttime = int.Parse(t.Substring(3, t.Length - 3));
                delaytime += ttime;
                CadenceSign cs = new CadenceSign();
                cs.drumType = CommHelper.DrumTypeToKeyCode(temp);
                cs.DelayTimeMill = delaytime;
                //图像
                Image img = new Image();
                if (cs.drumType == DrumType.FaceAll)
                {
                    img.Width = 45;
                    img.Height = 45;
                    img.Source = ResourceMgr.SignimgBgCache[1];
                }
                else if (cs.drumType == DrumType.SideAll)
                {
                    img.Width = 45;
                    img.Height = 45;
                    img.Source = ResourceMgr.SignimgBgCache[3];
                }
                else if (cs.drumType == DrumType.RightFace || cs.drumType == DrumType.LeftFace)
                {
                    img.Width = 35;
                    img.Height = 35;
                    img.Source = ResourceMgr.SignimgBgCache[0];
                }
                else
                {
                    img.Width = 35;
                    img.Height = 35;
                    img.Source = ResourceMgr.SignimgBgCache[2];
                }
                //img.Visibility = System.Windows.Visibility.Collapsed;
                //TranslateTransform ttf = new TranslateTransform();
                //ttf.X = 0; ttf.Y = 0;
                //img.RenderTransform = new TranslateTransform();
                img.RenderTransform = null;
                img.Effect = null;

                img.CacheMode = new BitmapCache();
                if (cs.drumType == DrumType.RightFace || cs.drumType == DrumType.LeftFace
                    || cs.drumType == DrumType.RightSide || cs.drumType == DrumType.LeftSide)
                {
                    Canvas.SetLeft(img, signLeftLocation);
                    Canvas.SetTop(img, 33);
                }
                else
                {
                    Canvas.SetLeft(img, signLeftLocation);
                    Canvas.SetTop(img, 28);
                }

                cs.currentSign = img;
                CadenceSigns[i] = cs;
                i++;
            }
            //添入显示控件

            foreach (CadenceSign cs in CadenceSigns)
            {
                LayoutRoot.Children.Add(cs.currentSign);
            }

            AddLastBecloud();
            firstlongTime = CadenceSigns[0].DelayTimeMill;
            signCount = CadenceSigns.Length;
        }

        /// <summary>
        /// 添加最后的蒙板
        /// </summary>
        private void AddLastBecloud()
        {
            Rectangle becloud = new Rectangle();
            becloud.Fill = new SolidColorBrush(Colors.White);
            becloud.Width = 100; becloud.Height = 100;
            Canvas.SetLeft(becloud, signLeftLocation - 20);
            Canvas.SetTop(becloud, 28);
            LayoutRoot.Children.Add(becloud);
        }


        /// <summary>
        /// 计算达到中心点延迟.以及分数时间等级差
        /// </summary>
        private void CalcDelayOffset()
        {
            double left = Canvas.GetLeft(ellEndPoint);
            double offset = signLeftLocation - left;
            double dt = offset / (1 * speed);
            arriveEndPointOffset = (int)(dt * 10);
            //分数等级差
            normalArrage = (ellEndPoint.Width / 3) * (10);
            niceArrage = (ellEndPoint.Width / 10) * (10);
            lostArrage = (ellEndPoint.Width) * (10);
        }

        /// <summary>
        /// 移除该控件并发送消息
        /// </summary>
        /// <param name="cs"></param>
        /// <param name="ste">0初始化 1 未被敲击 2 被敲中</param>
        /// <param name="ht"></param>
        private void RemoveAndSendMsg(CadenceSign cs, byte ste, HitType ht)
        {
            LayoutRoot.Children.Remove(cs.currentSign);
            indexActual++;
            cs.state = ste;
            HitArgs ha = new HitArgs();
            ha.HitType = ht;
            OnHit(this, ha);
        }

        /// <summary>
        /// 判断按键是否符合游戏规则
        /// </summary>
        private bool KnockKeyIsCodex(CadenceSign cs, DrumType k)
        {
            bool isCodex = false;

            if (k == cs.drumType) isCodex = true;
            if (k == DrumType.LeftSide && cs.drumType == DrumType.RightSide) isCodex = true;
            if (k == DrumType.RightSide && cs.drumType == DrumType.LeftSide) isCodex = true;
            if (k == DrumType.LeftFace && cs.drumType == DrumType.RightFace) isCodex = true;
            if (k == DrumType.RightFace && cs.drumType == DrumType.LeftFace) isCodex = true;
            return isCodex;
        }




    }
}
