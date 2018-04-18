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
            new DialogNode(4, new SpeechBuilder().
                Add(new Sentence("Great stuff!  Now listen carefully.")).
                Add(new Break() { Time = "1s" }).
                Add(new Sentence("Hello, nine nine nine.")).
                Add(new Sentence("Hello, my husband has collapsed.")).
                Add(new Sentence("OK, is he breathing?")).
                Add(new Sentence("No, he's not.")).
                Add(new Sentence("OK, is he conscious?")).
                Add(new Sentence("No.")).
                Add(new Sentence("Tell me what happened.")).
                Add(new Sentence("He was working in the garden and then suddenly collapsed.")).
                Add(new Sentence("OK, I'm organising help for you now.")).
                Add(new Break() { Time = "1s" }).
                Add(new Sentence("Now it's your turn!")).
                Add(new Sentence("Don't worry, you will be doing a practice run with me first.")).
                Add(new Sentence("When you're ready, say Hello nine nine nine."))),
            new DialogNode(4, new SpeechBuilder().
                Add(new Sentence("Quit messing around.  Listen carefully.")).
                Add(new Break() { Time = "1s" }).
                Add(new Sentence("Hello, my husband has collapsed.")).
                Add(new Sentence("OK, is he breathing?")).
                Add(new Sentence("No, he's not.")).
                Add(new Sentence("OK, is he conscious?")).
                Add(new Sentence("No.")).
                Add(new Sentence("Tell me what happened.")).
                Add(new Sentence("He was working in the garden and then suddenly collapsed.")).
                Add(new Sentence("OK, I'm organising help for you now.")).
                Add(new Break() { Time = "1s" }).
                Add(new Sentence("Now it's your turn!")).
                Add(new Sentence("Don't worry, you will be doing a practice run with me first.")).
                Add(new Sentence("When you're ready, say Hello nine nine nine."))),
            new CheckNextIntentNode(5, typeof(StartCallIntentHandler), "C1Tutorial_StartCall_Correct"),
            new DialogNode(6, new SpeechBuilder().Add(new Sentence("My son has stopped moving."))),
            new CheckNextIntentNode(7, typeof(IsBreathingQuestionIntentHandler), "C1Tutorial_IsBreathing_Correct"),
            new DialogNode(8, new SpeechBuilder().Add(new Sentence("No he's not breathing."))),
            new CheckNextIntentNode(9, typeof(IsConsciousQuestionIntentHandler), "C1Tutorial_IsConscious_Correct"),
            new DialogNode(10, new SpeechBuilder().Add(new Sentence("No he's just lying there not moving."))),
            new CheckNextIntentNode(11, typeof(WhatHappenedQuestionIntentHandler), "C1Tutorial_WhatHappened_Correct"),
            new DialogNode(12, new SpeechBuilder().Add(new Sentence("It must be something he ate.  We were having dinner, but all of a sudden he just collapsed."))),
            new CheckNextIntentNode(13, typeof(EndCallIntentHandler), "C1Tutorial_EndCall_Correct"),
            new DialogNode(14, new SpeechBuilder().Add(new Sentence("Great job!"))),
        };

        #endregion
    }
}