using KompasAPI7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GetAllPartsClass
{
    public List<IPart7> GetParts(IPart7 TopPart)
    {
        List<IPart7> PartList = new List<IPart7>();
        if (TopPart != null)
        {
            var Parts = TopPart.PartsEx[0];
            if (Parts != null)
            {
                foreach (IPart7 item in Parts)
                {
                    if(!PartList.Contains(item))
                        PartList.Add(item);
                    if (!item.Detail)
                        GetSubParts(item, PartList); 
                }
            }
        }
        return PartList;
    }
    private List<IPart7> GetSubParts(IPart7 TopPart, List<IPart7> PartList)
    {
        if (TopPart != null)
        {
            var Parts = TopPart.PartsEx[0];
            if (Parts != null)
            {
                foreach (IPart7 item in Parts)
                {
                    if (!PartList.Contains(item))
                        PartList.Add(item);
                    if (!item.Detail)
                        GetSubParts(item, PartList);
                }
            }
        }
        return PartList.Distinct().ToList();
    }
}