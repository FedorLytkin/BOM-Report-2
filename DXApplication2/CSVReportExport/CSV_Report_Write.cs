
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VSNRM_Kompas.API_Toops;
using DevExpress.XtraTreeList;

namespace VSNRM_Kompas.CSVReportExport
{
    class CSV_Report_Write
    { 
        public List<ComponentInfo> GetListComponentInfo()
        {
            TreeList treeView = ((MainForm)Application.OpenForms["Form1"]).treeList1;
            List<ComponentInfo> records = new List<ComponentInfo>();
            foreach(TreeNode treeNode in treeView.Nodes)
            {
                records.Add((ComponentInfo)treeNode.Tag);
                if(treeNode.Nodes.Count > 0)
                    AddSubNode(records, treeNode);
            }
            return records;
        }
        private void AddSubNode(List<ComponentInfo> records, TreeNode treeNode)
        {
            foreach (TreeNode tempNode in treeNode.Nodes)
            {
                records.Add((ComponentInfo)tempNode.Tag);
                if (tempNode.Nodes.Count > 0)
                    AddSubNode(records, tempNode);
            }
        }
    }
}
