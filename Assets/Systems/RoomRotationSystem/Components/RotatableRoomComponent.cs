using System.Collections;
using UnityEngine;

namespace Assets.Systems.RoomRotationSystem.Components
{
    public class RotatableRoomComponent : GameComponent
    {
        public void AnimateRotation(Vector3 axis, int angle)
        {
            StartCoroutine(AnimateRotationRoutine(axis, angle));
        }

        public IEnumerator AnimateRotationRoutine(Vector3 axis, int angle)
        {
            for (var i = 0; i < angle; i++)
            {
                transform.RotateAround(transform.position, axis, 1);
                yield return null;
            }
        }
    }
}
