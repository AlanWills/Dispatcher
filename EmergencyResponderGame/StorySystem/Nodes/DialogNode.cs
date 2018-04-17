using Alexa.NET.Response.Ssml;
using EmergencyResponderGame.SpeechUtilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmergencyResponderGame.StorySystem.Nodes
{
    public class DialogNode
    {
        #region Properties and Fields

        public Speech Speech { get; }

        #endregion

        public DialogNode(SpeechBuilder speech)
        {
            Speech = speech.Build();
        }
    }
}
