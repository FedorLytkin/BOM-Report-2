using DevExpress.Diagram.Core;
using DevExpress.XtraDiagram;
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

namespace VSNRM_Kompas.Diagramm.ControlClass
{
    class DiagrammForm_ControllClass
    {
        private DiagramDataBindingController diagramDataBindingController1;
        TreeList treeView;
        ClassStructureGenerator classStructureGenerator;
        bool Create_Dublicate = false;
        public DiagrammForm_ControllClass(TreeList GetTreeView, DiagramDataBindingController GetDiagramDataBindingController)
        {
            treeView = GetTreeView;
            classStructureGenerator = new ClassStructureGenerator(treeView);
            diagramDataBindingController1 = GetDiagramDataBindingController;
            this.diagramDataBindingController1.GenerateItem += new System.EventHandler<DevExpress.XtraDiagram.DiagramGenerateItemEventArgs>(this.diagramDataBindingController1_GenerateItem);
            this.diagramDataBindingController1.GenerateConnector += new System.EventHandler<DevExpress.XtraDiagram.DiagramGenerateConnectorEventArgs>(this.diagramDataBindingController1_GenerateConnector);
            this.diagramDataBindingController1.UpdateConnector += new System.EventHandler<DevExpress.XtraDiagram.DiagramUpdateConnectorEventArgs>(this.diagramDataBindingController1_UpdateConnector);

            diagramDataBindingController1.DataSource = classStructureGenerator.ClassList(Create_Dublicate);
            //if (!Create_Dublicate) diagramDataBindingController1.DataSource = classStructureGenerator.ClassList(bt_Dublicate.Down); 
            //else diagramDataBindingController1.DataSource = classStructureGenerator.ClassList_Create_Dublicate(); 
            diagramDataBindingController1.ConnectorsSource = classStructureGenerator.ConnectionList(Create_Dublicate);
            diagramDataBindingController1.LayoutKind = DiagramLayoutKind.MindMapTree;
        }

        private void diagramDataBindingController1_GenerateConnector(object sender, DevExpress.XtraDiagram.DiagramGenerateConnectorEventArgs e)
        {
            switch (((ClassData)e.To).Type)
            {
                case ClassType.Assembly:
                    e.Connector = e.CreateConnectorFromTemplate("InterfaceConnector_Sborka");
                    break;
                case ClassType.Part:
                    e.Connector = e.CreateConnectorFromTemplate("InterfaceConnector_Detal");
                    break;
                case ClassType.Body:
                    e.Connector = e.CreateConnectorFromTemplate("InterfaceConnector_Material");
                    break;
                case ClassType.Standart:
                    e.Connector = e.CreateConnectorFromTemplate("InterfaceConnector_standart");
                    break;
                case ClassType.prochee:
                    e.Connector = e.CreateConnectorFromTemplate("InterfaceConnector_Prochee");
                    break;
                case ClassType.material:
                    e.Connector = e.CreateConnectorFromTemplate("InterfaceConnector_Material");
                    break;
                default:
                    e.Connector = e.CreateConnectorFromTemplate("InterfaceConnector_Other");
                    break;
            }
            //    if (((ClassData)e.From).Type == ClassType.Interface || ((ClassData)e.To).Type == ClassType.Interface)
            //e.Connector = e.CreateConnectorFromTemplate("InterfaceConnector");
            //else e.Connector = e.CreateConnectorFromTemplate("ClassConnector");
        }

        private void diagramDataBindingController1_GenerateItem(object sender, DevExpress.XtraDiagram.DiagramGenerateItemEventArgs e)
        {
            switch (((ClassData)e.DataObject).Type)
            {
                case ClassType.Assembly:
                    e.Item = e.CreateItemFromTemplate("Сборочные единицы");
                    //e.Item = e.CreateItemFromTemplate("ItemTemplate_Sborka");
                    break;
                case ClassType.Part:
                    e.Item = e.CreateItemFromTemplate("Детали");
                    //e.Item = e.CreateItemFromTemplate("ItemTemplate_Detal");
                    break;
                case ClassType.Body:
                    e.Item = e.CreateItemFromTemplate("Материалы");
                    //e.Item = e.CreateItemFromTemplate("ItemTemplate_Material");
                    break;
                case ClassType.Standart:
                    e.Item = e.CreateItemFromTemplate("Стандартные изделия");
                    //e.Item = e.CreateItemFromTemplate("ItemTemplate_standart");
                    break;
                case ClassType.prochee:
                    e.Item = e.CreateItemFromTemplate("Прочие изделия");
                    //e.Item = e.CreateItemFromTemplate("ItemTemplate_Prochee");
                    break;
                case ClassType.material:
                    e.Item = e.CreateItemFromTemplate("Материалы");
                    //e.Item = e.CreateItemFromTemplate("ItemTemplate_Material");
                    break;
                default:
                    e.Item = e.CreateItemFromTemplate("Другие");
                    //e.Item = e.CreateItemFromTemplate("ItemTemplate_Other");
                    break;
            }
        }

        private void diagramDataBindingController1_UpdateConnector(object sender, DevExpress.XtraDiagram.DiagramUpdateConnectorEventArgs e)
        {
            e.Connector.Bindings.Add(new DevExpress.Diagram.Core.DiagramBinding("Content", "ConnectorText"));
        }

    }
}
