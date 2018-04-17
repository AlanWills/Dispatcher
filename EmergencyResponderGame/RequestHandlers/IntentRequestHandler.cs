using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using Alexa.NET.Request.Type;
using Alexa.NET.Response.Ssml;
using Alexa.NET;
using EmergencyResponderGame.IntentHandlers;

namespace EmergencyResponderGame.RequestHandlers
{
    public class IntentRequestHandler : SkillRequestHandler
    {
        #region Properties and Fields

        /// <summary>
        /// All of the currently supported intent handlers.
        /// </summary>
        private static List<IntentHandler> IntentHandlers { get; set; } = new List<IntentHandler>()
        {
            new PlayGameIntentHandler()
        };

        #endregion

        #region Skill Request Handler Implementations

        /// <summary>
        /// Return true if the inputted request is an IntentRequest and it's Intent is not null.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override bool IsHandlerForRequest(SkillRequest request)
        {
            return request.Request is IntentRequest &&
                   (request.Request as IntentRequest).Intent != null;
        }

        /// <summary>
        /// Find an appropriate intent handler and use it to process the incoming intent.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="lambdaContext"></param>
        /// <returns></returns>
        public override SkillResponse HandleRequest(SkillRequest request, ILambdaContext lambdaContext)
        {
            IntentRequest intentRequest = request.Request as IntentRequest;
            lambdaContext.Logger.LogLine("Request Intent: " + intentRequest.Intent.Name);

            IntentHandler appropriateIntentHandler = IntentHandlers.Find(x => x.IsHandlerForIntent(intentRequest.Intent));
            return appropriateIntentHandler != null ? appropriateIntentHandler.HandleIntent(intentRequest.Intent, request.Session, lambdaContext) : ResponseBuilder.Empty();
        }

        #endregion
    }
}
