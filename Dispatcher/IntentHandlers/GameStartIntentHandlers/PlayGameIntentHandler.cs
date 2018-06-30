using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using Alexa.NET.Response.Ssml;
using Alexa.NET;
using Dispatcher.StorySystem;
using Dispatcher.StorySystem.Nodes;

namespace Dispatcher.IntentHandlers
{
    public class PlayGameIntentHandler : IntentHandler
    {
        #region Properties and Fields

        /// <summary>
        /// The name of the intent this handler will process.
        /// </summary>
        public override string IntentName { get { return Intents.PlayGameIntentName; } }

        #endregion

        #region Intent Handler Abstract Implementations
        
        /// <summary>
        /// The behaviour when we want to start the game.
        /// Begins the story from the beginning.
        /// </summary>
        /// <param name="intent"></param>
        /// <param name="session"></param>
        /// <param name="lambdaContext"></param>
        /// <returns></returns>
        public override SkillResponse HandleIntent(Intent intent, Session session, ILambdaContext lambdaContext)
        {
            return Story.CreateResponseForNode(0, intent, session, lambdaContext);
        }

        #endregion
    }
}
