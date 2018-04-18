using System;
using System.Collections.Generic;
using System.Text;

namespace EmergencyResponderGame.IntentHandlers
{
    public class IsConsciousQuestionIntentHandler : CheckIntentHandler
    {
        #region Properties and Fields

        /// <summary>
        /// The AWS intent name that this handler will process.
        /// </summary>
        public const string ConstIntentName = "IsConsciousQuestionIntent";

        /// <summary>
        /// The AWS intent name that this handler will process.
        /// </summary>
        public override string IntentName { get { return ConstIntentName; } }

        #endregion
    }
}
