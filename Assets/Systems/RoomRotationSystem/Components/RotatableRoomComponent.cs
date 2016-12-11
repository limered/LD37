using System.Collections;
using UnityEngine;

namespace Assets.Systems.RoomRotationSystem.Components
{
    public class RotatableRoomComponent : GameComponent
    {
        public void AnimateRotation(Vector3 axis, int angle, float speed)
        {
            StartCoroutine(AnimateRotationRoutine(axis, angle, speed));
        }

        public IEnumerator AnimateRotationRoutine(Vector3 axis, int angle, float speed)
        {
            for (var i = 0f; i < angle*100; i+= speed)
            {
                transform.RotateAround(transform.position, axis, speed/100);
                yield return null;
            }
        }
    }
}
