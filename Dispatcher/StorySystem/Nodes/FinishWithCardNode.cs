using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using Alexa.NET;
using Dispatcher.SpeechUtilities;

namespace Dispatcher.StorySystem.Nodes
{
    public class FinishWithCardNode : SpeechNode
    {
        #region Properties and Fields

        public override bool PausesStory { get { return true; } }

        public string Title { get; private set; }

        public string Content { get; private set; }

        #endregion

        public FinishWithCardNode(string title, string content, SpeechBuilder speechBuilder) : 
            base(-1, speechBuilder)
        {
            Title = title;
            Content = content;
        }

        #region Virtual Overrides

        public override SkillResponse CreateResponse()
        {
            SkillResponse response = ResponseBuilder.TellWithCard(Speech, Title, Content);
            response.Response.ShouldEndSession = true;

            return response;
        }
        
        #endregion
    }
}
