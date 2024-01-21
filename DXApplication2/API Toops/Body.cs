using Kompas6API5;
using Kompas6Constants;
using Kompas6Constants3D;
using KompasAPI7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Globalization;
using Kompas6API3D5COM;
using Kompas6Constants3D;
using VSNRM_Kompas;
using VSNRM_Kompas.API_Toops;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using VSNRM_Kompas.Options;
using DevExpress.XtraSplashScreen;
using VSNRM_Kompas.Dump;
using VSNRM_Kompas.Options.Column_Options;
using Microsoft.WindowsAPICodePack.Shell;
using System.IO.Compression;
using DevExpress.XtraTreeList.Columns;

namespace SaveDXF
{
    public class Body
    {
        public static string KompasVersion;
        public static string KompasVersionFlag= "КОМПАС-3D v19";
        //public static bool AppVersNOTValid; /*проверяет валидность данной версии компаса с версией КОМПАС-3D v18.1*/
        public static bool AppVersNOTValidStrong; /*проверяет валидность данной версии компаса с версией КОМПАС-3D v18.1*/ 
        public static KompasObject _kompasObject;
        public static IApplication _IApplication;
        public List<string> FindParam_Model;
        public List<object> FindModel_List;
        public List<string> OpenDocsPERED_Start;
        public bool All_Level_Search = false;
        public bool OnlySheetMetalls = false;
        public static bool thisFirstMessage = false;
        CFG_Class optionClassInBody;
        Option_Class IOption_Class;
        TreeList treeView;
        SplashScreenManager waitMng;

