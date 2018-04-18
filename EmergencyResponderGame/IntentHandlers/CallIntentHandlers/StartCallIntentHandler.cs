using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using EmergencyResponderGame.StorySystem;
using EmergencyResponderGame.StorySystem.Nodes;
using Alexa.NET;

namespace EmergencyResponderGame.IntentHandlers
{
    public class StartCallIntentHandler : CheckIntentHandler
    {
        #region Properties and Fields

        /// <summary>
        /// The name of the intent on AWS that we are processing using this handler.
        /// </summary>
        public const string ConstIntentName = "StartCallIntent";

        /// <summary>
        /// The name of the intent this handler will process.
        /// </summary>
        public override string IntentName { get { return ConstIntentName; } }

        #endregion
    }
}