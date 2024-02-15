using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Collections.ObjectModel;
using MyApp.MVVM.ViewModel;
using System;
using System.Windows;
using System.Linq;

namespace MyApp.Utilities
{
    public class JsonHandler
    {
        //TODO: implement repository pattern

        public JsonHandler(string file)
        {
            JFile = file;
        }

        public string JFile { get; }


        #region methods

        public void GenerateEmptyJson()
        {
            string newRootJson = "{}";
            File.WriteAllText(JFile, newRootJson);
        }

        private JArray generateTable(JObject root, string table)
        {
            JArray jArray = new JArray();
            root.Add(table, jArray);
            return jArray;
        }

        public void Insert(string table, object obj)
        {
            string currentJson = File.ReadAllText(JFile);
            string newJson;
            JObject rootObject = (JObject)JsonConvert.DeserializeObject(currentJson);
            JArray jTable = (JArray) rootObject[table] ?? generateTable(rootObject, table);

            jTable.Add(JObject.FromObject(obj));
            newJson = JsonConvert.SerializeObject(rootObject, Formatting.Indented);
            File.WriteAllText(JFile, newJson);
        }

        public object Get(string table, string id)
        {
            string currentJson = File.ReadAllText (JFile);
            JObject rootObject = JsonConvert.DeserializeObject<JObject>(currentJson);
            JArray jTable = (JArray)rootObject[table];

            if (jTable == null)
            {
                jTable = generateTable(rootObject, table);
            }

            return jTable.First(obj => obj["Id"].ToString() == id).ToObject<object>();
        }

        public List<object> GetAll(string table)
        {
            List<object> list = new List<object>();

            string currentJson = File.ReadAllText(JFile);
            JObject rootObject = (JObject)JsonConvert.DeserializeObject(currentJson);
            JArray jTable = (JArray)rootObject[table] ?? generateTable(rootObject, table);

            foreach(JObject obj in jTable)
            {
                list.Add(obj);
            }

            return list;
        }

        public void Update(string table, List<object> list)
        {
            string currentJson = File.ReadAllText(JFile);
            string newJson;
            JObject rootObject = (JObject)JsonConvert.DeserializeObject(currentJson);
            JArray oldTable = (JArray)rootObject[table] ?? generateTable(rootObject, table);
            JArray newTable = new JArray();

            foreach (object obj in list)
            {
                newTable.Add(JObject.FromObject(obj));
            }

            oldTable.Replace(newTable);
            newJson = JsonConvert.SerializeObject(rootObject, Formatting.Indented);
            File.WriteAllText(JFile, newJson);
        }

        public void InsertDefault(string key, string value)
        {
            string currentJson = File.ReadAllText(JFile);
            JObject rootObject = (JObject)JsonConvert.DeserializeObject(currentJson);
            JObject table = (JObject)rootObject["Defaults"];
            
            if (table == null)
            {
                var jObject = new JObject();
                rootObject.Add("Defaults", jObject);
                table = jObject;
            }

            if (table.ContainsKey(key)) { table[key] = value; }
            else { table.Add(key, value); }

            string newJson = JsonConvert.SerializeObject(rootObject, Formatting.Indented);
            File.WriteAllText(JFile, newJson);
        }

        public string GetDefault(string key)
        {
            string currentJson = File.ReadAllText(JFile);
            JObject rootObject = JObject.Parse(currentJson);
            JObject table = (JObject)rootObject["Defaults"];

            if (table != null && table.ContainsKey(key))
            {
                return table[key].ToString();
            }
            else
            {
                return "";
            }
            
        }

        #endregion methods

    }
}
