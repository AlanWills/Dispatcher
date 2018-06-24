using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.Request;
using Amazon.Lambda.Core;

namespace Dispatcher.StorySystem.Nodes
{
    public class FinishWithCardNode : BaseNode
    {
        public FinishWithCardNode() : 
            base(-1)
        {
        }

        #region Virtual Overrides

        public override BaseNode GetNextNode(Intent intent, Session session, ILambdaContext lambdaContext)
        {
            // This should automatically finish the story
            return null;
        }

        #endregion
    }
}
