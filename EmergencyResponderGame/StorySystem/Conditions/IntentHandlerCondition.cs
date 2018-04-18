using System;
using System.Collections.Generic;
using System.Text;
using EmergencyResponderGame.StorySystem.Nodes;
using EmergencyResponderGame.IntentHandlers;
using Alexa.NET.Request;

namespace EmergencyResponderGame.StorySystem.Conditions
{
    public class IntentHandlerCondition : Condition
    {
        #region Properties and Fields

        public string IntentName { get; }

        #endregion

        public IntentHandlerCondition(int nextNodeIndex, string intentName) :
            base(nextNodeIndex)
        {
            IntentName = intentName;
        }

        #region Choice Abstract Implementations
        
        public override bool IsMatchingCondition(Intent intent)
        {
            return intent.Name == IntentName;
        }

        #endregion
    }
}
