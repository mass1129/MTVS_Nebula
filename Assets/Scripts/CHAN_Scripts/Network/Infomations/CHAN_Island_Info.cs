using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandInfo
{
    public class Result
    {
        public double pc1 { get; set; }
        public double pc2 { get; set; }
        public double pc3 { get; set; }
        public string keyword1 { get; set; }
        public string keyword2 { get; set; }
    }

    public class Root
    {
        public int httpStatus { get; set; }
        public string message { get; set; }
        public Result results { get; set; }
    }

    

}