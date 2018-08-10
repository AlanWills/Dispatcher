using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Alexa.NET.Request.Type;
using Alexa.NET;
using Amazon.Lambda.Core;
using Alexa.NET.Response.Ssml;

namespace Dispatcher.RequestHandlers
{
    public class SessionEndedRequestHandler : SkillRequestHandler
    {
        #region Properties and Fields

        /// <summary>
        /// Returns true if the inputted request was a SessionEndedRequest.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override bool IsHandlerForRequest(SkillRequest request)
        {
            return request.Request is SessionEndedRequest;
        }

        /// <summary>
        /// Perform some debug logging and return an empty response.
        /// The session is over.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override SkillResponse HandleRequest(SkillRequest request, ILambdaContext lambdaContext)
        {
            Speech speech = new Speech();
            speech.Elements.Add(new Audio(BirthCallAudioConsts.Goodbye));
            return ResponseBuilder.Tell(speech);
        }

        #endregion
    }
}
