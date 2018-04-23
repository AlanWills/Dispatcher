using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Alexa.NET.Request.Type;
using Alexa.NET.Response.Ssml;
using Alexa.NET;
using Amazon.Lambda.Core;

namespace EmergencyResponderGame.RequestHandlers
{
    public class LaunchRequestHandler : SkillRequestHandler
    {
        #region Skill Request Handler Implementations

        /// <summary>
        /// Returns true if the inputted request corresponds to a LaunchRequest.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override bool IsHandlerForRequest(SkillRequest request)
        {
            return request.Request is LaunchRequest;
        }

        /// <summary>
        /// Provides a simple introduction when launched about how to play the game.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override SkillResponse HandleRequest(SkillRequest request, ILambdaContext lambdaContext)
        {
            Speech speech = new Speech();
            speech.Elements.Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/General/LaunchMessage.mp3"));

            SkillResponse response = ResponseBuilder.Tell(speech);
            response.Response.ShouldEndSession = false;

            return response;
        }

        #endregion
    }
}