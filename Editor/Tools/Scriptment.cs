using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace CCDKObjects
{
    [CreateAssetMenu(menuName = "Editor/Scriptment")]
    public class Scriptment : ScriptableObject
    {
        public bool parse = false;
        [Tooltip("Adds line numbers in comments to every line to debug generation issues.")]
        public bool debug = false;

        [Tooltip("These usings are required for Scriptment to work.")]
        [ReadOnly] public string[] defaultUsing = new string[] { "CCDKEngine", "CCDKGame", "System.Collections", "UnityEngine" };
        [Tooltip("Any other scripts to use.")]
        public List<string> additionalUsing = new List<string>();

        [Tooltip("Your Scriptment")]
        [TextArea(0,99999)]
        public string script;

        StreamWriter writer;
        string tabs="";
        int tab = 0;
        bool inState = false; //If we're in a state, when a line no longer has a tab, we will exit
        int stepCount = 0;

        enum ParseMode
        {
            None,
            Code, //This line is actual code (Prefixed with @)
            Dialogue, //This line is Dialogue (Prefixed with "", if a word was read just before the quotes it will be set as the Name and a Character will be found.
            Command, //This line is a Command (Converts the simplified code string into a C# method
            Variable //This line is a variable (Added to the list of vars)
        }
        ParseMode mode = 0;

        public string[] commands = new string[] { "label", "define", "jump", "scene", "play", "stop" };


        /** When editting the Script, update the C# script. **/
        private void OnValidate()
        {
            if (parse)
            {
                var path = AssetDatabase.GetAssetPath(this).Replace(this.name + ".asset", "") + this.name.Replace(" ", "") + ".cs";
                writer = new StreamWriter(path, false);

                if (writer != null)
                {
                    UpdateScript(path);
                }

                parse = false;
            }
        }

        private void UpdateScript(string path)
        {
            tabs = "";
            tab = 0;
            inState = false; //If we're in a state, when a line no longer has a tab, we will exit
            stepCount = 0;


            tab = 0;

            /** Get the Lines from the Script as an array **/
            string[] lines = script.GetLines(false);

            /** Initialize the Line List to add to from parsed Script lines. **/
            List<string> newScript = new List<string>();


            /* Set up the new CSharp Script */
            /** Using **/
            foreach(string use in defaultUsing)
            {
                newScript.Add("using " + use + ";" + GetLineNumber());
            }
            foreach (string use in additionalUsing)
            {
                newScript.Add("using " + use + ";" + GetLineNumber());
            }
            /** Space **/
            newScript.Add("");
            newScript.Add("public class SM_" + name.Replace(" ","") + " : CCDKEngine.Scriptment" + GetLineNumber());
            newScript.Add("{" + GetLineNumber());

            AddTab();

            int lineCount = 0;

            /** Parse the lines, adding to the new Script **/
            foreach (string line in lines)
            {
                lineCount++;

                mode = 0;

                List<string> words = new List<string>();

                int index = 0;
                foreach (char character in line)
                {
                    bool isOp = false;

                    /** Operators, change parse mode **/
                    if (character == '#') //Mark as Code, Add Word
                    {
                        isOp = TryParseMode("Code");
                    }
                    if (character == '"') //Mark as Dialogue, Add Word
                    {
                        isOp = TryParseMode("Dialogue");
                    }
                    if(character == '\t')
                    {
                        isOp = true;
                    }
                    if(character != '\t'&&inState&&index==0)
                    {
                        newScript.AddRange(ExitState());
                    }

                    //Add First Word
                    if (words.Count == 0)
                        words.Add("");

                    /** Check if the Character is an operator or space, if not add it to the Word **/
                    if (!isOp&&character != ' ') //Parse word
                    {
                        if (words.Count >= 1)
                        {
                            words[words.Count - 1] += character;
                        }
                    }

                    /** After space, check for commands, and add the Word to the List **/
                    if (character == ' '|| index == line.Length - 1) //New word
                    {
                        Debug.Log(string.Join(" ", words));

                        if (mode == ParseMode.None&& words.Count>0)
                        {
                            if (CheckIfCommand(words[0]))
                            {
                                TryParseMode("Command");
                            }
                        }

                        if(!(index == line.Length - 1))
                            words.Add("");
                    }

                    if (index == line.Length - 1)
                    {
                        switch (mode)
                        {
                            case ParseMode.Code:
                                if (inState)
                                {
                                    newScript.AddRange(AddStep(new string[]{ string.Join(" ", words) + ";" + GetLineNumber() }));
                                }
                                else
                                    Debug.LogError("Scriptment C# Statement outside label on "+ lineCount +", was subsequently ignored.");
                                break;

                            case ParseMode.Command:
                                newScript.AddRange(GetScriptFromCommand(words.ToArray()));
                                break;

                            case ParseMode.Dialogue:
                                break;

                            case ParseMode.Variable:
                                break;
                        }
                        mode = 0;
                    }

                    index++;
                }
            }

            if (inState)
            {
                newScript.AddRange(ExitState());
            }

            newScript.Add("}" + GetLineNumber());

            Debug.Log(string.Join(Environment.NewLine, newScript));
            
            writer.Write(string.Join(Environment.NewLine, newScript));
            writer.Close();
        }


        private void AddTab([System.Runtime.CompilerServices.CallerLineNumber] int line = 0)
        {
            tab++;
            if (debug)
                tabs = (new string('\t', tab)) + "/*" + line.ToString() + "*/";
            else
                tabs = new string('\t', tab);
        }
        private void RemoveTab([System.Runtime.CompilerServices.CallerLineNumber] int line = 0)
        {
            tab--;
            if (debug)
                tabs = (new string('\t', tab)) + "/*" + line.ToString() + "*/";
            else
                tabs = new string('\t', tab);
        }

        private bool TryParseMode(string mode)
        {
            if(this.mode == 0)
            {
                Enum.TryParse<ParseMode>(mode, out this.mode);
                return true;
            }
            return false;
        }

        private bool CheckIfCommand(string word)
        {
            foreach (string command in commands)
            {
                if(command == word)
                {
                    return true;
                }
            }
            return false;
        }

        private string[] GetScriptFromCommand(string[] commands)
        {
            List<string> codeBlock = new List<string>();

            if(commands[0] == "label")
            {
                codeBlock.Add(tabs + "public class "+commands[1]+ " : ScriptmentState" + GetLineNumber());
                codeBlock.Add(tabs + "{" + GetLineNumber());
                AddTab();
                codeBlock.Add(tabs + "CCDKEngine.Scriptment self;" + GetLineNumber());
                codeBlock.Add(tabs + "" + GetLineNumber());
                codeBlock.Add(tabs + "public override void Enter()" +GetLineNumber());
                codeBlock.Add(tabs + "{" + GetLineNumber());
                AddTab();
                codeBlock.Add(tabs + "self = (CCDKEngine.Scriptment)selfObj;"+GetLineNumber());
                RemoveTab();
                codeBlock.Add(tabs + "}" + GetLineNumber());
                codeBlock.Add(tabs + "" + GetLineNumber());
                inState = true;
            }
            if(commands[0] == "define")
            {
                if(commands.Length > 2)
                {
                    List<string> restOfCommands = new List<string>();
                    for(int i=0;i<commands.Length; i++)
                    {
                        if (i > 0)
                        {
                            restOfCommands.Add(commands[i]);
                        }
                    }

                    codeBlock.Add(tabs + "object " + string.Join(" ", restOfCommands) + ";" + GetLineNumber());
                }
                else
                {
                    codeBlock.Add(tabs + "object " + commands[1] + ";" + GetLineNumber());
                }
                codeBlock.Add("");
            }
            if(commands[0] == "jump")
            {
                if (inState)
                {
                    codeBlock.Add("self.stateToGoTo = " +'"'+  commands[1] + '"' + ";" + GetLineNumber());
                    codeBlock.Add("self.switchState = true;" + GetLineNumber());
                    codeBlock.AddRange(AddStep(codeBlock.ToArray()));
                    codeBlock.RemoveRange(0, 2);
                }
            }

            return codeBlock.ToArray();
        }

        private string[] ExitState()
        {
            List<string> codeBlock = new List<string>();

            RemoveTab();
            codeBlock.Add(tabs+ "}" + GetLineNumber());

            inState = false;
            stepCount = 0;

            return codeBlock.ToArray();
        }

        private string[] AddStep(string[] codeBlock)
        {
            List<string> newCodeBlock = new List<string>();

            stepCount++;
            newCodeBlock.Add(tabs + "public void Step_" + stepCount+ "()" + GetLineNumber());
            newCodeBlock.Add(tabs + "{" + GetLineNumber());
            AddTab();
            newCodeBlock.Add(tabs + string.Join("\n" + tabs,codeBlock));
            RemoveTab();
            newCodeBlock.Add(tabs + "}" +GetLineNumber());
            newCodeBlock.Add(tabs + "" + GetLineNumber());
            return newCodeBlock.ToArray();
        }

        private string GetLineNumber([System.Runtime.CompilerServices.CallerLineNumber] int line = 0)
        {
            if(debug)
                return "    //" + line.ToString();
            return "";
        }
    }
}