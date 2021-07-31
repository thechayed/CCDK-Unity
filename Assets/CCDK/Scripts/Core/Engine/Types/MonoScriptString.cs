/** MonoScriptStrings use MonoScriptString.Type to add some Dynamic Typing of the selected base Types **/

using B83.Unity.Attributes;
using System;
using System.Collections;
using UnityEngine;

namespace Script
{
    [System.Serializable]
    public class Any
    {
        [MonoScript]  public string script;

        public override string ToString()
        {
            return script;
        }
    }

    [System.Serializable]
    public class PawnClass
    {
        [MonoScript(type = typeof(CCDKEngine.PawnClass))] public string script;
        public override string ToString()
        {
            return script;
        }
    }

    [System.Serializable]
    public class ControllerClass
    {
        [MonoScript(type = typeof(CCDKEngine.ControllerClass))] public string script;
        public override string ToString()
        {
            return script;
        }
    }
}