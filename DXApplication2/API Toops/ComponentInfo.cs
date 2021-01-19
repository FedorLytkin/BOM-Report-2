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
        public string Key { get; set; }
        public double Area { get; set; }
        public Dictionary<string, string> ParamValueList { get; set; }
        public bool SheeMetall { get; set; }
        public bool HaveUnfold { get; set; }
        public Bitmap Slide { get; set; }




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
                Key = this.Key, 
                Area = this.Area, 
                ParamValueList = this.ParamValueList,
                SheeMetall = this.SheeMetall,
                HaveUnfold = this.HaveUnfold
            };
        }

    }
}