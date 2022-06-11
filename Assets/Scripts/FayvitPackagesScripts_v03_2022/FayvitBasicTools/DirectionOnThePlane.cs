using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionOnThePlane : MonoBehaviour
{
    public static Vector3 InTheUp(Vector3 origem, Vector3 extremidade)
    {
        return InTheUp(extremidade - origem);
    }

    public static Vector3 InTheUp(Vector3 dir)
    {
        return Vector3.ProjectOnPlane(dir, Vector3.up);
    }

    public static Vector3 NormalizedInTheUp(Vector3 dir)
    {
        return InTheUp(dir).normalized;
    }

    public static Vector3 NormalizedInTheUp(Vector3 origem, Vector3 extremidade)
    {
        return NormalizedInTheUp(origem, extremidade);
    }

    public static Vector3 RandomDir
    {
        get { return NormalizedInTheUp(Vector3.zero, Random.insideUnitSphere); }
    }
}
