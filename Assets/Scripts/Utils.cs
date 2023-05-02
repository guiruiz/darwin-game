using UnityEngine;

public class Utils
{
    public static float Normalize360Range(float degrees)
    {
        float mappedDegrees = degrees % 360.0f;

        if (mappedDegrees >= 0)
        {
            return mappedDegrees;
        }
        else
        {
            return mappedDegrees + 360f;
        }
    }

    public static Vector2 RotateVector(Vector2 vector, float radians)
    {
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, radians * Mathf.Rad2Deg), Vector3.one); // create a rotation matrix
        return rotationMatrix.MultiplyVector(vector); // apply the rotation matrix to the vector and return the result
    }

    public static float ToCircleAngle(float rotation)
    {
        return 360f - rotation;
    }
}
