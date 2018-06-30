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

        public string SmallImageUrl { get; private set; }

        public string LargeImageUrl { get; private set; }

        #endregion

        public FinishWithCardNode(string title, string content, string smallImageUrl, string largeImageUrl, SpeechBuilder speechBuilder) : 
            base(-1, speechBuilder)
        {
            Title = title;
            Content = content;
            SmallImageUrl = smallImageUrl;
            LargeImageUrl = largeImageUrl;
        }

        #region Virtual Overrides

        public override SkillResponse CreateResponse()
        {
            SkillResponse response = ResponseBuilder.Tell(Speech);
            StandardCard card = new StandardCard();
            card.Title = Title;
            card.Content = Content;
            card.Image = new CardImage();
            card.Image.SmallImageUrl = SmallImageUrl;
            card.Image.LargeImageUrl = LargeImageUrl;
            response.Response.Card = card;
            response.Response.ShouldEndSession = true;

            return response;
        }
        
        #endregion
    }
}
