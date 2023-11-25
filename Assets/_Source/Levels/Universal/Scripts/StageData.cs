using System;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    [Serializable]
    public class StageData
    {
        public List<Level.State> LevelsData = new List<Level.State>();
    }
}