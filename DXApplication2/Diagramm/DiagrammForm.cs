using DevExpress.Diagram.Core;
using DevExpress.XtraDiagram;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VSNRM_Kompas.API_Toops;

namespace VSNRM_Kompas.Diagramm
{
    public partial class DiagrammForm : DevExpress.XtraEditors.XtraForm
    {
        TreeList treeView;
        public DiagrammForm()
        {
            InitializeComponent();
            treeView = ((MainForm)System.Windows.Forms.Application.OpenForms["MainForm"]).treeList1;

            ViewModel viewModel = new ViewModel(treeView);
            diagramControl1.ItemContentChanged += DiagramControl1_ItemContentChanged;

            diagramDataBindingController1.GenerateItem += DiagramDataBindingController1_GenerateItem;
            diagramDataBindingController1.DataSource = viewModel.Items;
            diagramDataBindingController1.KeyMember = "Id";
            diagramDataBindingController1.ConnectorsSource = viewModel.Connections;
            diagramDataBindingController1.ConnectorFromMember = "From";
            diagramDataBindingController1.ConnectorToMember = "To";
            diagramDataBindingController1.LayoutKind = DiagramLayoutKind.MindMapTree;
            BestFitShapes();
        }
        void BestFitShapes()
        {
            foreach (DiagramItem item in diagramControl1.Items)
            {
                IDiagramShape shape = item as IDiagramShape;
                if (shape == null)
                    continue;

                Font testFont = new Font(shape.FontFamily.Name, (float)shape.FontSize, FontStyle.Regular);
                ((DiagramShape)shape).Width = TextRenderer.MeasureText(shape.Content, testFont).Width;

            }
        }
        private void DiagramDataBindingController1_GenerateItem(object sender, DiagramGenerateItemEventArgs e)
        { 
            var item = new DiagramImage
            {
                X = 0,
                Width = 150,
                Height = 50,
                
            };
            item.Bindings.Add(new DiagramBinding("Content", "Name"));  
            item.Appearance.BorderSize = 0;
            e.Item = item;
        }
        private void DiagramControl1_ItemContentChanged(object sender, DiagramItemContentChangedEventArgs e)
        {
            IDiagramShape shape = e.Item as IDiagramShape;
            if (shape == null)
                return;

            Font testFont = new Font(shape.FontFamily.Name, (float)shape.FontSize, FontStyle.Regular);
            ((DiagramShape)shape).Width = TextRenderer.MeasureText(shape.Content, testFont).Width + 5;
        }
        private void DiagramControl1_ItemCreating(object sender, DiagramItemCreatingEventArgs e)
        {
            IDiagramShape shape = e.Item as IDiagramShape;
            if (shape == null)
                return;

            Font testFont = new Font(shape.FontFamily.Name, (float)shape.FontSize, FontStyle.Regular);
            ((DiagramShape)shape).Width = TextRenderer.MeasureText(shape.Content, testFont).Width + 5;
        }
        public class ViewModel
        {
            public List<Item> Items { get; set; }
            public List<Link> Connections { get; set; }
            public ViewModel(TreeList treeView)
            {
                Items = new List<Item>();
                Connections = new List<Link>();
                foreach (TreeListNode node in treeView.Nodes)
                {
                    ComponentInfo componentinfo = (ComponentInfo)node.Tag;
                    Items.Add(new Item { Id = node.Id, Name = $"{componentinfo.Oboz}\n{componentinfo.Naim}", Picture = componentinfo.Slide });
                    if (node.HasChildren) TravelTreeList(node);
                }
            }
            private void TravelTreeList(TreeListNode ParentNode)
            {
                foreach (TreeListNode node in ParentNode.Nodes)
                {
                    ComponentInfo componentinfo = (ComponentInfo)node.Tag;
                    if(componentinfo.Body.Naim != null)
                        Items.Add(new Item { Id = node.Id, Name = $"{componentinfo.Body.Oboz}\n{componentinfo.Body.Naim}", Picture = componentinfo.Slide});
                    else
                        Items.Add(new Item { Id = node.Id, Name = $"{componentinfo.Oboz}\n{componentinfo.Naim}", Picture = componentinfo.Slide });
                    Connections.Add(new Link { From = ParentNode.Id, To = node.Id });
                    if (node.HasChildren) TravelTreeList(node);
                } 
            }
        }
        public class Item
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Bitmap Picture { get; set; }

        }

        public class Link
        {
            public object From { get; set; }
            public object To { get; set; }
        }

    }
}