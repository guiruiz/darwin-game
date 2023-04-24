using UnityEngine;

public class Utils
{
    public static float DegreesTo360Range(float degrees)
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

    // public static float Foo(float degrees)
    // {
    //     float mappedDegrees = degrees % 360.0f;
    //     if (mappedDegrees < 0)
    //     {
    //         mappedDegrees += 360.0f;
    //     }
    //     else if (mappedDegrees >= 360.0f)
    //     {
    //         mappedDegrees -= 360.0f;
    //     }
    //     return mappedDegrees;
    // }
}
