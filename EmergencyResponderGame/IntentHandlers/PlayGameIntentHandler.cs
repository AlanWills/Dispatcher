using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using Alexa.NET.Response.Ssml;
using Alexa.NET;
using EmergencyResponderGame.StorySystem;

namespace EmergencyResponderGame.IntentHandlers
{
    public class PlayGameIntentHandler : IntentHandler
    {
        #region Properties and Fields

        /// <summary>
        /// The name in the Alexa Dev Console for this intent.
        /// </summary>
        public const string IntentName = "PlayGameIntent";

        #endregion

        #region Intent Handler Abstract Implementations

        /// <summary>
        /// Returns true if the inputted intent matches the IntentName const variable.
        /// </summary>
        /// <param name="intent"></param>
        /// <returns></returns>
        public override bool IsHandlerForIntent(Intent intent)
        {
            return intent.Name == IntentName;
        }

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
            SkillResponse response = ResponseBuilder.Tell(Story.Nodes[0].Speech);
            response.Response.ShouldEndSession = Story.Nodes.Count == 1;
            response.SessionAttributes = response.SessionAttributes ?? new Dictionary<string, object>();
            response.SessionAttributes.Add("NodeIndex", 0);

            return response;
        }

        #endregion
    }
}
