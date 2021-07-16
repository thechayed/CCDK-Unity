using System.Collections;
using UnityEngine;
using CCDKEngine;

namespace CCDKGame
{
    public class PawnAudio : PawnClass
    {
        public AudioClipDictionary audioClips;

        public override  void Start()
        {
            state = pawn.data.audioInfo.state;
            base.Start();
        }
    }
}