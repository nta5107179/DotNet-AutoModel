using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AutoModel.Models.TreeNode
{
    public partial class TreeNodeMain : INotifyPropertyChanged
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
        ObservableCollection<TreeNodeServer> _childer;
        bool _isexpanded = true;

        public string text
        {
            set { _text = value; Notify("text"); }
            get { return _text; }
        }
        public ObservableCollection<TreeNodeServer> childer
        {
            set { _childer = value; Notify("childer"); }
            get { return _childer; }
        }
        public bool isexpanded
        {
            set { _isexpanded = value; Notify("isexpanded"); }
            get { return _isexpanded; }
        }
    }
}
