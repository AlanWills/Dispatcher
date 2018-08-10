using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using Dispatcher.StorySystem;

namespace Dispatcher.IntentHandlers
{
    public class SkipIntroIntentHandler : IntentHandler
    {
        #region Properties and Fields

        /// <summary>
        /// The name of the intent this handler will process.
        /// </summary>
        public override string IntentName
        {
            get { return Intents.SkipIntentName; }
        }

        #endregion

        #region Intent Handler Abstract Implementations

        /// <summary>
        /// The behaviour when we want help from within the game.
        /// If we are at the beginning, we skip to the node containing just the dispatcher intro.
        /// Otherwise we just carry on the story as normal.
        /// </summary>
        /// <param name="intent"></param>
        /// <param name="session"></param>
        /// <param name="lambdaContext"></param>
        /// <returns></returns>
        public override SkillResponse HandleIntent(Intent intent, Session session, ILambdaContext lambdaContext)
        {
            int currentNodeIndex = session.GetCurrentNodeIndex();
            if (currentNodeIndex == 1)
            {
                // 75 is currently the node with the skipped intro dialog
                return Story.CreateResponseForNode(75, null, session, lambdaContext);
            }
            else
            {
                return Story.CreateResponse(intent, session, lambdaContext);
            }
        }

        #endregion
    }
}
