using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoModel.Models
{
    public partial class ColumnMapModel
    {
        string _name;
        string _type;
        string _remark;
        string _tablename;

        public string name
        {
            set { _name = value; }
            get { return _name; }
        }
        public string type
        {
            set { _type = value; }
            get { return _type; }
        }
        public string remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        public string tablename
        {
            set { _tablename = value; }
            get { return _tablename; }
        }
    }
}
