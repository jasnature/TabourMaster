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

namespace TabourMaster.UControl
{
    /// <summary>
    /// 弹出框
    /// </summary>
    public partial class UChildMessage : ChildWindow
    {
        public UChildMessage()
        {
            InitializeComponent();
        }

        public void Show(string message)
        {
            Show(null, message, MessageBoxButton.OK);
        }

        public void Show(string title, string message)
        {
            Show(title, message, MessageBoxButton.OK);
        }

        public void Show(string title, string message, MessageBoxButton mbb)
        {
            if (title != null) this.Title = title;
            if (message != null) this.tbMessage.Text = message;
            if (mbb == MessageBoxButton.OK)
            {
                CancelButton.Visibility = System.Windows.Visibility.Collapsed;
                this.DialogResult = false;
            }
            Show();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

