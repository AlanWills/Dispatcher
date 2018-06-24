using Alexa.NET.Request;
using Alexa.NET.Response;
using Alexa.NET.Response.Ssml;
using Amazon.Lambda.Core;
using Dispatcher.IntentHandlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dispatcher.StorySystem.Nodes
{
    public abstract class BaseNode
    {
        #region Properties and Fields

        public int NextNodeIndex { get; }

        #endregion
        
        public BaseNode(int nextNodeIndex)
        {
            NextNodeIndex = nextNodeIndex;
        }

        #region Abstract and Virtual Functions

        public virtual BaseNode GetNextNode(Intent intent, Session session, ILambdaContext lambdaContext)
        {
            return NextNodeIndex < Story.Nodes.Count ? Story.Nodes[NextNodeIndex] : null;
        }

        public virtual void ModifySessionAttributes(Dictionary<string, object> attributes, Intent intent, Session session, ILambdaContext lambdaContext)
        {
        }

        #endregion
    }
}