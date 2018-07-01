using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using Dispatcher.StorySystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dispatcher.IntentHandlers
{
    public class StartOverIntentHandler : IntentHandler
    {
        #region Properties and Fields

        /// <summary>
        /// The name of the intent we are handling.
        /// </summary>
        public override string IntentName
        {
            get { return Intents.StartOverIntentName; }
        }

        #endregion

        #region Intent Handler Abstract Implementations

        /// <summary>
        /// Completely stop the session.
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
