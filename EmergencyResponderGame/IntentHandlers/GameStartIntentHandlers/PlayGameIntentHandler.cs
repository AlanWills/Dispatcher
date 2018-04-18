using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using Alexa.NET.Response.Ssml;
using Alexa.NET;
using EmergencyResponderGame.StorySystem;
using EmergencyResponderGame.StorySystem.Nodes;

namespace EmergencyResponderGame.IntentHandlers
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
        /// The behaviour when the game first starts.
        /// This is currently to play an introduction.
        /// </summary>
        /// <param name="intent"></param>
        /// <param name="session"></param>
        /// <param name="lambdaContext"></param>
        /// <returns></returns>
        public override SkillResponse HandleIntent(Intent intent, Session session, ILambdaContext lambdaContext)
        {
            if (session.Attributes.ContainsKey(Story.CurrentNodeIndexKey))
            {
                session.Attributes[Story.CurrentNodeIndexKey] = 0;
            }
            else
            {
                session.Attributes.Add(Story.CurrentNodeIndexKey, 0);
            }

            return Story.CreateResponse(intent, session, lambdaContext);
        }

        #endregion
    }
}
