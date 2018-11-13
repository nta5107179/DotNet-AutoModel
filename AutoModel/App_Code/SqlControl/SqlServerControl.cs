using AutoModel.Models;
using CoreClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AutoModel.App_Code.SqlControl
{
    public class SqlServerControl : Include, ISqlControl
    {
        OperateSqlClass_MSSQL m_opsql = new OperateSqlClass_MSSQL();

        public ObservableCollection<DatabaseModel> TestLink(string connectionstring)
        {
            ObservableCollection<DatabaseModel> list = null;
            try
            {
                m_opsql.Open(connectionstring);
                try
                {
                    string sql = string.Format("select name from sys.databases");
                    DataSet ds = m_opsql.Select(sql);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        list = new ObservableCollection<DatabaseModel>();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            DatabaseModel mod = new DatabaseModel();
                            mod.text = ds.Tables[0].Rows[i]["name"].ToString();
                            mod.value = ds.Tables[0].Rows[i]["name"].ToString();
                            list.Add(mod);
                        }
                    }
                    else
                    {
                        throw new Exception("无法读取数据库列表");
                    }
                }
                catch (Exception e) { throw e; }
                finally
                {
                    m_opsql.Close();
                }
            }
            catch (Exception e)
            {
                SystemError.Error(e.Message);
            }

            return list;
        }
        public DataTable GetDatabaseList(string connectionstring, string database)
        {
            DataTable dt = null;
            try
            {
                m_opsql.Open(connectionstring);
                try
                {
                    string where = "";
                    if (!string.IsNullOrEmpty(database))
                    {
                        where += string.Format(" and name='{0}'", database);
                    }
                    string sql = string.Format("select name from sysdatabases where 1=1{0}", where);
                    DataSet ds = m_opsql.Select(sql);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                    }
                    else
                    {
                        throw new Exception("无法读取数据库列表");
                    }
                }
                catch (Exception e) { throw e; }
                finally
                {
                    m_opsql.Close();
                }
            }
            catch (Exception e)
            {
                SystemError.Error(e.Message);
            }
            return dt;
        }

        public DataTable GetTableList(string connectionstring, string database)
        {
            DataTable dt = null;
            try
            {
                m_opsql.Open(connectionstring);
                try
                {
                    string sql = string.Format("select name from {0}.dbo.sysobjects where xtype='U'", database);
                    DataSet ds = m_opsql.Select(sql);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                    }
                    else
                    {
                        throw new Exception("无法读取表列表");
                    }
                }
                catch (Exception e) { throw e; }
                finally
                {
                    m_opsql.Close();
                }
            }
            catch (Exception e)
            {
                SystemError.Error(e.Message);
            }
            return dt;
        }

        public DataTable GetColumnList(string connectionstring, string database, string table = null)
        {
            DataTable dt = null;
            try
            {
                m_opsql.Open(connectionstring);
                try
                {
                    string where = "";
                    if (!string.IsNullOrEmpty(table))
                    {
                        where += string.Format(" and t1.name='{0}'", table);
                    }
                    string sql = string.Format(@"
select t1.name as tablename, t2.name, t3.name as type from {0}.dbo.sysobjects as t1 
left join {0}.dbo.syscolumns as t2 on t1.id=t2.id
left join {0}.dbo.systypes as t3 on t2.xtype=t3.xtype and t2.xusertype=t3.xusertype
where t1.xtype='U'{1}
                        ", database, where);
                    DataSet ds = m_opsql.Select(sql);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                    }
                    else
                    {
                        throw new Exception("无法读取列列表");
                    }
                }
                catch (Exception e) { throw e; }
                finally
                {
                    m_opsql.Close();
                }
            }
            catch (Exception e)
            {
                SystemError.Error(e.Message);
            }
            return dt;
        }

        public string GenerateModel(DataTable columns, string tablename)
        {
            List<ColumnMapModel> list = new List<ColumnMapModel>();
            for(int i = 0; i < columns.Rows.Count; i++)
            {
                list.Add(new ColumnMapModel()
                {
                    name = (string)columns.Rows[i]["name"],
                    type = GetDotNetType((string)columns.Rows[i]["type"]),
                    tablename = (string)columns.Rows[i]["tablename"]
                });
            }
            return base.GenerateModel(list, tablename);
        }

        public string GenerateModel2(DataTable columns, string tablename)
        {
            List<ColumnMapModel> list = new List<ColumnMapModel>();
            for (int i = 0; i < columns.Rows.Count; i++)
            {
                list.Add(new ColumnMapModel()
                {
                    name = (string)columns.Rows[i]["name"],
                    type = GetDotNetType((string)columns.Rows[i]["type"]),
                    tablename = (string)columns.Rows[i]["tablename"]
                });
            }
            return base.GenerateModel2(list, tablename);
        }

        public string GenerateModels(DataTable tables)
        {
            List<TableMapModel> list = new List<TableMapModel>();
            for (int i = 0; i < tables.Rows.Count; i++)
            {
                list.Add(new TableMapModel()
                {
                    name = (string)tables.Rows[i]["name"]
                });
            }
            return base.GenerateModels(list);
        }

        public string GenerateModels2(DataTable tables)
        {
            List<TableMapModel> list = new List<TableMapModel>();
            for (int i = 0; i < tables.Rows.Count; i++)
            {
                list.Add(new TableMapModel()
                {
                    name = (string)tables.Rows[i]["name"]
                });
            }
            return base.GenerateModels2(list);
        }

        public string GenerateGetModelList(DataTable tables, DataTable columns)
        {
            List<TableMapModel> tablelist = new List<TableMapModel>();
            for (int i = 0; i < tables.Rows.Count; i++)
            {
                tablelist.Add(new TableMapModel()
                {
                    name = (string)tables.Rows[i]["name"]
                });
            }
            List<ColumnMapModel> rowlist = new List<ColumnMapModel>();
            for (int i = 0; i < columns.Rows.Count; i++)
            {
                rowlist.Add(new ColumnMapModel()
                {
                    name = (string)columns.Rows[i]["name"],
                    type = GetDotNetType((string)columns.Rows[i]["type"]),
                    tablename = (string)columns.Rows[i]["tablename"]
                });
            }
            return base.GenerateGetModelList(tablelist, rowlist);
        }

        string GetDotNetType(string type)
        {
            string dotnettype = "";
            switch (type)
            {
                case "int":
                    dotnettype = "int?";
                    break;
                case "varchar":
                    dotnettype = "string";
                    break;
                case "nvarchar":
                    dotnettype = "string";
                    break;
                case "char":
                    dotnettype = "string";
                    break;
                case "nchar":
                    dotnettype = "string";
                    break;
                case "bit":
                    dotnettype = "bool?";
                    break;
                case "datetime":
                    dotnettype = "DateTime?";
                    break;
                case "float":
                    dotnettype = "double?";
                    break;
                case "decimal":
                    dotnettype = "double?";
                    break;
                case "money":
                    dotnettype = "double?";
                    break;
                case "date":
                    dotnettype = "DateTime?";
                    break;
                case "text":
                    dotnettype = "string";
                    break;
                case "ntext":
                    dotnettype = "string";
                    break;
                case "datetime2":
                    dotnettype = "DateTime?";
                    break;
            }
            return dotnettype;
        }

    }
}
