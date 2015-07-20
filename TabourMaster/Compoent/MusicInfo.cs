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

namespace TabourMaster.Compoent
{
    /// <summary>
    /// 节奏数据实体
    /// </summary>
    public class MusicInfo
    {
        private string musicName;

        public string MusicName
        {
            get { return musicName; }
            set { musicName = value; }
        }

        private bool isDefault = true;

        public bool IsDefault
        {
            get { return isDefault; }
            set { isDefault = value; }
        }

        private string key;

        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        private int musicLengthMill;

        public int MusicLengthMill
        {
            get { return musicLengthMill; }
            set { musicLengthMill = value; }
        }

        private int speed;

        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        private string musicData;

        public string MusicData
        {
            get { return musicData; }
            set { musicData = value; }
        }

    }
}
