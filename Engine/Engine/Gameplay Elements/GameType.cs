/* When a Level is set at the current Level, it is given the Game Type component to control
 Game Type information identically to Unreal's Game Info system.*/
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using CCDKEngine;

namespace CCDKGame
{
    public class GameType : FSM.Component
    {
        public bool init = false; 

        public bool gameplayStarted;
        public CCDKObjects.GameTypeInfo data;

        /**The GameType class of the Game Type that is being played.**/
        public static GameType currentGame;

        /**<summary>When the Game Type is starting, initialize it.</summary>**/
        private void Update()
        {
            if (Engine.singleton.initialized&&!init)
            {
                this.Invoke("LocalInitialization", 0f);
            }
        }


        #region Game Type Overrides
        /**<summary>Override to add extra Initialization functionality.</summary>**/
        public virtual void Initialization()
        {
        }

        /**<summary>Call to start the game, Override to add additional Game Start functionality.</summary>**/
        public virtual void StartGame()
        {
        }

        /**<summary>Call to End the game, Override to add additional Game End functionality.</summary>**/
        public virtual void EndGame()
        {
        }

        /**<summary>A Callback for when a Player Joins the game.</summary>**/
        public virtual void PlayerJoined()
        {
        }

        /**<summary>A Callback for when a Player Leaves the game.</summary>**/
        public virtual void PlayerLeft()
        {
        }

        /**<summary>Another class can request the spawning of a pawn in the Game using this method. Override to add additional conditions for possessing a pawn.</summary>**/
        public virtual Pawn TrySpawn(CCDKObjects.Pawn pawnToSpawn, Vector3 position = default(Vector3))
        {
            return PawnManager.CreatePawn(pawnToSpawn, position);
        }
        
        /**<summary>Asks the Game Type to let the Player Controller possess a Pawn. Override to add conditions to the possession requirements.</summary>**/
        public virtual Pawn RequestPossession(CCDKObjects.Pawn pawnToPossess)
        {
            return GetPawnFromDataType(pawnToPossess);
        }

        /**<summary>Asks the Game Type of possession of the Pawn is allowed. Override to add conditions to the possession requirements.</summary>**/
        public virtual bool IsPossessionAllowed(Pawn pawn)
        {
            return true;
        }
        #endregion

        #region Absolute Methods

        /**Initialization called in this script only.**/
        private void LocalInitialization()
        {
            /**Set the Default Player Controller for currently active Players whenever the Game Type begins**/
            if (data.defaultPlayerController != null)
            {
                foreach (Player player in PlayerManager.singleton.players)
                {
                    PlayerManager.SetPlayerController(player.ID, data.defaultPlayerController);
                }
            }

            this.Invoke("Initialization", 0f);

            init = true;
        }

        /**<summary>Return an unpossessed Pawn in the game that is paired with the passed data type.</summary>**/
        public Pawn GetPawnFromDataType(CCDKObjects.Pawn pawnToPossess)
        {
            foreach (Pawn pawn in PawnManager.PawnsInGame)
            {
                if (pawn.data == pawnToPossess&&pawn.controller==null)
                {
                    return pawn;
                }
            }
            return null;
        }
        #endregion

    }
}
