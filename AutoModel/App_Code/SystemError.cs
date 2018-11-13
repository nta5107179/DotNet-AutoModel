using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace AutoModel.App_Code
{
    public static class SystemError
    {
        public static void Error(string msg)
        {
            MessageBox.Show(msg, "错误", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
        }

        public static void Information(string msg)
        {
            MessageBox.Show(msg, "信息", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
        }

        public static MessageBoxResult ConfrimOKCancel(string msg)
        {
            MessageBoxResult mbr = MessageBox.Show(msg, "消息", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation, MessageBoxResult.Cancel);
            return mbr;
        }
    }
}
