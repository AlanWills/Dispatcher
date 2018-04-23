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
            // 0
            new SpeechNode(1, new SpeechBuilder().
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/Introduction/Introduction.mp3"))),
            // 1
            ConditionNode.CreateYesNoChoiceNode(2, 3),
            // 2
            new SpeechNode(4, new SpeechBuilder().
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Demo/C1_Demo_Handler_Ready_Yes.mp3")).
                Add(new Break() { Time = "1s" }).
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Demo/C1_Demo_Handler_WhatHappened.mp3")).
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Demo/C1_Demo_Caller_WhatHappened.mp3")).
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Demo/C1_Demo_Handler_Breathing.mp3"))),

                // NOT ALLOWED MORE THAN FIVE.  ARGH
                // Concatenate the actual call into one using FFMPEG
                //Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Demo/C1_Demo_Caller_Breathing.mp3")).
                //Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Demo/C1_Demo_Handler_Conscious.mp3")).
                //Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Demo/C1_Demo_Caller_Conscious.mp3")).
                //Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Demo/C1_Demo_Handler_EndCall.mp3")).
                //Add(new Break() { Time = "1s" }).
                //Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Demo/C1_Demo_Handler_EndDemo.mp3"))),
            // 3
            new SpeechNode(4, new SpeechBuilder().
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Demo/C1_Demo_Handler_Ready_No.mp3")).
                Add(new Break() { Time = "1s" }).
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Demo/C1_Demo_Handler_WhatHappened.mp3")).
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Demo/C1_Demo_Caller_WhatHappened.mp3")).
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Demo/C1_Demo_Handler_Breathing.mp3")).
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Demo/C1_Demo_Caller_Breathing.mp3")).
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Demo/C1_Demo_Handler_Conscious.mp3")).
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Demo/C1_Demo_Caller_Conscious.mp3")).
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Demo/C1_Demo_Handler_EndCall.mp3")).
                Add(new Break() { Time = "1s" }).
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Demo/C1_Demo_Handler_EndDemo.mp3"))),
            // 4
            new CheckNextIntentNode(5, Intents.StartCallIntentName, 6, "C1Tutorial_StartCall_Correct"),
            // 5
            new SpeechNode(7, new SpeechBuilder().
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Tutorial/C1_Tutorial_Handler_WhatHappened_Correct.mp3"))),
            // 6
            new SpeechNode(4, new SpeechBuilder().
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Tutorial/C1_Tutorial_Handler_WhatHappened_Incorrect.mp3"))),
            // 7
            new CheckNextIntentNode(8, Intents.IsBreathingQuestionIntentName, 9, "C1Tutorial_IsBreathing_Correct"),
            // 8
            new SpeechNode(10, new SpeechBuilder().
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Tutorial/C1_Tutorial_Handler_Breathing_Correct.mp3"))),
            // 9
            new SpeechNode(7, new SpeechBuilder().
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Tutorial/C1_Tutorial_Handler_Breathing_Incorrect.mp3"))),
            // 10
            new CheckNextIntentNode(11, Intents.IsConsciousQuestionIntentName, 12, "C1Tutorial_IsConscious_Correct"),
            // 11
            new SpeechNode(13, new SpeechBuilder().
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Tutorial/C1_Tutorial_Handler_Conscious_Correct.mp3"))),
            // 12
            new SpeechNode(10, new SpeechBuilder().
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Tutorial/C1_Tutorial_Handler_Conscious_Incorrect.mp3"))),
            // 13
            new CheckNextIntentNode(14, Intents.EndCallQuestionIntentName, 15, "C1Tutorial_EndCall_Correct"),
            // 14
            new SpeechNode(14, new SpeechBuilder().
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Tutorial/C1_Tutorial_Handler_EndCall_Correct.mp3"))),
            // 15
            new SpeechNode(13, new SpeechBuilder().
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Tutorial/C1_Tutorial_Handler_EndCall_Incorrect.mp3"))),
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
            BaseNode node = GetCurrentNode(session);
            BaseNode nextNode = node.GetNextNode(intent, session ,lambdaContext);

            return CreateResponseForNode(nextNode, intent, session, lambdaContext);
        }

        public static SkillResponse CreateResponseForNode(long nodeIndex, Intent intent, Session session, ILambdaContext lambdaContext)
        {
            return CreateResponseForNode(Nodes[(int)nodeIndex], intent, session, lambdaContext);
        }

        public static SkillResponse CreateResponseForNode(BaseNode node, Intent intent, Session session, ILambdaContext lambdaContext)
        {
            Dictionary<string, object> responseSessionAttributes = session.Attributes ?? new Dictionary<string, object>();

            while (node != null && !(node is SpeechNode))
            {
                node.ModifySessionAttributes(session.Attributes, intent, session, lambdaContext);
                node = node.GetNextNode(intent, session, lambdaContext);
            }

            SpeechNode speechNode = node as SpeechNode;
            lambdaContext.Logger.LogLine("Speech Node element count " + speechNode.Speech.Elements.Count);

            SkillResponse response = speechNode != null ? ResponseBuilder.Tell(speechNode.Speech) : ResponseBuilder.Empty();
            response.Response.ShouldEndSession = speechNode == null;

            // Update the record of the current node we are on
            response.SessionAttributes = responseSessionAttributes;

            long nextIndex = speechNode != null ? speechNode.NextNodeIndex : -1;
            lambdaContext.Logger.LogLine("Next Index: " + nextIndex);

            if (!responseSessionAttributes.ContainsKey(CurrentNodeIndexKey))
            {
                response.SessionAttributes.Add(CurrentNodeIndexKey, nextIndex);
            }
            else
            {
                response.SessionAttributes[CurrentNodeIndexKey] = nextIndex;
            }

            return response;
        }

        #endregion
    }
}