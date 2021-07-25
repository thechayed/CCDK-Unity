/** The Pawn's Costume controlls it's visual presence in the game, whether it be a Mesh or Sprite **/

using System.Collections;
using UnityEngine;
using CCDKEngine;

namespace CCDKGame
{
    public class PawnCostume : PawnClass
    {
        public GameObject costume;

        public override void Start()
        {
            state = pawn.data.costumeInfo.state;
            base.Start();
            costume = GameObject.Instantiate(pawn.data.costumeInfo.costumeObject);
            costume.transform.SetParent(pawn.transform);
        }
    }
}