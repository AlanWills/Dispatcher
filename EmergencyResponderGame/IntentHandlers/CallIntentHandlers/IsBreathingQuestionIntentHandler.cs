using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;

namespace EmergencyResponderGame.IntentHandlers
{
    public class IsBreathingQuestionIntentHandler : CheckIntentHandler
    {
        #region Properties and Fields

        /// <summary>
        /// The AWS intent name that this handler will process.
        /// </summary>
        public const string ConstIntentName = "IsBreathingQuestionIntent";

        /// <summary>
        /// The AWS intent name that this handler will process.
        /// </summary>
        public override string IntentName { get { return ConstIntentName; } }

        #endregion
    }
}
