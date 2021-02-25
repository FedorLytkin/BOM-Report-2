﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramDataControllerBehavior.Data
{
    public class ClassData
    {
        public string FileName { get; set; }
        public string Oboz { get; set; }
        public string Naim { get; set; }
        public string Material { get; set; }
        public Bitmap Slide { get; set; }
        public string Key { get; set; }
        public ClassType Type { get; set; }
    }

    public enum ClassType
    {
        Assembly,
        Part,
        Body,
        Standart,
        prochee,
        material
    }
    public class ConnectionData
    {
        string fromEmployeeID;
        public string ConnectorText { get; set; }
        public object ConnectedFrom { get { return fromEmployeeID; } set { fromEmployeeID = value.ToString(); } }
        public object ConnectedTo { get; set; }
    }
}