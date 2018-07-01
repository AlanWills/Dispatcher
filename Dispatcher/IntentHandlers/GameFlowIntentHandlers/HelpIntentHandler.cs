using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using Dispatcher.StorySystem;

namespace Dispatcher.IntentHandlers
{
    public class HelpIntentHandler : IntentHandler
    {
        #region Properties and Fields

        /// <summary>
        /// The name of the intent this handler will process.
        /// </summary>
        public override string IntentName
        {
            get { return Intents.HelpIntentName; }
        }

        #endregion

        #region Intent Handler Abstract Implementations

        /// <summary>
        /// The behaviour when we want help from within the game.
        /// We imagine that we don't know what to do and so get the dispatcher to progress the story.
        /// </summary>
        /// <param name="intent"></param>
        /// <param name="session"></param>
        /// <param name="lambdaContext"></param>
        /// <returns></returns>
        public override SkillResponse HandleIntent(Intent intent, Session session, ILambdaContext lambdaContext)
        {
            return Story.CreateResponse(intent, session, lambdaContext);
        }

        #endregion
    }
}
