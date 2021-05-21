﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class Option_Class
{
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
    }
