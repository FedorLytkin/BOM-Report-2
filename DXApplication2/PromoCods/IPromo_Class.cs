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
            PromoCodeList.Add("icut_length");
            PromoCodeList.Add("iproperty_translator");
            PromoCodeList.Add("iproject_clone");
            PromoCodeList.Add("ICheckProfileLength");
            PromoCodeList.Add("icanedit");
            PromoCodeList.Add("GetBase64FromImageForDrawing");
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
                        case "icut_length":
                            IOption_Class.IVC.SpecialPan = true;
                            IOption_Class.IVC.CutLength = true;
                            break;
                        case "iproperty_translator":
                            IOption_Class.IVC.SpecialPan = true;
                            IOption_Class.IVC.PropertyTranslation = true;
                            break;
                        case "ICheckProfileLength":
                            IOption_Class.IVC.SpecialPan = true;
                            IOption_Class.IVC.Check_ProfileValue = true;
                            break;
                        case "iproject_clone":
                            IOption_Class.IVC.ProjectClone = true;
                            break;
                        case "icanedit":
                            IOption_Class.IVC.EditOn = true;
                            break;
                        case "GetBase64FromImageForDrawing":
                            IOption_Class.IVC.GetBase64FromImageForDrawingPanelVis = true;
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
