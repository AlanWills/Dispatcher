using Alexa.NET.Request;
using EmergencyResponderGame.IntentHandlers;
using EmergencyResponderGame.StorySystem.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmergencyResponderGame.StorySystem.Conditions
{
    public abstract class Condition
    {
        #region Properties and Fields

        /// <summary>
        /// The index of the node we should progress to if this condition is fulfilled.
        /// </summary>
        public int NextNodeIndex { get; }

        #endregion

        public Condition(int nextNodeIndex)
        {
            NextNodeIndex = nextNodeIndex;
        }

        #region Abstract Functions

        public abstract bool IsMatchingCondition(Intent intent);

        #endregion
    }
}
