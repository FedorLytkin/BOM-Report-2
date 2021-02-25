﻿using DevExpress.Diagram.Core;
using DevExpress.XtraTreeList;
using DiagramDataControllerBehavior.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using VSNRM_Kompas;

namespace DiagramDataControllerBehavior
{
    public partial class DiagrammForm2 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        TreeList treeView;
        public DiagrammForm2()
        {
            InitializeComponent();
            treeView = ((MainForm)System.Windows.Forms.Application.OpenForms["MainForm"]).treeList1;
            ClassStructureGenerator classStructureGenerator = new ClassStructureGenerator(treeView);

            diagramDataBindingController1.DataSource = classStructureGenerator.ClassList();
            diagramDataBindingController1.ConnectorsSource = classStructureGenerator.ConnectionList();
            diagramDataBindingController1.LayoutKind = DiagramLayoutKind.MindMapTree;
        }

        private void diagramDataBindingController1_GenerateConnector(object sender, DevExpress.XtraDiagram.DiagramGenerateConnectorEventArgs e)
        {
        //    if (((ClassData)e.From).Type == ClassType.Interface || ((ClassData)e.To).Type == ClassType.Interface)
                e.Connector = e.CreateConnectorFromTemplate("InterfaceConnector");
            //else e.Connector = e.CreateConnectorFromTemplate("ClassConnector");
        }

        private void diagramDataBindingController1_GenerateItem(object sender, DevExpress.XtraDiagram.DiagramGenerateItemEventArgs e)
        {
            switch(((ClassData)e.DataObject).Type)
            {
                case ClassType.Assembly:
                    e.Item = e.CreateItemFromTemplate("ItemTemplate_Sborka");
                    break;
                case ClassType.Part:
                    e.Item = e.CreateItemFromTemplate("ItemTemplate_Detal");
                    break;
                case ClassType.Body:
                    e.Item = e.CreateItemFromTemplate("ItemTemplate_Material");
                    break;
                case ClassType.Standart:
                    e.Item = e.CreateItemFromTemplate("ItemTemplate_standart");
                    break;
                case ClassType.prochee:
                    e.Item = e.CreateItemFromTemplate("ItemTemplate_Prochee");
                    break;
                case ClassType.material:
                    e.Item = e.CreateItemFromTemplate("ItemTemplate_Material");
                    break;
                default:
                    e.Item = e.CreateItemFromTemplate("ItemTemplate_Other");
                    break;
            }
        }

        private void diagramDataBindingController1_UpdateConnector(object sender, DevExpress.XtraDiagram.DiagramUpdateConnectorEventArgs e) 
        {
            e.Connector.Bindings.Add(new DevExpress.Diagram.Core.DiagramBinding("Content", "ConnectorText"));
        }

        private void Bt_SaveAs_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var viewmodel = (ViewModel)diagramDataBindingController1.DataSource;

            var office = new Office();
            office.Elements = viewmodel.Elements;
            office.Connections = viewmodel.Connections;

            using (FileStream fileStream = new FileStream("classes.xml", FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Office));
                serializer.Serialize(fileStream, office);
            }
        }

        private void bt_Print_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            diagramControl1.Print();
        }

        private void bt_Export_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "BMP files (*.BMP)|*.BMP|GIF files (*.GIF)|*.GIF|PNG files (*.PNG)|*.PNG|JPEG files (*.JPEG)|*.JPEG";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                diagramControl1.ExportDiagram(saveFileDialog.FileName); 

                if (MessageBox.Show("Файл успешно создан! \nОткрыть?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    Process.Start(saveFileDialog.FileName);
                } 
            }
        }
    }
}