using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    public class Option_Class
    {
        public string Mass_MU_Name;
        public string Length_MU_Name;
        public string Area_MU_Name;
        public string Volume_MU_Name;

        public int Mass_MU_Value;
        public int Length_MU_Value;
        public int Area_MU_Value;
        public int Volume_MU_Value;
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
            ksMUnDocument = 2
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
            Mass_MU_Name = Mass_MU_text;
        }
        public void Set_MU_Length(string Length_MU_text)
        {
            switch (Length_MU_text)
            {
                case "Сантиметры":
                    Length_MU_Value = (int)MU_Length.ksLUnSM;
                    break;
                case "Миллиметры":
                    Mass_MU_Value = (int)MU_Length.ksLUnMM;
                    break;
                case "Дециметры":
                    Mass_MU_Value = (int)MU_Length.ksLUnDM;
                    break;
                case "Метры":
                    Mass_MU_Value = (int)MU_Length.ksLUnM;
                    break;
                case "Настройки документа":
                    Mass_MU_Value = (int)MU_Length.ksLUnDocument;
                    break;
            }
            Length_MU_Name = Length_MU_text;
        }
        public void Set_MU_Area(string Area_MU_text)
        {
            switch (Area_MU_text)
            {
                case "Сантиметры кв.":
                    Length_MU_Value = (int)MU_Length.ksLUnSM;
                    break;
                case "Миллиметры кв.":
                    Mass_MU_Value = (int)MU_Length.ksLUnMM;
                    break;
                case "Дециметры кв.":
                    Mass_MU_Value = (int)MU_Length.ksLUnDM;
                    break;
                case "Метры кв.":
                    Mass_MU_Value = (int)MU_Length.ksLUnM;
                    break;
                case "Настройки документа":
                    Mass_MU_Value = (int)MU_Length.ksLUnDocument;
                    break;
            }
            Area_MU_Name = Area_MU_text;
        }
        public void Set_MU_c(string Volume_MU_text)
        {
            switch (Volume_MU_text)
            {
                case "Сантиметры куб.":
                    Length_MU_Value = (int)MU_Length.ksLUnSM;
                    break;
                case "Миллиметры куб.":
                    Mass_MU_Value = (int)MU_Length.ksLUnMM;
                    break;
                case "Дециметры куб.":
                    Mass_MU_Value = (int)MU_Length.ksLUnDM;
                    break;
                case "Метры куб.":
                    Mass_MU_Value = (int)MU_Length.ksLUnM;
                    break;
                case "Настройки документа":
                    Mass_MU_Value = (int)MU_Length.ksLUnDocument;
                    break;
            }
            Volume_MU_Name = Volume_MU_text;
        }
    }
