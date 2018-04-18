using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using EmergencyResponderGame.StorySystem;
using EmergencyResponderGame.StorySystem.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmergencyResponderGame.IntentHandlers
{
    public abstract class CheckIntentHandler : IntentHandler
    {
        #region Intent Handler Abstract Implementations

        public override SkillResponse HandleIntent(Intent intent, Session session, ILambdaContext lambdaContext)
        {
            int currentNodeIndex = session.GetCurrentNodeIndex();
            lambdaContext.Logger.LogLine("Current node index " + currentNodeIndex);

            CheckNextIntentNode currentNode = Story.Nodes[currentNodeIndex] as CheckNextIntentNode;
            BaseNode nextNode = currentNode.GetNextNode(this);
            lambdaContext.Logger.LogLine("Next node null? " + (nextNode == null));

            SkillResponse response = nextNode != null ? ResponseBuilder.Tell(nextNode.GetSpeech(this)) : ResponseBuilder.Empty();
            response.Response.ShouldEndSession = nextNode == null;

            // Add a record whether the correct intent was triggered
            response.SessionAttributes = response.SessionAttributes ?? new Dictionary<string, object>();
            response.SessionAttributes.Add(currentNode.ParameterName, currentNode.IntentHandlerType == GetType());
            response.SessionAttributes.Add(Story.CurrentNodeIndexKey, nextNode.NextNodeIndex);

            return response;
        }

        #endregion
    }
}
