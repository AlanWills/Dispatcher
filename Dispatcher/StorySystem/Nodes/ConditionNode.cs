using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.Response.Ssml;
using Dispatcher.IntentHandlers;
using Dispatcher.StorySystem.Conditions;
using Alexa.NET.Request;
using Amazon.Lambda.Core;

namespace Dispatcher.StorySystem.Nodes
{
    public class ConditionNode : BaseNode
    {
        #region Properties and Fields
        
        private List<Condition> Conditions { get; } = new List<Condition>();

        #endregion

        public ConditionNode(int defaultChoiceIndex, params Condition[] choices) :
            base(defaultChoiceIndex)
        {
            if (choices != null)
            {
                for (int i = 0; i < choices.Length; ++i)
                {
                    Conditions.Add(choices[i]);
                }
            }
        }

        public override BaseNode GetNextNode(Intent intent, Session session, ILambdaContext lambdaContext)
        {
            Condition choice = Conditions.Find(x => x.IsMatchingCondition(intent));
            int index = choice != null ? choice.NextNodeIndex : NextNodeIndex;
            
            return Story.Nodes[index];
        }
        
        #region Factory Functions

        public static ConditionNode CreateYesNoChoiceNode(int yesIndex, int noIndex)
        {
            return new ConditionNode(noIndex,
                new IntentHandlerCondition(yesIndex, Intents.YesIntentName),
                new IntentHandlerCondition(noIndex, Intents.NoIntentName));
        }

        #endregion
    }
}
