using CCDKEngine;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace CCDKTimeline
{
    public class DialogueClip : PlayableAsset
    {
        public Dialogue dialogue;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<DialogueBehavior>.Create(graph);

            DialogueBehavior dialogueBehavior = playable.GetBehaviour();
            dialogueBehavior.dialogue = dialogue;

            return playable;
        }
    }
}