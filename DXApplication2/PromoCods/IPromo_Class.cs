using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSNRM_Kompas.PromoCods
{
    public class IPromo_Class
    {
        List<string> PromoCodeList;
        public Option_Class IOption_Class;
        public IPromo_Class()
        {
            AddPromoList();
        }
        private void AddPromoList()
        {
            PromoCodeList = new List<string>();
            PromoCodeList.Add("cut_length");
            PromoCodeList.Add("PropertyTranslator");
        }
        public bool checkPromo(string SetPromoText)
        {
            foreach(string tmp_Promo in PromoCodeList)
            {
                if(tmp_Promo.ToLower() == SetPromoText.ToLower())
                {
                    if (IOption_Class == null) return false;
                    switch (SetPromoText)
                    {
                        case "cut_length":
                            IOption_Class.VisiblePanel_SpecialPan = true;
                            IOption_Class.VisibleButton_CutLength = true;
                            break;
                        case "PropertyTranslator":
                            IOption_Class.VisiblePanel_SpecialPan = true;
                            IOption_Class.VisibleButton_PropertyTranslation = true;
                            break;
                    }
                    XMLContreller.XMLCLass xMLCLass = new XMLContreller.XMLCLass();
                    xMLCLass.IOptions.SaveOptions(IOption_Class, false);
                    return true;
                }
            }
            return false;
        }
    }
}
