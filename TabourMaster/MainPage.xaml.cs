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
using System.Windows.Navigation;
using TabourMaster.Compoent;

namespace TabourMaster
{

    /// <summary>
    /// create by liujing
	/// cnblog: http://www.cnblogs.com/NatureSex/
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage(PanelType pt)
        {
            InitializeComponent();

            LoadControl();
            NavigatedTo(null, pt);
            this.LayoutUpdated += new EventHandler(MainPage_LayoutUpdated);
        }

        private void LoadControl()
        {


            StartPanel ToControl1 = new StartPanel();
            ToControl1.Name = "sp";
            Grid.SetRow(ToControl1, 0);
            Canvas.SetZIndex(ToControl1, 9999);
            ToControl1.Visibility = Visibility.Collapsed;

            GamePanel ToControl2 = new GamePanel();
            ToControl2.Name = "gp";
            Grid.SetRow(ToControl2, 0);
            Canvas.SetZIndex(ToControl2, 9999);
            ToControl2.Visibility = Visibility.Collapsed;

            MusicListPanel ToControl3 = new MusicListPanel(false);
            ToControl3.Name = "mlp";
            Grid.SetRow(ToControl3, 0);
            Canvas.SetZIndex(ToControl3, 9999);
            ToControl3.Visibility = Visibility.Collapsed;

            MusicRecordSignPanel ToControl4 = new MusicRecordSignPanel();
            ToControl4.Name = "mrsp";
            Grid.SetRow(ToControl4, 0);
            Canvas.SetZIndex(ToControl4, 9999);
            ToControl4.Visibility = Visibility.Collapsed;

            GameEndPanel ToControl5 = new GameEndPanel();
            ToControl5.Name = "ge";
            Grid.SetRow(ToControl5, 0);
            Canvas.SetZIndex(ToControl5, 9999);
            ToControl5.Visibility = Visibility.Collapsed;

            ResourceMgr.CachePanel.Add(PanelType.StartPanel, ToControl1);
            ResourceMgr.CachePanel.Add(PanelType.GamePanel, ToControl2);
            ResourceMgr.CachePanel.Add(PanelType.MusicListPanel, ToControl3);
            ResourceMgr.CachePanel.Add(PanelType.MusicRecordSignPanel, ToControl4);
            ResourceMgr.CachePanel.Add(PanelType.GameEndPanel, ToControl5);

            LayoutRoot.Children.Add(ToControl1);
            LayoutRoot.Children.Add(ToControl2);
            LayoutRoot.Children.Add(ToControl3);
            LayoutRoot.Children.Add(ToControl4);
            LayoutRoot.Children.Add(ToControl5);
        }

        void MainPage_LayoutUpdated(object sender, EventArgs e)
        {
            if (Application.Current.IsRunningOutOfBrowser)
            {
                Application.Current.MainWindow.Width = 610;
                Application.Current.MainWindow.Height = 410;
            }
        }

        Control FromControl = null;
        Control ToControl = null;

        /// <summary>
        /// 导航到
        /// </summary>
        /// <param name="to"></param>
        public void NavigatedTo(Control from, PanelType to)
        {
            FromControl = from;
            ToControl = ResourceMgr.CachePanel[to];

            //属性
            ResetToPt(ToControl, -1200, 0);
            ((IMyControl)ToControl).ControlVisible();
            ToControl.UpdateLayout();
            CreateSb(ToControl, FromControl);
        }

        /// <summary>
        /// 恢复导航到控件的初始值
        /// </summary>
        /// <param name="ToControl"></param>
        private void ResetToPt(Control ToControl, double left, double opc)
        {
            ToControl.Opacity = opc;
            ToControl.Margin = new Thickness(left, 0, 0, 0);
            ToControl.RenderTransform = new CompositeTransform();
            ToControl.RenderTransformOrigin = new Point(0.5, 0.5);
            ToControl.Visibility = System.Windows.Visibility.Visible;
            ToControl.Focus();
        }

        Storyboard sbSwitch = null;
        DoubleAnimation ufadein = null;
        DoubleAnimation ufadeOut = null;
        DoubleAnimationUsingKeyFrames daukf = null;

        /// <summary>
        /// 生成动画
        /// </summary>
        private void CreateSb(Control uin, Control uout)
        {
            sbSwitch = new Storyboard();
            //if (ufadein == null)
            //{
            ufadein = new DoubleAnimation();
            ufadein.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 500));
            ufadein.To = 600;
            Storyboard.SetTargetProperty(ufadein, new PropertyPath("(UIElement.RenderTransform).(CompositeTransform.TranslateX)"));
            sbSwitch.Children.Add(ufadein);
            //}
            Storyboard.SetTarget(ufadein, uin);
            //透明度渐变
            //if (daukf == null)
            //{
            daukf = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTargetProperty(daukf, new PropertyPath("(UIElement.Opacity)"));
            EasingDoubleKeyFrame edkf = new EasingDoubleKeyFrame();
            edkf.KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0));
            edkf.Value = 0.1;
            EasingDoubleKeyFrame edkf2 = new EasingDoubleKeyFrame();
            edkf2.KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 500));
            edkf2.Value = 1;
            daukf.KeyFrames.Add(edkf);
            daukf.KeyFrames.Add(edkf2);
            sbSwitch.Children.Add(daukf);
            //}
            Storyboard.SetTarget(daukf, uin);

            if (uout != null)
            {
                //if (ufadeOut == null)
                //{
                ufadeOut = new DoubleAnimation();
                ufadeOut.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 500));
                ufadeOut.To = 610;
                Storyboard.SetTargetProperty(ufadeOut, new PropertyPath("(UIElement.RenderTransform).(CompositeTransform.TranslateX)"));
                sbSwitch.Children.Add(ufadeOut);
                //}
                Storyboard.SetTarget(ufadeOut, uout);
            }
            sbSwitch.Completed += new EventHandler(sbSwitch_Completed);
            sbSwitch.Begin();
        }

        void sbSwitch_Completed(object sender, EventArgs e)
        {
            CollapsedFormChild(FromControl);
            sbSwitch.Completed -= new EventHandler(sbSwitch_Completed);
            sbSwitch.Stop();
            ResetToPt(ToControl, 0, 1);
        }

        /// <summary>
        /// 隐藏来源的控件
        /// </summary>
        private void CollapsedFormChild(Control ui)
        {
            if (ui != null)
            {
                ui.Visibility = System.Windows.Visibility.Collapsed;
                ((IMyControl)ui).ControlCollapsed();
            }
        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

    }
}
