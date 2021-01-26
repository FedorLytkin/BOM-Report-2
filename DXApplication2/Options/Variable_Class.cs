using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSNRM_Kompas.Options
{
    public class Obj_Variable_Class
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public Obj_Variable_Class( )
        { 
        }
        public Obj_Variable_Class(string Name, object Value)
        {
            this.Name = Name;
            this.Value = Value;
        }
    }
    public class Int_Variable_Class
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public Int_Variable_Class(string Name, int Value)
        {
            this.Name = Name;
            this.Value = Value;
        }
    }
    public class Str_Variable_Class
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public Str_Variable_Class(string Name, string Value)
        {
            this.Name = Name;
            this.Value = Value;
        }
    }
    public class Bool_Variable_Class
    {
        public string Name { get; }
        public bool Value { get; set; }
        public Bool_Variable_Class(string Name, bool Value)
        {
            this.Name = Name;
            this.Value = Value;
        }
    }
}
