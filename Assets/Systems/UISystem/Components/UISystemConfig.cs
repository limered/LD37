using System.Collections;
using Assets.Systems.EnemySpawnerSystem.Components;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Systems.UISystem.Components
{
    public class UISystemConfig : GameComponent
    {
        public Canvas StartCanvas;
        public Canvas EndCanvas;
        public Text HealthText;
        public Text PointsText;
    }
}