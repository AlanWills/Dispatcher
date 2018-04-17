using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using EmergencyResponderGame.StorySystem;
using EmergencyResponderGame.StorySystem.Nodes;
using Alexa.NET;

namespace EmergencyResponderGame.IntentHandlers
{
    public class NoIntentHandler : IntentHandler
    {
        #region Properties and Fields

        /// <summary>
        /// The name of the intent this handler will process.
        /// </summary>
        public const string IntentName = "AMAZON.NoIntent";

        #endregion

        #region Intent Handler Abstract Implementations

        /// <summary>
        /// Returns true if the inputted intent's name matches the IntentName variable.
        /// </summary>
        /// <param name="intent"></param>
        /// <returns></returns>
        public override bool IsHandlerForIntent(Intent intent)
        {
            return intent.Name == IntentName;
        }

        /// <summary>
        /// Gets the current node in the story as a choice node and obtains the next node in the story based on the 'Yes' choice.
        /// </summary>
        /// <param name="intent"></param>
        /// <param name="session"></param>
        /// <param name="lambdaContext"></param>
        /// <returns></returns>
        public override SkillResponse HandleIntent(Intent intent, Session session, ILambdaContext lambdaContext)
        {
            int currentNodeIndex = session.GetCurrentNodeIndex();
            ChoiceNode choiceNode = Story.Nodes[currentNodeIndex] as ChoiceNode;
            int nextNodeIndex = choiceNode.GetNextNodeIndexForChoice(IntentName);
            BaseNode nextNode = Story.Nodes[nextNodeIndex];
            SkillResponse response = ResponseBuilder.Tell(nextNode.GetSpeech());

            response.Response.ShouldEndSession = Story.Nodes.Count == nextNode.NextNodeIndex;
            response.SessionAttributes = response.SessionAttributes ?? new Dictionary<string, object>();
            response.SessionAttributes.Add(Story.CurrentNodeIndexKey, nextNode.NextNodeIndex);

            return response;
        }

        #endregion
    }
}