using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoModel.Models
{
    public partial class TableMapModel
    {
        string _name;
        string _remark;

        public string name
        {
            set { _name = value; }
            get { return _name; }
        }
        public string remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
    }
}
