/** The Pawn's Costume controlls it's visual presence in the game, whether it be a Mesh or Sprite **/

using System.Collections;
using UnityEngine;
using CCDKEngine;
using System;

namespace CCDKGame
{
    public class PawnCostume : PawnClass
    {
        [HideInInspector] public GameObject costume;
        [HideInInspector] public GameObject mesh;
        [HideInInspector] public Animator animator;

        public void Start()
        {
        }

        public void MeshSetValue(string name, object value)
        {
            bool boolean = false;
            if (value.GetType() == boolean.GetType())
            {
                animator.SetBool(name, (bool)value);
            }
            float floating = 0f;
            if (value.GetType() == floating.GetType())
            {
                animator.SetFloat(name, (float)value);
            }
        }
    }
}