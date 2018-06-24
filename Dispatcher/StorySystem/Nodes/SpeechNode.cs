using Alexa.NET.Response.Ssml;
using Dispatcher.IntentHandlers;
using Dispatcher.SpeechUtilities;
using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.Response;
using Alexa.NET;
using Alexa.NET.Request;
using Amazon.Lambda.Core;

namespace Dispatcher.StorySystem.Nodes
{
    public class SpeechNode : BaseNode
    {
        #region Properties and Fields

        public override bool PausesStory { get { return true; } }

        public Speech Speech { get; }

        #endregion

        public SpeechNode(int nextNodeIndex, SpeechBuilder speech) :
            base(nextNodeIndex)
        {
            Speech = speech.Build();
        }

        #region Virtual and Abstract Functions

        public override SkillResponse CreateResponse()
        {
            SkillResponse response = ResponseBuilder.Tell(Speech);
            response.Response.ShouldEndSession = false;

            return response;
        }

        public override void ModifySessionAttributes(Dictionary<string, object> attributes, Intent intent, Session session, ILambdaContext lambdaContext)
        {
            base.ModifySessionAttributes(attributes, intent, session, lambdaContext);

            // Update the record of the current node we are on
            long nextIndex = NextNodeIndex;
            lambdaContext.Logger.LogLine("Next Index: " + nextIndex);

            if (!attributes.ContainsKey(Story.CurrentNodeIndexKey))
            {
                attributes.Add(Story.CurrentNodeIndexKey, nextIndex);
            }
            else
            {
                attributes[Story.CurrentNodeIndexKey] = nextIndex;
            }
        }

        #endregion
    }
}
