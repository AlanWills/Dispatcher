using Alexa.NET.Response.Ssml;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dispatcher.SpeechUtilities
{
    public class SpeechBuilder
    {
        #region Properties and Fields

        /// <summary>
        /// The speech which we are building up.
        /// </summary>
        private Speech Speech { get; } = new Speech();

        #endregion

        public SpeechBuilder Add(ISsml speechElement)
        {
            Speech.Elements.Add(speechElement);
            return this;
        }

        public Speech Build()
        {
            return Speech;
        }
    }
}
