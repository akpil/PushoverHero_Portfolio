using System;
using System.Collections.Generic;
using UnityEngine;

namespace Config.Data
{
    [Serializable]
    public class ChartData
    {
        public int ID;
        public List<Vector2> Stock;
        
        public ChartData GetClone()
        {
            return (ChartData)MemberwiseClone();
        }
    }
}