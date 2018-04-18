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
    public class YesIntentHandler : IntentHandler
    {
        #region Properties and Fields

        /// <summary>
        /// The name of the intent this handler will process.
        /// </summary>
        public const string ConstIntentName = "AMAZON.YesIntent";

        /// <summary>
        /// The name of the intent this handler will process.
        /// </summary>
        public override string IntentName { get { return ConstIntentName; } }

        #endregion

        #region Intent Handler Abstract Implementations

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
            BaseNode nextNode = choiceNode.GetNextNode(this);
            SkillResponse response = ResponseBuilder.Tell(nextNode.GetSpeech(this));

            response.Response.ShouldEndSession = Story.Nodes.Count == nextNode.NextNodeIndex;
            response.SessionAttributes = response.SessionAttributes ?? new Dictionary<string, object>();
            response.SessionAttributes.Add(Story.CurrentNodeIndexKey, nextNode.NextNodeIndex);

            return response;
        }

        #endregion
    }
}