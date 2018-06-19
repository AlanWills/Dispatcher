using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using Dispatcher.StorySystem;
using Dispatcher.StorySystem.Nodes;
using Alexa.NET;

namespace Dispatcher.IntentHandlers
{
    public class StartFromNodeIntentHandler : IntentHandler
    {
        #region Properties and Fields
        
        /// <summary>
        /// The name of the slot for the node index.
        /// </summary>
        public const string NodeIndexSlotName = "NodeIndex";

        /// <summary>
        /// The AWS intent name that this handler will process.
        /// </summary>
        public override string IntentName { get { return Intents.StartFromNodeIntentName; } }

        #endregion

        public override SkillResponse HandleIntent(Intent intent, Session session, ILambdaContext lambdaContext)
        {
            long nodeIndex = long.Parse(intent.Slots["NodeIndex"].Value);
            return Story.CreateResponseForNode(nodeIndex, intent, session, lambdaContext);
        }
    }
}
