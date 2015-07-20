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


    /// <summary>
    /// create by liujing
	/// cnblog: http://www.cnblogs.com/NatureSex/
    /// </summary>
namespace TabourMaster
{
    public partial class GameEndPanel : UserControl, IMyControl
    {
        public GameEndPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 设置结果信息
        /// </summary>
        /// <param name="ScoreSum"></param>
        /// <param name="MaxDoubleHits"></param>
        /// <param name="HitPercent"></param>
        /// <param name="CoolHits"></param>
        /// <param name="BadHits"></param>
        /// <param name="NormalHits"></param>
        /// <param name="TotalEvaluate"></param>
        public void DisplayGameEndInfo()
        {
            this.Dispatcher.BeginInvoke(delegate()
            {
                this.tbtop1.Text = (CommHelper.LastGameResultInfo.SignCount * 500).ToString().PadLeft(7, '0');
                this.tbtop2.Text = (CommHelper.LastGameResultInfo.SignCount * 300).ToString().PadLeft(7, '0');
                this.tbtop3.Text = (CommHelper.LastGameResultInfo.SignCount * 100).ToString().PadLeft(7, '0');

                this.lblScoreSum.Text = CommHelper.LastGameResultInfo.ScoreSum.ToString().PadLeft(7, '0');
                this.lblMaxDoubleHits.Text = CommHelper.LastGameResultInfo.MaxDoubleHits.ToString();
                this.lblHitPercent.Text = CommHelper.LastGameResultInfo.HitPercent.ToString();
                this.lblCoolHits.Text = CommHelper.LastGameResultInfo.CoolHits.ToString();
                this.lblBadHits.Text = CommHelper.LastGameResultInfo.BadHits.ToString();
                this.lblNormalHits.Text = CommHelper.LastGameResultInfo.NormalHits.ToString();
                this.lblTotalEvaluate.Text = CommHelper.LastGameResultInfo.TotalEvaluate;
                this.tbMusicName.Text = CommHelper.LastGameResultInfo.MusicName;
            });
        }

        /// <summary>
        /// 重置信息
        /// </summary>
        public void ReSetGameEndInfo()
        {
            this.lblScoreSum.Text = "0";
            this.lblMaxDoubleHits.Text = "0";
            this.lblHitPercent.Text = "0";
            this.lblCoolHits.Text = "0";
            this.lblBadHits.Text = "0";
            this.lblNormalHits.Text = "0";
            this.lblTotalEvaluate.Text = "差";

        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            CommHelper.BaseNavigate(this, PanelType.MusicListPanel);
        }

        private void btnBackMenu_Click(object sender, RoutedEventArgs e)
        {
            CommHelper.BaseNavigate(this, PanelType.StartPanel);
        }


        #region IMyControl 成员

        public void ControlVisible()
        {

        }

        public void ControlCollapsed()
        {
            ReSetGameEndInfo();
        }

        #endregion
    }
}
