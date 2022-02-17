using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VSNRM_Kompas.API_Toops
{
    class ComponentInfo: ICloneable
    {
        public string Oboz { get; set; }
        public string Naim { get; set; }
        public double Mass { get; set; }
        public string material { get; set; }
        public double QNT { get; set; }
        public bool QNT_False { get; set; }
        public double Total_QNT { get; set; }
        public string RazdelSP { get; set; }
        public string FFN { get; set; }
        public bool isDetal { get; set; }
        public bool standardComponent { get; set; }
        public bool isBody { get; set; }
        public bool isLocal { get; set; }
        public bool isPurchated { get; set; }
        public string Key { get; set; }
        public double Area { get; set; }
        public Dictionary<string, string> ParamValueList { get; set; }
        public bool SheeMetall { get; set; }
        public bool HaveUnfold { get; set; }
        public Bitmap Slide { get; set; }
        public Bitmap LargeSlide { get; set; }
        public bool HaveDrw { get; set; }
        public bool HaveSP { get; set; }
        public List<Drw_Info_Class> drw_List = new List<Drw_Info_Class>();
        public List<Variable_Class> Referense_Variable_List = new List<Variable_Class>();
        public long FL_Size { get; set; }

        public Drw_Info_Class drw_Info = new Drw_Info_Class();
        public Get_MCH _MCH = new Get_MCH();
        public Get_Body Body = new Get_Body();

        public class Get_MCH
        {
            public double Area { get; set; }
            public double Mass { get; set; }
            public double Volume { get; set; }
            public double Xc { get; set; }
            public double Yc { get; set; }
            public double Zc { get; set; }
        }
        public class Get_Body
        {
            public string Oboz { get; set; }
            public string Naim { get; set; }
            public double QNT { get; set; }
            public bool QNT_False { get; set; }
            public Dictionary<string, string> ParamValueList { get; set; }
        }
        public class Variable_Class
        {
            public string Name { get; set; }
            public string SourceFileName { get; set; }
        }

        public class Drw_Info_Class
        {
            public string FFN { get; set; }
            public string Oboz { get; set; }
            public string Naim { get; set; }
            public Bitmap Slide { get; set; }
            public Bitmap LargeSlide { get; set; }
            public long FL_Size { get; set; }
            public Dictionary<string, string> ParamValueList { get; set; }
        }
        public object Clone()
        {
            return new ComponentInfo 
            { 
                _MCH = this._MCH, 
                Body = this.Body, 
                Oboz = this.Oboz, 
                Naim = this.Naim, 
                Mass = this.Mass, 
                material = this.material, 
                QNT = this.QNT, 
                RazdelSP = this.RazdelSP, 
                FFN = this.FFN, 
                isDetal = this.isDetal, 
                standardComponent = this.standardComponent,
                isBody = this.isBody,
                Key = this.Key, 
                Area = this.Area, 
                ParamValueList = this.ParamValueList,
                SheeMetall = this.SheeMetall,
                HaveUnfold = this.HaveUnfold,
                Slide = this.Slide,
                LargeSlide = this.LargeSlide,
                drw_List = this.drw_List,
                drw_Info = this.drw_Info,
                FL_Size = this.FL_Size
            };
        }

    }
}