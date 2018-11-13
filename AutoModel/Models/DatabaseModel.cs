using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AutoModel.Models
{
    public partial class DatabaseModel : INotifyPropertyChanged
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
        string _value;

        public string text
        {
            set { _text = value; Notify("text"); }
            get { return _text; }
        }

        public string value
        {
            set { _value = value; Notify("value"); }
            get { return _value; }
        }
    }
}
