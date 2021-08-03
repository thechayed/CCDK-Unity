using CCDKGame;
using System.Collections;
using UnityEngine;

namespace TemplateGame
{
    public class TMPPawnAudio : PawnAudio
    {
        public float volume = 0.1f;
        public class Default: FSM.State
        {
            public TMPPawnAudio self;
            public override void Enter()
            {
                //SelfObj is this component's original class passed to the State class
                self = (TMPPawnAudio)selfObj;

                //You can access members of the original component this way, or Set/GetValues with..
                float newVol = (float)GetValue("volume");
                SetValue("volume", 2.0f);
            }
        }
    }
}