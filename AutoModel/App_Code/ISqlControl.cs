using AutoModel.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;

namespace AutoModel.App_Code
{
    /// <summary>
    /// 数据库控制接口
    /// </summary>
    public interface ISqlControl
    {
        ObservableCollection<DatabaseModel> TestLink(string connectionstring);
        DataTable GetDatabaseList(string connectionstring, string database);
        DataTable GetTableList(string connectionstring, string database);
        DataTable GetColumnList(string connectionstring, string database, string table = null);
        string GenerateModel(DataTable columns, string tablename);
        string GenerateModel2(DataTable columns, string tablename);
        string GenerateModels(DataTable tables);
        string GenerateModels2(DataTable tables);
        string GenerateGetModelList(DataTable tables, DataTable columns);
    }
}
