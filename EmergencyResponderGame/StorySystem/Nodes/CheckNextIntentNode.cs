using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.Request;
using Alexa.NET.Response.Ssml;
using Amazon.Lambda.Core;
using EmergencyResponderGame.IntentHandlers;

namespace EmergencyResponderGame.StorySystem.Nodes
{
    public class CheckNextIntentNode : BaseNode
    {
        #region Properties and Fields
        
        /// <summary>
        /// The name of the intent handler we are wanting this node to be processed by.
        /// </summary>
        public string IntentName { get; }

        /// <summary>
        /// The value we will store in the session attributes to track if the user successfully triggered the correct intent.
        /// </summary>
        public string ParameterName { get; }

        #endregion

        public CheckNextIntentNode(int nextNodeIndex, string intentName, string parameterName) : 
            base(nextNodeIndex)
        {
            IntentName = intentName;
            ParameterName = parameterName;
        }

        public override void ModifySessionAttributes(Dictionary<string, object> attributes, Intent intent, Session session, ILambdaContext lambdaContext)
        {
            base.ModifySessionAttributes(attributes, intent, session, lambdaContext);
            attributes.Add(ParameterName, IntentName == intent.Name);
        }
    }
}
