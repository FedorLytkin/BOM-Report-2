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
                if(Column.Caption != "Миниатюра")
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
    }
}
