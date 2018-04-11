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

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace EmergencyResponderGame
{
    public class Function
    {
        public SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            // Tokens cannot be the same otherwise things will not work
            context.Logger.LogLine("Request Type: " + input.GetRequestType().Name);

            if (input.Request is IntentRequest &&
                (input.Request as IntentRequest).Intent != null)
            {
                IntentRequest request = input.Request as IntentRequest;
                context.Logger.LogLine("Request Intent: " + request.Intent.Name);

                switch ((input.Request as IntentRequest).Intent.Name)
                {
                    case "PlayGameIntent":
                        {
                            Speech speech = new Speech();
                            speech.Elements.Add(new Sentence("Introduction or tutorial here."));
                            speech.Elements.Add(new Break() { Time = "2s" });
                            speech.Elements.Add(new Sentence("Are you ready for your first call?"));

                            SkillResponse response = ResponseBuilder.Tell(speech);
                            response.Response.ShouldEndSession = false;
                            response.SessionAttributes = response.SessionAttributes ?? new Dictionary<string, object>();
                            response.SessionAttributes.Add("State", "Introduction");

                            return response;
                        }

                    case "AMAZON.YesIntent":
                        {
                            string state = input.Session.Attributes["State"] as string;
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
                            string state = input.Session.Attributes["State"] as string;
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

                    default:
                        {
                            return ResponseBuilder.Empty();
                        }
                }
            }
            else if (input.Request is LaunchRequest)
            {
                Speech speech = new Speech();
                speech.Elements.Add(new Sentence("Welcome.  Say Start the game to begin."));

                SkillResponse response = ResponseBuilder.Tell(speech);
                response.Response.ShouldEndSession = false;

                return response;
            }

            else if (input.Request is SessionEndedRequest)
            {
                context.Logger.LogLine("Session ended");
            }

            return ResponseBuilder.Empty();
        }
    }
}
