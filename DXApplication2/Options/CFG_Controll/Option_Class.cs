using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class Option_Class
{
    public VisibleControls IVC = new VisibleControls();
    public CalcSheetMetalProperty ICShMProperty = new CalcSheetMetalProperty();
    public string Mass_MU_Name { get; set; }
    public string Length_MU_Name { get; set; }
    public string Area_MU_Name { get; set; }
    public string Volume_MU_Name { get; set; }
    public string Count_MU_Name { get; set; }

    public int Mass_MU_Value { get; set; }
    public int Length_MU_Value { get; set; }
    public int Area_MU_Value { get; set; }
    public int Volume_MU_Value { get; set; }
    public int Count_MU_Value { get; set; }
    public int TreeStatus_Value { get; set; }
    public bool Positio_On_Value { get; set; }
    public bool Material_In_Assemly { get; set; }
    public string Positio_Split_Value { get; set; }
    public bool Split_Naim { get; set; } = false;
    public bool Add_Drw { get; set; } = false;
    public bool Qnt_On_Line_In_Visual { get; set; } = false;
    public bool Dublicate_In_Visual { get; set; } = false;
    public bool All_Level_In_AllReport { get; set; } = false;
    public bool Check_ProfileValue { get; set; } = false;
    public bool AddTreeListForStandartKomponent { get; set; } = false;
    public Option_Class()
    {
        Mass_MU_Value = 1;
        Length_MU_Value = 1;
        Area_MU_Value = 1;
        Volume_MU_Value = 1;
        Count_MU_Value = 0;
        TreeStatus_Value = 2;

        Mass_MU_Name = "Килограммы";
        Length_MU_Name = "Миллиметры";
        Area_MU_Name = "Миллиметры кв.";
        Volume_MU_Name = "Миллиметры куб.";
        Count_MU_Name = "Штуки";
    }
    public enum MU_Length
    {
        ksLUnSM = 0,
        ksLUnMM = 1,
        ksLUnDM = 2,
        ksLUnM = 3,
        ksLUnDocument = 4
    }
    public enum MU_Mass
    {
        ksMUnGR = 0,
        ksMUnKG = 1,
        ksMUnDocument = 4
    }
    public enum Obj_Type_Enum
    {
        Document = 0,
        KompleKS = 1,
        Assembly = 2,
        Part = 3,
        Part_With_Unfold = 4,
        Part_NOT_Unfold = 5,
        Standart = 6,
        Pro4ee = 7,
        Material= 8,
        KompleKT = 9,
        Drawing = 10,
        Specification = 11
    }

    public static RepositoryItemImageComboBox GetRepositoryItemImageComboBox(object ImageList)
    {
        RepositoryItemImageComboBox rep = new RepositoryItemImageComboBox();

        rep.SmallImages = ImageList;
        rep.Items.Add(new ImageComboBoxItem("Сборка", (int)Option_Class.Obj_Type_Enum.Assembly, (int)Option_Class.Obj_Type_Enum.Assembly));
        rep.Items.Add(new ImageComboBoxItem("Документ", (int)Option_Class.Obj_Type_Enum.Document, (int)Option_Class.Obj_Type_Enum.Document));
        rep.Items.Add(new ImageComboBoxItem("Комплект", (int)Option_Class.Obj_Type_Enum.KompleKT, (int)Option_Class.Obj_Type_Enum.KompleKT));
        rep.Items.Add(new ImageComboBoxItem("Материал", (int)Option_Class.Obj_Type_Enum.Material, (int)Option_Class.Obj_Type_Enum.Material));
        rep.Items.Add(new ImageComboBoxItem("Деталь", (int)Option_Class.Obj_Type_Enum.Part, (int)Option_Class.Obj_Type_Enum.Part));
        rep.Items.Add(new ImageComboBoxItem("Прочее изделие", (int)Option_Class.Obj_Type_Enum.Pro4ee, (int)Option_Class.Obj_Type_Enum.Pro4ee));
        rep.Items.Add(new ImageComboBoxItem("Комплекс", (int)Option_Class.Obj_Type_Enum.KompleKS, (int)Option_Class.Obj_Type_Enum.KompleKS));
        rep.Items.Add(new ImageComboBoxItem("Стандартное изделие", (int)Option_Class.Obj_Type_Enum.Standart, (int)Option_Class.Obj_Type_Enum.Standart));
        rep.Items.Add(new ImageComboBoxItem("Деталь листовая без развертки", (int)Option_Class.Obj_Type_Enum.Part_NOT_Unfold, (int)Option_Class.Obj_Type_Enum.Part_NOT_Unfold));
        rep.Items.Add(new ImageComboBoxItem("Деталь листовая с разверткой", (int)Option_Class.Obj_Type_Enum.Part_With_Unfold, (int)Option_Class.Obj_Type_Enum.Part_With_Unfold));
        rep.Items.Add(new ImageComboBoxItem("Чертеж", (int)Option_Class.Obj_Type_Enum.Drawing, (int)Option_Class.Obj_Type_Enum.Drawing));
        rep.Items.Add(new ImageComboBoxItem("Спецификация", (int)Option_Class.Obj_Type_Enum.Specification, (int)Option_Class.Obj_Type_Enum.Specification));
        return rep;
    }
    public enum TreeStatus_Enum
    {
        treeStatus_None = 0,
        treeStatus_Expand = 1,
        treeStatus_ExpandAll = 2
    }
    public string GetTreeStatusNameByStatusEnum(TreeStatus_Enum TreeStatus)
    {
        switch (TreeStatus)
        {
            case TreeStatus_Enum.treeStatus_Expand:
                return "Раскрыть";
            case TreeStatus_Enum.treeStatus_ExpandAll:
                return "Раскрыть все";
            case TreeStatus_Enum.treeStatus_None:
                return "Свернуть";
        }
        return null;
    }
    public TreeStatus_Enum GetTreeStatusEnumByStatusName(string TreeStatusName)
    {
        switch (TreeStatusName)
        {
            case "Раскрыть":
                return TreeStatus_Enum.treeStatus_Expand;
            case "Раскрыть все":
                return TreeStatus_Enum.treeStatus_ExpandAll;
            case "Свернуть":
                return TreeStatus_Enum.treeStatus_None;
        }
        return TreeStatus_Enum.treeStatus_None;
    }
    public enum MU_Count
    {
        ksMUnSht = 0
    }
    public void Set_MU_Mass(string Mass_MU_text)
    {
        switch (Mass_MU_text)
        {
            case "Граммы":
                Mass_MU_Value = (int)MU_Mass.ksMUnGR;
                break;
            case "Килограммы":
                Mass_MU_Value = (int)MU_Mass.ksMUnKG;
                break;
            case "Настройки документа":
                Mass_MU_Value = (int)MU_Mass.ksMUnDocument;
                break;
        }
        this.Mass_MU_Name = Mass_MU_text;
    }

    public void Set_MU_Count(string Count_MU_text)
    {
        switch (Count_MU_text)
        {
            case "Штуки":
                this.Count_MU_Value = (int)MU_Count.ksMUnSht;
                break;
        }
        this.Count_MU_Name = Count_MU_text;
    }
    public void Set_MU_Length(string Length_MU_text)
    {
        switch (Length_MU_text)
        {
            case "Сантиметры":
                this.Length_MU_Value = (int)MU_Length.ksLUnSM;
                break;
            case "Миллиметры":
                this.Length_MU_Value = (int)MU_Length.ksLUnMM;
                break;
            case "Дециметры":
                this.Length_MU_Value = (int)MU_Length.ksLUnDM;
                break;
            case "Метры":
                this.Length_MU_Value = (int)MU_Length.ksLUnM;
                break;
            case "Настройки документа":
                this.Length_MU_Value = (int)MU_Length.ksLUnDocument;
                break;
        }
        this.Length_MU_Name = Length_MU_text;
    }
    public void Set_MU_Area(string Area_MU_text)
    {
        switch (Area_MU_text)
        {
            case "Сантиметры кв.":
                this.Area_MU_Value = (int)MU_Length.ksLUnSM;
                break;
            case "Миллиметры кв.":
                this.Area_MU_Value = (int)MU_Length.ksLUnMM;
                break;
            case "Дециметры кв.":
                this.Area_MU_Value = (int)MU_Length.ksLUnDM;
                break;
            case "Метры кв.":
                this.Area_MU_Value = (int)MU_Length.ksLUnM;
                break;
            case "Настройки документа":
                this.Area_MU_Value = (int)MU_Length.ksLUnDocument;
                break;
        }
        this.Area_MU_Name = Area_MU_text;
    }
    public void Set_MU_Volume(string Volume_MU_text)
    {
        switch (Volume_MU_text)
        {
            case "Сантиметры куб.":
                this.Volume_MU_Value = (int)MU_Length.ksLUnSM;
                break;
            case "Миллиметры куб.":
                this.Volume_MU_Value = (int)MU_Length.ksLUnMM;
                break;
            case "Дециметры куб.":
                this.Volume_MU_Value = (int)MU_Length.ksLUnDM;
                break;
            case "Метры куб.":
                this.Volume_MU_Value = (int)MU_Length.ksLUnM;
                break;
            case "Настройки документа":
                this.Volume_MU_Value = (int)MU_Length.ksLUnDocument;
                break;
        }
        this.Volume_MU_Name = Volume_MU_text;
    }
    public class VisibleControls
    {
        public bool SpecialPan { get; set; } = false;
        public bool CutLength { get; set; } = false;
        public bool PropertyTranslation { get; set; } = false;
        public bool ProjectClone { get; set; } = false;
        public bool Check_ProfileValue { get; set; } = false;
    }
    public class CalcSheetMetalProperty
    {
        public bool CutLengt_Calc { get; set; } = false;
        public bool BendCount_Calc { get; set; } = false;
        public bool HoleCount_Calc { get; set; } = false;
    }
}
