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

        public override SkillResponse HandleRequest(SkillRequest request, ILambdaContext lambdaContext)
        {
            IntentRequest intentRequest = request.Request as IntentRequest;
            lambdaContext.Logger.LogLine("Request Intent: " + intentRequest.Intent.Name);

            IntentHandler appropriateIntentHandler = IntentHandlers.Find(x => x.IsHandlerForIntent(intentRequest.Intent));
            return appropriateIntentHandler != null ? appropriateIntentHandler.HandleIntent(intentRequest.Intent, request.Session, lambdaContext) : ResponseBuilder.Empty();

            switch (intentRequest.Intent.Name)
            {
                case "AMAZON.YesIntent":
                    {
                        string state = request.Session.Attributes["State"] as string;
                        if (state == "Introduction")
                        {
                            Speech speech = new Speech();
                            speech.Elements.Add(new Sentence("Help, my leg is broken."));

                            SkillResponse response = ResponseBuilder.Tell(speech);
                            response.Response.ShouldEndSession = false;
                            response.SessionAttributes = response.SessionAttributes ?? new Dictionary<string, object>();
                            response.SessionAttributes.Add("State", "Question1");

                            return response;
                        }
                        else if (state == "Round1")
                        {
                            Speech speech = new Speech();
                            speech.Elements.Add(new Sentence("Help, my father has stopped breathing."));

                            SkillResponse response = ResponseBuilder.Tell(speech);
                            response.Response.ShouldEndSession = false;
                            response.SessionAttributes = response.SessionAttributes ?? new Dictionary<string, object>();
                            response.SessionAttributes.Add("State", "Question2");

                            return response;
                        }

                        return ResponseBuilder.Empty();
                    }

                case "AskQuestionIntent":
                    {
                        string state = request.Session.Attributes["State"] as string;
                        if (state == "Question1")
                        {
                            Speech speech = new Speech();
                            speech.Elements.Add(new Sentence("124 Goswell Road."));
                            speech.Elements.Add(new Break() { Time = "2s" });
                            speech.Elements.Add(new Sentence("Nice job.  Ready to try something a little harder?"));

                            SkillResponse response = ResponseBuilder.Tell(speech);
                            response.Response.ShouldEndSession = false;
                            response.SessionAttributes = response.SessionAttributes ?? new Dictionary<string, object>();
                            response.SessionAttributes.Add("State", "Round1");

                            return response;
                        }
                        else if (state == "Question2")
                        {
                            Speech speech = new Speech();
                            speech.Elements.Add(new Sentence("16C Crossley Street."));
                            speech.Elements.Add(new Break() { Time = "2s" });
                            speech.Elements.Add(new Sentence("We've just had news of an accident on the motorway.  Things are about to get manic!"));
                            speech.Elements.Add(new Break() { Time = "2s" });
                            speech.Elements.Add(new Sentence("Help, I've been in an accident and my husband is trapped in the car."));

                            SkillResponse response = ResponseBuilder.Tell(speech);
                            response.Response.ShouldEndSession = false;
                            response.SessionAttributes = response.SessionAttributes ?? new Dictionary<string, object>();
                            response.SessionAttributes.Add("State", "Question3");

                            return response;
                        }
                        else if (state == "Question3")
                        {
                            Speech speech = new Speech();
                            speech.Elements.Add(new Sentence("On the M25."));
                            speech.Elements.Add(new Break() { Time = "2s" });
                            speech.Elements.Add(new Sentence("Well done, that was an extremely tough situation.  We hope you enjoyed the game."));

                            SkillResponse response = ResponseBuilder.Tell(speech);
                            response.Response.ShouldEndSession = true;
                            response.SessionAttributes = response.SessionAttributes ?? new Dictionary<string, object>();
                            response.SessionAttributes.Add("State", "End");

                            return response;
                        }

                        return ResponseBuilder.Empty();
                    }
            }
        }

        #endregion
    }
}
