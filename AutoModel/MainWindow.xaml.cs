using AutoModel.App_Code;
using AutoModel.App_Code.SqlControl;
using AutoModel.Emun;
using AutoModel.Models;
using AutoModel.Models.TreeNode;
using AutoModel.Options;
using CoreClass;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoModel
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        OperateStringClass m_opstring = new OperateStringClass();
        OperateMemoryClass m_opmemory = new OperateMemoryClass();

        List<LinkConfigModel> m_linkconfiglist = new List<LinkConfigModel>();
        ObservableCollection<TreeNodeMain> m_treenodemainlist = new ObservableCollection<TreeNodeMain>();
        ObservableCollection<TreeNodeServer> m_treenodeserverlist = new ObservableCollection<TreeNodeServer>();

        public MainWindow()
        {
            InitializeComponent();
            SetTreeNodeMain();
        }

        #region 树-顶级
        void SetTreeNodeMain()
        {
            m_linkconfiglist = ConfigJson.LoadLink();
            tree_server.ItemsSource = m_treenodemainlist;
            m_treenodemainlist.Add(new TreeNodeMain()
            {
                text = "服务器列表",
                childer = m_treenodeserverlist
            });
            SetTreeNodeServer();
        }
        #endregion

        #region 树-服务器实例列表
        void SetTreeNodeServer()
        {
            for (int i = 0; i < m_linkconfiglist.Count; i++)
            {
                AddTreeNodeServer(m_linkconfiglist[i]);
            }
        }
        void AddTreeNodeServer(LinkConfigModel linkconfig)
        {
            ISqlControl sqlcontrol = null;
            if (linkconfig.type == SqlTypeEmun.SqlServer)
            {
                sqlcontrol = new SqlServerControl();
            }
            m_treenodeserverlist.Add(new TreeNodeServer()
            {
                id = linkconfig.id,
                text = linkconfig.name,
                connectionstring = linkconfig.connectionstring,
                database = linkconfig.database,
                type = linkconfig.type,
                childer = new ObservableCollection<TreeNodeDatabase>(),
                sqlcontrol = sqlcontrol
            });
        }
        void DelTreeNodeServer(int id)
        {
            for (int i = 0; i < m_treenodeserverlist.Count; i++)
            {
                if (m_treenodeserverlist[i].id == id)
                    m_treenodeserverlist.RemoveAt(i);
            }
        }
        void SetTreeNodeServerMenu(TreeView tree, TreeViewItem item)
        {
            TreeNodeServer treenodeserver = (TreeNodeServer)tree.SelectedItem;
            ContextMenu cm = new ContextMenu();

            MenuItem menu_link = new MenuItem();
            menu_link.Header = "连接";
            menu_link.Click += delegate (object obj, RoutedEventArgs earg)
            {
                SetTreeNodeDatabase(treenodeserver);
            };
            cm.Items.Add(menu_link);
            if (treenodeserver.childer.Count > 0)
                menu_link.IsEnabled = false;

            MenuItem menu_unlink = new MenuItem();
            menu_unlink.Header = "断开";
            menu_unlink.Click += delegate (object obj, RoutedEventArgs earg)
            {
                treenodeserver.childer.Clear();
            };
            cm.Items.Add(menu_unlink);

            cm.Items.Add(new Separator());

            MenuItem menu_del = new MenuItem();
            menu_del.Header = "删除";
            menu_del.Click += delegate (object obj, RoutedEventArgs earg)
            {
                MessageBoxResult mbr = SystemError.ConfrimOKCancel("确定删除此服务器实例吗？");
                if (mbr == MessageBoxResult.OK)
                {
                    if (ConfigJson.DelLink(treenodeserver.id))
                    {
                        DelTreeNodeServer(treenodeserver.id);
                        SystemError.Information("删除成功");
                    }
                }
            };
            cm.Items.Add(menu_del);

            item.ContextMenu = cm;
        }
        #endregion

        #region 树-数据库列表
        void SetTreeNodeDatabase(TreeNodeServer treenodeserver)
        {
            ISqlControl sqlcontrol = treenodeserver.sqlcontrol;
            DataTable dt_database = sqlcontrol.GetDatabaseList(treenodeserver.connectionstring, treenodeserver.database);
            for (int i = 0; i < dt_database.Rows.Count; i++)
            {
                TreeNodeDatabase treenodedatabase = new TreeNodeDatabase()
                {
                    text = dt_database.Rows[i]["name"].ToString(),
                    connectionstring = treenodeserver.connectionstring,
                    database = dt_database.Rows[i]["name"].ToString(),
                    childer = new ObservableCollection<TreeNodeTable>(),
                    sqlcontrol = sqlcontrol
                };
                treenodeserver.childer.Add(treenodedatabase);

                SetTreeNodeTable(treenodedatabase);
            }
        }
        void SetTreeNodeDatabaseMenu(TreeView tree, TreeViewItem item)
        {
            TreeNodeDatabase treenodedatabase = (TreeNodeDatabase)tree.SelectedItem;
            ISqlControl sqlcontrol = treenodedatabase.sqlcontrol;
            ContextMenu cm = new ContextMenu();

            MenuItem menu_generate = new MenuItem();
            menu_generate.Header = "生成Models";
            menu_generate.Click += delegate (object obj, RoutedEventArgs earg)
            {
                DataTable tables = sqlcontrol.GetTableList(treenodedatabase.connectionstring, treenodedatabase.database);
                text_content.Text = sqlcontrol.GenerateModels(tables);
                text_content2.Text = sqlcontrol.GenerateModels2(tables);
            };
            cm.Items.Add(menu_generate);

            MenuItem menu_generategetlist = new MenuItem();
            menu_generategetlist.Header = "生成GetModelList";
            menu_generategetlist.Click += delegate (object obj, RoutedEventArgs earg)
            {
                DataTable tables = sqlcontrol.GetTableList(treenodedatabase.connectionstring, treenodedatabase.database);
                DataTable columns = sqlcontrol.GetColumnList(treenodedatabase.connectionstring, treenodedatabase.database);
                text_content.Text = sqlcontrol.GenerateGetModelList(tables, columns);
                text_content2.Text = text_content.Text;
            };
            cm.Items.Add(menu_generategetlist);

            cm.Items.Add(new Separator());

            MenuItem menu_generatefile = new MenuItem();
            menu_generatefile.Header = "生成Models类库";
            menu_generatefile.Click += delegate (object obj, RoutedEventArgs earg)
            {
                GenerateFile dialog = new GenerateFile();
                dialog.Tag = treenodedatabase;
                dialog.Owner = this;
                dialog.ShowDialog();
            };
            cm.Items.Add(menu_generatefile);

            item.ContextMenu = cm;
        }
        #endregion

        #region 树-表列表
        void SetTreeNodeTable(TreeNodeDatabase treenodedatabase)
        {
            ISqlControl sqlcontrol = treenodedatabase.sqlcontrol;
            DataTable dt_table = sqlcontrol.GetTableList(treenodedatabase.connectionstring, treenodedatabase.database);
            for (int i = 0; i < dt_table.Rows.Count; i++)
            {
                treenodedatabase.childer.Add(new TreeNodeTable()
                {
                    text = dt_table.Rows[i]["name"].ToString(),
                    connectionstring = treenodedatabase.connectionstring,
                    database = treenodedatabase.database,
                    table = dt_table.Rows[i]["name"].ToString(),
                    sqlcontrol = sqlcontrol
                });
            }
        }
        void SetTreeNodeTableMenu(TreeView tree, TreeViewItem item)
        {
            TreeNodeTable treenodetable = (TreeNodeTable)tree.SelectedItem;
            ISqlControl sqlcontrol = treenodetable.sqlcontrol;
            ContextMenu cm = new ContextMenu();

            MenuItem menu_generate = new MenuItem();
            menu_generate.Header = "生成Model";
            menu_generate.Click += delegate (object obj, RoutedEventArgs earg)
            {
                DataTable columns = sqlcontrol.GetColumnList(treenodetable.connectionstring, treenodetable.database, treenodetable.table);
                text_content.Text = sqlcontrol.GenerateModel(columns, treenodetable.table);
                text_content2.Text = sqlcontrol.GenerateModel2(columns, treenodetable.table);
            };
            cm.Items.Add(menu_generate);

            item.ContextMenu = cm;
        }
        #endregion

        private void menu_exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        private void menu_newlink_Click(object sender, RoutedEventArgs e)
        {
            CreateLink form = new CreateLink();
            form.Owner = this;
            form.ShowDialog();
            if ((bool)form.DialogResult)
            {
                AddTreeNodeServer((LinkConfigModel)form.Tag);
            }
        }
        private void tree_server_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            DependencyObject source = e.OriginalSource as DependencyObject;
            while (source != null && source.GetType() != typeof(TreeViewItem))
                source = VisualTreeHelper.GetParent(source);
            TreeViewItem item = source as TreeViewItem;
            if (item != null)
            {
                item.Focus();
                e.Handled = true;

                TreeView tree = ((TreeView)sender);
                if (tree.SelectedItem.GetType() == typeof(TreeNodeServer))
                {
                    SetTreeNodeServerMenu(tree, item);
                }
                else if (tree.SelectedItem.GetType() == typeof(TreeNodeDatabase))
                {
                    SetTreeNodeDatabaseMenu(tree, item);
                }
                else if (tree.SelectedItem.GetType() == typeof(TreeNodeTable))
                {
                    SetTreeNodeTableMenu(tree, item);
                }
            }
        }

        private void tree_server_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            DependencyObject source = e.OriginalSource as DependencyObject;
            while (source != null && source.GetType() != typeof(TreeViewItem))
                source = VisualTreeHelper.GetParent(source);
            TreeViewItem item = source as TreeViewItem;
            if(item!=null && item.ContextMenu == null)
            {
                e.Handled = true;
            }
        }

        private void menu_about_Click(object sender, RoutedEventArgs e)
        {
            SystemError.Information("我是可爱的小熊猫...");
        }

        private void menu_namespace_Click(object sender, RoutedEventArgs e)
        {
            NamespaceConfig dialog = new NamespaceConfig();
            dialog.Owner = this;
            dialog.ShowDialog();
        }
    }
}
