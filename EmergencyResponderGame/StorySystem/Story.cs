using Alexa.NET.Response.Ssml;
using EmergencyResponderGame.IntentHandlers;
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

        /// <summary>
        /// The key for the session attribute we use to store the current node index.
        /// </summary>
        public const string CurrentNodeIndexKey = "CurrentNodeIndex";

        public static List<BaseNode> Nodes { get; private set; } = new List<BaseNode>()
        {
            new DialogNode(1, new SpeechBuilder().
                Add(new Sentence("Welcome to your training as an emergency call handler.")).
                Add(new Sentence("We're going to dive in at the deep end and learn about the first of four call categories.")).
                Add(new Sentence("This category is called C1 and is for life threatening cases such as heart attack or serious allergies.")).
                Add(new Sentence("I'm going to show you exactly what to do when receiving a call like this.")).
                Add(new Break() { Time = "1s" }).
                Add(new Sentence("Ready?"))),
            ChoiceNode.CreateYesNoChoiceNode(2, 3),
            new DialogNode(4, new SpeechBuilder().Add(new Sentence("Great stuff!  Now listen carefully."))),
            new DialogNode(4, new SpeechBuilder().Add(new Sentence("Quit messing around.  Listen carefully."))),
            new DialogNode(5, new SpeechBuilder().
                Add(new Break() { Time = "1s" }).
                Add(new Sentence("Hello, nine nine nine.")))
        };

        #endregion
    }
}