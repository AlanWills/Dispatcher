using Alexa.NET.Response.Ssml;
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

        public abstract Speech GetSpeech();
    }
}