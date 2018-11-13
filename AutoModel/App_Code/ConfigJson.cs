using AutoModel.Models;
using CoreClass;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoModel.App_Code
{
    public static class ConfigJson
    {
        static OperateMemoryClass m_opmemory = new OperateMemoryClass();

        public static bool AddLink(ref LinkConfigModel linkconfig)
        {
            bool b = false;
            try
            {
                string path = Directory.GetCurrentDirectory() + "/Config.json";
                JObject config;
                using (StreamReader sr = new StreamReader(path))
                {
                    config = JObject.Parse(sr.ReadToEnd());
                    JArray list = (JArray)config["linklist"];
                    linkconfig.id = list.Count > 0 ?
                        list.Select(a => (LinkConfigModel)m_opmemory.Deserialize(Convert.FromBase64String(a.ToString()))).ToList().Max(a => a.id) + 1 : 1;
                    list.Add(Convert.ToBase64String(m_opmemory.Serialize(linkconfig)));
                }
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.Write(config.ToString());
                }
                b = true;
            }
            catch(Exception e)
            {
                SystemError.Error(e.Message);
            }
            return b;
        }

        public static bool DelLink(int id)
        {
            bool b = false;
            try
            {
                string path = Directory.GetCurrentDirectory() + "/Config.json";
                JObject config;
                using (StreamReader sr = new StreamReader(path))
                {
                    config = JObject.Parse(sr.ReadToEnd());
                    JArray list = (JArray)config["linklist"];
                    List<LinkConfigModel> linkconfiglist = list.Select(a => (LinkConfigModel)m_opmemory.Deserialize(Convert.FromBase64String(a.ToString()))).ToList();
                    for(int i=0;i< linkconfiglist.Count; i++)
                    {
                        if (linkconfiglist[i].id == id)
                            list.RemoveAt(i);
                    }
                }
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.Write(config.ToString());
                }
                b = true;
            }
            catch (Exception e)
            {
                SystemError.Error(e.Message);
            }
            return b;
        }

        public static bool SetNamespace(string _namespace)
        {
            bool b = false;
            try
            {
                string path = Directory.GetCurrentDirectory() + "/Config.json";
                JObject config;
                using (StreamReader sr = new StreamReader(path))
                {
                    config = JObject.Parse(sr.ReadToEnd());
                    config["config"]["namespace"] = _namespace;
                }
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.Write(config.ToString());
                }
                b = true;
            }
            catch (Exception e)
            {
                SystemError.Error(e.Message);
            }
            return b;
        }

        public static List<LinkConfigModel> LoadLink()
        {
            List<LinkConfigModel> list = new List<LinkConfigModel>();
            try
            {
                string path = Directory.GetCurrentDirectory() + "/Config.json";
                using (StreamReader sr = new StreamReader(path))
                {
                    JObject treeconfig = JObject.Parse(sr.ReadToEnd());
                    JArray jarr = (JArray)treeconfig["linklist"];
                    for (int i = 0; i < jarr.Count; i++)
                    {
                        list.Add((LinkConfigModel)m_opmemory.Deserialize(Convert.FromBase64String(jarr[i].ToString())));
                    }
                }
            }
            catch (Exception e)
            {
                SystemError.Error(e.Message);
            }
            return list;
        }

        public static string GetNamespace()
        {
            string _namespace = "";
            try
            {
                string path = Directory.GetCurrentDirectory() + "/Config.json";
                using (StreamReader sr = new StreamReader(path))
                {
                    JObject jobj = JObject.Parse(sr.ReadToEnd());
                    _namespace = (string)jobj["config"]["namespace"];
                }
            }
            catch (Exception e)
            {
                SystemError.Error(e.Message);
            }
            return _namespace;
        }
    }
}
