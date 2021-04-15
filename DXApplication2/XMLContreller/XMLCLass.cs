using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using VSNRM_Kompas.Options;

namespace VSNRM_Kompas.XMLContreller
{
    class XMLCLass
    {
        public Columns_Option IColumns = new Columns_Option();
        public Options IOptions = new Options(); 
        public INILists IList = new INILists();
        public SystemInformation ISystemInformation = new SystemInformation();
        public class Columns_Option
        { 
            public void SaveColums(List<Column_Class> columns,bool askOptFileName)
            {
                string path = null;
                if (!askOptFileName)
                    path = OptionPath.Columns_FileXML;
                else
                {
                    System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
                    saveFileDialog.Filter = "XML files (*.XML)|*.XML";
                    if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) 
                        path = saveFileDialog.FileName; 
                    else return;
                }
                XmlSerializer writer =
                new XmlSerializer(typeof(List<Column_Class>));
                FileStream file = File.Create(path);

                writer.Serialize(file, columns);
                file.Close();
            }
            public List<Column_Class> GetColumns(bool askFileName)
            {
                string path = null ;
                if (askFileName)
                {
                    System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
                    openFileDialog.Filter = "XML files (*.XML)|*.XML";
                    if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        path = openFileDialog.FileName;
                    else return null;
                }
                else
                    path = OptionPath.Columns_FileXML;
                List<Column_Class> column_s = new List<Column_Class>();

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(path);
                string xmlString = xmlDocument.OuterXml;
                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(List<Column_Class>);
                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        column_s = (List<Column_Class>)serializer.Deserialize(reader);
                        reader.Close();
                    }
                    read.Close();
                }
                return column_s;
            }
        }
        public class INILists
        {
            public List<string> GetList(string Sourse_Path)
            {
                List<string> Params_List = new List<string>();
                string[] lines = File.ReadAllLines(Sourse_Path);
                foreach (string line in lines)
                    Params_List.Add(line);

                return Params_List;
            }
        }
        public class SystemInformation
        {
            public void Save(GetOptionInformation columns)
            {
                XmlSerializer writer =
                new XmlSerializer(typeof(GetOptionInformation));

                var path = OptionPath.SystemInformation_ForUser_FileXML;
                FileStream file = File.Create(path);

                writer.Serialize(file, columns);
                file.Close();
            }
            public GetOptionInformation GetInfo()
            {
                string path = OptionPath.SystemInformation_ForUser_FileXML;
                GetOptionInformation column_s = new GetOptionInformation();

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(path);
                string xmlString = xmlDocument.OuterXml;
                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(GetOptionInformation);
                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        column_s = (GetOptionInformation)serializer.Deserialize(reader);
                        reader.Close();
                    }
                    read.Close();
                }
                return column_s;
            }
        }
        public class Options
        {
            public void Save(List<Obj_Variable_Class> columns)
            {
                XmlSerializer writer =
                new XmlSerializer(typeof(List<Obj_Variable_Class>));

                var path = OptionPath.Options_FileXML;
                FileStream file = File.Create(path);

                writer.Serialize(file, columns);
                file.Close();
            }
            public List<Obj_Variable_Class> GetOptions()
            {
                string path = OptionPath.Options_FileXML;
                List<Obj_Variable_Class> column_s = new List<Obj_Variable_Class>();

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(path);
                string xmlString = xmlDocument.OuterXml;
                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(List<Obj_Variable_Class>);
                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        column_s = (List<Obj_Variable_Class>)serializer.Deserialize(reader);
                        reader.Close();
                    }
                    read.Close();
                }
                return column_s;
            }
            public string GetFieldValue(string FieldName)
            {
                string FieldValue = "";
                List<Obj_Variable_Class> fields = GetOptions();
                foreach (Obj_Variable_Class variable in fields)
                    if (variable.Name == FieldName)
                        return variable.Value.ToString();
                return FieldValue;
            }
            public void SaveOptions(Option_Class IOptions, bool askOptFileName)
            {
                string path = null;
                if (!askOptFileName)
                    path = OptionPath.MUOptions_FileXML;
                else
                {
                    System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
                    saveFileDialog.Filter = "XML files (*.XML)|*.XML";
                    if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        path = saveFileDialog.FileName;
                    else return;
                }
                XmlSerializer writer =
                new XmlSerializer(typeof(Option_Class));
                FileStream file = File.Create(path);

                writer.Serialize(file, IOptions);
                file.Close();
            }
            public Option_Class GetOptions(bool askFileName)
            {
                string path = null;
                if (askFileName)
                {
                    System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
                    openFileDialog.Filter = "XML files (*.XML)|*.XML";
                    if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        path = openFileDialog.FileName;
                    else return null;
                }
                else
                    path = OptionPath.MUOptions_FileXML;
                Option_Class IOption_Class = new Option_Class();

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(path);
                string xmlString = xmlDocument.OuterXml;
                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(Option_Class);
                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        IOption_Class = (Option_Class)serializer.Deserialize(reader);
                        reader.Close();
                    }
                    read.Close();
                }
                return IOption_Class;
            }
        }
    }
}
