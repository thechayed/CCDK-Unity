/** Global Extension Methods **/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class Extensions
{
    public static Vector3 AngleLerp(this Vector3 fromRotation, Vector3 toRotation, float alpha)
    {
        return new Vector3(Mathf.LerpAngle(fromRotation.x, toRotation.x, alpha), Mathf.LerpAngle(fromRotation.y, toRotation.y, alpha), Mathf.LerpAngle(fromRotation.z, toRotation.z, alpha));
    }

    public static Vector3 Lerp(this Vector3 fromPosition, Vector3 toPosition, float alpha)
    {
        return new Vector3(Mathf.Lerp(fromPosition.x, toPosition.x, alpha), Mathf.Lerp(fromPosition.y, toPosition.y, alpha), Mathf.Lerp(fromPosition.z,toPosition.z, alpha));
    }

    public static Type GetAssemblyType(this string typeName)
    {
        var type = Type.GetType(typeName);
        if (type != null) return type;
        foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
        {
            type = a.GetType(typeName);
            if (type != null) return type;
        }
        return null;
    }

    /** Components **/
    #region
    public static GameObject RemakeGOAndReplaceClass(this GameObject gameObject, string classToReplace)
    {
        GameObject newObject = new GameObject();

        foreach (Component item in gameObject.GetComponents(typeof(Component)))
        {
            if(item.GetType().FullName == classToReplace)
            {
                Component.DestroyImmediate(item, true);
                gameObject.AddComponent(classToReplace.GetAssemblyType());
            }
        }

        return newObject;
    }

    #endregion

    /**<summary>Get the Lines in a string as an array.</summary> **/
    public static string[] GetLines(this string str, bool removeEmptyLines = false)
    {
        return str.Split(new[] { "\r\n", "\r", "\n" },
            removeEmptyLines ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
    }

    /**<summary>Get A List from an IEnumerable.</summary>**/
    public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
    {
        return new List<TSource>(source);
    }
}