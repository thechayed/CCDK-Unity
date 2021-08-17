/* The Dialogue Box Componeent interfaces with the Dialogue Manager to display the Dialogue that has been recieved */

using System.Collections;
using UnityEngine;
using CCDKEngine;
using UnityEngine.UI;

namespace CCDKGame
{
    public class DialogueBox : MonoBehaviour
    {
        public string text;
        public bool display;
        private bool complete;

        public Text nameText;
        public Text dialogueText;
        public float waitTime = 0.05f;

        public AudioSource audioSource;
        public AudioClip audioClip;

        public Image portrait;

        /** The animator used for the animations of this Dialogue Box **/
        public Animator boxAnimator;
        public enum animStates
        {
            Hidden,
            Open,
            Displayed,
            Close
        }
        public animStates animState = (animStates) 0;

        private void Awake()
        {
            DialogueManager.box = this;
        }

        /** Tell the Dialogue Box to display a message **/
        public void DisplaySentence(string sentence = null)
        {
            complete = true;

            nameText.text = DialogueManager.speaker;

            if (sentence != null)
            {
                text = sentence;
                dialogueText.text = text;
                return;
            }
            text = "";

            for(int i = 0; i < DialogueManager.characters.Count; i++)
            {
                Invoke("DrawChar", waitTime*i);
            }

            StartDialogue();

            /** If a portrait Image exists for the Dialogue box, set it's sprite from the given Dialogue **/
            if (portrait != null)
            {
                if (DialogueManager.portrait != null)
                {
                    portrait.gameObject.SetActive(true);
                    portrait.sprite = DialogueManager.portrait;
                }
                else
                {
                    portrait.gameObject.SetActive(false);
                }
            }
        }

        /** If the Display Type is Crawl, we display each character one at a time. **/
        private void Update()
        {
            if(complete)
            {
                DialogueManager.sentenceDisplayed = true;
            }
            dialogueText.text = text;

            boxAnimator.SetInteger("State", (int)animState);
        }

        private void DrawChar()
        {
            text += DialogueManager.characters.Dequeue();
            if (audioSource != null)
            {
                audioSource.PlayOneShot(audioClip);
            }

            if(DialogueManager.characters.Count == 0)
            {
                complete = true;
            }
        }

        /* Animation States */

        public void StartDialogue()
        {
            animState = (animStates)1;
        }

        public void StartFinished()
        {
            animState = (animStates)2;
            display = true;
        }

        /** Called when all the Dialogue Queue is empty **/
        public void EndDialogue()
        {
            animState = (animStates) 3;
            display = false;
        }

        public void EndFinished()
        {
            animState = (animStates)0;
        }
    }
}