using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Alexa.NET.Response.Ssml;
using Amazon.Lambda.Core;
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

        public static List<BaseNode> Nodes { get; set; } = new List<BaseNode>()
        {
            new SpeechNode(1, new SpeechBuilder().
                Add(new Sentence("Welcome to your training as an emergency call handler.")).
                Add(new Sentence("We're going to dive in at the deep end and learn about the first of four call categories.")).
                Add(new Sentence("This category is called C1 and is for life threatening cases such as heart attack or serious allergies.")).
                Add(new Sentence("I'm going to show you exactly what to do when receiving a call like this.")).
                Add(new Break() { Time = "1s" }).
                Add(new Sentence("Ready?"))),
            ConditionNode.CreateYesNoChoiceNode(2, 3),
            new SpeechNode(4, new SpeechBuilder().
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
            new SpeechNode(4, new SpeechBuilder().
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
            new CheckNextIntentNode(5, Intents.StartCallIntentName, "C1Tutorial_StartCall_Correct"),
            new SpeechNode(6, new SpeechBuilder().Add(new Sentence("My son has stopped moving."))),
            new CheckNextIntentNode(7, Intents.IsBreathingQuestionIntentName, "C1Tutorial_IsBreathing_Correct"),
            new SpeechNode(8, new SpeechBuilder().Add(new Sentence("No he's not breathing."))),
            new CheckNextIntentNode(9, Intents.IsConsciousQuestionIntentName, "C1Tutorial_IsConscious_Correct"),
            new SpeechNode(10, new SpeechBuilder().Add(new Sentence("No he's just lying there not moving."))),
            new CheckNextIntentNode(11, Intents.WhatHappenedQuestionIntentName, "C1Tutorial_WhatHappened_Correct"),
            new SpeechNode(12, new SpeechBuilder().Add(new Sentence("It must be something he ate.  We were having dinner, but all of a sudden he just collapsed."))),
            new CheckNextIntentNode(13, Intents.EndCallQuestionIntentName, "C1Tutorial_EndCall_Correct"),
            new SpeechNode(14, new SpeechBuilder().Add(new Sentence("Great job!"))),
        };

        #endregion

        #region Runtime Functions

        public static BaseNode GetCurrentNode(Session session)
        {
            int currentNodeIndex = session.GetCurrentNodeIndex();
            return Nodes[currentNodeIndex];
        }

        public static SkillResponse CreateResponse(Intent intent, Session session, ILambdaContext lambdaContext)
        {
            BaseNode currentNode = GetCurrentNode(session);
            BaseNode nextNode = currentNode.GetNextNode(intent, session, lambdaContext);

            Dictionary<string, object> responseSessionAttributes = session.Attributes ?? new Dictionary<string, object>();

            while (nextNode != null && !(nextNode is SpeechNode))
            {
                nextNode.ModifySessionAttributes(responseSessionAttributes, intent, session, lambdaContext);
                nextNode = nextNode.GetNextNode(intent, session, lambdaContext);
            }

            SpeechNode speechNode = nextNode as SpeechNode;
            SkillResponse response = speechNode != null ? ResponseBuilder.Tell(speechNode.Speech) : ResponseBuilder.Empty();
            response.Response.ShouldEndSession = speechNode == null;
            
            // Update the record of the current node we are on
            response.SessionAttributes = responseSessionAttributes;
            response.SessionAttributes.Add(CurrentNodeIndexKey, speechNode != null ? speechNode.NextNodeIndex : -1);
            
            return response;
        }

        #endregion
    }
}