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
        public Image HitImage;

        public void FlashImage()
        {
            StartCoroutine(Flash(5, 15));
        }

        IEnumerator Flash(int on, int off)
        {
            for (var i = 0; i < on; i++)
            {
                var col = HitImage.color;
                HitImage.color = new Color(col.r, col.g, col.b, (0.6f/on)*i);
                yield return null;
            }

            for (var i = 0; i < off; i++)
            {
                var col = HitImage.color;
                HitImage.color = new Color(col.r, col.g, col.b, 0.6f - (0.6f / off) * i);
                yield return null;
            }
        }
    }
}