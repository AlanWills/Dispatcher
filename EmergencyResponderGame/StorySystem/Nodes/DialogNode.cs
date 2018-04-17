using Alexa.NET.Response.Ssml;
using EmergencyResponderGame.SpeechUtilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmergencyResponderGame.StorySystem.Nodes
{
    public class DialogNode : BaseNode
    {
        #region Properties and Fields

        private Speech Speech { get; }
        
        #endregion

        public DialogNode(int nextNodeIndex, SpeechBuilder speech) :
            base(nextNodeIndex)
        {
            Speech = speech.Build();
        }

        public override Speech GetSpeech()
        {
            return Speech;
        }
    }
}
