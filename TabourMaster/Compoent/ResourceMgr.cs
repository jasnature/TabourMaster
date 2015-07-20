using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Collections.Generic;

namespace TabourMaster.Compoent
{
    /// <summary>
    /// 静态音乐和图片资源管理
    /// </summary>
    public static class ResourceMgr
    {
        /// <summary>
        /// 静态初始化
        /// </summary>
        static ResourceMgr()
        {
            //信号图像
            SignimgBgCache[0] = new BitmapImage(new Uri("/Res/Imgs/drums/drum1.png", UriKind.Relative));
            SignimgBgCache[1] = new BitmapImage(new Uri("/Res/Imgs/drums/drum3.png", UriKind.Relative));
            SignimgBgCache[2] = new BitmapImage(new Uri("/Res/Imgs/drums/drum2.png", UriKind.Relative));
            SignimgBgCache[3] = new BitmapImage(new Uri("/Res/Imgs/drums/drum4.png", UriKind.Relative));
            SignimgBgCache_smile[0] = new BitmapImage(new Uri("/Res/Imgs/drums/rdrum1.png", UriKind.Relative));
            SignimgBgCache_smile[1] = new BitmapImage(new Uri("/Res/Imgs/drums/rdrum3.png", UriKind.Relative));
            SignimgBgCache_smile[2] = new BitmapImage(new Uri("/Res/Imgs/drums/rdrum2.png", UriKind.Relative));
            SignimgBgCache_smile[3] = new BitmapImage(new Uri("/Res/Imgs/drums/rdrum4.png", UriKind.Relative));
            //魂图片
            FetchimgCache[0] = new BitmapImage(new Uri("/Res/Imgs/fetch1.png", UriKind.Relative));
            FetchimgCache[1] = new BitmapImage(new Uri("/Res/Imgs/fetch2.png", UriKind.Relative));
            FetchimgCache[2] = new BitmapImage(new Uri("/Res/Imgs/fetch3.png", UriKind.Relative));
            FetchimgCache[3] = new BitmapImage(new Uri("/Res/Imgs/fetch4.png", UriKind.Relative));
            FetchimgCache[4] = new BitmapImage(new Uri("/Res/Imgs/fetch5.png", UriKind.Relative));
            FetchimgCache[5] = new BitmapImage(new Uri("/Res/Imgs/fetch6.png", UriKind.Relative));
            FetchimgCache[6] = new BitmapImage(new Uri("/Res/Imgs/fetch7.png", UriKind.Relative));
            FetchimgCache[7] = new BitmapImage(new Uri("/Res/Imgs/fetch8.png", UriKind.Relative));
            FetchimgCache[8] = new BitmapImage(new Uri("/Res/Imgs/fetch9.png", UriKind.Relative));
        }

        /// <summary>
        /// 控件缓存
        /// </summary>
        public static Dictionary<PanelType, Control> CachePanel = new Dictionary<PanelType, Control>();

        /// <summary>
        /// 缓存鼓图像（闭嘴鼓）
        /// 0.黄小 1.黄大 2.蓝小 3.蓝大
        /// </summary>
        public static BitmapImage[] SignimgBgCache = new BitmapImage[4];

        /// <summary>
        /// 缓存鼓图像（开口鼓）
        /// 0.黄小 1.黄大 2.蓝小 3.蓝大
        /// </summary>
        public static BitmapImage[] SignimgBgCache_smile = new BitmapImage[4];

        /// <summary>
        /// 魂动画
        /// </summary>
        public static BitmapImage[] FetchimgCache = new BitmapImage[9];


    }
}
