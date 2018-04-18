using Alexa.NET.Response.Ssml;
using EmergencyResponderGame.IntentHandlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmergencyResponderGame.StorySystem.Nodes
{
    public abstract class BaseNode
    {
        #region Properties and Fields

        public int NextNodeIndex { get; }
        
        #endregion

        public BaseNode(int nextNodeIndex)
        {
            NextNodeIndex = nextNodeIndex;
        }

        #region Abstract and Virtual Functions

        public abstract Speech GetSpeech(IntentHandler currentIntentHandler);

        public virtual BaseNode GetNextNode(IntentHandler currentIntentHandler)
        {
            return NextNodeIndex < Story.Nodes.Count ? Story.Nodes[NextNodeIndex] : null;
        }

        #endregion
    }
}