using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TabourMaster.Compoent;
using System.ComponentModel;

namespace TabourMaster.UControl
{
    /// <summary>
    /// 鼓面
    /// </summary>
    public partial class Drumhead : UserControl
    {
        public Drumhead()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Drumhead_Loaded);
        }

        void Drumhead_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBaseInfo();
        }

        private bool isEnableSound = true;

        private float soundSize = 0.5f;

        /// <summary>
        /// 敲击鼓声音大小
        /// </summary>
        public float SoundSize
        {
            get { return soundSize; }
            set { soundSize = value; }
        }

        /// <summary>
        /// 是否启用音乐播放
        /// </summary>
        public bool IsEnableSound
        {
            get { return isEnableSound; }
            set { isEnableSound = value; }
        }

        #region 公共事件

        /// <summary>
        /// 敲击鼓后激发的事件
        /// </summary>
        public event Action<Drumhead, DrumKnockArgs> Knocked;

        #endregion

        private static SolidColorBrush defaultKnockSideBrush = new SolidColorBrush(Colors.Green);
        private static SolidColorBrush defaultKnockFaceBrush = new SolidColorBrush(Colors.Orange);

        //太鼓声音
        private static MediaElement[] medongs = new MediaElement[3];
        private static MediaElement[] mekas = new MediaElement[3];

        #region 公共属性

        /// <summary>
        /// 设置或获取敲击鼓侧显示的颜色
        /// </summary>
        public SolidColorBrush DefaultBorderBrush
        {
            get { return defaultKnockSideBrush; }
            set { defaultKnockSideBrush = value; }
        }

        /// <summary>
        /// 设置或获取敲击鼓正显示的颜色
        /// </summary>
        public SolidColorBrush DefaultFaceBrush
        {
            get { return defaultKnockFaceBrush; }
            set { defaultKnockFaceBrush = value; }
        }

        /// <summary>
        /// 左侧面
        /// </summary>
        [Description("获取左侧面鼓的对象引用")]
        public Path LeftSide
        {
            get { return leftSide; }
        }
        /// <summary>
        /// 右侧面
        /// </summary>
        [Description("获取右侧面鼓的对象引用")]
        public Path RightSide
        {
            get { return rightSide; }
        }
        /// <summary>
        /// 左正面
        /// </summary>
        [Description("获取左正面鼓的对象引用")]
        public Path LeftFace
        {
            get { return leftDrum; }
        }
        /// <summary>
        /// 右正面
        /// </summary>
        [Description("获取右正面鼓的对象引用")]
        public Path RightFace
        {
            get { return rightDrum; }
        }
        #endregion

        #region 私有方法

        //鼓初始颜色
        private Brush saveSideInnerBrush;
        private Brush saveFaceInnerBrush;
        private Brush saveSideStrokeBrush;
        private Brush saveFaceStrokeBrush;

        //声音索引
        int dong_index = 0;
        int ka_index = 0;

        /// <summary>
        /// 播放声音(1 dong - 2 ka)
        /// </summary>
        /// <param name="me"></param>
        /// <param name="type">1 dong 2 ka</param>
        private void SoundPlay(MediaElement[] me, byte type)
        {
            if (IsEnableSound)
            {
                if (type == 1)
                {
                    me[dong_index].Stop();
                    me[dong_index].Play();
                    dong_index++;
                }
                else
                {
                    me[ka_index].Stop();
                    me[ka_index].Play();
                    ka_index++;
                }
                if (dong_index >= 3) dong_index = 0;
                if (ka_index >= 3) ka_index = 0;
            }
        }

        /// <summary>
        /// 加载一些初始的属性
        /// </summary>
        private void LoadBaseInfo()
        {
            //颜色
            ComebackSet();
            //添加强力的声音
            if (IsEnableSound)
            {
                for (int i = 0; i < medongs.Length; i++)
                {
                    medongs[i]= new MediaElement();
                    mekas[i] = new MediaElement();
                }

                medongs[0].Source = new Uri("/Res/Sound/dong.wma", UriKind.Relative);
                medongs[1].Source = new Uri("/Res/Sound/dong.wma", UriKind.Relative);
                medongs[2].Source = new Uri("/Res/Sound/dong.wma", UriKind.Relative);
                mekas[0].Source = new Uri("/Res/Sound/ka.wma", UriKind.Relative);
                mekas[1].Source = new Uri("/Res/Sound/ka.wma", UriKind.Relative);
                mekas[2].Source = new Uri("/Res/Sound/ka.wma", UriKind.Relative);


                medongs[0].Volume = medongs[1].Volume = medongs[2].Volume = soundSize;
                mekas[0].Volume = mekas[1].Volume = mekas[2].Volume = soundSize;
                medongs[0].AutoPlay = medongs[1].AutoPlay = medongs[2].AutoPlay = false;
                mekas[0].AutoPlay = mekas[1].AutoPlay = mekas[2].AutoPlay = false;

                for (int i = 0; i < medongs.Length; i++)
                {
                    LayoutRoot.Children.Add(medongs[i]);
                    LayoutRoot.Children.Add(mekas[i]);
                }

            }
        }


        /// <summary>
        /// 播放动画
        /// </summary>
        private void PlayLeftSide()
        {
            SoundPlay(mekas, 2);
            leftSide.Fill = defaultKnockSideBrush;
            leftSide.StrokeThickness = 2;
        }
        private void RelaseLeftSide()
        {
            leftSide.Fill = saveSideInnerBrush;
            leftSide.StrokeThickness = 1;
        }
        /// <summary>
        /// 播放动画
        /// </summary>
        private void PlayRightSide()
        {
            SoundPlay(mekas, 2);
            rightSide.Fill = defaultKnockSideBrush;
            rightSide.StrokeThickness = 2;
        }
        /// <summary>
        /// 播放动画
        /// </summary>
        private void RelaseRightSide()
        {
            rightSide.Fill = saveSideInnerBrush;
            rightSide.StrokeThickness = 1;
        }
        /// <summary>
        /// 播放动画
        /// </summary>
        private void PlayLeftFace()
        {
            SoundPlay(medongs, 1);
            leftDrum.Fill = defaultKnockFaceBrush;
            leftDrum.StrokeThickness = 2;
        }
        private void RelaseLeftFace()
        {
            leftDrum.Fill = saveFaceInnerBrush;
            leftDrum.StrokeThickness = 1;
        }
        /// <summary>
        /// 播放动画
        /// </summary>
        private void PlayRightFace()
        {
            SoundPlay(medongs, 1);
            rightDrum.Fill = defaultKnockFaceBrush;
            rightDrum.StrokeThickness = 2;
        }
        private void RelaseRightFace()
        {
            rightDrum.Fill = saveFaceInnerBrush;
            rightDrum.StrokeThickness = 1;
        }
        #endregion

        #region 公共方法


        /// <summary>
        /// 敲击鼓
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="iterate"></param>
        public void KnockPlay(DrumType dt)
        {
            
            switch (dt)
            {
                case DrumType.LeftFace:
                    PlayLeftFace();
                    break;
                case DrumType.RightFace:
                    PlayRightFace();
                    break;
                case DrumType.LeftSide:
                    PlayLeftSide();
                    break;
                case DrumType.RightSide:
                    PlayRightSide();
                    break;
                case DrumType.FaceAll:
                    PlayLeftFace();
                    PlayRightFace();
                    break;
                case DrumType.SideAll:
                    PlayLeftSide();
                    PlayRightSide();
                    break;
            }

        }
        /// <summary>
        /// 抬起敲下的鼓
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="iterate"></param>
        public void RelaseKnockPlay(DrumType dt)
        {
            switch (dt)
            {
                case DrumType.LeftFace:
                    RelaseLeftFace();
                    break;
                case DrumType.RightFace:
                    RelaseRightFace();
                    break;
                case DrumType.LeftSide:
                    RelaseLeftSide();
                    break;
                case DrumType.RightSide:
                    RelaseRightSide();
                    break;
                case DrumType.FaceAll:
                    RelaseLeftFace();
                    RelaseRightFace();
                    break;
                case DrumType.SideAll:
                    RelaseLeftSide();
                    RelaseRightSide();
                    break;
            }
            if (Knocked != null)
            {
                DrumKnockArgs dka = new DrumKnockArgs();
                dka.DrumType = dt;
                Knocked(this, dka);
            }
        }
        #endregion

        /// <summary>
        /// 恢复鼓默认设置
        /// </summary>
        public void ComebackSet()
        {
            saveSideInnerBrush = leftSide.Fill;
            saveFaceInnerBrush = leftDrum.Fill;
            saveSideStrokeBrush = leftSide.Stroke;
            saveFaceStrokeBrush = leftDrum.Stroke;
        }

    }
}
