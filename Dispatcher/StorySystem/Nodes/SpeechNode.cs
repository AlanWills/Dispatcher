using Alexa.NET.Response.Ssml;
using Dispatcher.IntentHandlers;
using Dispatcher.SpeechUtilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dispatcher.StorySystem.Nodes
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
