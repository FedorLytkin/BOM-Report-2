using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSNRM_Kompas;
using VSNRM_Kompas.API_Toops;
using System.IO;

namespace DiagramDataControllerBehavior.Data
{
    public class ClassStructureGenerator
    {
        TreeList treeView;
        List<string> KeyList;
        const string Material = "Материал";
        const string RazdelSP = "Раздел спецификации";


        const string Assembly = "Сборочные единицы";
        const string Part = "Детали";
        const string Standart = "Стандартные изделия";
        const string prochee = "Прочие изделия";
        const string material = "Материалы";
        public ClassStructureGenerator(TreeList MaintreeView)
        {
            treeView = MaintreeView;
            KeyList = new List<string>();
        }
        private ClassType GetType_By_RazdelSP(ComponentInfo componentInfo)
        {
            string Razdel_SP = null;
            if (componentInfo.isBody)
            {
                if (componentInfo.Body.ParamValueList.ContainsKey(RazdelSP))
                {
                    Razdel_SP = componentInfo.Body.ParamValueList[RazdelSP]; 
                }
                else { return ClassType.Body; }
            }
            else
            {
                if (componentInfo.ParamValueList.ContainsKey(RazdelSP) && !string.IsNullOrEmpty(componentInfo.ParamValueList[RazdelSP]))
                {
                    Razdel_SP = componentInfo.ParamValueList[RazdelSP];
                }
                else
                {
                    if (componentInfo.isDetal && componentInfo.standardComponent) return ClassType.Standart;
                    if (!componentInfo.isDetal) return ClassType.Assembly;
                    if (!componentInfo.isDetal && !componentInfo.standardComponent) return ClassType.Part;
                }
            }
            if (string.IsNullOrEmpty(Razdel_SP)) return ClassType.Part;
            switch (Razdel_SP)
            {
                case Assembly:
                    return ClassType.Assembly; 
                case Part:
                    return ClassType.Part; 
                case Standart:
                    return ClassType.Standart; 
                case prochee:
                    return ClassType.prochee; 
                case material:
                    return ClassType.material; 
                default:
                    return ClassType.Part; 
            }
        }
        public List<ClassData> ClassList()
        {
            var list = new List<ClassData>();
            List<TreeListNode> nodes = treeView.GetNodeList();
            foreach (TreeListNode node in nodes)
            {
                ComponentInfo componentInfo = (ComponentInfo)node.Tag;
                ClassData item = new ClassData();
                if (!KeyList.Contains(componentInfo.Key))
                {
                    item = SetData(item, componentInfo);
                    item.Key = componentInfo.Key;
                    list.Add(item);
                    KeyList.Add(componentInfo.Key);
                }
            }

            return list;
        }
        private ClassData SetData(ClassData item, ComponentInfo componentInfo)
        { 
            if (componentInfo.isBody)
            {
                item.FileName = Path.GetFileNameWithoutExtension(componentInfo.FFN);
                item.Oboz = componentInfo.Body.Oboz;
                item.Naim = componentInfo.Body.Naim;
                if (componentInfo.Body.ParamValueList.ContainsKey(Material))
                    item.Material = componentInfo.Body.ParamValueList[Material];
            }
            else
            {
                item.FileName = Path.GetFileNameWithoutExtension(componentInfo.FFN);
                item.Oboz = componentInfo.Oboz;
                item.Naim = componentInfo.Naim;
                if (componentInfo.ParamValueList.ContainsKey(Material))
                    item.Material = componentInfo.ParamValueList[Material];
            }
            item.Slide = componentInfo.LargeSlide;
            item.Type = GetType_By_RazdelSP(componentInfo);
            return item;
        }
        public List<ClassData> ClassList_Create_Dublicate()
        {
            var list = new List<ClassData>();
            List<TreeListNode> nodes = treeView.GetNodeList();
            foreach (TreeListNode node in nodes)
            {
                ComponentInfo componentInfo = (ComponentInfo)node.Tag;
                ClassData item = new ClassData(); 
                item = SetData(item, componentInfo);
                item.Key = node.Id.ToString();
                list.Add(item);
            }

            return list;
        }
        public List<ConnectionData> ConnectionList(bool CreateDublicate)
        {
            KeyList = new List<string>();
            var cList = new List<ConnectionData>();

            foreach (TreeListNode node in treeView.Nodes)
            {
                if (node.HasChildren)
                { 
                    if (!CreateDublicate) TravelTreeList(node, cList); 
                    else TravelTreeList_CreateDublicate(node, cList);
                }
            }

            return cList;
        }
        private void TravelTreeList_CreateDublicate(TreeListNode ParentNode, List<ConnectionData> cList)
        {
            ComponentInfo Parent_componentinfo = (ComponentInfo)ParentNode.Tag;
            foreach (TreeListNode node in ParentNode.Nodes)
            {
                ComponentInfo componentinfo = (ComponentInfo)node.Tag;
                if (componentinfo.Body.Naim != null)
                {
                    cList.Add(new ConnectionData()
                    {
                        ConnectedTo = node.Id.ToString(),
                        ConnectorText = componentinfo.Body.QNT.ToString(),
                        ConnectedFrom = ParentNode.Id.ToString()
                    });
                }
                else
                {
                    cList.Add(new ConnectionData()
                    {
                        ConnectedTo = node.Id.ToString(),
                        ConnectorText = componentinfo.QNT.ToString(),
                        ConnectedFrom = ParentNode.Id.ToString()
                    });
                } 
                if (node.HasChildren) TravelTreeList_CreateDublicate(node, cList);
            }
        }
        private void TravelTreeList(TreeListNode ParentNode, List<ConnectionData>  cList)
        {
            ComponentInfo Parent_componentinfo = (ComponentInfo)ParentNode.Tag;
            foreach (TreeListNode node in ParentNode.Nodes)
            {
                ComponentInfo componentinfo = (ComponentInfo)node.Tag;
                if (componentinfo.Body.Naim != null)
                {
                    cList.Add(new ConnectionData()
                    {
                        ConnectedTo = componentinfo.Key,
                        ConnectorText = componentinfo.Body.QNT.ToString(),
                        ConnectedFrom = Parent_componentinfo.Key
                    });
                }
                else
                {
                    cList.Add(new ConnectionData()
                    {
                        ConnectedTo = componentinfo.Key,
                        ConnectorText = componentinfo.QNT.ToString(),
                        ConnectedFrom = Parent_componentinfo.Key
                    });
                }
                if (node.HasChildren) TravelTreeList(node, cList);
            }
        }
    }
}
