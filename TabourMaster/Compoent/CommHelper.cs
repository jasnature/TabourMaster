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
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using System.IO;

namespace TabourMaster.Compoent
{
    /// <summary>
    /// 工具类
    /// </summary>
    public static class CommHelper
    {
        /// <summary>
        /// 当前选择的歌曲信息
        /// </summary>
        public static MusicInfo CurrnetSelectMusic;

        public static GameResultInfo LastGameResultInfo = new GameResultInfo();

        /// <summary>
        /// 本地录制的音乐列表,通过文件读取
        /// </summary>
        public static List<MusicInfo> LocalMusicInfos = null;

        public static readonly string MusicBasePath = "/Res/Mus/";

        /// <summary>
        /// 转换鼓面字符串到按键映射
        /// </summary>
        /// <param name="signStr"></param>
        /// <returns></returns>
        public static DrumType DrumTypeToKeyCode(string signStr)
        {
            //关键帧分别形容鼓面的
            //LS 左侧敲击
            //LF 左正敲击
            //RF 右正敲击
            //RS 右侧敲击
            //SA 左右侧一起
            //FA 左右正一起
            //-->
            switch (signStr)
            {
                case "LS":
                    return (DrumType)Key.D;
                case "LF":
                    return (DrumType)Key.F;
                case "RF":
                    return (DrumType)Key.J;
                case "RS":
                    return (DrumType)Key.K;
                case "FA":
                    return DrumType.FaceAll;
                case "SA":
                    return DrumType.SideAll;
            }
            return 0;
        }

        /// <summary>
        /// 转换鼓面字符串到按键映射
        /// </summary>
        /// <param name="signStr"></param>
        /// <returns></returns>
        public static DrumType KeyCodeToDrumType(int keyCode)
        {
            return (DrumType)keyCode;
        }

        /// <summary>
        /// 求绝对值
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double Abs(double d)
        {
            if (d < 0) return d * -1;
            return d;
        }


        /// <summary>
        /// 检验按键是否合法
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public static bool CheckKeyLicity(Key k)
        {
            if (k == Key.D || k == Key.F || k == Key.J || k == Key.K || k == Key.L || k == Key.S) return true;
            return false;
        }


        /// <summary>
        ///  调用父类的导航
        /// </summary>
        /// <param name="owen">自己的类型</param>
        /// <param name="pt">导航到的类型</param>
        public static void BaseNavigate(Control own, PanelType pt)
        {
            own.Dispatcher.BeginInvoke(() =>
            {
                MainPage mp = ((Grid)own.Parent).Parent as MainPage;
                if (mp != null)
                {
                    mp.NavigatedTo(own, pt);
                }
            });
        }

        #region 本地存储操作

        static long _50MB = 50 * 1024 * 1024;

        static long _10MB = 10 * 1024 * 1024;

        /// <summary>
        /// 应用程序域本地存储
        /// </summary>
        public static IsolatedStorageFile currentISF = IsolatedStorageFile.GetUserStoreForApplication();

        /// <summary>
        /// 检查容量
        /// </summary>
        /// <returns></returns>
        public static void CheckDiskCapcity()
        {
            if (currentISF.UsedSize + _10MB > currentISF.AvailableFreeSpace || currentISF.AvailableFreeSpace < _10MB)
            {
                if (currentISF.IncreaseQuotaTo(_50MB))
                {
                    MessageBox.Show("分配额度成功!");
                }
            }
        }

        /// <summary>
        /// 获取本地存储的信号文件列表
        /// </summary>
        /// <returns></returns>
        public static List<MusicInfo> GetLocalMusicList()
        {
            if (!currentISF.DirectoryExists("Musics"))
            {
                currentISF.CreateDirectory("Musics");
            }

            string[] paths = currentISF.GetFileNames("Musics/*.sgn");
            LocalMusicInfos = new List<MusicInfo>(paths.Length);

            IsolatedStorageFileStream isfs = null;
            StreamReader sr = null;
            MusicInfo mi = null;
            foreach (string p in paths)
            {
                using (isfs = new IsolatedStorageFileStream("Musics/" + p, FileMode.Open, FileAccess.Read, currentISF))
                {
                    using (sr = new StreamReader(isfs))
                    {
                        try
                        {
                            mi = new MusicInfo();
                            mi.MusicName = sr.ReadLine();//name
                            mi.IsDefault = sr.ReadLine().Equals("0") ? false : true;//IsDefault
                            mi.Key = sr.ReadLine();//key
                            mi.MusicLengthMill = int.Parse(sr.ReadLine());//musicLengthMill
                            mi.Speed = int.Parse(sr.ReadLine());//speed
                            mi.MusicData = sr.ReadLine();//data
                            LocalMusicInfos.Add(mi);
                        }
                        catch (Exception)
                        {
                            sr.Close();
                            isfs.Close();
                            currentISF.DeleteFile("Musics/" + p);
                        }
                        sr.Close();
                    }
                    isfs.Close();
                }
            }
            return LocalMusicInfos;
        }

        /// <summary>
        /// 删除本地存储的sgn文件和音乐文件
        /// </summary>
        /// <param name="mi"></param>
        /// <returns></returns>
        public static bool DeleteLocalSignAndMusic(MusicInfo mi)
        {
            currentISF.DeleteFile("Musics/" + mi.Key + "_Sign.sgn");
            currentISF.DeleteFile("Musics/" + mi.Key);
            return true;
        }

        /// <summary>
        /// 获取本地存储音乐文件流
        /// </summary>
        /// <param name="musicName"></param>
        /// <returns></returns>
        public static Stream GetMusicStream(string musicName)
        {
            return currentISF.OpenFile("Musics/" + musicName, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        #endregion


    }
}
