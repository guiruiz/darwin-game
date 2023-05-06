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

    public static Vector3 Vector2To3(Vector2 v)
    {
        return new Vector3(v.x, 0f, v.y);
    }

    public static Vector2 Vector3To2(Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }


    public static void LogForce(string name, Vector2 force)
    {
        float forceDeg = Mathf.Atan2(force.y, force.x) * Mathf.Rad2Deg;
        Debug.Log($"{name} = {force} / mag= {force.magnitude} / {forceDeg} degrees");
    }


    public static void DrawForce(Vector3 origin, Vector2 force, Color color)
    {
        float yOffset = 5f;
        float lineDuration = 0.05f;
        float multiplier = 10f;

        force = force * multiplier;
        Vector3 lineOrigin = new Vector3(origin.x, yOffset, origin.z);
        Vector3 lineEnd = new Vector3(origin.x + force.x, yOffset, origin.z + force.y);

        Debug.DrawLine(lineOrigin, lineEnd, color, lineDuration);
    }

    public static float RotationToDeg(float rotation)
    {
        return 360f - rotation;
    }

    public static float DegToRad(float deg)
    {
        return deg * Mathf.Deg2Rad;
    }

    public static float GetHullWindDeg(Boat boat, Wind wind)
    {
        // 0 -> 360 anti clock
        float hullDeg = Utils.RotationToDeg(boat.hullRotation);
        float windDeg = Utils.RotationToDeg(wind.rotation);
        float hullWindDeg = hullDeg - windDeg;

        return Utils.Normalize360Range(hullWindDeg);
    }

    public static float GetSailWindDeg(Boat boat, Wind wind)
    {
        // 0 -> 360 anti clock
        float sailDeg = Utils.RotationToDeg(boat.mastRotation);
        float sailWindDeg = sailDeg + GetHullWindDeg(boat, wind);
        return Utils.Normalize360Range(sailWindDeg);
    }

}
