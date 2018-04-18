using Alexa.NET.Response.Ssml;
using EmergencyResponderGame.IntentHandlers;
using EmergencyResponderGame.SpeechUtilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmergencyResponderGame.StorySystem.Nodes
{
    public class SpeechNode : BaseNode
    {
        #region Properties and Fields

        public Speech Speech { get; }
        
        #endregion

        public SpeechNode(int nextNodeIndex, SpeechBuilder speech) :
            base(nextNodeIndex)
        {
            Speech = speech.Build();
        }
    }
}
