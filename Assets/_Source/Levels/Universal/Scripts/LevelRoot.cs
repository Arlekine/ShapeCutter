using System;
using UISystem;
using UnityEngine;

namespace Levels
{
    public abstract class LevelRoot : MonoBehaviour, IUISpawner
    {
        protected UI _ui;

        public Action<LevelResult> Completed;

        public abstract void Init(UI ui, TutorialHolder tutorial);
    }
}
