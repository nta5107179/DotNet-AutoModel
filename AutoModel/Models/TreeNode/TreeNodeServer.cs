using AutoModel.App_Code;
using AutoModel.Emun;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AutoModel.Models.TreeNode
{
    public partial class TreeNodeServer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify(string propertyName)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        int _id;
        string _text;
        string _database;
        string _connectionstring;
        SqlTypeEmun _type;
        ObservableCollection<TreeNodeDatabase> _childer;
        ISqlControl _sqlcontrol;
        bool _isexpanded = true;

        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        public string text
        {
            set { _text = value; Notify("text"); }
            get { return _text; }
        }
        public string database
        {
            set { _database = value; Notify("database"); }
            get { return _database; }
        }
        public string connectionstring
        {
            set { _connectionstring = value; Notify("connectionstring"); }
            get { return _connectionstring; }
        }
        public SqlTypeEmun type
        {
            set { _type = value; Notify("type"); }
            get { return _type; }
        }
        public ObservableCollection<TreeNodeDatabase> childer
        {
            set { _childer = value; Notify("childer"); }
            get { return _childer; }
        }
        public ISqlControl sqlcontrol
        {
            set { _sqlcontrol = value; }
            get { return _sqlcontrol; }
        }
        public bool isexpanded
        {
            set { _isexpanded = value; Notify("isexpanded"); }
            get { return _isexpanded; }
        }
    }
}
