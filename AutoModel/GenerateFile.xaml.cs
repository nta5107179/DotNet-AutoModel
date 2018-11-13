using AutoModel.App_Code;
using AutoModel.Models.TreeNode;
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
using System.IO;
using System.Data;

namespace AutoModel
{
    /// <summary>
    /// GenerateFile.xaml 的交互逻辑
    /// </summary>
    public partial class GenerateFile : Window
    {
        public GenerateFile()
        {
            InitializeComponent();
        }

        private void but_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void but_ok_Click(object sender, RoutedEventArgs e)
        {
            TreeNodeDatabase treenodedatabase = (TreeNodeDatabase)this.Tag;
            ISqlControl sqlcontrol = treenodedatabase.sqlcontrol;

            DataTable tables = sqlcontrol.GetTableList(treenodedatabase.connectionstring, treenodedatabase.database);
            DataTable columns = sqlcontrol.GetColumnList(treenodedatabase.connectionstring, treenodedatabase.database);
            List<Dictionary<string, object>> tablecolumnslist = new List<Dictionary<string, object>>();
            for(int i = 0; i < tables.Rows.Count; i++)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                DataTable tablecolumns = columns.Clone();
                DataRow[] drarr = columns.Select().Where(a => (string)a["tablename"] == (string)tables.Rows[i]["name"]).ToArray();
                for(int j = 0; j < drarr.Length; j++)
                {
                    tablecolumns.Rows.Add(drarr[j].ItemArray);
                }
                dic["tablename"] = (string)tables.Rows[i]["name"];
                dic["columns"] = tablecolumns;
                tablecolumnslist.Add(dic);
            }

            string modelspath = text_path.Text + "/Models";
            Directory.CreateDirectory(modelspath);
            using (StreamWriter sw = new StreamWriter(modelspath + "/Models.cs", false, Encoding.UTF8))
            {
                sw.Write(sqlcontrol.GenerateModels(tables));
            }
            for(int i=0;i< tablecolumnslist.Count; i++)
            {
                using (StreamWriter sw = new StreamWriter(modelspath + "/" + tablecolumnslist[i]["tablename"] + ".cs", false, Encoding.UTF8))
                {
                    sw.Write(sqlcontrol.GenerateModel((DataTable)tablecolumnslist[i]["columns"], (string)tablecolumnslist[i]["tablename"]));
                }
            }
            using (StreamWriter sw = new StreamWriter(modelspath + "/GetModelList.cs", false, Encoding.UTF8))
            {
                sw.Write(sqlcontrol.GenerateGetModelList(tables, columns));
            }

            string notifymodelspath = text_path.Text + "/Notify Models";
            Directory.CreateDirectory(notifymodelspath);
            using (StreamWriter sw = new StreamWriter(notifymodelspath + "/Models.cs", false, Encoding.UTF8))
            {
                sw.Write(sqlcontrol.GenerateModels2(tables));
            }
            for (int i = 0; i < tablecolumnslist.Count; i++)
            {
                using (StreamWriter sw = new StreamWriter(notifymodelspath + "/" + tablecolumnslist[i]["tablename"] + ".cs", false, Encoding.UTF8))
                {
                    sw.Write(sqlcontrol.GenerateModel2((DataTable)tablecolumnslist[i]["columns"], (string)tablecolumnslist[i]["tablename"]));
                }
            }
            using (StreamWriter sw = new StreamWriter(notifymodelspath + "/GetModelList.cs", false, Encoding.UTF8))
            {
                sw.Write(sqlcontrol.GenerateGetModelList(tables, columns));
            }
            
            SystemError.Information("类库已成功生成");
            this.DialogResult = true;
        }

        private void but_browes_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folder = new System.Windows.Forms.FolderBrowserDialog();
            if (folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                text_path.Text = folder.SelectedPath;
            }
        }
    }
}
