using System.Collections;
using UnityEngine;
using CCDKEngine;
using System;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace CCDKGame
{
    public class PawnInputHandler : PawnClass
    {
        /** Simple list of byte values storing the value of an input **/
        public List<byte> inputs = new List<byte>();

        /** This is called from a posessing Player Controller, the Pawn decides which methods are attached to each Action **/
        public void InputPerformed(InputAction.CallbackContext ctx)
        {
            if(pawn.pawnData.inputinfo.actions.Get(ctx.action.name) != null)
            {
                if (GetType().GetMethod(pawn.pawnData.inputinfo.actions.Get(ctx.action.name)) != null)
                {
                    GetType().GetMethod(pawn.pawnData.inputinfo.actions.Get(ctx.action.name)).Invoke(this, new object[] { ctx });
                    SetByte(pawn.pawnData.inputinfo.actions.GetIndex(ctx.action.name),1);
                }
            }
        }

        public void InputCanceled(InputAction.CallbackContext ctx)
        {
            if (pawn.pawnData.inputinfo.actions.Get(ctx.action.name) != null)
            {
                if (GetType().GetMethod(pawn.pawnData.inputinfo.actions.Get(ctx.action.name) + "_Cancel") != null)
                {
                    GetType().GetMethod(pawn.pawnData.inputinfo.actions.Get(ctx.action.name) + "_Cancel").Invoke(this, new object[] { ctx });
                    SetByte(pawn.pawnData.inputinfo.actions.GetIndex(ctx.action.name), 0);
                }
            }
        }

        /** Set Input values **/
        private void SetByte(int index, byte value)
        {
            while (index > inputs.Count)
            {
                inputs.Add(0);
            }
            inputs[index] = value;
        }
    }
}