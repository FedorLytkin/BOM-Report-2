using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Positio_CalcBR_Class
{
    public static void PositioSet(TreeListNodes nodes, bool Positio_CalcBR_Value, bool UseParentPositio, string ParentPositioValue, string PositioSplitter)
    {
        if (!Positio_CalcBR_Value) return;
        if ((nodes.TreeList).Columns["Позиция"] == null) return;
        for (int index = 0; index < nodes.Count; index++)
        {
            string PositioVal = UseParentPositio && !string.IsNullOrWhiteSpace(ParentPositioValue) ? $@"{ParentPositioValue}{PositioSplitter}{index + 1}" : $@"{index + 1}";
            nodes[index].SetValue("Позиция", PositioVal);
            if (nodes[index].HasChildren) PositioSet(nodes[index].Nodes, Positio_CalcBR_Value, UseParentPositio, PositioVal, PositioSplitter);
        }
    }

}
