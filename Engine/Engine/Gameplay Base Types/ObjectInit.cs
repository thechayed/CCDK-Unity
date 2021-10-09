using System.Collections;
using UnityEngine;

namespace CCDKGame
{
    [ExecuteAlways]
    public class ObjectInit : MonoBehaviour
    {
        private void Awake()
        {
            CCDKEditor.GOManager.AddObject(gameObject);
        }
    }
}