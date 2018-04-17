using Alexa.NET.Response.Ssml;
using EmergencyResponderGame.SpeechUtilities;
using EmergencyResponderGame.StorySystem.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmergencyResponderGame.StorySystem
{
    public static class Story
    {
        #region Properties and Fields

        public static List<DialogNode> Nodes { get; private set; } = new List<DialogNode>()
        {
            new DialogNode(new SpeechBuilder().
                Add(new Sentence("Welcome to your training as an emergency call handler.")).
                Add(new Sentence("We're going to dive in at the deep end.")))
        };

        #endregion
    }
}
