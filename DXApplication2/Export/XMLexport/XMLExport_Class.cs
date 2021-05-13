using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSNRM_Kompas.Export.XMLexport
{
    class XMLExport_Class
    {
        private StreamWriter sr;
        public bool SplitNode = true;
        public void exportToXml(TreeList ThisTreelist, string filename)
        {
            sr = new StreamWriter(filename, false, System.Text.Encoding.UTF8);
            //Write the header
            sr.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");

            List<string> ParamList = new List<string>();
            //add columns info
            sr.WriteLine("\t<Columns>");
            foreach (TreeListColumn Column in ThisTreelist.Columns)
            {
                if(Column.Caption != "Миниатюра" && Column.Visible)
                {
                    sr.WriteLine($"\t\t<Column>{Column.Caption}</Column>");
                    ParamList.Add(Column.Caption);
                }
            }
            sr.WriteLine("\t</Columns>");

            sr.WriteLine("\t<Nodes>");
            //add nodes info
            foreach (TreeListNode node in ThisTreelist.GetNodeList())
            {
                int ParentId = -1;
                if (node.ParentNode != null)
                    ParentId = node.ParentNode.Id;

                sr.WriteLine($"\t\t<Node Id=\"{node.Id}\" ParentId=\"{ParentId}\">");
                sr.WriteLine("\t\t\t<NodeData>");
                foreach (string ParamName in ParamList)
                {
                    sr.WriteLine($"\t\t\t\t<Cell ParameterName=\"{ParamName}\" ParameterValue=\"{node.GetValue(ParamName)}\"></Cell>");
                }
                sr.WriteLine("\t\t\t</NodeData>");
                sr.WriteLine("\t\t</Node>");
            }
            sr.WriteLine("\t</Nodes>");

            sr.Close();
        }
        private void AddThisNodes(TreeListNodes ThisNodes, List<string> ParamList, int tabInd)
        {
            //add nodes info
            foreach (TreeListNode node in ThisNodes)
            {
                if (SplitNode)
                {
                    VSNRM_Kompas.API_Toops.ComponentInfo componentinfo = (API_Toops.ComponentInfo)node.Tag;
                    if (componentinfo != null)
                    {
                        int count = (string.IsNullOrWhiteSpace(node.GetValue("Количество общ.").ToString()) ? 1 : Convert.ToInt32(node.GetValue("Количество общ.")));
                        for (int i = 0; i < count; i++)
                        {
                            sr.WriteLine($"{new string('\t', tabInd)}<Object Name=\"{(componentinfo.isBody ? componentinfo.Body.Naim :componentinfo.Naim)}\">");
                            foreach (string ParamName in ParamList)
                            {
                                string ParamVal = Convert.ToString(node.GetValue(ParamName));
                                if (!string.IsNullOrWhiteSpace(ParamVal))
                                    sr.WriteLine($"{new string('\t', tabInd + 1)}<Attribute Name=\"{ParamName}\">{ParamVal}</Attribute>");
                            }

                            if (node.HasChildren)
                                AddThisNodes(node.Nodes, ParamList, tabInd + 1);
                            sr.WriteLine($"{new string('\t', tabInd)}</Object>");
                        }
                    }
                }
                else
                {
                    sr.WriteLine($"{new string('\t', tabInd)}<Object Name={node.Id}>");
                    foreach (string ParamName in ParamList)
                        if (!string.IsNullOrWhiteSpace(node.GetValue(ParamName).ToString()))
                            sr.WriteLine($"{new string('\t', tabInd + 1)}<Attribute Name=\"{ParamName}\">{node.GetValue(ParamName)}</Attribute>");

                    if (node.HasChildren)
                        AddThisNodes(node.Nodes, ParamList, tabInd + 1);
                    sr.WriteLine($"{new string('\t', tabInd)}</Object>");
                } 
            }
        }
        public void exportToXml2(TreeList ThisTreelist, string filename)
        {
            sr = new StreamWriter(filename, false, System.Text.Encoding.UTF8);
            //Write the header
            sr.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            List<string> ParamList = new List<string>();
            //add columns info
            //sr.WriteLine("\t<Columns>");
            foreach (TreeListColumn Column in ThisTreelist.Columns)
                if (Column.Caption != "Миниатюра" && Column.Visible)
                    ParamList.Add(Column.Caption);
            sr.WriteLine("<Root>");
            AddThisNodes(ThisTreelist.Nodes, ParamList, 1);
            sr.WriteLine("</Root>");

            sr.Close();
        }
    }
}
