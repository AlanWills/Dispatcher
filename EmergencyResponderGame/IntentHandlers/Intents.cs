using System;
using System.Collections.Generic;
using System.Text;

namespace EmergencyResponderGame
{
    public static class Intents
    {
        #region Utility Intent Names

        /// <summary>
        /// The name of the Yes intent on AWS lambda.
        /// </summary>
        public const string YesIntentName = "AMAZON.YesIntent";

        /// <summary>
        /// The name of the No intent on AWS lambda.
        /// </summary>
        public const string NoIntentName = "AMAZON.NoIntent";

        #endregion

        #region Story Start Intent Names

        /// <summary>
        /// The name of the PlayGame intent on AWS lambda.
        /// </summary>
        public const string PlayGameIntentName = "PlayGameIntent";

        /// <summary>
        /// The name of the StartFromNode intent on AWS lambda.
        /// </summary>
        public const string StartFromNodeIntentName = "StartFromNodeIntent";

        #endregion

        #region Story Progression Intent Names

        /// <summary>
        /// The name of the StartCall intent on AWS lambda.
        /// </summary>
        public const string StartCallIntentName = "StartCallIntent";

        /// <summary>
        /// The name of the IsBreathingQuestion intent on AWS lambda.
        /// </summary>
        public const string IsBreathingQuestionIntentName = "IsBreathingQuestionIntent";

        /// <summary>
        /// The name of the IsConsciousQuestion intent on AWS lambda.
        /// </summary>
        public const string IsConsciousQuestionIntentName = "IsConsciousQuestionIntent";

        /// <summary>
        /// The name of the CanSmileQuestion intent on AWS lambda.
        /// </summary>
        public const string CanSmileQuestionIntentName = "CanSmileQuestionIntent";

        /// <summary>
        /// The name of the RaiseArmsQuestion intent on AWS lambda.
        /// </summary>
        public const string RaiseArmsQuestionIntentName = "RaiseArmsQuestionIntent";

        /// <summary>
        /// The name of the SpeakPhraseQuestion intent on AWS lambda.
        /// </summary>
        public const string SpeakPhraseQuestionIntentName = "SpeakPhraseQuestionIntent";

        /// <summary>
        /// The name of the EndCall intent on AWS lambda.
        /// </summary>
        public const string EndCallIntentName = "EndCallIntent";

        #endregion
    }
}
