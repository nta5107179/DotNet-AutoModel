using AutoModel.App_Code;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AutoModel.Models.TreeNode
{
    public partial class TreeNodeDatabase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify(string propertyName)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        string _text;
        string _database;
        string _connectionstring;
        ObservableCollection<TreeNodeTable> _childer;
        ISqlControl _sqlcontrol;
        bool _isexpanded = false;

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
        public ObservableCollection<TreeNodeTable> childer
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