        public static void Init()
        {
            string progID = "KOMPAS.Application.5";
            try
            {
                object COMObject = Marshal.GetActiveObject(progID);
                if (COMObject != null)
                {
                    _kompasObject = (KompasObject)COMObject;
                }
            }
            catch (COMException e)
            {

            }
            if (_kompasObject == null)
            {
                Type t = Type.GetTypeFromProgID(progID);
                _kompasObject = (KompasObject)Activator.CreateInstance(t);
                _kompasObject.Visible = true;
            }
            if (_kompasObject != null)
            {
                _IApplication = _kompasObject.ksGetApplication7();
            }
            KompasVersion = _IApplication.ApplicationName[true];
            KompasVersionFlag = $"КОМПАС-3D v{getCadVersionFlag()}";
            AppVersNOTValidStrong = Convert.ToBoolean(string.Compare(KompasVersion, KompasVersionFlag));
            AppVersNOTValidStrong = System.Text.RegularExpressions.Regex.IsMatch(KompasVersion, KompasVersionFlag);
        }
        public static bool AppVersNOTValidStrongMessage()
        {
            if (!AppVersNOTValidStrong)
                MessageBox.Show($"Версия CAD-системы({KompasVersion}) не совпадает с рекомендованной версией {KompasVersionFlag}." +
                                Environment.NewLine +  $"Обновите {System.Windows.Forms.Application.ProductName} до требуемой версии CAD системы, либо установите {KompasVersionFlag}", 
                                System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return AppVersNOTValidStrong;
        }
        static string getCadVersionFlag()
        {
            string[] appvers = System.Windows.Forms.Application.ProductVersion.Split('.');
            return $"{appvers[0]}";
        }
        public void OpenDocument(string FullFileName)
        {
            //процедура открывает документ из списка
            if (!File.Exists(FullFileName)) return;
            IKompasDocument _IKompasDocument = (IKompasDocument)_IApplication.Documents.Open(FullFileName, true, false);
            _IKompasDocument.Active = true;
        }
        public void OpenDocumentParam_API7()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "a3d files (*.a3d)|*.a3d|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK) 
                OpenThisDocument(openFileDialog.FileName);  
        }
        public void UpDateTreeList()
        {
            TreeList treeView = ((MainForm)System.Windows.Forms.Application.OpenForms["MainForm"]).treeList1; 
            if (treeView.Nodes.Count == 0) return;
            TreeListNode node = treeView.Nodes[0];
            ComponentInfo component = (ComponentInfo)node.Tag;
            string FFN = component.FFN;
            OpenThisDocument(FFN);
        }
        public void OpenThisDocument()
        {
            if (!AppVersNOTValidStrongMessage()) return;
            if ((_IApplication.ActiveDocument is IKompasDocument3D) != true)
                return;
            IKompasDocument3D _IKompasDocument = (IKompasDocument3D)_IApplication.ActiveDocument;
            IPart7 TopPart = _IKompasDocument.TopPart;
            if (TopPart == null) { IPart7NothingMsg(_IKompasDocument.PathName); return; }

            CheckMainControl();
            treeView.Nodes.Clear(); 

            TreeListNode node = treeView.Nodes.Add();
            if (node.Tag == null)
            {
                node.Tag = GetParam(TopPart);
                if(IOption_Class.Add_Drw) AddDrwNode(node, (ComponentInfo)node.Tag);
                AddCellsInNode(node, (ComponentInfo)node.Tag);
            }
            Recource(TopPart, node);
            NodeExpand(node);
            Positio_CalcBR_Class.PositioSet(treeView.Nodes, IOption_Class.Positio_CalcBR_Value, IOption_Class.Positio_On_Value, null, IOption_Class.Positio_Split_Value);
            CloseDocs();
        }
        private void NodeExpand(TreeListNode node)
        {
            switch ((Option_Class.TreeStatus_Enum)IOption_Class.TreeStatus_Value)
            {
                case Option_Class.TreeStatus_Enum.treeStatus_Expand:
                    node.Expand();
                    break;
                case Option_Class.TreeStatus_Enum.treeStatus_ExpandAll:
                    node.ExpandAll();
                    break;
                case Option_Class.TreeStatus_Enum.treeStatus_None:
                    node.Collapse();
                    break;
            }
        }
        private void PositioSet(TreeListNode node)
        {
            if (IOption_Class.Positio_CalcBR_Value)
            {

            }
        }
        private void CheckMainControl()
        {
            waitMng = ((MainForm)System.Windows.Forms.Application.OpenForms["MainForm"]).splashScreenManager2;
            treeView = ((MainForm)System.Windows.Forms.Application.OpenForms["MainForm"]).treeList1;
            FindParam_Model = ColumnsConf_Save_Read.FindParams();   //получаем список искомых параметров
            FindModel_List = new List<object>();                    //обнуляем список обработанных файлов
            optionClassInBody = ((MainForm)System.Windows.Forms.Application.OpenForms["MainForm"]).Main_Options;
            IOption_Class = ((MainForm)System.Windows.Forms.Application.OpenForms["MainForm"]).option_Class;
            OpenDocsPERED_Start = GetInvisibleDocument();
            if (MainForm.thisDemo)
            {
                Lock_Column_Class lock_Column_ = new Lock_Column_Class();
                foreach (Column_Class column_ in lock_Column_.Lock_Column)
                    FindParam_Model.Remove(column_.Caption);
            }
        }
        public void OpenThisDocument(string FFN)
        {
            if (!AppVersNOTValidStrongMessage()) return;

            CheckMainControl();
            treeView.Nodes.Clear();
            try
            {
                TreeListNode node;
                IKompasDocument3D _IKompasDocument = (IKompasDocument3D)_IApplication.Documents.Open(FFN, false, true);
                IPart7 TopPart = _IKompasDocument.TopPart;
                if(TopPart == null) { IPart7NothingMsg(FFN); return; }
                int currentEmbody = 0;
                IEmbodimentsManager _IEmbodimentsManager = (IEmbodimentsManager)TopPart;
                int EmbodyCount = _IEmbodimentsManager.EmbodimentCount;
                for (int j = 0; j < EmbodyCount; j++)
                {
                    Embodiment tmp_Embodiment; 
                    tmp_Embodiment = _IEmbodimentsManager.Embodiment[j];
                    if (tmp_Embodiment.IsCurrent == true) { currentEmbody = j; }
                    tmp_Embodiment.IsCurrent = true;
                    node = treeView.Nodes.Add();
                    if (node.Tag == null)
                    {
                        if (tmp_Embodiment.Part == null) { IPart7NothingMsg(FFN); return; }
                        node.Tag = GetParam(tmp_Embodiment.Part);
                        if (IOption_Class.Add_Drw) AddDrwNode(node, (ComponentInfo)node.Tag);
                        AddCellsInNode(node, (ComponentInfo)node.Tag); 
                    }
                    Recource(tmp_Embodiment.Part, node);
                    NodeExpand(node);
                }
                _IEmbodimentsManager.Embodiment[currentEmbody].IsCurrent = true;

                //_IKompasDocument.Close(DocumentCloseOptions.kdDoNotSaveChanges);
                CloseDocs();
            }
            catch { }
        }
        private void IPart7NothingMsg(string ComponentName)
        {
            ShowMsgBox($@"Ошибка при чтении файла {Path.GetFileName(ComponentName)}. Поврежденный файл!", MessageBoxIcon.Error);
        }
        private ComponentInfo GetExistNode_By_ComponentKey(string ComponentKey)
        {
            foreach (ComponentInfo component in FindModel_List) 
            {
                if (component.Key == ComponentKey) return component;
            }
            return null;
        }
        private void AddWaitStatus(string Text)
        {
            waitMng.SetWaitFormDescription(Text);
        }
        private ComponentInfo GetComponentInfoInWorkList(string componentKey, List<ComponentInfo> WorkList)
        {
            foreach(ComponentInfo component in WorkList)
            {
                if (component.Key == componentKey)
                    return component;
            }
            return null;
        }
        public void Recource(IPart7 TopPart, TreeListNode node)
        //процедура перебирает компоненты в сборке
        { 
            if (TopPart != null)
            {
                if(!TopPart.Detail) getBodyResoure(TopPart, (ComponentInfo)node.Tag, node);
                var Parts = TopPart.PartsEx[1];
                if (Parts != null)
                {
                    //List<string> PartList = GetPartList(TopPart);
                    List<ComponentInfo> WorkList = new List<ComponentInfo>();
                    foreach (IPart7 item in Parts)
                    {
                        try
                        {
                            bool addComponent = false;
                            if (item.CreateSpcObjects) addComponent = true;
                            else
                            {
                                if (IOption_Class.Add_NotSPCreateObject) addComponent = true;
                            }
                            if (addComponent)
                            {
                                string itemKey = null;
                                itemKey = GetComponentKey(item);
                                ComponentInfo componentInfo = GetComponentInfoInWorkList(itemKey, WorkList);
                                if (componentInfo == null) componentInfo = GetParam(item);
                                WorkList.Add(componentInfo);
                                AddWaitStatus(File.Exists(componentInfo.FFN) ? Path.GetFileNameWithoutExtension(componentInfo.FFN) : componentInfo.FFN);
                                if (!componentInfo.QNT_False)
                                    componentInfo.QNT = Convert.ToDouble(TopPart.InstanceCount[(Part7)item]);
                                //if (!componentInfo.QNT_False)
                                //    componentInfo.QNT = GetQNTIn_PartsList(itemKey, PartList);
                                try { componentInfo.ParamValueList["Количество"] = componentInfo.QNT.ToString(); } catch { }
                                
                                TreeListNode TempNode;
                                TempNode = AddNode(node, componentInfo, false);
                                TempNode.Tag = componentInfo;
                                //getBodyResoure(item, componentInfo, TempNode);
                                if (!item.Detail && All_Level_Search) 
                                {
                                    if(!componentInfo.isPurchated || IOption_Class.AddTreeListForStandartKomponent)
                                        Recource(item, TempNode);
                                }
                            }
                        }
                        catch (Exception Ex)
                        {
                            ShowMsgBox("Ошибка при обработке компонента " + item.FileName + Environment.NewLine + Ex.Message, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
        private static void ShowMsgBox(string MsgText, MessageBoxIcon icon)
        {
            MessageBox.Show(MsgText, System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, icon);
        }
        private ComponentInfo Add_BodyInfo_In_Component(IPart7 Part, IBody7 _body, ComponentInfo componentInfo)
        {
            ComponentInfo componentInfo_Copy = (ComponentInfo)componentInfo.Clone();
            componentInfo_Copy.Body = GetParamBody(_body);
            double kolvo = Math.Floor(Convert.ToDouble(GetPropertyBodyIPart7(Part, _body, "Количество")));
            if (kolvo == 0)
            {
                componentInfo_Copy.Body.QNT = 1;
                componentInfo_Copy.Body.QNT_False = true;
            }
            else
            {
                componentInfo_Copy.Body.QNT = kolvo;
                componentInfo_Copy.Body.QNT_False = false;
            }
            componentInfo_Copy.isBody = true;
            componentInfo_Copy.Key = GetComponentKey(Part, _body);
            Dictionary<string, string> ParamValueList = new Dictionary<string, string>();
            foreach (string ParamName in FindParam_Model)
            {
                string ParamValue = null;
                ParamValue = OptionsFold.tools_class.FixInvalidChars_St(GetPropertyBodyIPart7(Part, _body, ParamName), "");
                if (IOption_Class.Split_Naim && ParamName == "Наименование") { ParamValue = OptionsFold.tools_class.SplitString(ParamValue); componentInfo_Copy.Body.Naim = ParamValue; }
                if(ParamName == "Габарит")
                {
                    double X1 = 0, Y1 = 0, Z1 = 0, X2 = 0, Y2 = 0, Z2 = 0;
                    _body.GetGabarit(out X1, out Y1, out Z1, out X2, out Y2, out Z2);
                    ParamValue = Convert.ToString(Math.Round(Math.Abs(X1 - X2), 3) + "x" + Math.Round(Math.Abs(Y1 - Y2), 3) + "x" + Math.Round(Math.Abs(Z1 - Z2), 3));
                }
                ParamValueList.Add(ParamName, ParamValue);
            }
            componentInfo_Copy.Body.ParamValueList = ParamValueList;
            return componentInfo_Copy;
        }
        private void getBodyResoure(IPart7 Part, ComponentInfo componentInfo, TreeListNode node)
        {
            try
            {
                Part = ((IKompasDocument3D)_IApplication.Documents[Part.FileName]).TopPart;
            }
            catch { }
            
            IFeature7 feature = (IFeature7)Part;
            var RB = feature.ResultBodies;
            if (RB != null) 
                try
                {
                    foreach (IBody7 _body in RB)
                    {
                        try
                        {
                            if (_body.CreateSpcObjects)
                            {
                                ComponentInfo componentInfo_Copy = Add_BodyInfo_In_Component(Part, _body, componentInfo);

                                TreeListNode TempNode;
                                TempNode = AddNode(node, componentInfo_Copy, true);
                            }
                        }
                        catch { }
                    }
                }
                catch
                {
                    IBody7 _body = (IBody7)RB;
                    ComponentInfo componentInfo_Copy = Add_BodyInfo_In_Component(Part, _body, componentInfo);

                    TreeListNode TempNode;
                    TempNode = AddNode(node, componentInfo_Copy, true);
                } 
            //if (!IsOpen)
            //    kompasDocument.Close(DocumentCloseOptions.kdDoNotSaveChanges);
        }
        private double GetQNTIn_PartsList(string Find_Item_Key, List<string> PartList)
        {
            double QNT = 0;
            foreach (string item in PartList.Distinct())
            {
                if (item == Find_Item_Key) return PartList.Where(x => x == item).Count();
            }
            return QNT;
        }
        private List<string> GetPartList(IPart7 Parent_Assembly)
        {
            List<string> PartList = new List<string>();
            var Parts = Parent_Assembly.Parts;
            foreach (IPart7 item in Parts)
            {
                try
                {
                    bool addComponent = false;
                    if (item.CreateSpcObjects) addComponent = true;
                    else
                    {
                        if (IOption_Class.Add_NotSPCreateObject) addComponent = true;
                    }
                    if (addComponent)
                    {
                        PartList.Add(GetComponentKey(item));
                    }
                        
                }
                catch { }
            }
            return PartList;
        }
        private void AddCellsInNode(TreeListNode Node, ComponentInfo.Drw_Info_Class Drw_componentInfo)
        {

            Dictionary<string, string> ParValues = Drw_componentInfo.ParamValueList;
            foreach (string FieldName in FindParam_Model)
            {
                Node.SetValue(FieldName, ParValues[FieldName]);
            }
            //Node.SetValue("Имя файла", Path.GetFileNameWithoutExtension(Drw_componentInfo.FFN));
            Node.SetValue("Миниатюра", Drw_componentInfo.Slide);
            switch (Path.GetExtension(Drw_componentInfo.FFN).ToUpper())
            {
                case ".CDW":
                    Node.ImageIndex = (int)Option_Class.Obj_Type_Enum.Drawing;
                    Node.SelectImageIndex = (int)Option_Class.Obj_Type_Enum.Drawing;
                    Node.StateImageIndex = (int)Option_Class.Obj_Type_Enum.Drawing;
                    break;
                case ".SPW":
                    Node.ImageIndex = (int)Option_Class.Obj_Type_Enum.Specification;
                    Node.SelectImageIndex = (int)Option_Class.Obj_Type_Enum.Specification;
                    Node.StateImageIndex = (int)Option_Class.Obj_Type_Enum.Specification;
                    break;
            }
        }
        private void AddCellsInNode(TreeListNode Node, ComponentInfo componentInfo)
        {
            Dictionary<string, string> ParValues = componentInfo.ParamValueList;
            foreach(string FieldName in FindParam_Model)
            {
                switch (FieldName)
                {
                    case "Миниатюра":
                        Node.SetValue(FieldName, componentInfo.Slide);
                        break;
                    case "Локальная деталь":
                        Node.SetValue(FieldName, componentInfo.isLocal);
                        break;
                    default:
                        Node.SetValue(FieldName, ParValues[FieldName]);
                        break;
                }
            }
            if (componentInfo.standardComponent)
                SetNodeImageIndex(Node, Option_Class.Obj_Type_Enum.Standart);
            else
                SetNodeImageIndex(Node, Option_Class.Obj_Type_Enum.Assembly);
            string RazdelSP = "";

            if (ParValues.ContainsKey("Раздел спецификации")) RazdelSP = ParValues["Раздел спецификации"];

            if (!string.IsNullOrEmpty(RazdelSP)) SetNodeImageIndex_By_Section_Name(RazdelSP, Node);

            if (componentInfo.isDetal && !componentInfo.standardComponent)
                SetNodeImageIndex(Node, Option_Class.Obj_Type_Enum.Part);
            if (componentInfo.SheeMetall)
            {
                if (componentInfo.HaveUnfold)
                    SetNodeImageIndex(Node, Option_Class.Obj_Type_Enum.Part_With_Unfold);
                else
                    SetNodeImageIndex(Node, Option_Class.Obj_Type_Enum.Part_NOT_Unfold);
            }
            if (treeView.Columns["Тип объекта"] != null) Node.SetValue("Тип объекта", Node.ImageIndex);
            if (treeView.Columns["Миниатюра(base64)"] != null) Node.SetValue("Миниатюра(base64)", componentInfo.SlideBase64);
            if (IOption_Class.Positio_On_Value && !IOption_Class.Positio_CalcBR_Value)
            {
                TreeListColumn Positio = treeView.Columns["Позиция"];
                if (Positio != null)
                {
                    string ThisPosition = Convert.ToString(Node.GetValue("Позиция"));
                    string ParentPosition = Convert.ToString(GetParentPositio(Node));
                    Node.SetValue("Позиция", string.IsNullOrEmpty(ParentPosition) ? ThisPosition : ParentPosition + IOption_Class.Positio_Split_Value + ThisPosition);

                    //if (Node.Level == 1 || Node.Level == 0)
                    //    Node.SetValue("Позиция", ThisPosition);
                    //else
                    //    Node.SetValue("Позиция", ParentPosition + IOption_Class.Positio_Split_Value + ThisPosition);
                }
            }
        }
        private void SetNodeImageIndex(TreeListNode Node, Option_Class.Obj_Type_Enum obj_Type)
        {
            Node.ImageIndex = (int)obj_Type;
            Node.SelectImageIndex = (int)obj_Type;
            Node.StateImageIndex = (int)obj_Type;
        }
        private string GetParentPositio(TreeListNode Node)
        {
            TreeListNode ParentNode = Node.ParentNode;
            if (ParentNode == null) return "";
            return Convert.ToString(ParentNode.GetValue("Позиция"));
        }
        private void AddCellsInNode(TreeListNode Node, ComponentInfo.Get_Body _Body)
        {
            Dictionary<string, string> ParValues = _Body.ParamValueList;
            foreach (string FieldName in FindParam_Model)
            {
                Node.SetValue(FieldName, ParValues[FieldName]);
            }
            SetNodeImageIndex(Node, Option_Class.Obj_Type_Enum.Material);

            if(ParValues.ContainsKey("Раздел спецификации")) SetNodeImageIndex_By_Section_Name(ParValues["Раздел спецификации"], Node);

        }
        private void SetNodeImageIndex_By_Section_Name(string Section_Name, TreeListNode Node)
        {

            switch (Section_Name)
            {
                case "Детали":
                    SetNodeImageIndex(Node, Option_Class.Obj_Type_Enum.Part);
                    break;
                case "Материалы":
                    SetNodeImageIndex(Node, Option_Class.Obj_Type_Enum.Material);
                    break;
                case "Сборочные единицы":
                    SetNodeImageIndex(Node, Option_Class.Obj_Type_Enum.Assembly);
                    break;
                case "Прочие изделия":
                    SetNodeImageIndex(Node, Option_Class.Obj_Type_Enum.Pro4ee);
                    break;
                case "Стандартные изделия":
                    SetNodeImageIndex(Node, Option_Class.Obj_Type_Enum.Standart);
                    break;
                case "Комплекты":
                    SetNodeImageIndex(Node, Option_Class.Obj_Type_Enum.KompleKT);
                    break;
                case "Комплексы":
                    SetNodeImageIndex(Node, Option_Class.Obj_Type_Enum.KompleKS);
                    break;
                case "Документация":
                    SetNodeImageIndex(Node, Option_Class.Obj_Type_Enum.Document);
                    break;
            }
        }
        private double GetTotalQNT(TreeListNode Node)
        {
            double total_QNT = 0;
            ComponentInfo componentInfo = (ComponentInfo)Node.Tag;
            TreeListNode ParentNode = Node.ParentNode;
            if (ParentNode != null)
            {
                ComponentInfo Parent_componentInfo = (ComponentInfo)ParentNode.Tag;
                total_QNT = Convert.ToDouble(Node.GetValue("Количество")) * GetTotalQNT(ParentNode);
            }
            else
            {
                total_QNT = componentInfo.QNT;
            }
            return total_QNT;
        }
        private TreeListNode AddNode(TreeListNode ThisNode, ComponentInfo _componentInfo, bool AddBodyTree)
        {
            TreeListNode ChildNode = null;
            if (ThisNode != null)
            {
                foreach (TreeListNode TempNode in ThisNode.Nodes)
                {
                    ComponentInfo temp_componentInfo = (ComponentInfo)TempNode.Tag;
                    if (temp_componentInfo.Key == _componentInfo.Key)
                    {
                        //temp_componentInfo.QNT = Convert.ToDouble(TempNode.GetValue("Количество")) + 1;
                        double QNT = 0;
                        if (AddBodyTree)
                        {
                            QNT = temp_componentInfo.Body.QNT + _componentInfo.Body.QNT;
                            temp_componentInfo.Body.QNT = QNT;
                        }
                        else
                            QNT = temp_componentInfo.QNT + _componentInfo.QNT;
                        ChildNode = TempNode;
                        ChildNode.SetValue("Количество", QNT);
                        ChildNode.SetValue("Количество общ.", GetTotalQNT(ChildNode));
                        ChildNode.SetValue("Тип объекта", ChildNode.ImageIndex);
                        ChildNode.Tag = temp_componentInfo;
                        return ChildNode;
                    }
                }
            }
            ChildNode = ThisNode.Nodes.Add();

            //ComponentInfo FindModel_Item = (ComponentInfo)_componentInfo.Clone();
            //FindModel_Item.QNT = 1;
            if (!AddBodyTree)
                AddCellsInNode(ChildNode, _componentInfo);
            else
                AddCellsInNode(ChildNode, _componentInfo.Body);
            _componentInfo.Total_QNT = GetTotalQNT(ChildNode);
            ChildNode.SetValue("Количество общ.", _componentInfo.Total_QNT);
            ChildNode.Tag = _componentInfo;
            if (IOption_Class.Add_Drw) AddDrwNode(ChildNode, _componentInfo);

            return ChildNode;
        }
        private void AddDrwNode(TreeListNode ThisNode, ComponentInfo _componentInfo)
        {

            foreach (ComponentInfo.Drw_Info_Class drw_Info in _componentInfo.drw_List)
            {
                ComponentInfo Drw_Component = (ComponentInfo)_componentInfo.Clone();
                switch (Path.GetExtension(drw_Info.FFN).ToUpper())
                {
                    case ".SPW":
                        Drw_Component.HaveSP = true;
                        break;
                    case ".CDW":
                        Drw_Component.HaveDrw = true;
                        break;
                }
                Drw_Component.drw_Info = drw_Info;
                Drw_Component.Key += Drw_Component.drw_Info.FFN + "|" + _componentInfo.FFN;
                Drw_Component.FFN = Drw_Component.drw_Info.FFN;
                Drw_Component.Referense_Variable_List = null;
                FindModel_List.Add(Drw_Component);
                TreeListNode Drw_Node = ThisNode.Nodes.Add();
                AddCellsInNode(Drw_Node, drw_Info);
                Drw_Node.Tag = Drw_Component;
            }
        }
        private List<string> GetDrwDocs(IPart7 part_)
        { 
            IKompasDocument3D doci3D = (IKompasDocument3D)GetIKompasDocument(part_.FileName, false, false);
            //doci3D.Active = true;
            if (doci3D == null) return null;
            IPart7 part7 = doci3D.TopPart;
            if (part7 == null) return null;

            List<string> drwS = new List<string>();

            IProductDataManager productDataMenager = doci3D as IProductDataManager;
            if (productDataMenager == null) return null;

            dynamic arrAttachDoc = productDataMenager.ObjectAttachedDocuments[(IPropertyKeeper)part7];

            if (arrAttachDoc != null)
                foreach (var tDoc in arrAttachDoc)
                {
                    if (!string.IsNullOrEmpty(tDoc.ToString()) && File.Exists(tDoc.ToString()))
                    {
                        drwS.Add(tDoc.ToString());
                    }   
                }
                    
            return drwS;
        }
        public List<string> GetExternal(string FileName)
        {

            IKompasDocument doc = (IKompasDocument)_IApplication.Documents.Open(FileName, true, false); ;
            if (doc == null) return null;
            List<string> drwS = new List<string>(); 

            IProductDataManager productData = (IProductDataManager)doc;
            IKompasDocument3D doc3d = (IKompasDocument3D)doc;
            
            IPropertyKeeper propertyKeeper = (IPropertyKeeper)doc3d.TopPart;
            IProductDataManager ss = productData.ObjectAttachedDocuments[propertyKeeper];
            ksProductObjectTypeEnum ksProduct = ksProductObjectTypeEnum.ksPOTDocumentObject;
            productData.AddProductObject(propertyKeeper, @"C:\Users\admin_veza\Desktop\test 1\Стойка верхняя _ ЕЛГ 02.01.20.000.spw", ksProduct);
            

            ISpecificationDescriptions specification = doc.SpecificationDescriptions;
            if(specification.Count > 0)
            {
                for(int y = 0; y < specification.Count; y++)
                {
                    SpecificationDescription description = specification[y];
                    description.ShowAllObjects = true;
                    string sp = description.SpecificationDocumentName;
                    //sp = specification[y].SpecificationDocumentName;
                }
            }
            return drwS;
        }
        public void getSP()
        {
            ksDocument3D ksDocument = _kompasObject.ActiveDocument3D();
            ksSpecification ksSpecification = ksDocument.GetSpecification();
            bool state = false;
            object param = null;
            ksSpecification.ksGetSpcDescription(-1, param, out state);
        }
        public bool SetPropertyIPart7(string FileName, string PropertyName, object PropertyValue)
        {
            IKompasDocument3D _IKompasDocument3D = (IKompasDocument3D)GetIKompasDocument(FileName, false, false);
            IPart7 part_ = _IKompasDocument3D.TopPart;
            if (_IApplication != null)
            {
                IPropertyMng _IPropertyMng = (IPropertyMng)_IApplication;
                if (_IPropertyMng != null)
                {
                    int count = _IPropertyMng.PropertyCount[_IKompasDocument3D];
                    for (int i = 0; i < count; i++)
                    {
                        IProperty Property = _IPropertyMng.GetProperty(_IKompasDocument3D, i);

                        if (PropertyName == Property.Name)
                        {
                            SetValueProperty(part_, Property, PropertyValue);
                            break;
                        }
                    }
                }
            }

            return false;
        }
        public string GetPropertyIDrw(string FileName, string PropertyName)
        {
            IKompasDocument _IKompasDocumentDrw = (IKompasDocument)GetIKompasDocument(FileName, false, false);
            if (_IApplication != null)
            {
                IPropertyMng _IPropertyMng = (IPropertyMng)_IApplication;
                if (_IPropertyMng != null)
                {
                    int count = _IPropertyMng.PropertyCount[_IKompasDocumentDrw];
                    for (int i = 0; i < count; i++)
                    {
                        IProperty Property = _IPropertyMng.GetProperty(_IKompasDocumentDrw, i);
                        if (PropertyName == Property.Name)
                        {
                            object returnObject;
                            if (GetValueProperty(_IKompasDocumentDrw, Property, out returnObject))
                            {
                                if (returnObject.GetType().Name == "Double") returnObject = Math.Round(Convert.ToDouble(returnObject), 3);
                                if (!string.IsNullOrEmpty(returnObject.ToString()))
                                {
                                    return returnObject.ToString();
                                }
                            }
                            break;
                        }
                    }
                }
            }
            return null;
        }
        public string GetPropertyIPart7(IPart7 part_, string PropertyName)
        {
            if (PropertyName == "Миниатюра") return null;
            IKompasDocument3D _IKompasDocument3D = (IKompasDocument3D)GetIKompasDocument(part_.FileName, false, false);
            if (_IApplication != null)
            {
                IPropertyMng _IPropertyMng = (IPropertyMng)_IApplication;
                if (_IPropertyMng != null)
                {
                    int count = _IPropertyMng.PropertyCount[_IKompasDocument3D];
                    for (int i = 0; i < count; i++)
                    {
                        IProperty Property = _IPropertyMng.GetProperty(_IKompasDocument3D, i);

                        if (PropertyName == Property.Name)
                        {
                            object returnObject;
                            if (GetValueProperty(part_, Property, out returnObject))
                            {
                                if (returnObject.GetType().Name == "Double") returnObject = Math.Round(Convert.ToDouble(returnObject), 3);
                                if (!string.IsNullOrEmpty(returnObject.ToString()))
                                {
                                    return returnObject.ToString();
                                }
                            }
                            break;
                        }
                    }
                }
            }
            return GetVaribleValByName(part_, PropertyName);
        }
        public string GetPropertyBodyIPart7(IPart7 part_, IBody7 Body, string PropertyName)
        {
            object res = "" ;
            IKompasDocument3D _IKompasDocument3D = (IKompasDocument3D)GetIKompasDocument(part_.FileName, false, false);
            if (_IApplication != null)
            {
                IPropertyMng _IPropertyMng = (IPropertyMng)_IApplication;
                if (_IPropertyMng != null)
                {
                    bool Baseunit, FromSource;
                    Baseunit = false;
                    IPropertyKeeper propertyKeeper = (IPropertyKeeper)Body;
                    if (propertyKeeper != null)
                    {
                        IProperty Property = _IPropertyMng.GetProperty(_IKompasDocument3D, PropertyName);
                        if (Property != null)
                        {
                            propertyKeeper.GetPropertyValue((_Property)Property, out res, Baseunit, out FromSource);
                        }
                    }
                }
            }
            if (res != null) { if (res.GetType().Name == "Double") res = Math.Round(Convert.ToDouble(res), 3); } else { return null; }
            return res.ToString();
        }
        public static bool SetValueProperty(IPart7 part, IProperty Property, object Value)
        {
            bool res = false;
            if (Property != null)
            {
                IPropertyKeeper Prop = (IPropertyKeeper)part;

                if (Prop != null)
                {
                    bool Baseunit, FromSource;
                    Baseunit = false;
                    _Property classresProperty = (_Property)Property;
                    if (classresProperty != null)
                    { 
                        Prop.SetPropertyValue((_Property)classresProperty, Value, Baseunit);
                    }
                }
            }
            return res;
        }
        public static bool GetValueProperty(IKompasDocument _IKompasDocumentDrw, IProperty Property, out object returnObject)
        {
            bool res = false;
            returnObject = null;
            if (Property != null)
            {
                IPropertyKeeper Prop = _IKompasDocumentDrw as IPropertyKeeper;

                if (Prop != null)
                {
                    bool Baseunit, FromSource;
                    Baseunit = false;
                    _Property classresProperty = (_Property)Property;
                    if (classresProperty != null)
                    {
                        Prop.GetPropertyValue((_Property)classresProperty, out returnObject, Baseunit, out FromSource);
                        if (returnObject != null)
                        {
                            res = true;
                        }
                    }
                }
            }
            return res;
        }
        public static bool GetValueProperty(IPart7 part, IProperty Property, out object returnObject)
        {
            bool res = false;
            returnObject = null;
            if (Property != null)
            {
                IPropertyKeeper Prop = (IPropertyKeeper)part;

                if (Prop != null)
                {
                    bool Baseunit, FromSource;
                    Baseunit = false;
                    _Property classresProperty = (_Property)Property;
                    if (classresProperty != null)
                    {
                        Prop.GetPropertyValue((_Property)classresProperty, out returnObject, Baseunit, out FromSource);
                        if (returnObject != null)
                        {
                            res = true;
                        }
                    }
                }
            }
            return res;
        }
        public string GetVaribleValByName(IPart7 part_, string PropertyName)
        {
            string res = "";
            try
            {
                IKompasDocument3D _IKompasDocument3D = (IKompasDocument3D)GetIKompasDocument(part_.FileName, false, false);
                if (_IKompasDocument3D == null) return null;
                part_ = _IKompasDocument3D.TopPart;
                if (part_ != null)
                {
                    IFeature7 IF = (IFeature7)part_;
                    Object[] ars = null;
                    if (IF != null && IF.VariablesCount[false, true] > 0)
                    {
                        try
                        {
                            ars = IF.Variables[false, true];
                        }
                        catch { }
                    }
                    if (ars != null)
                    {
                        foreach (var v in ars)
                        {
                            string Expression = (v as IVariable7).Expression;
                            if ((v as IVariable7).Name == PropertyName)
                            {
                                res = (v as IVariable7).Value.ToString();
                                break;
                            }
                        }
                    }
                }
            }
            catch {
                ShowMsgBox($"Ошибка при обработке компонента {part_.FileName}\nНе удалось найти Переменную {PropertyName} в списке переменных!", MessageBoxIcon.Error);
            }
            return res;
        }
        private static IKompasDocument GetIKompasDocument(string FullFileName, bool Visible, bool ReadOnly)
        {
            //процедура открывает документ из списка
            IKompasDocument _IKompasDocument = (IKompasDocument)_IApplication.Documents.Open(FullFileName, Visible, ReadOnly);
            return _IKompasDocument;
        }
        private void CloseDocs()
        {
            foreach (ComponentInfo component in FindModel_List)
            {
                CloseInvisibleDocument(component.FFN);
            }
            foreach (IKompasDocument document in _IApplication.Documents)
            {
                bool find = false;
                foreach (string FileName in OpenDocsPERED_Start)
                    if (document.PathName == FileName)
                    {
                        find = true;
                        break;
                    }
                if (!find)
                    document.Close(DocumentCloseOptions.kdDoNotSaveChanges);
            }
            
        }
        public List<string> GetInvisibleDocument()
        {
            List<string> docs = new List<string>();
            for(int doc_id = 0; doc_id <_IApplication.Documents.Count; doc_id++)
            {
                IKompasDocument document = (IKompasDocument)_IApplication.Documents[doc_id];
                if(document != null) docs.Add(document.PathName);
            }
            //foreach(IKompasDocument document in _IApplication.Documents)
            //    document.Close(DocumentCloseOptions.kdDoNotSaveChanges);

            return docs;
        }
        public static void CloseInvisibleDocument(string FullFileName)
        {
            //процедура закрывает документ
            IKompasDocument _IKompasDocument = (IKompasDocument)_IApplication.Documents[FullFileName];
            if (_IKompasDocument != null && _IKompasDocument.Visible == false) _IKompasDocument.Close(DocumentCloseOptions.kdDoNotSaveChanges);
        }
        private ComponentInfo.Get_Body GetParamBody(IBody7 body)
        {
            ComponentInfo.Get_Body _Body = new ComponentInfo.Get_Body();
            _Body.Naim = body.Name;
            _Body.Oboz = body.Marking;
            return _Body;
        }
        private string GetComponentKey(IPart7 part, IBody7 body)
        {
            return part.FileName + "|" + ((string.IsNullOrEmpty(part.Marking)) ? part.Name : part.Marking) + "|" + ((string.IsNullOrEmpty(body.Marking)) ? body.Name : body.Marking);

            if (IOption_Class.Positio_On_Value)
                return part.FileName + "|" + ((string.IsNullOrEmpty(part.Marking)) ? part.Name : part.Marking) + "|" + ((string.IsNullOrEmpty(body.Marking)) ? body.Name : body.Marking) + "|" + OptionsFold.tools_class.FixInvalidChars_St(GetPropertyBodyIPart7(part, body, "Позиция"), "");
            else
                return part.FileName + "|" + ((string.IsNullOrEmpty(part.Marking)) ? part.Name : part.Marking) + "|" + ((string.IsNullOrEmpty(body.Marking)) ? body.Name : body.Marking);
        }
        private string GetComponentKey(IPart7 part)
        {
            return part.FileName + "|" + (string.IsNullOrEmpty(part.Marking) ? part.Name : part.Marking);
        }
        private ComponentInfo GetParam(IPart7 part)
        {
            string ComponentKey = GetComponentKey(part);

            //FindModel_List
            ComponentInfo iMSH = null ;// = GetExistNode_By_ComponentKey(ComponentKey);
            if (iMSH != null) return iMSH;

            IMassInertiaParam7 massInertiaParam = (IMassInertiaParam7)part;
            try
            {
                massInertiaParam.Calculate();
                massInertiaParam.LengthUnits = (ksLengthUnitsEnum)IOption_Class.Length_MU_Value; //ksLengthUnitsEnum.ksLUnMM;
                massInertiaParam.MassUnits = (ksMassUnitsEnum)IOption_Class.Mass_MU_Value;  //ksMassUnitsEnum.ksMUnKG; 
            }
            catch { }

            iMSH = new ComponentInfo();
            Dictionary<string, string> ParamValueList = new Dictionary<string, string>();
            //iMSH.ParamValueList = FindParam_Model;
            foreach (string ParamName in FindParam_Model)
            {
                string ParamValue = "";
                try
                {
                    switch (ParamName)
                    {
                        case "Обозначение":
                            ParamValue = OptionsFold.tools_class.FixInvalidChars_St(part.Marking, "");
                            break;
                        case "Имя файла":
                            if (part.FileName.IndexOf(">") < 0)
                                ParamValue = System.IO.Path.GetFileNameWithoutExtension(part.FileName);
                            break;
                        case "Путь файла":
                            ParamValue = part.FileName;
                            break;
                        case "Расположение файла":
                            if (part.FileName.IndexOf(">") < 0)
                                ParamValue = System.IO.Path.GetDirectoryName(part.FileName);
                            break;
                        case "Наименование":
                            ParamValue = OptionsFold.tools_class.FixInvalidChars_St(part.Name, "");
                            if (IOption_Class.Split_Naim) ParamValue = OptionsFold.tools_class.SplitString(ParamValue);
                            break;
                        case "Масса":
                            ParamValue = OptionsFold.tools_class.FixInvalidChars_St(Math.Round(part.Mass, 3).ToString(), "");
                            break;
                        case "Материал":
                            if (part.Detail && !part.Standard)
                                ParamValue = OptionsFold.tools_class.FixInvalidChars_St(part.Material, "");
                            else
                                if (IOption_Class.Material_In_Assemly)
                                ParamValue = (!part.Detail | part.Standard) ? OptionsFold.tools_class.FixInvalidChars_St(part.Material, "") : "";
                            break;
                        case "Толщина":
                            ParamValue = Convert.ToString(GetThicknessPart(part, true));
                            break;
                        case "Количество":
                            ParamValue = OptionsFold.tools_class.FixInvalidChars_St(GetPropertyIPart7(part, ParamName), "");
                            if (string.IsNullOrEmpty(ParamValue)) ParamValue = "1";
                            break;
                        case "Количество общ.":
                        case "Миниатюра":
                            //ParamValue = OptionsFold.tools_class.FixInvalidChars_St(GetPropertyIPart7(part, ParamName), "");
                            //if (string.IsNullOrEmpty(ParamValue)) ParamValue = "1";
                            break;
                        case "Площадь":
                            massInertiaParam.LengthUnits = (ksLengthUnitsEnum)IOption_Class.Area_MU_Value;
                            ParamValue = Math.Round(massInertiaParam.Area, 3).ToString();
                            break;
                        case "Объем":
                            massInertiaParam.LengthUnits = (ksLengthUnitsEnum)IOption_Class.Volume_MU_Value;
                            ParamValue = Math.Round(massInertiaParam.Volume, 3).ToString();
                            break;
                        case "Xc":
                            massInertiaParam.LengthUnits = (ksLengthUnitsEnum)IOption_Class.Length_MU_Value;
                            ParamValue = massInertiaParam.Xc.ToString();
                            break;
                        case "Yc":
                            massInertiaParam.LengthUnits = (ksLengthUnitsEnum)IOption_Class.Length_MU_Value;
                            ParamValue = massInertiaParam.Yc.ToString();
                            break;
                        case "Zc":
                            massInertiaParam.LengthUnits = (ksLengthUnitsEnum)IOption_Class.Length_MU_Value;
                            ParamValue = massInertiaParam.Zc.ToString();
                            break;
                        case "Полное имя файла":
                            ParamValue = part.FileName;
                            break;
                        case "Тип объекта":
                            break;
                        case "Габарит":
                            ksPart kPart = _kompasObject.TransferInterface(part, (int)Kompas6Constants.ksAPITypeEnum.ksAPI5Auto, 0);
                            double X1 = 0, Y1 = 0, Z1 = 0, X2 = 0, Y2=0, Z2 = 0;
                            if(kPart!=null) kPart.GetGabarit(true, false, out X1, out Y1, out Z1, out X2, out Y2, out Z2);
                            ParamValue = Convert.ToString(Math.Round(Math.Abs(X1 - X2), 3) + "x" + Math.Round(Math.Abs(Y1 - Y2), 3) + "x" + Math.Round(Math.Abs(Z1 - Z2), 3));
                            break;
                        default:
                            if (string.IsNullOrEmpty(ParamValue))
                                ParamValue = OptionsFold.tools_class.FixInvalidChars_St(GetPropertyIPart7(part, ParamName), "");
                            break;
                    }
                }
                catch (Exception Ex)
                {
                    ShowMsgBox("Ошибка в компоненте " + part.FileName +
                        Environment.NewLine + $"При обработки параметра {ParamName} произошла ошибка" +
                        Environment.NewLine + Ex.Message, MessageBoxIcon.Error); ;
                }
                massInertiaParam.LengthUnits = (ksLengthUnitsEnum)IOption_Class.Length_MU_Value;
                ParamValueList.Add(ParamName, ParamValue);
            }
            iMSH.ParamValueList = ParamValueList;
            string KolvoStr = GetPropertyIPart7(part, "Количество");
            double Kolvo = string.IsNullOrEmpty(KolvoStr) ? 0 : Convert.ToDouble(KolvoStr);
            if (Kolvo == 1 || Kolvo == 0)
            {
                iMSH.QNT = 1;
                iMSH.QNT_False = false;
            }
            else
            {
                iMSH.QNT = Kolvo;
                iMSH.QNT_False = true;
            }
            iMSH.Oboz = part.Marking;
            iMSH.Mass = part.Mass;
            iMSH.Naim = part.Name;
            if (IOption_Class.Split_Naim) iMSH.Naim = OptionsFold.tools_class.SplitString(part.Name);
            iMSH._MCH.Xc = massInertiaParam.Xc;
            iMSH._MCH.Yc = massInertiaParam.Yc;
            iMSH._MCH.Zc = massInertiaParam.Zc; 
            iMSH._MCH.Area = massInertiaParam.Area;
            iMSH._MCH.Volume = massInertiaParam.Volume;
            iMSH.FFN = part.FileName;
            iMSH.material = part.Material;
            iMSH.FL_Size = GetFileSize(part.FileName);
            bool isSHeetmetall = false;
            iMSH.SheeMetall = IsSheetMetal(part, out isSHeetmetall);
            iMSH.HaveUnfold = GetHasFlatPattern(part);

            iMSH.isLocal = part.IsLocal;
            iMSH.isDetal = part.Detail;
            iMSH.standardComponent = part.Standard;
            iMSH.isPurchated = GetIsPurhated(part, iMSH.ParamValueList.ContainsKey("Раздел спецификации")? iMSH.ParamValueList["Раздел спецификации"]: null);
            iMSH.Key = ComponentKey;
            //4555
            //int newWidth = 32;
            //int newHeight = newWidth;
            ShellFile shellFile = null;
            if (File.Exists(part.FileName))
            {
                shellFile = ShellFile.FromFilePath(part.FileName);
                iMSH.Slide = BitmapClass.resizeImage(shellFile.Thumbnail.LargeBitmap, IOption_Class.ModelSlideSize); // shellFile.Thumbnail.SmallBitmap;
                iMSH.LargeSlide = shellFile.Thumbnail.LargeBitmap;
                iMSH.SlideBase64 = BitmapClass.GetBase32(shellFile.Thumbnail.LargeBitmap);
            }
            //iMSH.Slide = new System.Drawing.Bitmap(iMSH.LargeSlide, new System.Drawing.Size(newWidth, newHeight));
            //iMSH.Slide.SetResolution(newWidth * 3, newWidth * 3);

            if (!iMSH.isLocal) iMSH.Referense_Variable_List = GetLinkProertiList(part.FileName);
            iMSH.CopyGeometriesList = GetCopyGeometries(part);
            List<string> DrwList = GetDrwDocs(part);
            if(DrwList != null)
            {
                iMSH.drw_List = new List<ComponentInfo.Drw_Info_Class>();
                foreach (string drw_Name in DrwList)
                {
                    iMSH.drw_List.Add(getDrwParam(drw_Name));
                }
            }

            return iMSH;
        }
        private bool GetIsPurhated(IPart7 part, string SectionName)
        {
            if (part == null) return false;
            if (part.Standard) return true;
            if (string.IsNullOrEmpty(SectionName)) return false;
            switch (SectionName)
            {
                case "Стандартные изделия":
                case "Прочие изделия":
                case "Материалы":
                    return true;
                default:
                    return false;
            }
        }
        private long GetFileSize(string FileName)
        {
            if (File.Exists(FileName))
            {
                FileInfo fileInfo = new FileInfo(FileName);
                return fileInfo.Length;
            }
            return -1;
        } 
        private List<ComponentInfo.Variable_Class> GetLinkProertiList(string PartFileName)
        {
            List<ComponentInfo.Variable_Class> linkVars = new List<ComponentInfo.Variable_Class>();
            bool OpenDoc = false;
            IKompasDocument3D document3D;
            document3D = (IKompasDocument3D)_IApplication.Documents[PartFileName];
            if (document3D != null) OpenDoc = true;
            else
                document3D = (IKompasDocument3D)_IApplication.Documents.Open(PartFileName, OpenVisible, false);
            
            if (document3D == null) return linkVars;
            string ParamName = null;
            try
            {
                IPart7 part7 = document3D.TopPart;
                string ffn = part7.FileName;
                IFeature7 feature7 = (IFeature7)part7;
                var VariableCollection = feature7.Variables[false, true];
                if (VariableCollection != null)
                {
                    try
                    {
                        foreach (Variable7 variable7 in VariableCollection)
                        {
                            ParamName = variable7.Name;
                            if (!string.IsNullOrEmpty(variable7.LinkDocumentName))
                                linkVars.Add(new ComponentInfo.Variable_Class { Name = ParamName, SourceFileName = variable7.LinkDocumentName });
                        }
                    }
                    catch { }
                }
            }
            catch (Exception Ex)
            {
                ShowMsgBox("Ошибка при изменении связанных файлов у документа" + PartFileName + Environment.NewLine + Ex.Message, MessageBoxIcon.Error);
            }
            if (!OpenDoc) document3D.Close(DocumentCloseOptions.kdSaveChanges);

            return linkVars;
        }
        private ComponentInfo.Drw_Info_Class getDrwParam(string drw_Name)
        {
            ComponentInfo.Drw_Info_Class drw_Info = new ComponentInfo.Drw_Info_Class();
            drw_Info.FFN = drw_Name;
            drw_Info.FL_Size = GetFileSize(drw_Name);
            drw_Info.Naim = OptionsFold.tools_class.FixInvalidChars_St(GetPropertyIDrw(drw_Name, "Наименование"), "");
            drw_Info.Oboz= OptionsFold.tools_class.FixInvalidChars_St(GetPropertyIDrw(drw_Name, "Обозначение"), "");
            
            ShellFile shellFile = ShellFile.FromFilePath(drw_Name);
            drw_Info.Slide = BitmapClass.resizeImage(shellFile.Thumbnail.LargeBitmap, IOption_Class.ModelSlideSize); //shellFile.Thumbnail.SmallBitmap;
            drw_Info.LargeSlide = shellFile.Thumbnail.LargeBitmap;
            drw_Info.SlideBase64 = BitmapClass.GetBase32(shellFile.Thumbnail.LargeBitmap);
            Dictionary<string, string> ParamValueList = new Dictionary<string, string>();
            //iMSH.ParamValueList = FindParam_Model;
            foreach (string ParamName in FindParam_Model)
            {
                string ParamValue = "";
                switch (ParamName)
                {
                    //case "Обозначение":
                    //    ParamValue = OptionsFold.tools_class.FixInvalidChars_St(GetPropertyIDrw(drw_Name, ParamName), "");
                    //    break;
                    case "Имя файла":
                        ParamValue = System.IO.Path.GetFileNameWithoutExtension(drw_Name);
                        break;
                    case "Путь файла":
                        ParamValue = drw_Name;
                        break;
                    case "Расположение файла":
                        ParamValue = System.IO.Path.GetDirectoryName(drw_Name);
                        break; 
                    case "Полное имя файла":
                        ParamValue = drw_Name;
                        break;
                    default:
                        ParamValue = OptionsFold.tools_class.FixInvalidChars_St(GetPropertyIDrw(drw_Name, ParamName), "");
                        break;
                }
                ParamValueList.Add(ParamName, ParamValue);
            }
            drw_Info.ParamValueList = ParamValueList;

            return drw_Info;
        }
        public static double GetThicknessPart(IPart7 Part_, bool inSource = true)
        //процедура возвращает значение толщины ЛТ
        {
            try
            {
                ISheetMetalContainer pSheetMetalContainer = (ISheetMetalContainer)Part_;
                ISheetMetalBodies pSheetMetalBodies = pSheetMetalContainer.SheetMetalBodies;
                if (pSheetMetalBodies.Count != 0)
                {
                    ISheetMetalBody pSheetMetalBody = pSheetMetalBodies.SheetMetalBody[0];
                    return pSheetMetalBody.Thickness;
                }
                else
                {
                    if (pSheetMetalContainer.SheetMetalRuledShells.Count != 0)
                    {
                        ISheetMetalBody obech = pSheetMetalContainer.SheetMetalRuledShells.SheetMetalBody[0];
                        return obech.Thickness;
                    }
                    if (AppVersNOTValidStrong) return GetThickBeVarible(Part_, true);/*если версия компаса не валидна*/
                }
            }
            catch  { } 
            return 0;
        }

        public static bool IsSheetMetal(IPart7 Part, out bool NoSheetMetal)
        //проверка на листовой материал
        {
            bool isOpen;
            NoSheetMetal = false;
            ISheetMetalContainer _ISheetMetalContainer = (ISheetMetalContainer)Part;
            if (_ISheetMetalContainer != null)
            {
                ISheetMetalBodies _ISheetMetalBodies = _ISheetMetalContainer.SheetMetalBodies;
                if (_ISheetMetalBodies != null)
                {
                    if (_ISheetMetalBodies.Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        VariableTable _VariableTable = Part.VariableTable;
                        if (_VariableTable != null)
                        {
                            if (isVaribale(Part, true))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static bool isVaribale(IPart7 part_, bool inSource = true)
        {
            bool res = false;
            if (part_ != null)
            {
                IFeature7 IF = (IFeature7)part_;
                Object[] ars = null;
                if (IF != null && IF.VariablesCount[false, inSource] > 0)
                {
                    try
                    {
                        ars = IF.Variables[false, inSource];
                    }
                    catch (Exception exc) { }
                }
                if (ars != null)
                {
                    foreach (var v in ars)
                    {
                        string Expression = (v as IVariable7).Expression;
                        if ((v as IVariable7).Name == "SM_Thickness")
                        {
                            res = true;
                            break;
                        }
                    }
                }
            }
            return res;
        }
        public static bool GetHasFlatPattern(IPart7 part)
        {
            //процедура проверяет наличие развертки для данной детали
            //проверка на существование развертки в детали
            //если есть развертка - возвращает 1, если нет - -
            //для версии компас ниже 18, выдается ошибка и возвращается 1
            if (!AppVersNOTValidStrong) return true;
            try
            {
                ISheetMetalContainer _ISheetMetalContainer = (ISheetMetalContainer)part;
                ISheetMetalBendUnfoldParameters _SheetMetalBendUnfoldParameters = _ISheetMetalContainer.SheetMetalBendUnfoldParameters;
                if (_SheetMetalBendUnfoldParameters != null)
                {
                    if (_SheetMetalBendUnfoldParameters.IsCreated == false)
                        return false;
                    else
                        return true;
                }
                else
                    return false;
            }
            catch (Exception Ex)
            {
                if (thisFirstMessage == true)
                {
                    ShowMsgBox("Ошибка при проверке наличия развертки!" + Environment.NewLine +
                                   "Возможно вы используете версия Kompas ниже " + Body.KompasVersion + Environment.NewLine +
                                   "Во избежание ошибок при создании разверток, рекомендуем обновиться до версии" + Body.KompasVersion + Environment.NewLine +
                                   Ex.Message, MessageBoxIcon.Error);
                    thisFirstMessage = false;
                }
                return true;
            }
        }
        public static double GetThickBeVarible(IPart7 part_, bool inSource = true)
        {
            double res = 0;
            if (part_ != null)
            {
                IFeature7 IF = (IFeature7)part_;
                Object[] ars = null;
                if (IF != null && IF.VariablesCount[false, inSource] > 0)
                {
                    ars = IF.Variables[false, inSource];
                }
                if (ars != null)
                {
                    foreach (var v in ars)
                    {
                        string Expression = (v as IVariable7).Expression;
                        if ((v as IVariable7).Name == "SM_Thickness")
                        {
                            res = Convert.ToDouble((v as IVariable7).Value);
                            break;
                        }
                    }

                }
            }
            return res;
        }
        #region "Транслировать параметры" 
        public void TransProp_St1_V2()
        {
            if (!AppVersNOTValidStrongMessage()) return;
            if ((_IApplication.ActiveDocument is IKompasDocument3D) != true)
                return;
            IKompasDocument3D _IKompasDocument = (IKompasDocument3D)_IApplication.ActiveDocument;
            if (_IKompasDocument == null) return;
            CheckMainControl();
            IPart7 TopPart = _IKompasDocument.TopPart;
            if (TopPart == null) { IPart7NothingMsg(_IKompasDocument.Path); return; }

            GetAllPartsClass getAllParts = new GetAllPartsClass();
            List<IPart7> partList = getAllParts.GetParts(TopPart);

            foreach(IPart7 part in partList) 
                TransProp_AddProp(part, _IKompasDocument);
            CloseDocs();
        }

        public void TransProp_St1_old()
        {
            if (!AppVersNOTValidStrongMessage()) return;
            if ((_IApplication.ActiveDocument is IKompasDocument3D) != true)
                return;
            IKompasDocument3D _IKompasDocument = (IKompasDocument3D)_IApplication.ActiveDocument;
            if (_IKompasDocument == null) return;
            CheckMainControl();
            IPart7 TopPart = _IKompasDocument.TopPart;
            if (TopPart == null) { IPart7NothingMsg(_IKompasDocument.Path); return; }

            TransProp_AddProp(TopPart, _IKompasDocument);
            TransPropTravelByAssemly2(TopPart, _IKompasDocument); 
            CloseDocs();
        }
        
        private void TransPropTravelByAssemly2(IPart7 TopPart, IKompasDocument3D _IKompasDocument)
        //процедура перебирает компоненты в сборке
        {
            if (TopPart != null)
            {
                var Parts = TopPart.PartsEx[0];
                if (Parts != null)
                {
                    foreach (IPart7 item in Parts)
                    {
                        try
                        {
                            TransProp_AddProp(item, _IKompasDocument); 
                            AddWaitStatus(File.Exists(item.FileName) ? Path.GetFileNameWithoutExtension(item.FileName): item.FileName);
                            if (!item.Detail)
                                TransPropTravelByAssemly2(item, _IKompasDocument);
                        }
                        catch (Exception Ex)
                        {
                            ShowMsgBox("Ошибка при обработке компонента " + item.FileName + Environment.NewLine + Ex.Message, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
        public string TransProp_SetPropertyBodyIPart7(IPart7 part_, IBody7 Body, string PropertyName)
        {
            object res = "";
            IKompasDocument3D _IKompasDocument3D = (IKompasDocument3D)GetIKompasDocument(part_.FileName, false, false);
            if (_IApplication != null)
            {
                IPropertyMng _IPropertyMng = (IPropertyMng)_IApplication;
                if (_IPropertyMng != null)
                {
                    bool Baseunit, FromSource;
                    Baseunit = false;
                    IPropertyKeeper propertyKeeper = (IPropertyKeeper)Body;
                    if (propertyKeeper != null)
                    {
                        IProperty Property = _IPropertyMng.GetProperty(_IKompasDocument3D, PropertyName);
                        if (Property != null)
                        {
                            propertyKeeper.GetPropertyValue((_Property)Property, out res, Baseunit, out FromSource);
                        }
                    }
                }
            }
            if (res != null) { if (res.GetType().Name == "Double") res = Math.Round(Convert.ToDouble(res), 3); } else { return null; }
            return res.ToString();
        }
        private void TransProp_AddProp(IPart7 part_, IKompasDocument3D Parent_IKompasDocument)
        {
            AddWaitStatus(File.Exists(part_.FileName) ? Path.GetFileNameWithoutExtension(part_.FileName) : part_.FileName);
            IKompasDocument3D This_IKompasDocument3D = (IKompasDocument3D)GetIKompasDocument(part_.FileName, false, false); 
            if (This_IKompasDocument3D == Parent_IKompasDocument || This_IKompasDocument3D == null) return;
            IPart7 part_thisDetal = This_IKompasDocument3D.TopPart;
            if (_IApplication != null)
            {
                IPropertyMng _IPropertyMng = (IPropertyMng)_IApplication;
                if (_IPropertyMng != null)
                {
                    int count = _IPropertyMng.PropertyCount[Parent_IKompasDocument];
                    for (int i = 0; i < count; i++)
                    {
                        bool find_Prop = false;
                        IProperty Property = _IPropertyMng.GetProperty(Parent_IKompasDocument, i);
                        object returnObject;
                        GetValueProperty(part_, Property, out returnObject);
                        if (!string.IsNullOrEmpty(Convert.ToString(returnObject)))
                        {
                            int count_thisPart = _IPropertyMng.PropertyCount[This_IKompasDocument3D];
                            for (int y = 0; y < count_thisPart; y++)
                            {
                                IProperty Property_thisPart = _IPropertyMng.GetProperty(This_IKompasDocument3D, y);
                                if (Property_thisPart.Name == Property.Name)
                                {
                                    find_Prop = true;
                                    if (returnObject != null)
                                        TransProp_SetValueProperty(part_thisDetal, Property_thisPart, returnObject);
                                    break;
                                }
                            }
                            if (!find_Prop)
                            {
                                object obj = null;
                                IProperty Property_new = _IPropertyMng.AddProperty(This_IKompasDocument3D, obj);
                                Property_new.Name = Property.Name;
                                Property_new.Update();
                                TransProp_SetValueProperty(part_thisDetal, Property_new, returnObject);
                            }
                        }
                    }
                }
            }
        }
        private void WalkAssembly(IPart7 TopPart)
        {
            if (TopPart != null)
            {
                var Parts = TopPart.PartsEx[0];
                if (Parts != null)
                {
                    foreach (IPart7 item in Parts)
                    {
                        try
                        {
                            AddWaitStatus(Path.GetFileNameWithoutExtension(item.FileName));
                            if (!item.Detail)
                                WalkAssembly(item);
                            else
                            {
                                bool isSheetMetal = IsSheetMetal(item, out isSheetMetal);
                                if (isSheetMetal) PartCalc(item);
                            }
                        }
                        catch (Exception Ex)
                        {
                            ShowMsgBox("Ошибка при обработке компонента " + item.FileName + Environment.NewLine + Ex.Message, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
        private void PartCalc(IPart7 part)
        {
            if (part == null) return;
            if (!part.Detail || part.Standard) return;
            ISheetMetalContainer sheetMetalContainer = (ISheetMetalContainer)part;
            if (sheetMetalContainer == null) return;

            ksPart kPart = _kompasObject.TransferInterface(part, (int)Kompas6Constants.ksAPITypeEnum.ksAPI5Auto, 0);
            if (kPart == null) return;
            string marking = kPart.marking;
            int BendCount = 0;
            int CutCount = 0;
            double CutLentgth = 0;
            GetBend(sheetMetalContainer, kPart, out BendCount, out CutCount, out CutLentgth);
            List<ShProperty> properties = new List<ShProperty>();
            if (IOption_Class.ICShMProperty.BendCount_Calc) properties.Add(new ShProperty { Name = "Количество гибов", Value = BendCount });
            if (IOption_Class.ICShMProperty.HoleCount_Calc) properties.Add(new ShProperty { Name = "Количество врезок", Value = CutCount });
            if (IOption_Class.ICShMProperty.CutLengt_Calc) properties.Add(new ShProperty { Name = "Длина реза", Value = CutLentgth });
            AddProperty(part, properties);
            //MessageBox.Show($"Колво сгибов: {BendCount}\nКолво вырезов: {CutCount}\nДлина реза: {CutLentgth}");
        }
        class ShProperty
        {
            public string Name { get; set; }
            public object Value { get; set; }
        }
        private void AddProperty(IPart7 part_, string ParamName, object ParamValue)
        {
            IKompasDocument3D This_IKompasDocument3D = (IKompasDocument3D)GetIKompasDocument(part_.FileName, false, false);
            IPart7 part_thisDetal = This_IKompasDocument3D.TopPart;
            if (_IApplication != null)
            {
                IPropertyMng _IPropertyMng = (IPropertyMng)_IApplication;
                if (_IPropertyMng != null)
                {
                    bool find_Prop = false;
                    int count = _IPropertyMng.PropertyCount[This_IKompasDocument3D];
                    for (int i = 0; i < count; i++)
                    {
                        IProperty Property = _IPropertyMng.GetProperty(This_IKompasDocument3D, i);
                        if(Property.Name == ParamName)
                        {
                            find_Prop = true;
                            TransProp_SetValueProperty(part_thisDetal, Property, ParamValue);
                        }
                    } 
                    if (!find_Prop)
                    {
                        object obj = null;
                        IProperty Property_new = _IPropertyMng.AddProperty(This_IKompasDocument3D, obj);
                        Property_new.Name = ParamName;
                        Property_new.Update();
                        TransProp_SetValueProperty(part_thisDetal, Property_new, ParamValue);
                    }
                }
            }
        }
        private void AddProperty(IPart7 part_, List<ShProperty> shProperties)
        {
            IKompasDocument3D This_IKompasDocument3D = (IKompasDocument3D)GetIKompasDocument(part_.FileName, false, false);
            IPart7 part_thisDetal = null; 
            IEmbodimentsManager embodimentsManager = (IEmbodimentsManager)This_IKompasDocument3D.TopPart;
            if (embodimentsManager.EmbodimentCount > 0)
            {
                Embodiment embodiment = embodimentsManager.Embodiment[part_.Marking];
                if (embodiment != null) part_thisDetal = embodiment.Part;
            }
            else
                part_thisDetal = This_IKompasDocument3D.TopPart;

            if (_IApplication != null)
            {
                IPropertyMng _IPropertyMng = (IPropertyMng)_IApplication;
                if (_IPropertyMng != null)
                {
                    foreach(ShProperty shProperty in shProperties)
                    {
                        bool find_Prop = false;
                        int count = _IPropertyMng.PropertyCount[This_IKompasDocument3D];
                        for (int i = 0; i < count; i++)
                        {
                            IProperty Property = _IPropertyMng.GetProperty(This_IKompasDocument3D, i);
                            if (Property.Name == shProperty.Name)
                            {
                                find_Prop = true;
                                TransProp_SetValueProperty(part_thisDetal, Property, shProperty.Value);
                                break;
                            }
                        }
                        if (!find_Prop)
                        {
                            object obj = null;
                            IProperty Property_new = _IPropertyMng.AddProperty(This_IKompasDocument3D, obj);
                            Property_new.Name = shProperty.Name;
                            Property_new.Update();
                            TransProp_SetValueProperty(part_thisDetal, Property_new, shProperty.Value);
                        }
                    } 
                }
            }
        }
        public void getSheeteMetalBends()
        { 
            CheckMainControl();
            if (!AppVersNOTValidStrongMessage()) return;
            if ((_IApplication.ActiveDocument is IKompasDocument3D) != true)
                return;
            IKompasDocument3D _IKompasDocument = (IKompasDocument3D)_IApplication.ActiveDocument;
            IPart7 part = _IKompasDocument.TopPart;
            if (part == null) return;
            if (!part.Detail)
                WalkAssembly(part);
            else
            {
                bool isSheetMetal = IsSheetMetal(part, out isSheetMetal);
                if (isSheetMetal) PartCalc(part);
            }

            CloseDocs();
        }
        public void test()
        {
            //Выполнение основной части программы
            if (_kompasObject != null)
            {
                //получаем файл модели
                IKompasDocument3D doc3D = (IKompasDocument3D)_IApplication.ActiveDocument;
                //получаем верхний компонент
                IPart7 top3D = doc3D.TopPart;
                //перестраиваем документ
                doc3D.RebuildDocument();
                //получаем интерфейс дерева построения
                IFeature7 pFeat = (IFeature7)top3D.Owner;
                //получаем массив элементов дерева построения
                Object[] featCol = pFeat.SubFeatures[0, false, false];
                //получаем и выводим кол-во элементов в дереве построения
                int featCount = featCol.Count();
                Console.WriteLine(@"Всего компонентов дерева построения: {0}", featCount.ToString());
                Console.WriteLine();
                //определяем переменные для работы с компонентами дерева
                IFeature7 curFeat = null;   //текущий элемент
                int lcf = 0;                //кол-во переменных в текущем элементе
                int i = 0;                  //счетчик компонентов дерева
                int j = 0;                  //счетчик переменных компонента
                string name_of_object = null;//имя компонента дерева
                bool is_sheet = false;      //проверка на "листовое тело"
                //перебираем компоненты дерева
                do
                {
                    //получаем текущий компонент
                    curFeat = (IFeature7)featCol[i];
                    //получаем кол-во переменных в компоненте
                    lcf = curFeat.VariablesCount[false, false];
                    //получаем и выводим имя компонента
                    name_of_object = curFeat.Name;
                    Console.WriteLine(@"{0} - {1} переменных", name_of_object, lcf.ToString());
                    Console.WriteLine();

                    is_sheet = false;
                    //перебираем переменные в текущем компоненте
                    j = 0;
                    do
                    {
                        Console.WriteLine(curFeat.Variable[false, false, j].Name + @" " +
                            +curFeat.Variable[false, false, j].Value + @" " + curFeat.Variable[false, false, j].ParameterNote);
                        if (curFeat.Variable[false, false, j].ParameterNote == @"Толщина листового тела") { is_sheet = true; }
                        //Console.WriteLine(@"Имя переменной: {0}", curFeat.Variable[false,false,j].Name);
                        //Console.WriteLine(@"Значение переменной: {0}", curFeat.Variable[false, false, j].Value);
                        //Console.WriteLine(@"Комментарий к переменной: {0}", curFeat.Variable[false, false, j].ParameterNote);
                        j++;
                    } while (j < lcf);
                    //проводим проверку на листовове тело
                    if (is_sheet)
                    {
                        if (lcf == 4)
                        {
                            Console.WriteLine(@"Листовое тело построено по замкнутому контуру");
                        }
                        else { Console.WriteLine(@"Листовое тело построено по незамкнутому контуру"); }
                        is_sheet = false;
                    }
                    else { Console.WriteLine(@"Элемент дерева не является листовым телом"); }
                    Console.WriteLine();
                    i++;
                } while (i < featCount);

                Console.ReadLine();
            }
            else { _IApplication.MessageBoxEx(@"Текущий документ не является моделью", @"Информация о документе", 0); }

        }

        private void GetBend(ISheetMetalContainer sheetMetalContainer, ksPart kPart, out int BendCount, out int CutCount, out double CutLentgth)
        {
            BendCount = 0;
            CutCount = 0;
            CutLentgth = 0;
            SheetMetalBends sheetMetalBends = sheetMetalContainer.SheetMetalBends;
            ISheetMetalBendedStraightens sheetMetalBendedStraightens = sheetMetalContainer.SheetMetalBendedStraightens;
            BendCount = sheetMetalBendedStraightens.Count;
            if (IOption_Class.ICShMProperty.BendCount_Calc)
            {
                AddWaitStatus($"Сгибы в {Path.GetFileNameWithoutExtension(kPart.fileName)}");
                for (int ii = 0; ii < sheetMetalBends.Count; ii++)
                {
                    SheetMetalBend sheetMetalBend = (SheetMetalBend)sheetMetalBends[ii];
                    if (sheetMetalBend.BendObjects != null)
                    {
                        try
                        {
                            foreach (object bend in sheetMetalBend.BendObjects)
                                BendCount += 1;
                        }
                        catch { BendCount += 1; }
                    }
                    else
                        BendCount += 1;
                }
                SheetMetalLineBends sheetMetalLineBends = sheetMetalContainer.SheetMetalLineBends;
                BendCount += sheetMetalLineBends.Count;
            }
            
            if(IOption_Class.ICShMProperty.CutLengt_Calc || IOption_Class.ICShMProperty.HoleCount_Calc)
            {
                SheetMetalBodies sheetMetalBodies = sheetMetalContainer.SheetMetalBodies;
                SheetMetalBody sheetMetalBody = (SheetMetalBody)sheetMetalBodies[0];

                ksBodyCollection iBodyCollection = kPart.BodyCollection();
                ksBody body = iBodyCollection.GetByIndex(0);
                ksFaceCollection faceCollection = body.FaceCollection();
                AddWaitStatus($"Контур в {Path.GetFileNameWithoutExtension(kPart.fileName)}");
                for (int yy = 0; yy < faceCollection.GetCount(); yy++)
                {
                    ksFaceDefinition faceDefinition = faceCollection.GetByIndex(yy);
                    ksSurface surface = faceDefinition.GetSurface();
                    if (IOption_Class.ICShMProperty.HoleCount_Calc) CutCount += surface.BoundaryCount;
                    if (IOption_Class.ICShMProperty.CutLengt_Calc)
                    {
                        ksEdgeCollection edgeCollection = faceDefinition.EdgeCollection();
                        int edgeCount = edgeCollection.GetCount();
                        for (int ee = 0; ee < edgeCount; ee++)
                        {
                            ksEdgeDefinition edgeDefinition = edgeCollection.GetByIndex(ee);
                            double length = edgeDefinition.GetLength(0x1);
                            if(edgeCount == 2)
                            {
                                double min_Length = 0;
                                for (int zz = 0; zz < edgeCount; zz++)
                                {
                                    ksEdgeDefinition edgeDefinition2 = edgeCollection.GetByIndex(zz);
                                    double length2 = edgeDefinition2.GetLength(0x1);
                                    if (Math.Round(length2, 5) != Math.Round(sheetMetalBody.Thickness, 5) && min_Length < length2)
                                    {
                                        min_Length = length2;
                                    }
                                }
                                CutLentgth += min_Length;
                                break;
                            }
                            else if(edgeCount == 4)
                            { 
                                if (Math.Round(length, 5) == Math.Round(sheetMetalBody.Thickness, 5))
                                {
                                    double min_Length = 0;
                                    for (int zz = 0; zz < edgeCount; zz++)
                                    {
                                        ksEdgeDefinition edgeDefinition2 = edgeCollection.GetByIndex(zz);
                                        double length2 = edgeDefinition2.GetLength(0x1);
                                        if (Math.Round(length2, 5) != Math.Round(sheetMetalBody.Thickness, 5))
                                        {
                                            min_Length = length2;
                                        }
                                    }
                                    CutLentgth += min_Length;
                                    break;
                                }
                            }
                        }
                    }
                }
                CutLentgth = Math.Round(CutLentgth, 3);

                CutCount = ((CutCount - faceCollection.GetCount()) / 2) + 1;
            } 
        }
        public static bool TransProp_SetValueProperty(IPart7 part, IProperty Property, object Val)
        {
            bool res = false; 
            if (Property != null)
            {
                IPropertyKeeper Prop = (IPropertyKeeper)part;

                if (Prop != null)
                {
                    _Property classresProperty = (_Property)Property;
                    if (classresProperty != null)
                        res = Prop.SetPropertyValue((_Property)classresProperty, Val, true);
                }
            }
            return res;
        }
        #endregion 
        #region копировать модель

        public void SetSourseChancge(string SB_FilenameIN, string SB_FilenameOUT)
        {
            if (!AppVersNOTValidStrongMessage()) return;
            try
            {
                _IApplication.HideMessage = ksHideMessageEnum.ksHideMessageNo; //отключаем все сообщения от компаса
                SB_FilenameIN = SB_FilenameIN.Replace("/", @"\"); 
                if (!Directory.Exists(Path.GetDirectoryName(SB_FilenameOUT)))
                    Directory.CreateDirectory(Path.GetDirectoryName(SB_FilenameOUT));
                bool docVis = true;
                ksDocument3D iDocument3D = (ksDocument3D)_kompasObject.Document3D();
                if (iDocument3D.Open(SB_FilenameIN, docVis))
                    iDocument3D.SaveAs(SB_FilenameOUT);
                else
                {
                    iDocument3D = (ksDocument3D)_kompasObject.ActiveDocument3D();
                    iDocument3D.SaveAs(SB_FilenameOUT); 
                }
                //File.Copy(SB_FilenameIN, SB_FilenameOUT, true);
                iDocument3D.Open(SB_FilenameOUT, docVis);

                AddWaitStatus("Скопирован:" + Path.GetFileName(SB_FilenameOUT));

                Dictionary<string, string> AssemblyParts = new Dictionary<string, string>();

                var iPartCollection = iDocument3D.PartCollection(true);
                ksPart iPart = iDocument3D.GetPart((short)Part_Type.pTop_Part);
                int Count = iPartCollection.GetCount();
                for (int j = 0; j < Count; j++)
                {
                    string OutPartFileName = null;
                    try
                    {
                        var iPartIndex = iPartCollection.GetByIndex(j);
                        string file = iPartIndex.fileName;
                        OutPartFileName = GetFullFileName(SB_FilenameOUT, file);
                        if (!Directory.Exists(Path.GetDirectoryName(OutPartFileName)))
                            Directory.CreateDirectory(Path.GetDirectoryName(OutPartFileName));

                        bool NeedSetlink = false;
                        if (!File.Exists(OutPartFileName))
                        {
                            AddWaitStatus("Скопирован:" + Path.GetFileName(OutPartFileName));
                            File.Copy(file, OutPartFileName, true);
                            NeedSetlink = true;
                            if (!iPartIndex.IsDetail)
                                AssemblyParts.Add(iPartIndex.fileName, OutPartFileName);
                        }
                        iPartIndex.fileName = OutPartFileName;
                        if (NeedSetlink) SetLinkInProperty(OutPartFileName, SB_FilenameOUT);
                        iPartIndex.Update();
                    }
                    catch
                    {
                        AddWaitStatus("Ошибка при изменении файл-источника для детали: " + Path.GetFileName(OutPartFileName));
                    }
                }
                iDocument3D.Save();
                iDocument3D.close();

                foreach (KeyValuePair<string, string> IParam in AssemblyParts)
                {
                    SetSourseChancge(IParam.Key, IParam.Value);
                    SetLinkInProperty(IParam.Value, SB_FilenameOUT);
                }
            }
            catch { }
            _IApplication.HideMessage = ksHideMessageEnum.ksShowMessage;
        }
        private string GetFullFileName(string ParentFileName, string PartFileName)
        {
            string startPath = Dump.ModelFolderInDump;
            string[] p1_folders = Path.GetDirectoryName(ParentFileName).Split(Convert.ToChar(@"\"));
            string[] p2_folders = Path.GetDirectoryName(PartFileName).Split(Convert.ToChar(@"\"));
            string res_path = "";
            bool check_f = false;
            int findInd = -1;
            for (int i = p1_folders.Length - 1; i >= 0; i--)
            {
                string p1_Folder = p1_folders[i];
                if (!string.IsNullOrEmpty(p1_Folder))
                {
                    for (int j = p2_folders.Length - 1; j >= 0; j--)
                    {
                        string p2_Folder = p2_folders[j];
                        if (p1_Folder == p2_Folder) { check_f = true; findInd = j; }
                    }
                    if (check_f)
                    {
                        for (int j = findInd; j < p2_folders.Length; j++)
                        {
                            string p2_Folder = p2_folders[j];
                            res_path += $@"\{p2_Folder}";
                        }
                        return startPath + $@"\{res_path.Trim(Convert.ToChar(@"\"))}\{Path.GetFileName(PartFileName)}";
                    }

                }
            }
            return startPath + $@"\{res_path.Trim(Convert.ToChar(@"\"))}\{Path.GetFileName(PartFileName)}";
        }

        public void SetLinkInProperty(string OutPartFileName, string SB_FilenameOUT)
        {
            string ParamName = null;
            ksDocument3D iDocument3D_1 = (ksDocument3D)_kompasObject.Document3D();
            bool op = iDocument3D_1.Open(OutPartFileName, true);

            ksPart iPart_1 = (ksPart)iDocument3D_1.GetPart((int)Part_Type.pTop_Part);
            ksVariableCollection VariableCollection;
            try
            {
                // получаем интерфейс объекта дерева построения
                ksFeature iFeature = iPart_1.GetFeature();
                //получаем коллекцию внешних переменных
                VariableCollection = iFeature.VariableCollection;
                // обновляем коллекцию внешних переменных
                VariableCollection.refresh();
            }
            catch
            {
                VariableCollection = iPart_1.VariableCollection();
                VariableCollection.refresh();
            }
            //считаем количество переменных в детали
            int count_1 = VariableCollection.GetCount();
            for (int i = 0; i < count_1; i++)
            {
                try
                {
                    //Получаем интерфейс переменной по её имени
                    ksVariable Variable = VariableCollection.GetByIndex(i);
                    ParamName = Variable.name;
                    if (!string.IsNullOrEmpty(Variable.linkDocName))
                    {
                        Variable.SetLink(SB_FilenameOUT, Variable.linkVarName);
                        iPart_1.RebuildModel();
                    }
                }
                catch
                {
                    AddWaitStatus($"Ошибка при изменении параметра {ParamName} для детали: " + Path.GetFileName(OutPartFileName));
                }
            }
            iDocument3D_1.Save();
            iDocument3D_1.close();
        }

        public void SetLinkInVariable()
        {
            //процедура запускается из Детали
            //находит переменную с именем "Имя_ссылочной_переменной_в_детали"
            //делает ссылочным данную переменную, SetLink(DOC, NAME), где DOC = "Модель сборки.a3d", NAME = "Имя_ссылочной_переменной_в_сборке"

            //ОПИСАНИЕ ВОПРОСА!
            //Компас не дает возможности, создать ссылку на значение переменной, находящейся не в "00" исполнении
            //при использовании процедуры SetLink(DOC, NAME), ссылка "привязывается" к "00" исполнению
            //невозможно программано задать исполнение, к которой необходимо сослаться

            ksDocument3D iDocument3D = (ksDocument3D)_kompasObject.ActiveDocument3D();

            ksPart iPart = (ksPart)iDocument3D.GetPart((int)Part_Type.pTop_Part);
            ksVariableCollection VariableCollection;

            // получаем интерфейс объекта дерева построения
            ksFeature iFeature = iPart.GetFeature();
            //получаем коллекцию переменных
            VariableCollection = iFeature.VariableCollection;
            // обновляем коллекцию переменных
            VariableCollection.refresh();
            //считаем количество переменных в детали
            int count_1 = VariableCollection.GetCount();
            for (int i = 0; i < count_1; i++)
            {
                //Получаем интерфейс переменной по её имени
                ksVariable Variable = VariableCollection.GetByIndex(i);
                string ParamName = Variable.name;
                if (ParamName == "Имя_ссылочной_переменной_в_детали")
                {
                    //ВОПРОС!!!(ОПИСАНИЕ СМ. ВЫШЕ)
                    Variable.SetLink("c:\\Модель сборки.a3d", "Имя_ссылочной_переменной_в_сборке");
                    //ВОПРОС!!!(ОПИСАНИЕ СМ. ВЫШЕ)
                    iPart.RebuildModel();
                    break;
                }
            }
            iDocument3D.Save();
            iDocument3D.close();
        }
        #endregion
        #region копировать Проект
        List<string> SetLinkVariableCompeteFile_List;
        List<string> SetSourseCompeteFile_List;
        public VSNRM_Kompas.ProjectClone.Pr_Clone_Class _Pr_Clone_Class;
        bool OpenVisible = true;
        public void SetLinks(List<TreeListNode> AllComponents)
        {
            //_IApplication.Visible = false;
            SetLinkVariableCompeteFile_List = new List<string>();
            SetSourseCompeteFile_List = new List<string>();
            List<VSNRM_Kompas.ProjectClone.CopyProjectHelperClass.CopyFileInfo> CopyFileList = new List<VSNRM_Kompas.ProjectClone.CopyProjectHelperClass.CopyFileInfo>();
            waitMng = ((MainForm)System.Windows.Forms.Application.OpenForms["MainForm"]).splashScreenManager2;
            waitMng.ShowWaitForm();
            waitMng.SetWaitFormCaption("Копирование проекта");
            waitMng.SetWaitFormDescription("Копирование файлов проекта");
            foreach (TreeListNode node in AllComponents)
            {
                if (node.Checked)
                {
                    ComponentInfo componentInfo = (ComponentInfo)node.Tag;
                    string CopyFileName = $@"{node.GetValue("Сохранить в папке")}\{node.GetValue("Сохранить в имени")}{node.GetValue("Тип")}";
                    if (!Directory.Exists(Path.GetDirectoryName(CopyFileName)))
                        Directory.CreateDirectory(Path.GetDirectoryName(CopyFileName));
                    string oboz = node["Обозначение"].ToString();
                    if (CopyFileList.Find(x => x.CopyFileName == CopyFileName && x.Oboz == oboz) == null)
                    {
                        File.Copy(componentInfo.FFN, CopyFileName, true);
                        CopyFileList.Add(new VSNRM_Kompas.ProjectClone.CopyProjectHelperClass.CopyFileInfo{
                            CopyFileName = CopyFileName, Oboz =  oboz, Node = node});
                    }
                }
            }
            CopyFileList = CopyFileList.Distinct().ToList();  //удаляю дубликаты
            _IApplication.HideMessage = ksHideMessageEnum.ksHideMessageNo; //отключаем все сообщения от компаса
            foreach (VSNRM_Kompas.ProjectClone.CopyProjectHelperClass.CopyFileInfo CopyFile in CopyFileList)
            {
                waitMng.SetWaitFormDescription($"{Path.GetFileName(CopyFile.CopyFileName)}");
                switch (Path.GetExtension(CopyFile.CopyFileName).ToUpper())
                {
                    case ".CDW":
                        SetLinkInDRW(CopyFile.CopyFileName, CopyFile.Node.Nodes.ToList());
                        break;
                    case ".A3D": 
                        SetSourseChancge_ModelAPI7(CopyFile.CopyFileName, CopyFile.Node, GetDonorFileNameByAllComponents(CopyFile.CopyFileName, AllComponents));
                        break;
                }
            }
            foreach (VSNRM_Kompas.ProjectClone.CopyProjectHelperClass.CopyFileInfo CopyFile in CopyFileList)
            {
                switch (Path.GetExtension(CopyFile.CopyFileName).ToUpper())
                {
                    case ".SPW":
                        waitMng.SetWaitFormDescription($"{Path.GetFileName(CopyFile.CopyFileName)}");
                        SetLinkInSPDoc(CopyFile.CopyFileName, AllComponents);
                        break; 
                }
            }
            _IApplication.HideMessage = ksHideMessageEnum.ksHideMessageYes; //отключаем все сообщения от компаса
            waitMng.CloseWaitForm();
            //_IApplication.Visible = true;
        }
        public void SetLinkInSPDoc(string FileName, List<TreeListNode> AllComponents)
        {
            bool OpenDoc = false;
            ISpecificationDocument iKompasDocument;
            iKompasDocument = (ISpecificationDocument)_IApplication.Documents[FileName];
            if (iKompasDocument != null) OpenDoc = true;
            else 
                iKompasDocument = (ISpecificationDocument)_IApplication.Documents.Open(FileName, OpenVisible, false);
            if (iKompasDocument == null) return;
            SpecificationDescriptions iSpecificationDescriptions = iKompasDocument.SpecificationDescriptions;
            SpecificationDescription iSpecificationDescription = iSpecificationDescriptions.Active;


            AttachedDocuments ModelattachedDocuments = iKompasDocument.AttachedDocuments;
            foreach (AttachedDocument attachedDocument in ModelattachedDocuments)
            {
                string Name = attachedDocument.Name;
                string NewFileName = GetFileNameByAllComponents(Name, AllComponents);
                attachedDocument.Delete();
                if (!string.IsNullOrEmpty(NewFileName)) ModelattachedDocuments.Add(NewFileName, true);
                iKompasDocument.RebuildDocument();
            }

            //iKompasDocument.Save();
            iKompasDocument.RebuildDocument();

            dynamic Objects = iSpecificationDescription.Objects;
            if (Objects != null)
            {
                foreach (ISpecificationObject specificationObject in Objects)
                {
                    ksSpecificationObjectTypeEnum ObjectType = specificationObject.ObjectType;
                    if ((int)ObjectType == 1 || (int)ObjectType == 2)
                    {
                        AttachedDocuments attachedDocuments = specificationObject.AttachedDocuments;
                        if(attachedDocuments!= null)
                        {
                            List<string> ReplaceFileList = new List<string>();
                            foreach (AttachedDocument attachedDocument in attachedDocuments)
                                ReplaceFileList.Add(attachedDocument.Name);

                            int jj = attachedDocuments.Count - 1;
                            while (attachedDocuments.Count > 0)
                            {
                                AttachedDocument attachedDocument = attachedDocuments[jj];
                                if (attachedDocument != null) attachedDocument.Delete();
                                jj--;
                            }
                            specificationObject.Update();
                            foreach (string ReplaceFile in ReplaceFileList)
                            {
                                string NewFileName = GetFileNameByAllComponents(ReplaceFile, AllComponents);
                                if (string.IsNullOrEmpty(NewFileName) && IsExportFileName(ReplaceFile, AllComponents)) attachedDocuments.Add(ReplaceFile, true);

                                if (!string.IsNullOrEmpty(NewFileName)) attachedDocuments.Add(NewFileName, true);
                                specificationObject.Update();
                            }
                        }
                    }
                }
            }
            iKompasDocument.Save();
            if(!OpenDoc) iKompasDocument.Close(DocumentCloseOptions.kdSaveChanges);
        }
        public void SetAttachedDoc(string FileName)
        {
            bool OpenDoc = false;
            IKompasDocument3D iKompasDocument;
            iKompasDocument = (IKompasDocument3D)_IApplication.Documents[FileName];
            if (iKompasDocument != null) OpenDoc = true;
            else
                iKompasDocument = (IKompasDocument3D)_IApplication.Documents.Open(FileName, OpenVisible, false);
            if (iKompasDocument == null) return;
            try
            {
                IPart7 part7 = iKompasDocument.TopPart;
                IProductDataManager productDataMenager = iKompasDocument as IProductDataManager;
                if (productDataMenager == null) return;

                dynamic arrAttachDoc = productDataMenager.ObjectAttachedDocuments[(IPropertyKeeper)part7];
                List<string> drwS = new List<string>();
                if (arrAttachDoc != null)
                    foreach (var tDoc in arrAttachDoc)
                    {
                        drwS.Add(tDoc.ToString());
                    }
                foreach(string fn in drwS)
                {
                    productDataMenager.DeleteProductObject(fn);
                    iKompasDocument.RebuildDocument();
                }
                //ICustom
                iKompasDocument.Save();
                //productDataMenager.DeleteReferenceData
                //string ffn = @"C: \Users\admin_veza\Desktop\Новая папка(3)\01 - ЕЛГ 02.01.10.000 СБ Стойка нижняя.cdw";
                //productDataMenager.AddProductObject((IPropertyKeeper)part7, ffn, ksProductObjectTypeEnum.ksPOTAllObjects);
                

                //arrAttachDoc = productDataMenager.ObjectAttachedDocuments[(IPropertyKeeper)part7];
                //drwS = new List<string>();
                //if (arrAttachDoc != null)
                //    foreach (var tDoc in arrAttachDoc)
                //    {
                //        if (!string.IsNullOrEmpty(tDoc.ToString()) && File.Exists(tDoc.ToString()))
                //        {
                //            drwS.Add(tDoc.ToString());
                //        }
                //    }
            }
            catch(Exception Ex)
            {
                ShowMsgBox("Ошибка при изменении связанных файлов у документа" + FileName + Environment.NewLine + Ex.Message, MessageBoxIcon.Error);
            }
            iKompasDocument.Save();
            if (!OpenDoc) iKompasDocument.Close(DocumentCloseOptions.kdSaveChanges);
        }
        public void SetLinkInDRW(string FileName, List<TreeListNode> AllComponents)
        {
            bool OpenDoc = false;
            IKompasDocument2D document2D;
            document2D = (IKompasDocument2D)_IApplication.Documents[FileName];
            if (document2D != null) OpenDoc = true;
            else
                document2D = (IKompasDocument2D)_IApplication.Documents.Open(FileName, OpenVisible, false);
            IKompasDocument2D1 kompasDocument2D1 = null;
            try
            {
                kompasDocument2D1 = (IKompasDocument2D1)document2D;
                ViewsAndLayersManager viewsAndLayersManager = document2D.ViewsAndLayersManager;
                Views views = viewsAndLayersManager.Views;
                for (int ii = 0; ii < views.Count; ii++)
                {
                    KompasAPI7.View view = (KompasAPI7.View)views[ii];
                    if (view.Type == KompasAPIObjectTypeEnum.ksObjectAssociationView)
                    {
                        IAssociationView associationView = (IAssociationView)view;
                        string Name = associationView.SourceFileName;
                        string New_FileName = GetFileNameByAllComponents(Name, AllComponents);
                        if (!string.IsNullOrEmpty(New_FileName)) associationView.SourceFileName = New_FileName;
                        view.Update();
                    }
                }
            }
            catch (Exception Ex)
            {
                ShowMsgBox("Ошибка при изменении связанных файлов у документа" + FileName + Environment.NewLine + Ex.Message, MessageBoxIcon.Error);
            }
            if(kompasDocument2D1!=null) kompasDocument2D1.RebuildDocument();
            if(!OpenDoc && document2D!=null) document2D.Close(DocumentCloseOptions.kdSaveChanges);
        }
        string GetDonorFileNameByAllComponents(string ExportFileName, List<TreeListNode> AllComponents)
        {
            if (string.IsNullOrEmpty(ExportFileName)) return null;
            foreach (TreeListNode node in AllComponents)
            {
                if(node.GetValue("Сохранить в имени").ToString() == Path.GetFileNameWithoutExtension(ExportFileName))
                {
                    ComponentInfo componentInfo = (ComponentInfo)node.Tag;
                    return componentInfo.FFN;
                }   
            }
            return null;
        }
        bool IsExportFileName(string ExportFileName, List<TreeListNode> AllComponents)
        {
            if (string.IsNullOrEmpty(ExportFileName)) return false;
            foreach (TreeListNode node in AllComponents)
            { 
                if ($@"{node.GetValue("Сохранить в папке")}\{node.GetValue("Сохранить в имени")}{node.GetValue("Тип")}" == ExportFileName && node.Checked)
                    return true;
            }
            return false;
        }
        string GetFileNameByAllComponents(string DonorFileName, List<TreeListNode> AllComponents)
        {
            if (string.IsNullOrEmpty(DonorFileName)) return null;
            
            if (!File.Exists(DonorFileName))
            {
                foreach (TreeListNode node in AllComponents)
                {
                    ComponentInfo componentInfo = (ComponentInfo)node.Tag;
                    if (Path.GetFileName(componentInfo.FFN) == Path.GetFileName(DonorFileName) && node.Checked)
                        return $@"{node.GetValue("Сохранить в папке")}\{node.GetValue("Сохранить в имени")}{node.GetValue("Тип")}";
                }
            }
            else
            {
                foreach (TreeListNode node in AllComponents)
                {
                    ComponentInfo componentInfo = (ComponentInfo)node.Tag;
                    if (componentInfo.FFN == DonorFileName && node.Checked)
                        return $@"{node.GetValue("Сохранить в папке")}\{node.GetValue("Сохранить в имени")}{node.GetValue("Тип")}";
                }

                //foreach (TreeListNode node in AllComponents)
                //{
                //    ComponentInfo componentInfo = (ComponentInfo)node.Tag;
                //    if (Path.GetFileName(componentInfo.FFN) == Path.GetFileName(DonorFileName) && node.Checked)
                //        return $@"{node.GetValue("Сохранить в папке")}\{node.GetValue("Сохранить в имени")}{node.GetValue("Тип")}";
                //}
            }
            return DonorFileName;
        }
        string GetFileNameByAllComponents(string DonorFileName, List<TreeListNode> AllComponents, string ParentFileNamee)
        {
            if (string.IsNullOrEmpty(DonorFileName)) return null;

            foreach (TreeListNode node in AllComponents)
            {
                ComponentInfo componentInfo = (ComponentInfo)node.Tag;
                if (componentInfo.FFN == ParentFileNamee && !node.Checked)
                    return DonorFileName;
            }
            foreach (TreeListNode node in AllComponents)
            {
                ComponentInfo componentInfo = (ComponentInfo)node.Tag;
                if (componentInfo.FFN == DonorFileName && node.Checked)
                    return $@"{node.GetValue("Сохранить в папке")}\{node.GetValue("Сохранить в имени")}{node.GetValue("Тип")}";
            }

            foreach (TreeListNode node in AllComponents)
            {
                ComponentInfo componentInfo = (ComponentInfo)node.Tag;
                if (Path.GetFileName(componentInfo.FFN) == Path.GetFileName(DonorFileName) && node.Checked)
                    return $@"{node.GetValue("Сохранить в папке")}\{node.GetValue("Сохранить в имени")}{node.GetValue("Тип")}";
            }
            return DonorFileName;
        }
        string GetCopiFileNameByAllComponents(string NewPartFileName, string OperationName, List<TreeListNode> AllComponents)
        {
            string res = null;
            if (string.IsNullOrEmpty(NewPartFileName)) return null;
            foreach (TreeListNode node in AllComponents)
            {
                if (node.Checked && $@"{node["Сохранить в папке"]}\{node["Сохранить в имени"]}{node["Тип"]}" == NewPartFileName)
                {
                    ComponentInfo componentInfo = (ComponentInfo)node.Tag;
                    foreach(ComponentInfo.CopyGeometry copyGeometry in componentInfo.CopyGeometriesList)
                        if(copyGeometry.OperationName == OperationName)
                        {
                            res = GetFileNameByAllComponents(copyGeometry.DocumentName, AllComponents);
                            if (string.IsNullOrEmpty(res)) return res;
                        }
                }
            }
            return res;
        }
        private void GetParamsByObozAndNaim(string NewFileName, string OldOboz, string OldName, out string ReturnOboz, out string ReturnNaim, List<TreeListNode> AllComponents)
        {
            ReturnOboz = null;
            ReturnNaim = null;
            if (string.IsNullOrEmpty(NewFileName)) return;
            foreach (TreeListNode node in AllComponents)
            {
                ComponentInfo componentInfo = (ComponentInfo)node.Tag;
                if (node.GetValue("Сохранить в имени").ToString() == Path.GetFileNameWithoutExtension(NewFileName) && componentInfo.Oboz == OldOboz && componentInfo.Naim == OldName && node.Checked)
                {
                    ReturnOboz = node.GetValue("Сохранить в Обозначении").ToString();
                    ReturnNaim = node.GetValue("Сохранить в Наименовании").ToString();
                    return;
                }
            }
            if (ReturnOboz == null) ReturnOboz = OldOboz;
            if (ReturnNaim == null) ReturnNaim = OldName;
        }
        private void SetCloneProperty(IKompasDocument3D document3D, IPart7 part7, string New_FileName, List<TreeListNode> AllComponents)
        {
            //процедура изменяет Обозначение и Наименование у нового компонента
            if (document3D == null) return;
            if (_IApplication != null)
            {
                IPropertyMng _IPropertyMng = (IPropertyMng)_IApplication;
                if (_IPropertyMng != null)
                {
                    string NewOboz;
                    string NewNaim;
                    GetParamsByObozAndNaim(New_FileName, part7.Marking, part7.Name, out NewOboz, out NewNaim, AllComponents);
                    if (NewOboz == part7.Marking && NewNaim == part7.Name ) return;
                    int count = _IPropertyMng.PropertyCount[document3D];
                    int SetObozAndName = 0;
                    for (int i = 0; i < count; i++)
                    {
                        IProperty Property = _IPropertyMng.GetProperty(document3D, i);
                        if(Property.Name == "Обозначение" || Property.Name == "Наименование")
                        {
                            IPropertyKeeper Prop = (IPropertyKeeper)part7;

                            if (Prop != null)
                            {
                                bool res;
                                _Property classresProperty = (_Property)Property;
                                if (classresProperty != null)
                                    res = Prop.SetPropertyValue((_Property)classresProperty, (Property.Name == "Обозначение" ? NewOboz : NewNaim), true);
                                SetObozAndName += 1;
                            }
                        }
                        if (SetObozAndName == 2) break;
                    }
                }
            }
        }
        public void SetSourseChancge_ModelAPI7(string ExportFileName, TreeListNode ParentNode, string DonorFileName)
        {
            if (IsSourseCompeteFile(ExportFileName)) return;
            bool OpenDoc = false;
            IKompasDocument3D document3D;
            document3D = (IKompasDocument3D)_IApplication.Documents[DonorFileName];
            if (document3D != null) OpenDoc = true;
            else
                document3D = (IKompasDocument3D)_IApplication.Documents.Open(DonorFileName, OpenVisible, false);
            //if (document3D == null) return;
            document3D.SaveAs(ExportFileName);
            document3D = (IKompasDocument3D)_IApplication.Documents.Open(ExportFileName, OpenVisible, false);
            IPart7 part7 = document3D.TopPart;
            if (part7 == null) { IPart7NothingMsg(ExportFileName); return; }
            int currentEmbody = 0;
            IEmbodimentsManager _IEmbodimentsManager = (IEmbodimentsManager)part7;
            int EmbodyCount = _IEmbodimentsManager.EmbodimentCount;
            currentEmbody = _IEmbodimentsManager.CurrentEmbodimentIndex;
            
            for (int j = 0; j < EmbodyCount; j++)
            {
                Embodiment tmp_Embodiment;
                tmp_Embodiment = _IEmbodimentsManager.Embodiment[j];
                //if (tmp_Embodiment.IsCurrent == true) { currentEmbody = j; }
                tmp_Embodiment.IsCurrent = true;
                if (tmp_Embodiment.Part == null) { IPart7NothingMsg(ExportFileName); return; }
                var Parts = tmp_Embodiment.Part.Parts;//PartsEx[0];
                if (Parts != null)
                {
                    foreach (IPart7 part in Parts)
                    {
                        try
                        {
                            if (!part.IsLocal)
                            {
                                IFeature7 feature7 = (IFeature7)part;
                                string Name = part.FileName;

                                waitMng.SetWaitFormDescription($"{Path.GetFileName(Name)}");

                                bool checkedNode = false;
                                TreeListNode node = null;
                                string New_FileName = PartFileName(ParentNode.Nodes.ToList(), Path.GetFileName(Name), part.Marking, part.Name, out checkedNode, out node);
                                if (checkedNode)
                                    New_FileName = GetFileNameByAllComponents(Name, ParentNode.Nodes.ToList());

                                if (string.IsNullOrEmpty(New_FileName))
                                    New_FileName = _Pr_Clone_Class.getFileNameWithFindOptions(Name);
                                if (!File.Exists(New_FileName)) continue;
                                if (!string.IsNullOrEmpty(New_FileName))
                                {
                                    try
                                    {
                                        if (!File.Exists(New_FileName) && checkedNode)
                                            File.Copy(Name, New_FileName, true);
                                        if (part.FileName != New_FileName) part.FileName = New_FileName;
                                        if(checkedNode) SetLinkInProperty_ModelAPI7(New_FileName, ParentNode.Nodes.ToList());
                                    }
                                    catch(Exception ex1)
                                    {
                                        MessageBox.Show($"Ошибка при копировании/изменении ссылок у модели {Name} на {New_FileName}\n{ex1.Message}", System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    part.Update();
                                }
                                if (!part.Detail)
                                {
                                    string Part_ExportFileName = GetFileNameByAllComponents(part.FileName, ParentNode.Nodes.ToList());
                                    if (string.IsNullOrEmpty(Part_ExportFileName) && IsExportFileName(part.FileName, ParentNode.Nodes.ToList())) SetSourseChancge_ModelAPI7(part.FileName, node, part.FileName);

                                    if (!string.IsNullOrEmpty(Part_ExportFileName)) SetSourseChancge_ModelAPI7(Part_ExportFileName, node, part.FileName);
                                    //SetSourseChancge_ModelAPI7(Part_ExportFileName, AllComponents, part.FileName);
                                }
                            }
                        }
                        catch (Exception Ex)
                        {
                            ShowMsgBox("Ошибка при изменении связанных файлов у документа" + part.FileName + Environment.NewLine + Ex.Message, MessageBoxIcon.Error);
                        }
                    }
                }
                _IEmbodimentsManager.Embodiment[currentEmbody].IsCurrent = true;
                part7.Update();
            }   

            
            if(!OpenDoc) document3D.Close(DocumentCloseOptions.kdSaveChanges);
            SetLinkInProperty_ModelAPI7(ExportFileName, ParentNode.Nodes.ToList());
        }
        private TreeListNode GetTreeListNode(List<TreeListNode> AllComponents, string byFullFileName)
        {
            foreach (TreeListNode node in AllComponents)
            {
                string Direct = node["Расположение"].ToString();
                string FileName = node["Имя файла"].ToString();
                string Extension = node["Тип"].ToString();
                string FullFileName = $@"{Direct}\{FileName}{Extension}";
                if (byFullFileName == FullFileName)
                    return node;
            }
            return null;
        }
        private string PartFileName(List<TreeListNode> AllComponents, string GetFileName, string GetOboz, string GetNaim, out bool NodeCheck, out TreeListNode FindNode)
        {
            NodeCheck = false;
            FindNode = null;
            foreach (TreeListNode node in AllComponents)
            {
                string FileName = node["Имя файла"].ToString();
                string Oboz = node["Обозначение"].ToString();
                string Naim = node["Наименование"].ToString();
                string Extension = node["Тип"].ToString();
                if (Path.GetFileNameWithoutExtension(GetFileName) == FileName /*&& Oboz == GetOboz*/ && Naim == GetNaim && Extension == Path.GetExtension(GetFileName))
                {
                    NodeCheck = node.Checked;
                    FindNode = node;
                    return $@"{node["Расположение"]}\{node["Имя файла"]}{Extension}";
                }
            }
            return null;
        }
        private bool IsLinkVariableCompeteFile(string PartFileName)
        {
            if (SetLinkVariableCompeteFile_List.Contains(PartFileName))
                return true;
            else
                SetLinkVariableCompeteFile_List.Add(PartFileName);
            return false;
        }
        private bool IsSourseCompeteFile(string PartFileName)
        {
            if (SetSourseCompeteFile_List.Contains(PartFileName))
                return true;
            else
                SetSourseCompeteFile_List.Add(PartFileName);
            return false;
        }
        public void SetFeatureLink(string PartFileName)
        {
            bool OpenDoc = false;
            IKompasDocument3D document3D;
            document3D = (IKompasDocument3D)_IApplication.Documents[PartFileName];
            if (document3D != null) OpenDoc = true;
            else
                document3D = (IKompasDocument3D)_IApplication.Documents.Open(PartFileName, OpenVisible, false);
            document3D.HideInComponentsMode = true;

            Part7 part = document3D.TopPart;
            IModelContainer modelContainer = (IModelContainer)part;
            Holes3D holes3D = modelContainer.Holes3D;
            if(holes3D != null)
            {
                foreach(Hole3D hole3D in holes3D)
                {
                    ICopiesGeometry iCGs = (ICopiesGeometry)hole3D;
                    if(iCGs != null)
                    {
                        foreach(ICopyGeometry copyGeometry in iCGs)
                        {
                            string ffn= copyGeometry.DocumentFileName;
                        }
                    }
                }
            }
        }
        public void SetLinkInProperty_ModelAPI7(string PartFileName, List<TreeListNode> AllComponents)
        {
            if (IsLinkVariableCompeteFile(PartFileName)) return; 
            bool OpenDoc = false;
            IKompasDocument3D document3D;
            document3D = (IKompasDocument3D)_IApplication.Documents[PartFileName];
            if (document3D != null) OpenDoc = true;
            else
                document3D = (IKompasDocument3D)_IApplication.Documents.Open(PartFileName, OpenVisible, false);
            string ParamName = null;
            SetLinkComponovGeometry(document3D, AllComponents);
            try
            {
                int currentEmbody = 0;
                IEmbodimentsManager _IEmbodimentsManager = (IEmbodimentsManager)document3D.TopPart;
                int EmbodyCount = _IEmbodimentsManager.EmbodimentCount;
                currentEmbody = _IEmbodimentsManager.CurrentEmbodimentIndex;

                for (int j = 0; j < EmbodyCount; j++)
                {
                    Embodiment tmp_Embodiment;
                    tmp_Embodiment = _IEmbodimentsManager.Embodiment[j];
                    //if (tmp_Embodiment.IsCurrent == true) { currentEmbody = j; }
                    tmp_Embodiment.IsCurrent = true;
                    if (tmp_Embodiment.Part == null) { IPart7NothingMsg(PartFileName); return; }
                    IPart7 part7 = tmp_Embodiment.Part;
                    try
                    {
                        SetCloneProperty(document3D, part7, PartFileName, AllComponents);
                    }
                    catch(Exception ex) 
                    {
                        MessageBox.Show($"Ошибка при изменении Обозначение/Наименование!\n{ex.Message}", System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    try
                    {
                        string ffn = part7.FileName;
                        IFeature7 feature7 = (IFeature7)part7;
                        var VariableCollection = feature7.Variables[false, true];
                        if (VariableCollection != null)
                        {
                            try
                            {
                                foreach (Variable7 variable7 in VariableCollection)
                                {
                                    SetVariableLink(variable7, AllComponents, PartFileName);
                                }
                            }
                            catch
                            {
                                if (VariableCollection != null)
                                {
                                    Variable7 variable7 = (Variable7)VariableCollection;
                                    if(variable7 != null)
                                        SetVariableLink(variable7, AllComponents, PartFileName);
                                }
                            }
                            part7.RebuildModel(true);
                        }
                    }
                    catch(Exception ex1)
                    {
                        ShowMsgBox("Ошибка при изменении ссылок в переменных у документа" + PartFileName + Environment.NewLine + ex1.Message, MessageBoxIcon.Error);
                    }
                }
                _IEmbodimentsManager.Embodiment[currentEmbody].IsCurrent = true;
                document3D.Save();
            }
            catch (Exception Ex)
            {
                ShowMsgBox("Ошибка при изменении связанных файлов у документа" + PartFileName + Environment.NewLine + Ex.Message, MessageBoxIcon.Error);
            } 
            if(!OpenDoc) document3D.Close(DocumentCloseOptions.kdSaveChanges);
        }
        private void SetVariableLink(Variable7 variable7, List<TreeListNode> AllComponents, string PartFileName)
        {
            string ParamName = null;
            ParamName = variable7.Name;
            if (!string.IsNullOrEmpty(variable7.LinkDocumentName))
            {
                try
                {
                    string link_FileName = variable7.LinkDocumentName;
                    string New_link_FileName = GetFileNameByAllComponents(link_FileName, AllComponents, PartFileName);
                    if (!string.IsNullOrEmpty(New_link_FileName))
                    {
                        if (!IsExportFileName(link_FileName, AllComponents))
                        {
                            if (!File.Exists(New_link_FileName))
                                New_link_FileName = getSousrFilaName(link_FileName, AllComponents, ParamName);
                        }
                    }
                    else
                        New_link_FileName = getSousrFilaName(link_FileName, AllComponents, ParamName);
                    if (File.Exists(New_link_FileName))
                    {
                        try
                        {
                            variable7.SetLink(New_link_FileName, variable7.LinkVariableName);
                            //string LinkEmbodimentMarking = variable7.GetLinkEmbodimentMarking(ksVariantMarkingTypeEnum.ksVMFullMarking, true);
                            //variable7.SetLinkEmbodiment(New_link_FileName, variable7.LinkVariableName, LinkEmbodimentMarking);
                        }
                        catch
                        {
                            variable7.SetLink(New_link_FileName, variable7.LinkVariableName);
                        }

                    }
                }
                catch (Exception ex2)
                {
                    MessageBox.Show($"Ошибка при изменении переменной {variable7.Name}!\n{ex2.Message}", System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private List<string> LinkComponovGeometryDone = new List<string>();
        bool checExistListComponovGeometry(string DocumentFileName)
        {
            //проверяет список обработанных документов на компоновочную геометрию
            if (!LinkComponovGeometryDone.Contains(DocumentFileName))
            {
                LinkComponovGeometryDone.Add(DocumentFileName);
                return true; 
            }
            return false;
        }
        private List<ComponentInfo.CopyGeometry> GetCopyGeometries(IPart7 part7)
        {
            List<ComponentInfo.CopyGeometry> copyGeometries = new List<ComponentInfo.CopyGeometry>();

            IFeature7 feature7 = (IFeature7)part7;
            var VariableCollection = feature7.Variables[false, true];
            bool islayout = part7.IsLayoutGeometry;
            IModelContainer modelContainer = (IModelContainer)part7;
            ICopiesGeometry iCGS = modelContainer.CopiesGeometry;
            foreach (ICopyGeometry copyGeometry in iCGS)
            {
                string name = copyGeometry.Name;
                string reffn = copyGeometry.DocumentFileName;
                copyGeometries.Add(new ComponentInfo.CopyGeometry() { DocumentName = reffn, OperationName = name });
            }
            return copyGeometries;
        }
        public void SetLinkComponovGeometry(IKompasDocument3D document3D, List<TreeListNode> AllComponents)
        {
            if(document3D == null) return;
            IKompasDocument1 kompasDocument1 = null;
            string link_FileName = document3D.PathName;
            if (!checExistListComponovGeometry(link_FileName)) return;
            try
            {
                int currentEmbody = 0;
                IEmbodimentsManager _IEmbodimentsManager = (IEmbodimentsManager)document3D.TopPart;
                int EmbodyCount = _IEmbodimentsManager.EmbodimentCount;
                currentEmbody = _IEmbodimentsManager.CurrentEmbodimentIndex;

                for (int j = 0; j < EmbodyCount; j++)
                {
                    Embodiment tmp_Embodiment;
                    tmp_Embodiment = _IEmbodimentsManager.Embodiment[j];
                    //if (tmp_Embodiment.IsCurrent == true) { currentEmbody = j; }
                    tmp_Embodiment.IsCurrent = true;
                    if (tmp_Embodiment.Part == null) { IPart7NothingMsg(link_FileName); return; }
                    IPart7 part7 = tmp_Embodiment.Part;
                    
                    string ffn = part7.FileName;
                    IFeature7 feature7 = (IFeature7)part7;
                    var VariableCollection = feature7.Variables[false, true];
                    bool islayout = part7.IsLayoutGeometry;
                    IModelContainer modelContainer = (IModelContainer)part7;
                    ICopiesGeometry iCGS = modelContainer.CopiesGeometry;
                    foreach (ICopyGeometry copyGeometry in iCGS)
                    {
                        string name = copyGeometry.Name;
                        string reffn = copyGeometry.DocumentFileName; 
                        int refID = copyGeometry.Reference;
                        string New_link_FileName = GetCopiFileNameByAllComponents(link_FileName, name, AllComponents);
                        if (!string.IsNullOrEmpty(New_link_FileName))
                        {
                            if (File.Exists(New_link_FileName))
                            {
                                try
                                {
                                    if (kompasDocument1 == null) kompasDocument1 = (IKompasDocument1)document3D;
                                    if (kompasDocument1 != null)
                                    {
                                        bool res = kompasDocument1.ReplaceExternalFilesNames(true, copyGeometry.DocumentFileName, New_link_FileName);
                                        if (res) kompasDocument1.RedrawDocument(ksRedrawDocumentModeEnum.ksRedrawFull);
                                    }
                                }
                                catch(Exception ex2)
                                {
                                    MessageBox.Show($"Ошибка при изменении источника у компоновочной геометрии {link_FileName}!\nOldFileName:{reffn}\nNewfilename:{New_link_FileName}\n{ex2.Message}", System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        
                    }
                    
                }
                _IEmbodimentsManager.Embodiment[currentEmbody].IsCurrent = true;
                document3D.Save();
            }
            catch (Exception Ex)
            {
                ShowMsgBox($"Ошибка при изменении источника у компоновочной геометрии {link_FileName}!\n{Ex.Message}", MessageBoxIcon.Error);
            }
        }
        private void SetLinkForCopyGeometry(IPart7 part7, string NewDocReference)
        {
            //процедура изменения ссылок файл-источника, в операциях Копировать объекта
            IModelContainer modelContainer = (IModelContainer)part7;
            ICopiesGeometry iCGS = modelContainer.CopiesGeometry;
            foreach (ICopyGeometry copyGeometry in iCGS)
            {
                string name = copyGeometry.Name;
                string old_ref_fl = copyGeometry.DocumentFileName;
                //свойство DocumentFileName доступно только для чтения, как изменить ссылку на другой файл-источник?
                //copyGeometry.DocumentFileName = NewDocReference;
            }
        }

        private string getSousrFilaName(string PartFileName, List<TreeListNode> AllComponents, string ParamName)
        {
            foreach (TreeListNode node in AllComponents)
            {
                if (node.GetValue("Имя файла").ToString() == Path.GetFileNameWithoutExtension(PartFileName))
                {
                    ComponentInfo componentInfo = (ComponentInfo)node.Tag;
                    foreach (ComponentInfo.Variable_Class Link_Variable in componentInfo.Referense_Variable_List)
                    {
                        if (Link_Variable.Name == ParamName)
                        {
                            return Link_Variable.SourceFileName;
                        }
                    }
                    return componentInfo.FFN;
                }
            }
            return null;
        }
        #endregion
        public void GetAttr(string filname)
        {
            IKompasDocument kompasDocument0 = (IKompasDocument)_IApplication.Documents.Open(filname, true, false);
            IKompasDocument1 kompasDocument = (IKompasDocument1)kompasDocument0;
            IAttrTypeMng attrTypeMng = (IAttrTypeMng)_IApplication;
            var attributes = kompasDocument.Attributes[0, 0, 0, 0, 0, kompasDocument0];
            Dictionary<string, string> attr = new Dictionary<string, string>();
            foreach(IAttribute _IAttribute in attributes)
            {
                attr.Add(_IAttribute.AttributeType.TypeName, (string)(_IAttribute.Value[0,0]) );
            }
        }
    }
}