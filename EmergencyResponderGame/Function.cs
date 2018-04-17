using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Alexa.NET.Request.Type;
using Alexa.NET.Request;
using Alexa.NET;
using Alexa.NET.Response;
using Alexa.NET.Response.Ssml;
using EmergencyResponderGame.RequestHandlers;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace EmergencyResponderGame
{
    public class Function
    {
        /// <summary>
        /// All of the available skill request handlers that can handle incoming skill requests.
        /// </summary>
        private static List<SkillRequestHandler> SkillRequestHandlers { get; set; } = new List<SkillRequestHandler>()
        {
            // Intent request is most likely
            new IntentRequestHandler(),
            new LaunchRequestHandler(),
            new SessionEndedRequestHandler()
        };

        public SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            context.Logger.LogLine("Request Type: " + input.GetRequestType().Name);

            // Either use the appropriate request handler or just return an empty response.
            SkillRequestHandler appropriateRequestHandler = SkillRequestHandlers.Find(x => x.IsHandlerForRequest(input));
            return appropriateRequestHandler != null ? appropriateRequestHandler.HandleRequest(input, context) : ResponseBuilder.Empty();
        }
    }
}
