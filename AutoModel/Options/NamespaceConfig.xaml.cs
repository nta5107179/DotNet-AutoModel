using AutoModel.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AutoModel.Options
{
    /// <summary>
    /// NameSpaceConfig.xaml 的交互逻辑
    /// </summary>
    public partial class NamespaceConfig : Window
    {
        public NamespaceConfig()
        {
            InitializeComponent();
            text_namespace.Text = ConfigJson.GetNamespace();
        }

        private void but_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void but_ok_Click(object sender, RoutedEventArgs e)
        {
            ConfigJson.SetNamespace(text_namespace.Text);
            SystemError.Information("配置成功");
            this.DialogResult = true;
        }
    }
}
