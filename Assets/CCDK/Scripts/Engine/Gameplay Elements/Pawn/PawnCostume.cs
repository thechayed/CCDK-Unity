/** The Pawn's Costume controlls it's visual presence in the game, whether it be a Mesh or Sprite **/

using System.Collections;
using UnityEngine;
using CCDKEngine;
using System;

namespace CCDKGame
{
    public class PawnCostume : PawnClass
    {
        public GameObject costume;
        public GameObject mesh;
        public Animator animator;


        public override void Start()
        {
            base.Start();
            costume = GameObject.Instantiate(pawn.data.costumeInfo.costumeObject);
            costume.transform.SetParent(pawn.transform);
            mesh = costume.transform.Find("Mesh").gameObject;
            animator = mesh.GetComponent<Animator>();
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