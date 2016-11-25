namespace Assets.Scripts.Utils
{
    using UnityEngine;

    public static class VectorUtils
    {
        public static float Angle(Vector2 vector)
        {
            return Angle(vector, Vector2.up);
        }

        public static float Angle(Vector2 vector, Vector2 zeroAngleNormal)
        {
            return Mathf.Deg2Rad * Vector2.Angle(zeroAngleNormal, vector.normalized);
        }

        public static Vector2 FromAngle(float angle)
        {
            angle += 90 * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }

        public static float AngleBetweenPoints(Vector2 position1, Vector2 position2)
        {
            var fromLine = position2 - position1;
            var toLine = new Vector2(1, 0);

            var angle = Vector2.Angle(fromLine, toLine);
            var cross = Vector3.Cross(fromLine, toLine);

            if (cross.z > 0)
                angle = 360f - angle;

            return angle * Mathf.Deg2Rad;
        }
    }
}