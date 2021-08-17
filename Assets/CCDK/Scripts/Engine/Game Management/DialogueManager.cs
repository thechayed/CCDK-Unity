/* The Dialogue Manager is a global object that recieves Dialogue information and communicates with the HUD to display it */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCDKGame;

namespace CCDKEngine
{
    public class DialogueManager
    {

        public static Queue<Dialogue> dialogues = new Queue<Dialogue>();
        public static Queue<string> sentences = new Queue<string>();
        public static Queue<char> characters = new Queue<char>();

        public static DialogueBox box;
        public static bool sentenceDisplayed = false;

        public static Sprite portrait;

        public static string lastSource = "";

        /** The name of the current speaker **/ 
        public static string speaker;

        public enum DisplayType
        {
            Crawl,
            Instant
        }
        public static DisplayType displayType = 0;

        /** Starting a conversation stores all the dialogues that should be player for the conversation, so they can be displayed one after another **/
        public static void StartConversation(Dialogue[] dialogueToDisplay, string source = "")
        {
            foreach(Dialogue dialogue in dialogueToDisplay)
            {
                dialogues.Enqueue(dialogue);
            }
            StartDialogue(dialogues.Dequeue());
            lastSource = source;
        }

        /** Start a Dialogue **/
        public static void StartDialogue(Dialogue dialogue)
        {
            portrait = dialogue.portrait;
            sentences.Clear();
            foreach(string sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }
            DisplayNextSentence();
            speaker = dialogue.name;
            Debug.Log(dialogue.name);
        }

        /** Display the next sentnece of the current Dialogue **/
        public static void DisplayNextSentence()
        {
            sentenceDisplayed = false;

            /** If their are no more sentences, end Dialogue **/
            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
            }

            /** Handle the Display of Dialogue based on the type **/
            switch ((int) displayType)
            {
                case 0:
                    string sentence = sentences.Dequeue();
                    foreach (char character in sentence)
                    {
                        characters.Enqueue(character);
                    }
                    box.DisplaySentence();
                    break;
                case 1:
                    box.DisplaySentence(sentences.Dequeue());
                    break;
            }
        }

        public static void EndDialogue(string source = "")
        {
            if(source == lastSource)
            {
                /** If there are no more Dialogues to display, tell the Dialogue box that Dialogue has ended (Unless t **/
                if (dialogues.Count == 0)
                {
                    box.EndDialogue();
                    return;
                }
                /** Otherwise, start the next Dialogue in the conversation **/
                StartDialogue(dialogues.Dequeue());
            }
        }
    }
}