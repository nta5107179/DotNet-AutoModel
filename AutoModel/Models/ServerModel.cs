using AutoModel.Emun;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AutoModel.Models
{
    public partial class ServerModel : INotifyPropertyChanged
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
        SqlTypeEmun _value;

        public string text
        {
            set { _text = value; Notify("text"); }
            get { return _text; }
        }

        public SqlTypeEmun value
        {
            set { _value = value; Notify("value"); }
            get { return _value; }
        }
    }
}
