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

namespace SaveDXF
{
    public class Body
    {
        public static string KompasVersion;
        public static string KompasVersionFlag= "КОМПАС-3D v18";
        public static bool AppVersNOTValid; /*проверяет валидность данной версии компаса с версией КОМПАС-3D v18.1*/
        public static bool AppVersNOTValidStrong; /*проверяет валидность данной версии компаса с версией КОМПАС-3D v18.1*/ 
        public static KompasObject _kompasObject;
        public static IApplication _IApplication;
        public List<string> FindParam_Model;
        public List<object> FindModel_List;
        public List<string> OpenDocsPERED_Start;
        public bool All_Level_Search = false;
        public bool Split_Naim = false;
        public bool Add_Drw = false;
        public bool OnlySheetMetalls = false;
        public static bool thisFirstMessage = false;
        CFG_Class optionClassInBody;
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
            AppVersNOTValid = Convert.ToBoolean(string.Compare(KompasVersion, KompasVersionFlag));
            AppVersNOTValidStrong = System.Text.RegularExpressions.Regex.IsMatch(KompasVersion, KompasVersionFlag);
        }
        public static bool AppVersNOTValidStrongMessage()
        {
            if (!AppVersNOTValidStrong)
                MessageBox.Show($"Версия CAD-системы({KompasVersion}) не совпадает с рекомендованной версией {KompasVersionFlag}." +
                                Environment.NewLine +  $"Обновите {System.Windows.Forms.Application.ProductName} до требуемой версии CAD системы, либо установите {KompasVersionFlag}", System.Windows.Forms.Application.ProductName);
            return AppVersNOTValidStrong;
        } 
        public void OpenDocument(string FullFileName)
        {
            //процедура открывает документ из списка
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
            if (TopPart == null) { IPart7NothingMsg(_IKompasDocument.Path); return; }

            CheckMainControl();
            treeView.Nodes.Clear(); 

            TreeListNode node = treeView.Nodes.Add();
            if (node.Tag == null)
            {
                node.Tag = GetParam(TopPart);
                if(Add_Drw) AddDrwNode(node, (ComponentInfo)node.Tag);
                AddCellsInNode(node, (ComponentInfo)node.Tag);
            }
            Recource(TopPart, node);
            node.ExpandAll();
            CloseDocs();
        }
        private void CheckMainControl()
        {
            waitMng = ((MainForm)System.Windows.Forms.Application.OpenForms["MainForm"]).splashScreenManager2;
            treeView = ((MainForm)System.Windows.Forms.Application.OpenForms["MainForm"]).treeList1;
            FindParam_Model = ColumnsConf_Save_Read.FindParams();   //получаем список искомых параметров
            FindModel_List = new List<object>();                    //обнуляем список обработанных файлов
            optionClassInBody = ((MainForm)System.Windows.Forms.Application.OpenForms["MainForm"]).Main_Options;
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
                        if (Add_Drw) AddDrwNode(node, (ComponentInfo)node.Tag);
                        AddCellsInNode(node, (ComponentInfo)node.Tag); 
                    }
                    Recource(tmp_Embodiment.Part, node);
                    node.ExpandAll();
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
        public void Recource(IPart7 TopPart, TreeListNode node)
        //процедура перебирает компоненты в сборке
        { 
            if (TopPart != null)
            {
                if(!TopPart.Detail) getBodyResoure(TopPart, (ComponentInfo)node.Tag, node);
                var Parts = TopPart.PartsEx[1];
                if (Parts != null)
                {
                    foreach (IPart7 item in Parts)
                    {
                        try
                        {
                            bool ItemHidden = item.Hidden;
                            if (optionClassInBody.Add_InVisiblePart.Value) ItemHidden = false;
                            if (ItemHidden != true)
                            {
                                ComponentInfo componentInfo = GetParam(item);
                                AddWaitStatus(Path.GetFileNameWithoutExtension(componentInfo.FFN));
                                if (!componentInfo.QNT_False)
                                    componentInfo.QNT = GetQNTIn_PartsList(item.FileName + "|" + item.Marking, GetPartList(TopPart));
                                try { componentInfo.ParamValueList["Количество"] = componentInfo.QNT.ToString(); } catch { }
                                
                                TreeListNode TempNode;
                                TempNode = AddNode(node, componentInfo, false);
                                TempNode.Tag = componentInfo;
                                //getBodyResoure(item, componentInfo, TempNode);
                                if (!item.Detail && All_Level_Search) 
                                {
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
            double kolvo = Convert.ToDouble(GetPropertyBodyIPart7(Part, _body, "Количество"));
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
            componentInfo_Copy.Key = Part.FileName + "|" + Part.Marking + "|" + _body.Marking;
            Dictionary<string, string> ParamValueList = new Dictionary<string, string>();
            foreach (string ParamName in FindParam_Model)
            {
                string ParamValue = null;
                ParamValue = OptionsFold.tools_class.FixInvalidChars_St(GetPropertyBodyIPart7(Part, _body, ParamName), "");
                if (Split_Naim && ParamName == "Наименование") { ParamValue = OptionsFold.tools_class.SplitString(ParamValue); componentInfo_Copy.Body.Naim = ParamValue; }
                ParamValueList.Add(ParamName, ParamValue);
            }
            componentInfo_Copy.Body.ParamValueList = ParamValueList;
            return componentInfo_Copy;
        }
        private void getBodyResoure(IPart7 Part, ComponentInfo componentInfo, TreeListNode node)
        {
            //bool IsOpen = false;
            //IKompasDocument3D kompasDocument = (IKompasDocument3D)_IApplication.Documents[Part.FileName];
            //if(kompasDocument != null) IsOpen = true; 
            //else
            //    kompasDocument = (IKompasDocument3D)_IApplication.Documents.Open(Part.FileName, true, true);
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
                            ComponentInfo componentInfo_Copy = Add_BodyInfo_In_Component(Part, _body, componentInfo);

                            TreeListNode TempNode;
                            TempNode = AddNode(node, componentInfo_Copy, true); 
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
        private double GetQNTInParts(IPart7 Find_Item, IPart7 TopPart)
        {
            double QNT = 0;
            var Parts = TopPart.Parts;
            foreach (IPart7 item in Parts)
            {
                try
                {
                    bool ItemHidden = item.Hidden;
                    if (optionClassInBody.Add_InVisiblePart.Value) ItemHidden = false;
                    if (ItemHidden != true)
                    {
                        if (Find_Item.FileName + "|" + Find_Item.Marking == item.FileName + "|" + item.Marking) 
                            QNT += 1;
                    }
                }
                catch{}
            }
            return QNT;
        }
        private double GetQNTIn_PartsList(string Find_Item_Key, List<string> PartList)
        {
            double QNT = 0;
            foreach (string item in PartList)
            {
                if (item == Find_Item_Key) QNT += 1;
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
                    bool ItemHidden = item.Hidden;
                    if (optionClassInBody.Add_InVisiblePart.Value) ItemHidden = false;
                    if (ItemHidden != true)
                        PartList.Add(item.FileName + "|" + item.Marking);
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
                    Node.ImageIndex = 10;
                    Node.SelectImageIndex = 10;
                    Node.StateImageIndex = 10;
                    break;
                case ".SPW":
                    Node.ImageIndex = 11;
                    Node.SelectImageIndex = 11;
                    Node.StateImageIndex = 11;
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
                    default:
                        Node.SetValue(FieldName, ParValues[FieldName]);
                        break;
                }
            }
            if (componentInfo.standardComponent)
            {
                Node.ImageIndex = 7;
                Node.SelectImageIndex = 7;
                Node.StateImageIndex = 7;
            }
            else
            {
                Node.ImageIndex = 0;
                Node.SelectImageIndex = 0;
                Node.StateImageIndex = 0;
            }
            string RazdelSP = "";

            if (ParValues.ContainsKey("Раздел спецификации")) RazdelSP = ParValues["Раздел спецификации"];

            if (!string.IsNullOrEmpty(RazdelSP)) SetNodeImageIndex_By_Section_Name(RazdelSP, Node);

            if (componentInfo.isDetal && !componentInfo.standardComponent)
            {
                Node.ImageIndex = 4;
                Node.SelectImageIndex = 4;
                Node.StateImageIndex = 4;
            }
            if (componentInfo.SheeMetall)
            {
                if (componentInfo.HaveUnfold)
                {
                    Node.ImageIndex = 9;
                    Node.SelectImageIndex = 9;
                    Node.StateImageIndex = 9;
                }
                else
                {
                    Node.ImageIndex = 8;
                    Node.SelectImageIndex = 8;
                    Node.StateImageIndex = 8;
                }
            }
        }
        private void AddCellsInNode(TreeListNode Node, ComponentInfo.Get_Body _Body)
        {
            Dictionary<string, string> ParValues = _Body.ParamValueList;
            foreach (string FieldName in FindParam_Model)
            {
                Node.SetValue(FieldName, ParValues[FieldName]);
            }
            Node.ImageIndex = 3;
            Node.SelectImageIndex = 3;
            Node.StateImageIndex = 3;

            string RazdelSP = "";

            try
            {
                RazdelSP = ParValues["Раздел спецификации"];
                if (!string.IsNullOrEmpty(RazdelSP)) SetNodeImageIndex_By_Section_Name(RazdelSP, Node);
            }
            catch { }

        }
        private void SetNodeImageIndex_By_Section_Name(string Section_Name, TreeListNode Node)
        {

            switch (Section_Name)
            {
                case "Детали":
                    Node.ImageIndex = 4;
                    Node.SelectImageIndex = 4;
                    Node.StateImageIndex = 4;
                    break;
                case "Материалы":
                    Node.ImageIndex = 3;
                    Node.SelectImageIndex = 3;
                    Node.StateImageIndex = 3;
                    break;
                case "Сборочные единицы":
                    Node.ImageIndex = 0;
                    Node.SelectImageIndex = 0;
                    Node.StateImageIndex = 0;
                    break;
                case "Прочие изделия":
                    Node.ImageIndex = 5;
                    Node.SelectImageIndex = 5;
                    Node.StateImageIndex = 5;
                    break;
                case "Стандартные изделия":
                    Node.ImageIndex = 7;
                    Node.SelectImageIndex = 7;
                    Node.StateImageIndex = 7;
                    break;
                case "Комплекты":
                    Node.ImageIndex = 6;
                    Node.SelectImageIndex = 6;
                    Node.StateImageIndex = 6;
                    break;
                case "Комплексы":
                    Node.ImageIndex = 2;
                    Node.SelectImageIndex = 2;
                    Node.StateImageIndex = 2;
                    break;
                case "Документация":
                    Node.ImageIndex = 2;
                    Node.SelectImageIndex = 2;
                    Node.StateImageIndex = 2;
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
                        ChildNode.Tag = temp_componentInfo;
                        return ChildNode;
                    }
                }
            }
            ChildNode = ThisNode.Nodes.Add();

            ComponentInfo FindModel_Item = (ComponentInfo)_componentInfo.Clone();
            //FindModel_Item.QNT = 1;
            FindModel_List.Add(FindModel_Item);
            if (!AddBodyTree)
                AddCellsInNode(ChildNode, _componentInfo);
            else
                AddCellsInNode(ChildNode, _componentInfo.Body);
            _componentInfo.Total_QNT = GetTotalQNT(ChildNode);
            ChildNode.SetValue("Количество общ.", _componentInfo.Total_QNT);
            ChildNode.Tag = _componentInfo;

            if (Add_Drw) AddDrwNode(ChildNode, _componentInfo);

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
                part_ = _IKompasDocument3D.TopPart;
                if (part_ != null)
                {
                    IFeature7 IF = (IFeature7)part_;
                    Object[] ars = null;
                    if (IF != null && IF.VariablesCount[false, true] > 0)
                    {
                        ars = IF.Variables[false, true];
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
                docs.Add(document.PathName);
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
        private ComponentInfo GetParam(IPart7 part)
        {
            string ComponentKey = part.FileName + "|" + part.Marking;

            //FindModel_List
            ComponentInfo iMSH = GetExistNode_By_ComponentKey(ComponentKey);
            if (iMSH != null) return iMSH;

            IMassInertiaParam7 massInertiaParam = (IMassInertiaParam7)part;
            try
            {
                massInertiaParam.Calculate();
                massInertiaParam.LengthUnits = ksLengthUnitsEnum.ksLUnMM;
                massInertiaParam.MassUnits = ksMassUnitsEnum.ksMUnKG;
            }
            catch { }

            iMSH = new ComponentInfo();
            Dictionary<string, string> ParamValueList = new Dictionary<string, string>();
            //iMSH.ParamValueList = FindParam_Model;
            foreach (string ParamName in FindParam_Model)
            {
                string ParamValue = "";
                switch (ParamName)
                {
                    case "Обозначение":
                        ParamValue = OptionsFold.tools_class.FixInvalidChars_St(part.Marking, "");
                        break;
                    case "Имя файла":
                        ParamValue = System.IO.Path.GetFileNameWithoutExtension(part.FileName);
                        break;
                    case "Путь файла":
                        ParamValue = part.FileName;
                        break;
                    case "Расположение файла":
                        ParamValue = System.IO.Path.GetDirectoryName(part.FileName);
                        break;
                    case "Наименование":
                        ParamValue = OptionsFold.tools_class.FixInvalidChars_St(part.Name, "");
                        if (Split_Naim) ParamValue = OptionsFold.tools_class.SplitString(ParamValue);
                        break;
                    case "Масса":
                        ParamValue = OptionsFold.tools_class.FixInvalidChars_St(Math.Round(part.Mass, 3).ToString(), "");
                        break;
                    case "Материал":
                        ParamValue = OptionsFold.tools_class.FixInvalidChars_St(part.Material, "");
                        break;
                    case "Толщина":
                        ParamValue = Convert.ToString(GetThicknessPart(part, true));
                        break;
                    case "Количество":
                        ParamValue = OptionsFold.tools_class.FixInvalidChars_St(GetPropertyIPart7(part, ParamName), "");
                        if (string.IsNullOrEmpty(ParamValue)) ParamValue = "1";
                        break;
                    case "Количество общ.":
                        //ParamValue = OptionsFold.tools_class.FixInvalidChars_St(GetPropertyIPart7(part, ParamName), "");
                        //if (string.IsNullOrEmpty(ParamValue)) ParamValue = "1";
                        break;
                    case "Площадь":
                        ParamValue = Math.Round(massInertiaParam.Area, 3).ToString();
                        break;
                    case "Объем":
                        ParamValue = Math.Round(massInertiaParam.Volume, 3).ToString();
                        break;
                    case "Xc":
                        ParamValue = massInertiaParam.Xc.ToString();
                        break;
                    case "Yc":
                        ParamValue = massInertiaParam.Yc.ToString();
                        break;
                    case "Zc":
                        ParamValue = massInertiaParam.Zc.ToString();
                        break;
                    case "Полное имя файла":
                        ParamValue = part.FileName;
                        break;
                    //case "Наличие развертки":
                    //    if (GetHasFlatPattern(part))
                    //        ParamValue = "1";
                    //    else
                    //        ParamValue = "-1";
                    //    break;
                    default:
                        ParamValue = OptionsFold.tools_class.FixInvalidChars_St(GetPropertyIPart7(part, ParamName), "");
                        //ParamValue = OptionsFold.tools_class.FixInvalidChars_St(GetPropertyIPart7(part, ParamName), "");
                        break;
                }
                ParamValueList.Add(ParamName, ParamValue);
            }
            iMSH.ParamValueList = ParamValueList;
            double Kolvo = Convert.ToDouble(GetPropertyIPart7(part, "Количество"));
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
            if (Split_Naim) iMSH.Naim = OptionsFold.tools_class.SplitString(part.Name);
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

            iMSH.isDetal = part.Detail;
            iMSH.standardComponent = part.Standard;
            iMSH.Key = ComponentKey;
            //4555
            ShellFile shellFile = ShellFile.FromFilePath(part.FileName);
            iMSH.Slide = shellFile.Thumbnail.SmallBitmap;
            iMSH.LargeSlide = shellFile.Thumbnail.LargeBitmap;
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
        private long GetFileSize(string FileName)
        {
            if (File.Exists(FileName))
            {
                FileInfo fileInfo = new FileInfo(FileName);
                return fileInfo.Length;
            }
            return -1;
        }
        private ComponentInfo.Drw_Info_Class getDrwParam(string drw_Name)
        {
            ComponentInfo.Drw_Info_Class drw_Info = new ComponentInfo.Drw_Info_Class();
            drw_Info.FFN = drw_Name;
            drw_Info.FL_Size = GetFileSize(drw_Name);
            drw_Info.Naim = OptionsFold.tools_class.FixInvalidChars_St(GetPropertyIDrw(drw_Name, "Наименование"), "");
            drw_Info.Oboz= OptionsFold.tools_class.FixInvalidChars_St(GetPropertyIDrw(drw_Name, "Обозначение"), "");
            
            ShellFile shellFile = ShellFile.FromFilePath(drw_Name);
            drw_Info.Slide = shellFile.Thumbnail.SmallBitmap;
            drw_Info.LargeSlide = shellFile.Thumbnail.LargeBitmap;

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
                    if (AppVersNOTValid) return GetThickBeVarible(Part_, true);/*если версия компаса не валидна*/
                    if (pSheetMetalContainer.SheetMetalRuledShells.Count != 0)
                    {
                        ISheetMetalBody obech = pSheetMetalContainer.SheetMetalRuledShells.SheetMetalBody[0];
                        return obech.Thickness;
                    } 
                }
            }
            catch  { } 
            return -1;
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
            if (!AppVersNOTValid) return true;
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
        public void SetFieldValue(string FFN, string FieldValue)
        {

        }
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
        #endregion

        #region копировать Проект
        List<string> SetLinkVariableCompeteFile_Lise;
        bool OpenVisible = true;
        public void SetLinks(List<TreeListNode> AllComponents)
        {
            //_IApplication.Visible = false;
            SetLinkVariableCompeteFile_Lise = new List<string>();
            List<string> CopyFileList = new List<string>();
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

                    File.Copy(componentInfo.FFN, CopyFileName, true);
                    CopyFileList.Add(CopyFileName);
                }
            }
            _IApplication.HideMessage = ksHideMessageEnum.ksHideMessageNo; //отключаем все сообщения от компаса
            foreach (string FileName in CopyFileList)
            {
                waitMng.SetWaitFormDescription($"{Path.GetFileName(FileName)}");
                switch (Path.GetExtension(FileName).ToUpper())
                {
                    //case ".SPW":
                    //    SetLinkInSPDoc(FileName, AllComponents);
                    //    break;
                    case ".CDW":
                        SetLinkInDRW(FileName, AllComponents);
                        break;
                    case ".A3D": 
                        SetSourseChancge_ModelAPI7(FileName, AllComponents, GetDonorFileNameByAllComponents(FileName, AllComponents));
                        break;
                }
            }
            foreach (string FileName in CopyFileList)
            {
                switch (Path.GetExtension(FileName).ToUpper())
                {
                    case ".SPW":
                        waitMng.SetWaitFormDescription($"{Path.GetFileName(FileName)}");
                        SetLinkInSPDoc(FileName, AllComponents);
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
            dynamic Objects = iSpecificationDescription.Objects;
            if (Objects != null)
            {
                foreach (ISpecificationObject specificationObject in Objects)
                {
                    ksSpecificationObjectTypeEnum ObjectType = specificationObject.ObjectType;
                    if ((int)ObjectType == 1 || (int)ObjectType == 2)
                    {
                        AttachedDocuments attachedDocuments = specificationObject.AttachedDocuments;
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
            AttachedDocuments ModelattachedDocuments = iKompasDocument.AttachedDocuments;
            foreach (AttachedDocument attachedDocument in ModelattachedDocuments)
            {
                string Name = attachedDocument.Name;
                string NewFileName = GetFileNameByAllComponents(Name, AllComponents);
                if (!string.IsNullOrEmpty(NewFileName)) ModelattachedDocuments.Add(NewFileName, true);

                if (!string.IsNullOrEmpty(NewFileName)) ModelattachedDocuments.Add(NewFileName, true);
                attachedDocument.Delete();
            }

            iKompasDocument.RebuildDocument();
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

                arrAttachDoc = productDataMenager.ObjectAttachedDocuments[(IPropertyKeeper)part7];
                List<string> drwS = new List<string>();
                if (arrAttachDoc != null)
                    foreach (var tDoc in arrAttachDoc)
                    {
                        //if (!string.IsNullOrEmpty(tDoc.ToString()) && File.Exists(tDoc.ToString()))
                        //{
                            drwS.Add(tDoc.ToString());
                        //}
                    }
                foreach(string fn in drwS)
                {
                    productDataMenager.DeleteProductObject(fn);
                    iKompasDocument.RebuildDocument();
                }
                string ffn = @"C: \Users\admin_veza\Desktop\Новая папка(3)\01 - ЕЛГ 02.01.10.000 СБ Стойка нижняя.cdw";
                productDataMenager.AddProductObject((IPropertyKeeper)part7, ffn, ksProductObjectTypeEnum.ksPOTAllObjects);
                

                arrAttachDoc = productDataMenager.ObjectAttachedDocuments[(IPropertyKeeper)part7];
                drwS = new List<string>();
                if (arrAttachDoc != null)
                    foreach (var tDoc in arrAttachDoc)
                    {
                        if (!string.IsNullOrEmpty(tDoc.ToString()) && File.Exists(tDoc.ToString()))
                        {
                            drwS.Add(tDoc.ToString());
                        }
                    }
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
            foreach (TreeListNode node in AllComponents)
            {
                ComponentInfo componentInfo = (ComponentInfo)node.Tag;
                if (componentInfo.FFN == DonorFileName && node.Checked)
                    return $@"{node.GetValue("Сохранить в папке")}\{node.GetValue("Сохранить в имени")}{node.GetValue("Тип")}";
            }

            foreach (TreeListNode node in AllComponents)
            {
                ComponentInfo componentInfo = (ComponentInfo)node.Tag;
                if (Path.GetFileName(componentInfo.FFN) == Path.GetFileName(DonorFileName))
                    return $@"{node.GetValue("Сохранить в папке")}\{node.GetValue("Сохранить в имени")}{node.GetValue("Тип")}";
            }
            return null;
        }
        public void SetSourseChancge_ModelAPI7(string ExportFileName, List<TreeListNode> AllComponents, string DonorFileName)
        {
            bool OpenDoc = false;
            IKompasDocument3D document3D;
            document3D = (IKompasDocument3D)_IApplication.Documents[DonorFileName];
            if (document3D != null) OpenDoc = true;
            else
                document3D = (IKompasDocument3D)_IApplication.Documents.Open(DonorFileName, OpenVisible, false);
            if (document3D == null) return;
            document3D.SaveAs(ExportFileName);
            document3D = (IKompasDocument3D)_IApplication.Documents.Open(ExportFileName, OpenVisible, false); 

            IPart7 part7 = document3D.TopPart;
            if (part7 == null) { IPart7NothingMsg(ExportFileName); return; }
            int currentEmbody = 0;
            IEmbodimentsManager _IEmbodimentsManager = (IEmbodimentsManager)part7;
            int EmbodyCount = _IEmbodimentsManager.EmbodimentCount;
            for (int ii = 0; ii < EmbodyCount; ii++)
            {
                Embodiment tmp_Embodiment;
                tmp_Embodiment = _IEmbodimentsManager.Embodiment[ii];
                if (tmp_Embodiment.IsCurrent == true) { currentEmbody = ii; break; }
            }

            for (int j = 0; j < EmbodyCount; j++)
            {
                Embodiment tmp_Embodiment;
                tmp_Embodiment = _IEmbodimentsManager.Embodiment[j];
                //if (tmp_Embodiment.IsCurrent == true) { currentEmbody = j; }
                tmp_Embodiment.IsCurrent = true;
                if (tmp_Embodiment.Part == null) { IPart7NothingMsg(ExportFileName); return; }
                var Parts = tmp_Embodiment.Part.PartsEx[0];
                if (Parts != null)
                {
                    foreach (IPart7 part in Parts)
                    {
                        try
                        {
                            IFeature7 feature7 = (IFeature7)part; 
                            string Name = part.FileName;
                            string New_FileName = GetFileNameByAllComponents(Name, AllComponents);
                            if (!string.IsNullOrEmpty(New_FileName))
                            {
                                if (!Directory.Exists(Path.GetDirectoryName(New_FileName)))
                                    Directory.CreateDirectory(Path.GetDirectoryName(New_FileName));
                                part.FileName = New_FileName;
                                SetLinkInProperty_ModelAPI7(New_FileName, AllComponents);
                                part.Update();
                            }
                            if (!part.Detail)
                            {
                                string Part_ExportFileName = GetFileNameByAllComponents(part.FileName, AllComponents);
                                if (string.IsNullOrEmpty(Part_ExportFileName) && IsExportFileName(part.FileName, AllComponents)) SetSourseChancge_ModelAPI7(part.FileName, AllComponents, part.FileName);

                                if (!string.IsNullOrEmpty(Part_ExportFileName)) SetSourseChancge_ModelAPI7(Part_ExportFileName, AllComponents, part.FileName);
                                //SetSourseChancge_ModelAPI7(Part_ExportFileName, AllComponents, part.FileName);
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
            SetLinkInProperty_ModelAPI7(ExportFileName, AllComponents);
        }
        private bool IsLinkVariableCompeteFile(string PartFileName)
        {
            if (SetLinkVariableCompeteFile_Lise.Contains(PartFileName))
                return true;
            else
                SetLinkVariableCompeteFile_Lise.Add(PartFileName);
            return false;
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
            try
            {
                IPart7 part7 = document3D.TopPart;
                string ffn = part7.FileName;
                IFeature7 feature7 = (IFeature7)part7;
                var VariableCollection = feature7.Variables[false, true];
                if (VariableCollection != null)
                {
                    foreach (Variable7 variable7 in VariableCollection)
                    {
                        ParamName = variable7.Name;
                        if (!string.IsNullOrEmpty(variable7.LinkDocumentName))
                        {
                            string link_FileName = variable7.LinkDocumentName;
                            string New_link_FileName = GetFileNameByAllComponents(link_FileName, AllComponents);
                            if (!string.IsNullOrEmpty(New_link_FileName) && !IsExportFileName(link_FileName, AllComponents)) variable7.SetLink(New_link_FileName, variable7.LinkVariableName);
                            part7.RebuildModel(true);
                        }
                    }
                }
                document3D.Save();
            }
            catch (Exception Ex)
            {
                ShowMsgBox("Ошибка при изменении связанных файлов у документа" + PartFileName + Environment.NewLine + Ex.Message, MessageBoxIcon.Error);
            } 
            if(!OpenDoc) document3D.Close(DocumentCloseOptions.kdSaveChanges);
        }
        #endregion
    }
}