using AutoModel.Emun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoModel.Models
{
    /// <summary>
    /// 数据库连接配置类
    /// </summary>
    [Serializable]
    public partial class LinkConfigModel
    {
        int _id;
        string _name;
        string _connectionstring;
        string _database;
        SqlTypeEmun _type;

        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        public string name
        {
            set { _name = value; }
            get { return _name; }
        }
        public string connectionstring
        {
            set { _connectionstring = value; }
            get { return _connectionstring; }
        }
        public string database
        {
            set { _database = value; }
            get { return _database; }
        }
        public SqlTypeEmun type
        {
            set { _type = value; }
            get { return _type; }
        }
    }
}
