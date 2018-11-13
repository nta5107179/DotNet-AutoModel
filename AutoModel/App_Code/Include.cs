using AutoModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoModel.App_Code
{
    public class Include
    {
        string m_namespace = ConfigJson.GetNamespace();

        public Include()
        {
            if (string.IsNullOrEmpty(m_namespace))
            {
                m_namespace = "Models";
            }
        }

        /// <summary>
        /// 普通实体
        /// </summary>
        /// <returns></returns>
        protected string GenerateModel(List<ColumnMapModel> columns, string tablename)
        {
            List<string> codelist = new List<string>();
            codelist.Add("using System;\r\n");
            codelist.Add("namespace "+ m_namespace);
            codelist.Add("{");
            codelist.Add("    [Serializable]");
            codelist.Add("    public partial class " + tablename);
            codelist.Add("    {");
            codelist.Add("        public " + tablename + "()");
            codelist.Add("        { }");
            codelist.Add("        #region Model");
            for (int i = 0; i < columns.Count; i++)
            {
                codelist.Add("        private " + columns[i].type + " _" + columns[i].name + ";");
            }
            for (int i = 0; i < columns.Count; i++)
            {
                codelist.Add("        public " + columns[i].type + " " + columns[i].name);
                codelist.Add("        {");
                codelist.Add("            set { _"+ columns[i].name + " = value; }");
                codelist.Add("            get { return _" + columns[i].name + "; }");
                codelist.Add("        }");
            }
            codelist.Add("        #endregion Model");
            codelist.Add("    }");
            codelist.Add("}");
            return string.Join("\r\n", codelist);
        }
        /// <summary>
        /// 双向绑定实体
        /// </summary>
        /// <returns></returns>
        protected string GenerateModel2(List<ColumnMapModel> columns, string tablename)
        {
            List<string> codelist = new List<string>();
            codelist.Add("using System;");
            codelist.Add("using System.ComponentModel;\r\n");
            codelist.Add("namespace "+ m_namespace);
            codelist.Add("{");
            codelist.Add("    [Serializable]");
            codelist.Add("    public partial class " + tablename + " : INotifyPropertyChanged");
            codelist.Add("    {");
            codelist.Add("        public event PropertyChangedEventHandler PropertyChanged;\r\n");
            codelist.Add("        public " + tablename + "()");
            codelist.Add("        { }");
            codelist.Add("        #region Model");
            for (int i = 0; i < columns.Count; i++)
            {
                codelist.Add("        private " + columns[i].type + " _" + columns[i].name + ";");
            }
            for (int i = 0; i < columns.Count; i++)
            {
                codelist.Add("        public " + columns[i].type + " " + columns[i].name);
                codelist.Add("        {");
                codelist.Add("            set { _" + columns[i].name + " = value; Notify("+ columns[i].name + "); }");
                codelist.Add("            get { return _" + columns[i].name + "; }");
                codelist.Add("        }");
            }
            codelist.Add("        #endregion Model");
            codelist.Add("        private void Notify(string propertyName)");
            codelist.Add("        {");
            codelist.Add("            if (PropertyChanged != null)");
            codelist.Add("            {");
            codelist.Add("                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));");
            codelist.Add("            }");
            codelist.Add("        }");
            codelist.Add("    }");
            codelist.Add("}");
            return string.Join("\r\n", codelist);
        }

        /// <summary>
        /// 普通实体集合
        /// </summary>
        /// <returns></returns>
        protected string GenerateModels(List<TableMapModel> tables)
        {
            List<string> codelist = new List<string>();
            codelist.Add("using System;\r\n");
            codelist.Add("namespace "+ m_namespace);
            codelist.Add("{");
            codelist.Add("    [Serializable]");
            codelist.Add("    public partial class Models");
            codelist.Add("    {");
            codelist.Add("        public Models()");
            codelist.Add("        { }");
            codelist.Add("        #region Model");
            for (int i = 0; i < tables.Count; i++)
            {
                codelist.Add("        private " + tables[i].name + " _" + tables[i].name + ";");
            }
            for (int i = 0; i < tables.Count; i++)
            {
                codelist.Add("        public " + tables[i].name + " " + tables[i].name);
                codelist.Add("        {");
                codelist.Add("            set { _" + tables[i].name + " = value; }");
                codelist.Add("            get { return _" + tables[i].name + "; }");
                codelist.Add("        }");
            }
            codelist.Add("        #endregion Model");
            codelist.Add("    }");
            codelist.Add("}");
            return string.Join("\r\n", codelist);
        }

        /// <summary>
        /// 双向绑定实体集合
        /// </summary>
        /// <returns></returns>
        protected string GenerateModels2(List<TableMapModel> tables)
        {
            List<string> codelist = new List<string>();
            codelist.Add("using System;");
            codelist.Add("using System.ComponentModel;\r\n");
            codelist.Add("namespace " + m_namespace);
            codelist.Add("{");
            codelist.Add("    [Serializable]");
            codelist.Add("    public partial class Models : INotifyPropertyChanged");
            codelist.Add("    {");
            codelist.Add("        public event PropertyChangedEventHandler PropertyChanged;\r\n");
            codelist.Add("        public Models()");
            codelist.Add("        { }");
            codelist.Add("        #region Model");
            for (int i = 0; i < tables.Count; i++)
            {
                codelist.Add("        private " + tables[i].name + " _" + tables[i].name + ";");
            }
            for (int i = 0; i < tables.Count; i++)
            {
                codelist.Add("        public " + tables[i].name + " " + tables[i].name);
                codelist.Add("        {");
                codelist.Add("            set { _" + tables[i].name + " = value; Notify(" + tables[i].name + "); }");
                codelist.Add("            get { return _" + tables[i].name + "; }");
                codelist.Add("        }");
            }
            codelist.Add("        #endregion Model");
            codelist.Add("        private void Notify(string propertyName)");
            codelist.Add("        {");
            codelist.Add("            if (PropertyChanged != null)");
            codelist.Add("            {");
            codelist.Add("                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));");
            codelist.Add("            }");
            codelist.Add("        }");
            codelist.Add("    }");
            codelist.Add("}");
            return string.Join("\r\n", codelist);
        }
        
        /// <summary>
        /// 表泛型集合
        /// </summary>
        /// <returns></returns>
        protected string GenerateGetModelList(List<TableMapModel> tables, List<ColumnMapModel> columns)
        {
            List<string> codelist = new List<string>();
            codelist.Add("using System;");
            codelist.Add("using System.Collections.Generic;");
            codelist.Add("using System.Data;\r\n");
            codelist.Add("namespace " + m_namespace);
            codelist.Add("{");
            codelist.Add("    [Serializable]");
            codelist.Add("    public class GetModelList");
            codelist.Add("    {");
            codelist.Add("        public GetModelList()");
            codelist.Add("        { }");
            for (int i = 0; i < tables.Count; i++)
            {
                codelist.Add("        public List<" + tables[i].name + "> " + tables[i].name + "(DataTable dt)");
                codelist.Add("        {");
                codelist.Add("            List<" + tables[i].name + "> modelList = new List<" + tables[i].name + ">();");
                codelist.Add("            " + tables[i].name + " model;");
                codelist.Add("            for (int n = 0; n < dt.Rows.Count; n++)");
                codelist.Add("            {");
                codelist.Add("                model = new " + tables[i].name + "();");
                List<ColumnMapModel> _columns = columns.Where(a => a.tablename == tables[i].name).ToList();
                for (int j = 0; j < _columns.Count; j++)
                {
                    codelist.Add("                try");
                    codelist.Add("                {");
                    codelist.Add("                    model." + _columns[j].name + " = (" + _columns[j].type.Replace("?", "") + ")dt.Rows[n][\"" + _columns[j].name + "\"];");
                    codelist.Add("                }");
                    codelist.Add("                catch { }");
                }
                codelist.Add("                modelList.Add(model);");
                codelist.Add("            }");
                codelist.Add("            return modelList;");
                codelist.Add("        }");
            }
            codelist.Add("    }");
            codelist.Add("}");
            return string.Join("\r\n", codelist);
        }
    }
}
