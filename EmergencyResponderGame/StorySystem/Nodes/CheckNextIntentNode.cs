using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.Response.Ssml;
using EmergencyResponderGame.IntentHandlers;

namespace EmergencyResponderGame.StorySystem.Nodes
{
    public class CheckNextIntentNode : BaseNode
    {
        #region Properties and Fields

        /// <summary>
        /// The type of the intent handler we are wanting this node to be processed by.
        /// </summary>
        public Type IntentHandlerType { get; }

        /// <summary>
        /// The value we will store in the session attributes to track if the user successfully triggered the correct intent.
        /// </summary>
        public string ParameterName { get; }

        #endregion

        public CheckNextIntentNode(int nextNodeIndex, Type intentHandlerType, string parameterName) : 
            base(nextNodeIndex)
        {
            IntentHandlerType = intentHandlerType;
            ParameterName = parameterName;
        }

        #region Base Node Abstract Implementations

        public override Speech GetSpeech(IntentHandler currentIntentHandler)
        {
            return new Speech();
        }

        #endregion
    }
}
