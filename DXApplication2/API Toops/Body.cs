﻿using Kompas6API5;
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
        public bool All_Level_Search = false;
        public bool Split_Naim = false;
        public bool OnlySheetMetalls = false;
        public static bool thisFirstMessage = false;
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
                            if (item.Hidden != true)
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
            componentInfo_Copy.Key = Part.FileName + "|" + Part.Marking + "|" + _body.Marking;
            Dictionary<string, string> ParamValueList = new Dictionary<string, string>();
            foreach (string ParamName in FindParam_Model)
            {
                string ParamValue = null;
                ParamValue = OptionsFold.tools_class.FixInvalidChars_St(GetPropertyBodyIPart7(Part, _body, ParamName), "");
                if (Split_Naim && ParamName == "Наименование") { ParamValue = OptionsFold.tools_class.SplitString(ParamValue); }
                ParamValueList.Add(ParamName, ParamValue);
            }
            componentInfo_Copy.Body.ParamValueList = ParamValueList;
            return componentInfo_Copy;
        }
        private void getBodyResoure(IPart7 Part, ComponentInfo componentInfo, TreeListNode node)
        {
            IFeature7 feature = (IFeature7)Part;
            var RB = feature.ResultBodies;
            if (RB == null) return;
            try
            {
                foreach (IBody7 _body in RB)
                {
                    try
                    {
                        ComponentInfo componentInfo_Copy = Add_BodyInfo_In_Component(Part, _body, componentInfo);

                        TreeListNode TempNode;
                        TempNode = AddNode(node, componentInfo_Copy, true);
                        TempNode.Tag = componentInfo_Copy;
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
                TempNode.Tag = componentInfo_Copy; 
            }
        }
        private double GetQNTInParts(IPart7 Find_Item, IPart7 TopPart)
        {
            double QNT = 0;
            var Parts = TopPart.Parts;
            foreach (IPart7 item in Parts)
            {
                try
                {
                    if (item.Hidden != true)
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
                    if (item.Hidden != true)
                        PartList.Add(item.FileName + "|" + item.Marking);
                }
                catch { }
            }
            return PartList;
        }
        private void AddCellsInNode(TreeListNode Node, ComponentInfo componentInfo)
        {
            Dictionary<string, string> ParValues = componentInfo.ParamValueList;
            foreach(string FieldName in FindParam_Model)
            {
                Node.SetValue(FieldName, ParValues[FieldName]);
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
                            QNT = temp_componentInfo.Body.QNT + _componentInfo.Body.QNT;
                        else
                            QNT = temp_componentInfo.QNT + _componentInfo.QNT;
                        ChildNode = TempNode;
                        ChildNode.SetValue("Количество", QNT);
                        ChildNode.SetValue("Количество общ.", GetTotalQNT(ChildNode));
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
            ChildNode.SetValue("Количество общ.", GetTotalQNT(ChildNode));
            return ChildNode;
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
        }

        public static void CloseInvisibleDocument(string FullFileName)
        {
            //процедура закрывает документ
            IKompasDocument _IKompasDocument = (IKompasDocument)_IApplication.Documents[FullFileName];
            if (_IKompasDocument != null && _IKompasDocument.Visible == false) _IKompasDocument.Close(DocumentCloseOptions.kdDoNotSaveChanges);
        }
        private ComponentInfo GetParam(ksPart part)
        {
            ksMassInertiaParam massInertiaParam = (ksMassInertiaParam)part.CalcMassInertiaProperties((uint)ksLengthUnitsEnum.ksLUnMM);
            IPropertyMng propertyManager = part as IPropertyMng;
            //string oboz = propertyManager.GetProperty(kompasDocument, 4); //обозначение
            //string SSect = propertyManager.GetProperty(kompasDocument, 20);  //раздел спецификации


            ComponentInfo iMSH = new ComponentInfo();
            iMSH._MCH.Area = massInertiaParam.F;
            iMSH._MCH.Mass = massInertiaParam.m;
            iMSH._MCH.Volume = massInertiaParam.v;
            iMSH._MCH.Xc= massInertiaParam.xc;
            iMSH._MCH.Yc = massInertiaParam.yc;
            iMSH._MCH.Zc = massInertiaParam.zc;

            iMSH.FFN = part.fileName;
            iMSH.material = part.material;
            iMSH.Oboz = part.marking;
            iMSH.Naim = part.name;
            iMSH.Mass = part.GetMass();
            iMSH.isDetal = part.IsDetail();
            iMSH.standardComponent = part.standardComponent;
            iMSH.QNT = 1;
            iMSH.Key = part.fileName + "|" + part.marking;
            return iMSH;
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
            massInertiaParam.Calculate();
            massInertiaParam.LengthUnits = ksLengthUnitsEnum.ksLUnMM;
            massInertiaParam.MassUnits = ksMassUnitsEnum.ksMUnKG;

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
            iMSH._MCH.Xc = massInertiaParam.Xc;
            iMSH._MCH.Yc = massInertiaParam.Yc;
            iMSH._MCH.Zc = massInertiaParam.Zc; 
            iMSH._MCH.Area = massInertiaParam.Area;
            iMSH._MCH.Volume = massInertiaParam.Volume;
            iMSH.FFN = part.FileName;
            iMSH.material = part.Material;
            bool isSHeetmetall = false;
            iMSH.SheeMetall = IsSheetMetal(part, out isSHeetmetall);
            iMSH.HaveUnfold = GetHasFlatPattern(part);

            iMSH.isDetal = part.Detail;
            iMSH.standardComponent = part.Standard;
            iMSH.Key = ComponentKey;
            return iMSH;
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
    }
}