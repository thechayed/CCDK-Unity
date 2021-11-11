using B83.Unity.Attributes;
using UnityEditor;
using UnityEngine;

namespace CCDKObjects
{
    [CreateAssetMenu(menuName = "Game Objects/Objective")]
    public class Objective : PrefabSO
    {
        [MonoScript(type = typeof(CCDKGame.Objective))] public string objectiveClass = "CCDKGame.Objective";

        public int reward = 5;
        public enum ObjectiveType
        {
            Zone,
            Item,
            Damage,
            Custom
        }
        /**<summary>The Type of the Objective can automatically determine some functionality inside the Objective class.</summary>**/
        public ObjectiveType objectiveType = 0;

        /**<summary>Setting this number will tell the Game Type that this Objective belongs to a team at the start of Play.</summary>**/
        public int startingTeam = -1;

        /**<summary>TTA determines how long the "Capturing Condition" must be met in order to Acquire the Objective. Commonly used for Zones and Items, by having the player stand at it's position.</summary>**/
        public float timeToAcquire = 5.0f;

        /**<summary>The amount of health the Objective has. Typically used for Damage Objectives, to determine how much damage needs to be done to destroy it.</summary>**/
        public float health = 100f;
        
        [Tooltip("Whether this objective should attach to the player that collects it.")]
        public bool attachToCollector = false;
        
        [Tooltip("The item to pass to the Pawn when collecting from this Objective.")]
        public IventoryItem itemToCollect;


#if UNITY_EDITOR
        public override void OnEnable()
        {
            className = objectiveClass;
            defaultObjectPrefab = Resources.Load<GameObject>("CCDK/PrefabDefaults/Object");
            base.OnEnable();
        }

        public override void Awake()
        {
            className = objectiveClass;
            defaultObjectPrefab = Resources.Load<GameObject>("CCDK/PrefabDefaults/Object");
            base.Awake();
        }

        public override void OnValidate()
        {
            className = objectiveClass;
            base.OnValidate();
        }
#endif
    }
}