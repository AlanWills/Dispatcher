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
        /// The index of the node which we will progress to if the intent we are checking is not the one we are waiting for.
        /// </summary>
        public int IncorrectIntentNextNodeIndex { get; }

        /// <summary>
        /// The value we will store in the session attributes to track if the user successfully triggered the correct intent.
        /// </summary>
        public string ParameterName { get; }

        #endregion

        public CheckNextIntentNode(int nextNodeIndex, string intentName, int incorrectIntentNextNodeIndex, string parameterName) : 
            base(nextNodeIndex)
        {
            IntentName = intentName;
            IncorrectIntentNextNodeIndex = incorrectIntentNextNodeIndex;
            ParameterName = parameterName;
        }

        public override void ModifySessionAttributes(Dictionary<string, object> attributes, Intent intent, Session session, ILambdaContext lambdaContext)
        {
            base.ModifySessionAttributes(attributes, intent, session, lambdaContext);
            attributes.Add(ParameterName, IntentName == intent.Name);
        }

        public override BaseNode GetNextNode(Intent intent, Session session, ILambdaContext lambdaContext)
        {
            if (intent.Name == IntentName)
            {
                return base.GetNextNode(intent, session, lambdaContext);
            }
            else
            {
                return Story.Nodes[IncorrectIntentNextNodeIndex];
            }
        }
    }
}
