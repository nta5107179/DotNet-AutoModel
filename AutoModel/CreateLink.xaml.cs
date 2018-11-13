using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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
using AutoModel.App_Code;
using CoreClass;
using Newtonsoft.Json.Linq;
using System.IO;
using AutoModel.Emun;
using AutoModel.Models;
using AutoModel.App_Code.SqlControl;

namespace AutoModel
{
    /// <summary>
    /// CreateLink.xaml 的交互逻辑
    /// </summary>
    public partial class CreateLink : Window
    {
        ISqlControl m_sqlcontrol;
        LinkConfigModel m_linkconfig = new LinkConfigModel();

        public CreateLink()
        {
            InitializeComponent();
            init();
        }
        
        /// <summary>
        /// 初始化
        /// </summary>
        void init()
        {
            ObservableCollection<ServerModel> list = new ObservableCollection<ServerModel>()
            {
                new ServerModel(){ text = "Sql Server", value = SqlTypeEmun.SqlServer }
            };
            cb_sqltype.ItemsSource = list;
        }

        /// <summary>
        /// 数据库操作
        /// </summary>
        /// <returns></returns>
        bool ControlSql(bool isTest)
        {
            bool b = false;
            SqlTypeEmun ste = (SqlTypeEmun)cb_sqltype.SelectedValue;
            ObservableCollection<DatabaseModel> list = null;
            string connectionstring = null;
            if (ste == SqlTypeEmun.SqlServer)
            {
                m_sqlcontrol = new SqlServerControl();
                connectionstring = string.Format("server={0};uid={1};pwd={2}",
                    text_server.Text,
                    text_uid.Text,
                    text_pwd.Password);
                list = m_sqlcontrol.TestLink(connectionstring);
            }
            if (list != null && connectionstring != null)
            {
                if (isTest)
                    cb_database.ItemsSource = list;
                else
                {
                    m_linkconfig.name = text_server.Text;
                    m_linkconfig.connectionstring = connectionstring;
                    m_linkconfig.database = (string)cb_database.SelectedValue;
                    m_linkconfig.type = ste;
                }
                b = true;
            }
            return b;
        }

        private void cb_sqltype_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void but_cancol_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void but_test_Click(object sender, RoutedEventArgs e)
        {
            bool b = ControlSql(true);
            if (b)
            {
                cb_database.IsEnabled = true;
            }
        }

        private void but_ok_Click(object sender, RoutedEventArgs e)
        {
            bool b = ControlSql(false);
            if (b && ConfigJson.AddLink(ref m_linkconfig))
            {
                Tag = m_linkconfig;
                SystemError.Information("数据库创建成功");
                DialogResult = true;
            }
        }
    }
}
