using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using CCDKEngine;

namespace CCDKGame
{
    public class Pawn : PossessableObject
    {
        [ReadOnly] public CCDKObjects.Pawn pawnData;

        /** Whether the Constructor method has already been called **/
        [HideInInspector]
        public bool constructed;

        public ComponentConstructor<Script.PawnClass> componentConstructor;

        /** Base classes **/
        [HideInInspector] public PawnMovement movement;
        [HideInInspector] public PawnAudio audio;
        [HideInInspector] public PawnCostume costume;
        [HideInInspector] public PawnInputHandler input;
        [HideInInspector] public PawnInventoryManager inventory;
        [HideInInspector] public PawnLife stats;
        private string[] children = new string[] { "movement", "audio", "costume", "input", "inventory", "stats" };

        public override void Start()
        {
            base.Start();
            foreach(string child in children)
            {
                InvokeInChildren(child, "Start");
            }
            pawnData = (CCDKObjects.Pawn)data;

            Engine.AddPawn(gameObject);
        }

        public virtual void Update()
        {
            foreach (string child in children)
            {
                InvokeInChildren(child, "Start");
            }
        }

        /** Use Reflection to check if child classes contain Unity MB events, and call them if they do. **/
        public void InvokeInChildren(string name, string method)
        {
            if (this.GetType().GetField(name).GetType().GetMethod(method) != null)
            {
                this.GetType().GetField(name).GetType().GetMethod(method).Invoke(this.GetType().GetField(name), null);
            }
        }

        private void OnEnable()
        {
            movement = new PawnMovement();
            movement.pawn = this;
            audio = new PawnAudio();
            audio.pawn = this;
            costume = new PawnCostume();
            costume.pawn = this;
            input = new PawnInputHandler();
            input.pawn = this;
            inventory = new PawnInventoryManager();
            inventory.pawn = this;
            stats = new PawnLife();
            stats.pawn = this;

            PawnManager.GetPawn(this);
        }

        /** Override this function for Pawn classes to set their own default classes when spawned into the game **/
        public virtual Dictionary<string> GetDefaultClasses()
        {
            return null;
        }

        //public void Reset()
        //{
        //    componentConstructor.RemovePreviousClasses();
        //}

        public bool SetController(Controller controller)
        {
            if (pawnData.possessable)
            {
                if(this.controller == null)
                {
                    this.controller = controller;

                    return true;
                }
            }
            return false;
        }

        public void DestroySelf()
        {
            DestroyImmediate(this,true);
        }

        private void OnDestroy()
        {
            Engine.RemovePawn(gameObject);
            PawnManager.RemovePawn(this);
        }
    }
}