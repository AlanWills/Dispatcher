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
    public class StartFromNodeIntentHandler : IntentHandler
    {
        #region Properties and Fields

        /// <summary>
        /// The AWS intent name that this handler will process.
        /// </summary>
        public const string ConstIntentName = "StartFromNodeIntent";

        /// <summary>
        /// The name of the slot for the node index.
        /// </summary>
        public const string NodeIndexSlotName = "NodeIndex";

        /// <summary>
        /// The AWS intent name that this handler will process.
        /// </summary>
        public override string IntentName { get { return ConstIntentName; } }

        #endregion

        public override SkillResponse HandleIntent(Intent intent, Session session, ILambdaContext lambdaContext)
        {
            int nodeIndex = int.Parse(intent.Slots["NodeIndex"].Value);
            BaseNode node = Story.Nodes[nodeIndex - 1];
            SkillResponse response = ResponseBuilder.Tell(node.GetSpeech(this));

            response.Response.ShouldEndSession = Story.Nodes.Count == node.NextNodeIndex;
            response.SessionAttributes = response.SessionAttributes ?? new Dictionary<string, object>();
            response.SessionAttributes.Add(Story.CurrentNodeIndexKey, node.NextNodeIndex);

            return response;
        }
    }
}
