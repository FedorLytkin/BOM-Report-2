using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VSNRM_Kompas.ArticleTools
{
    class CustomTreeNode
    {
        private String hyperlink;
        private String[] DataArray;
        //private String data;

        public CustomTreeNode()
        {
            //Something
        } 

        public String Hyperlink
        {
            get
            {
                return hyperlink;
            }
            set
            {
                hyperlink = value;
            }
        }

        public String[] Data
        {
            get
            {
                return DataArray;
            }
            set
            {
                DataArray = value;
            }
        }
    }
}
