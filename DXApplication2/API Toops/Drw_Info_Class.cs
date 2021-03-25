using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSNRM_Kompas.API_Toops
{
    class Drw_Info_Class
    {
        public string FFN { get; set; }
        public string Oboz { get; set; }
        public string Naim { get; set; }
        public Bitmap Slide { get; set; }
        public Bitmap LargeSlide { get; set; }
        public DocType type { get; set; }
        public long FL_Size { get; set; }
        public enum DocType
        {
            SP = 0,
            Drw = 1
        }
    }
}
