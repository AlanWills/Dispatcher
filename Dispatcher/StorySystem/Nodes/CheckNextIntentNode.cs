using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Alexa.NET.Response.Ssml;
using Amazon.Lambda.Core;
using Dispatcher.IntentHandlers;
using Alexa.NET;

namespace Dispatcher.StorySystem.Nodes
{
    public class CheckNextIntentNode : BaseNode
    {
        #region Properties and Fields

        public override bool PausesStory { get { return false; } }

        public Dictionary<string, int> ValidIntents { get; }

        /// <summary>
        /// The index of the node which we will progress to if the intent we are checking is not the one we are waiting for.
        /// </summary>
        public int IncorrectIntentNextNodeIndex { get; }

        /// <summary>
        /// The value we will store in the session attributes to track if the user successfully triggered the correct intent.
        /// </summary>
        public string ParameterName { get; }

        #endregion

        #region Constructors

        public CheckNextIntentNode(int nextNodeIndex, string intentName, int incorrectIntentNextNodeIndex, string parameterName) : 
            base(nextNodeIndex)
        {
            ValidIntents = new Dictionary<string, int>() { { intentName, nextNodeIndex } };
            IncorrectIntentNextNodeIndex = incorrectIntentNextNodeIndex;
            ParameterName = parameterName;
        }

        public CheckNextIntentNode(Dictionary<string, int> possibleValidIntents, int incorrectIntentNextNodeIndex, string parameterName) :
            base(incorrectIntentNextNodeIndex)
        {
            ValidIntents = possibleValidIntents;
            IncorrectIntentNextNodeIndex = incorrectIntentNextNodeIndex;
            ParameterName = parameterName;
        }

        #endregion

        #region Base Node Overrides

        public override SkillResponse CreateResponse()
        {
            return ResponseBuilder.Empty();
        }

        public override void ModifySessionAttributes(Dictionary<string, object> attributes, Intent intent, Session session, ILambdaContext lambdaContext)
        {
            base.ModifySessionAttributes(attributes, intent, session, lambdaContext);
            attributes.Add(ParameterName, ValidIntents.ContainsKey(intent.Name));
        }

        public override BaseNode GetNextNode(Intent intent, Session session, ILambdaContext lambdaContext)
        {
            if (ValidIntents.ContainsKey(intent.Name))
            {
                lambdaContext.Logger.LogLine("Found intent " + intent.Name + " with node index " + ValidIntents[intent.Name]);
                return Story.Nodes[ValidIntents[intent.Name]];
            }
            else
            {
                lambdaContext.Logger.LogLine("No intent found.  Using node index " + IncorrectIntentNextNodeIndex);
                return Story.Nodes[IncorrectIntentNextNodeIndex];
            }
        }

        #endregion
    }
}
