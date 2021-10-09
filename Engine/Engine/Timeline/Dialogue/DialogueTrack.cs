using CCDKGame;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CCDKTimeline
{
    [TrackBindingType(typeof(DialogueBox))]
    [TrackClipType(typeof(DialogueClip))]
    public class DialogueTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<DialogueTrackMixer>.Create(graph, inputCount);
        }
    }
}