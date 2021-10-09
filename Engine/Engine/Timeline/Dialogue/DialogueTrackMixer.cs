using CCDKEngine;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace CCDKTimeline
{
    public class DialogueTrackMixer : PlayableBehaviour
    {
        public Dialogue dialogue;

        private bool displayed = false;
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            float totalWeight = 0f;
            int inputCount = playable.GetInputCount();
            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);

                if (inputWeight > 0f)
                {
                    ScriptPlayable<DialogueBehavior> inputPlayable = (ScriptPlayable<DialogueBehavior>)playable.GetInput(i);
                    DialogueBehavior input = inputPlayable.GetBehaviour();

                    if (!input.displayed)
                    {
                        DialogueManager.StartConversation(new Dialogue[] { input.dialogue }, "Timeline");
                        input.displayed = true;
                    }
                }

                totalWeight += inputWeight;
            }

            if (totalWeight < 0.5f)
            {
                DialogueManager.EndDialogue("Timeline");
            }
        }
    }
}