using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.Response.Ssml;
using EmergencyResponderGame.IntentHandlers;

namespace EmergencyResponderGame.StorySystem.Nodes
{
    public class ChoiceNode : BaseNode
    {
        #region Properties and Fields
        
        public Dictionary<string, int> Choices { get; } = new Dictionary<string, int>();

        #endregion

        public ChoiceNode(int defaultChoiceIndex, params KeyValuePair<string, int>[] choices) :
            base(defaultChoiceIndex)
        {
            if (choices != null)
            {
                for (int i = 0; i < choices.Length; ++i)
                {
                    Choices.Add(choices[i].Key, choices[i].Value);
                }
            }
        }

        public int GetNextNodeIndexForChoice(string choice)
        {
            if (!Choices.ContainsKey(choice))
            {
                return NextNodeIndex;
            }

            return Choices[choice];
        }

        public override Speech GetSpeech()
        {
            return new Speech();
        }

        #region Factory Functions

        public static ChoiceNode CreateYesNoChoiceNode(int yesIndex, int noIndex)
        {
            return new ChoiceNode(noIndex,
                new KeyValuePair<string, int>(YesIntentHandler.IntentName, yesIndex),
                new KeyValuePair<string, int>(NoIntentHandler.IntentName, noIndex));
        }

        #endregion
    }
}
