using System.Collections;
using UnityEngine;

namespace Assets.Systems.DamagingSystem.Components
{
    internal class DealDamageComponent : GameComponent
    {
        public float DamageToHealth = 1;
        public float PushForceStrength = 2000000000;

        public void SetPlayerKinematic(Rigidbody body)
        {
            StartCoroutine(SetKinematik(body));
        }

        private IEnumerator SetKinematik(Rigidbody body)
        {
            yield return new WaitForSeconds(0.2f);
            body.isKinematic = true;
        }
    }
}