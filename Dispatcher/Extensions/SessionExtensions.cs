using Alexa.NET.Request;
using Dispatcher.StorySystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dispatcher
{
    public static class SessionExtensions
    {
        /// <summary>
        /// Utility function for obtaining templated information from the session attributes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="attributeKey"></param>
        /// <returns></returns>
        public static T GetSessionAttribute<T>(this Session session, string attributeKey)
        {
            if (!session.Attributes.ContainsKey(attributeKey))
            {
                return default(T);
            }

            return (T)session.Attributes[attributeKey];
        }

        /// <summary>
        /// Obtain the current node index from the session.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="attributeKey"></param>
        /// <returns></returns>
        public static int GetCurrentNodeIndex(this Session session)
        {
            // AWS converts 32 bit ints to 64 bit ints automatically, so we have to convert back here
            long currentNodeIndex = session.GetSessionAttribute<long>(Story.CurrentNodeIndexKey);
            return (int)currentNodeIndex;
        }
    }
}
