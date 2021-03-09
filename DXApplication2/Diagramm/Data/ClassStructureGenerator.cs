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
        public bool Create_Qnt_On_Line = false;
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
            //if (string.IsNullOrEmpty(componentInfo.ParamValueList["Количество общ."]))
            //    item.Qnt = 0;
            //else
            //    item.Qnt = Convert.ToDouble(componentInfo.ParamValueList["Количество общ."]);
            item.Qnt = componentInfo.Total_QNT;
            item.Slide = componentInfo.LargeSlide;
            item.Type = GetType_By_RazdelSP(componentInfo);
            return item;
        }
        public List<ClassData> ClassList(bool CreateDublicate)
        {
            var list = new List<ClassData>();
            List<TreeListNode> nodes = treeView.GetNodeList();
            foreach (TreeListNode node in nodes)
            {
                ComponentInfo componentInfo = (ComponentInfo)node.Tag;
                ClassData item = new ClassData();
                item = SetData(item, componentInfo);
                if (CreateDublicate)
                {
                    item.Key = node.Id.ToString();
                    list.Add(item);
                }
                else
                {
                    if (!KeyList.Contains(componentInfo.Key))
                    {
                        item.Qnt = GetTotalCount(node, componentInfo.Total_QNT);
                        item.Key = componentInfo.Key;
                        list.Add(item);

                        KeyList.Add(componentInfo.Key);
                    }
                }
            }

            return list;
        }
        private double GetTotalCount(TreeListNode ThisNode, double Qnt)
        {
            ComponentInfo ThisComponentInfo = (ComponentInfo)ThisNode.Tag;
            List<TreeListNode> nodes = treeView.GetNodeList();
            foreach (TreeListNode oNode in nodes)
            {
                ComponentInfo componentInfo = (ComponentInfo)oNode.Tag;
                if (ThisComponentInfo.Key == componentInfo.Key && ThisNode.Id != oNode.Id)
                {
                    return componentInfo.Total_QNT + ThisComponentInfo.Total_QNT;
                }
            }
            return Qnt;
        }
        public List<ConnectionData> ConnectionList(bool CreateDublicate)
        {
            KeyList = new List<string>();
            var cList = new List<ConnectionData>();

            foreach (TreeListNode node in treeView.Nodes)
            {
                if (node.HasChildren)
                    TravelTreeList(node, cList, CreateDublicate); 
            }

            return cList;
        }
        private void TravelTreeList(TreeListNode ParentNode, List<ConnectionData>  cList, bool CreateDublicate)
        {
            ComponentInfo Parent_componentinfo = (ComponentInfo)ParentNode.Tag;
            foreach (TreeListNode node in ParentNode.Nodes)
            {
                ComponentInfo componentinfo = (ComponentInfo)node.Tag;
                ConnectionData connectionData = new ConnectionData();
                if (Create_Qnt_On_Line)
                {
                    if (componentinfo.isBody) connectionData.ConnectorText = componentinfo.Body.QNT.ToString(); 
                    else connectionData.ConnectorText = componentinfo.QNT.ToString(); 
                }

                if (CreateDublicate)
                {
                    connectionData.ConnectedTo = node.Id.ToString();
                    connectionData.ConnectedFrom = ParentNode.Id.ToString();
                }
                else
                {
                    connectionData.ConnectedTo = componentinfo.Key;
                    connectionData.ConnectedFrom = Parent_componentinfo.Key;
                }
                cList.Add(connectionData); 
                if (node.HasChildren) TravelTreeList(node, cList, CreateDublicate);
            }
        }
    }
}
