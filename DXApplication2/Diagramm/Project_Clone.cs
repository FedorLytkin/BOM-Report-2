using DevExpress.XtraBars;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using SaveDXF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VSNRM_Kompas.Diagramm.ControlClass;

namespace VSNRM_Kompas.Diagramm
{
    public partial class Project_Clone : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        RepositoryItemPictureEdit pictureEdit;
        Body CAD_Body;
        ControlClass.Ploject_Clone_ControllClass PC;
        public Project_Clone(Body body, TreeList sourceTreeList)
        {
            InitializeComponent();
            CAD_Body = body;
            pictureEdit = treeList1.RepositoryItems.Add("PictureEdit") as RepositoryItemPictureEdit;
            this.treeList1.CustomNodeCellEdit += new DevExpress.XtraTreeList.GetCustomNodeCellEditEventHandler(this.treeList1_CustomNodeCellEdit);

            foreach (TreeListColumn col in sourceTreeList.Columns)
            {
                treeList1.Columns.AddField(col.FieldName).Visible = col.Visible;
                treeList1.Columns[col.FieldName].VisibleIndex = col.VisibleIndex;
            }
            //foreach (TreeListColumn col in sourceTreeList.Columns)
            //{
            //}

            sourceTreeList.NodesIterator.DoOperation(new CopyNodesOperation(treeList1));

            treeList1.ExpandAll();
        }

        private void treeList1_CustomNodeCellEdit(object sender, GetCustomNodeCellEditEventArgs e)
        {
            if (e.Column.FieldName == "Миниатюра")
            {
                e.RepositoryItem = pictureEdit;
            }
        }
        private void Project_Clone_Load(object sender, EventArgs e)
        {

        }

    }
}