using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Alexa.NET.Response.Ssml;
using Amazon.Lambda.Core;
using EmergencyResponderGame.IntentHandlers;
using EmergencyResponderGame.Resources;
using EmergencyResponderGame.SpeechUtilities;
using EmergencyResponderGame.StorySystem.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmergencyResponderGame.StorySystem
{
    public static class Story
    {
        #region Properties and Fields

        /// <summary>
        /// The key for the session attribute we use to store the current node index.
        /// </summary>
        public const string CurrentNodeIndexKey = "CurrentNodeIndex";

        public static List<BaseNode> Nodes { get; set; } = new List<BaseNode>()
        {
            // 0 - Introduce C1
            new SpeechNode(1, new SpeechBuilder().
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/Introduction/Introduction.mp3"))),
            // 1 - Check if player ready
            ConditionNode.CreateYesNoChoiceNode(2, 3),
            // 2 - C1 Demo, Yes
            new SpeechNode(4, new SpeechBuilder().
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Demo/C1_Demo_Handler_Ready_Yes.mp3")).
                Add(new Break() { Time = "1s" }).
                Add(new Audio(AudioResources.Phone_Ringing)).
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Demo/C1_Demo_Call.mp3")).
                Add(new Break() { Time = "1s" }).
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Demo/C1_Demo_Handler_EndDemo.mp3"))),
            // 3 - C1 Demo, No
            new SpeechNode(4, new SpeechBuilder().
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Demo/C1_Demo_Handler_Ready_No.mp3")).
                Add(new Break() { Time = "1s" }).
                Add(new Audio(AudioResources.Phone_Ringing)).
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Demo/C1_Demo_Call.mp3")).
                Add(new Break() { Time = "1s" }).
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Demo/C1_Demo_Handler_EndDemo.mp3"))),
            // 4 - Tutorial, Start Call
            new CheckNextIntentNode(5, Intents.StartCallIntentName, 6, "C1_Tutorial_StartCall_Correct"),
            // 5 - Tutorial, Start Call Correct
            new SpeechNode(7, new SpeechBuilder().
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Tutorial/C1_Tutorial_Handler_WhatHappened_Correct.mp3"))),
            // 6 - Tutorial, Start Call Incorrect
            new SpeechNode(4, new SpeechBuilder().
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Tutorial/C1_Tutorial_Handler_WhatHappened_Incorrect.mp3"))),
            // 7 - Tutorial, Breathing Question
            new CheckNextIntentNode(8, Intents.IsBreathingQuestionIntentName, 9, "C1_Tutorial_IsBreathing_Correct"),
            // 8 - Tutorial, Breathing Question Correct
            new SpeechNode(10, new SpeechBuilder().
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Tutorial/C1_Tutorial_Handler_Breathing_Correct.mp3"))),
            // 9 - Tutorial, Breathing Question Incorrect
            new SpeechNode(7, new SpeechBuilder().
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Tutorial/C1_Tutorial_Handler_Breathing_Incorrect.mp3"))),
            // 10 - Tutorial, Conscious Question
            new CheckNextIntentNode(11, Intents.IsConsciousQuestionIntentName, 12, "C1_Tutorial_IsConscious_Correct"),
            // 11 - Tutorial, Conscious Question Correct
            new SpeechNode(13, new SpeechBuilder().
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Tutorial/C1_Tutorial_Handler_Conscious_Correct.mp3"))),
            // 12 - Tutorial, Conscious Question Incorrect
            new SpeechNode(10, new SpeechBuilder().
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Tutorial/C1_Tutorial_Handler_Conscious_Incorrect.mp3"))),
            // 13 - Tutorial, End Call
            new CheckNextIntentNode(14, Intents.EndCallIntentName, 15, "C1_Tutorial_EndCall_Correct"),
            // 14 - Tutorial, End Call Correct
            new SpeechNode(16, new SpeechBuilder().
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Tutorial/C1_Tutorial_Handler_EndCall_Correct.mp3"))),
            // 15 - Tutorial, End Call Incorrect
            new SpeechNode(13, new SpeechBuilder().
                Add(new Audio("https://s3-eu-west-1.amazonaws.com/nine-nine-nine/C1_Tutorial/C1_Tutorial_Handler_EndCall_Incorrect.mp3"))),
            // 16 - C1 Call 1, Start Call
            new CheckNextIntentNode(17, Intents.StartCallIntentName, 18, "C1_Call1_StartCall_Correct"),
            // 17 - C1 Call 1, Start Call Correct
            new SpeechNode(19, new SpeechBuilder().
                Add(new Sentence("Yeah, I think this guys having a drug overdose."))),
            // 18 - C1 Call 1, Start Call Incorrect
            new SpeechNode(19, new SpeechBuilder().
                Add(new Sentence("I need an ambulance, can you help me please!"))),
            // 19 - C1 Call 1, Breathing Question
            new CheckNextIntentNode(20, Intents.IsBreathingQuestionIntentName, 21, "C1_Call1_Breathing_Correct"),
            // 20 - C1 Call 1, Breathing Question Correct
            new SpeechNode(22, new SpeechBuilder().
                Add(new Sentence("Not really.  It's very erratic."))),
            // 21 - C1 Call 1, Breathing Question Incorrect
            new SpeechNode(22, new SpeechBuilder().
                Add(new Sentence("He's in a really bad place.  I don't know what to do here."))),
            // 22 - C1 Call 1, Conscious Question
            new CheckNextIntentNode(23, Intents.IsConsciousQuestionIntentName, 24, "C1_Call1_Conscious_Correct"),
            // 23 - C1 Call 1, Conscious Question Correct
            new SpeechNode(25, new SpeechBuilder().
                Add(new Sentence("Yeah, but he's thrashing about."))),
            // 24 - C1 Call 1, Conscious Question Incorrect
            new SpeechNode(25, new SpeechBuilder().
                Add(new Sentence("He's thrashing about a lot, but I don't know whether I should try and stop him."))),
            // 25 - C1 Call 1, End Call
            new CheckNextIntentNode(26, Intents.EndCallIntentName, 24, "C1_Call1_EndCall_Correct"),
            // 26 - Introduce C2
            new SpeechNode(27, new SpeechBuilder().
                Add(new Sentence("I'm now going to teach you the next call category, C2.")).
                Add(new Sentence("This is for emergencies like strokes, serious burns and epileptic fits.")).
                Add(new Sentence("Since these can vary quite a bit, we have to ask a few more questions to guage how serious an emergency it is.")).
                Add(new Sentence("We'll do the same format as before; listen to me answer a call and then I'll hand over to you."))),
            // 27 - Check if player ready
            ConditionNode.CreateYesNoChoiceNode(28, 29),
            // 28 - C2 Demo Call, Yes
            new SpeechNode(30, new SpeechBuilder().
                Add(new Sentence("I'm impressed")).
                Add(new Break() { Time="1s" }).
                Add(new Audio(AudioResources.Phone_Ringing)).
                Add(new Sentence("Hello, emergency ambulance, tell me exactly what's happened.")).
                Add(new Sentence("Hello, I think my mum's had a bit of a stroke because the left side of her body has gone limp.")).
                Add(new Sentence("Is she breathing?")).
                Add(new Sentence("Yes, she's breathing.")).
                Add(new Sentence("And is she conscious?")).
                Add(new Sentence("Yes, she's conscious, but not responding much.")).
                Add(new Sentence("Is she able to smile?")).
                Add(new Sentence("Not really, no.")).
                Add(new Sentence("And could you get her to raise her arms above her head for me.")).
                Add(new Sentence("She tried, but she can't move her left arm at all.")).
                Add(new Sentence("Is she able to say 'The early bird catches the worm'?")).
                Add(new Sentence("Her speech is really slurred at the moment, I couldn't make out much.")).
                Add(new Sentence("OK, I'm organising help for you now.")).
                Add(new Break() { Time = "1s" }).
                Add(new Sentence("As you can see, C2s can be a lot more complex.")).
                Add(new Sentence("It's up to you to ask the right questions to properly assess the seriousness of the emergency.")).
                Add(new Sentence("I want you to see if you can remember everything by doing a call with me.")).
                Add(new Sentence("In a second, the phone will ring.")).
                Add(new Sentence("Answer it when you're ready"))),
            // 29 - C2 Demo Call, No
            new SpeechNode(30, new SpeechBuilder().
                Add(new Sentence("OK, let's recap quickly.")).
                Add(new Sentence("C2 calls are for serious emergencies, but nothing life threatening.")).
                Add(new Sentence("These include strokes and severe burns.")).
                Add(new Sentence("You're about to hear me deal with a C2 call and then you will try one with me.")).
                Add(new Break() { Time = "1s" }).
                Add(new Audio(AudioResources.Phone_Ringing)).
                Add(new Sentence("Hello, emergency ambulance, tell me exactly what's happened.")).
                Add(new Sentence("Hello, I think my mum's had a bit of a stroke because the left side of her body has gone limp.")).
                Add(new Sentence("Is she breathing?")).
                Add(new Sentence("Yes, she's breathing.")).
                Add(new Sentence("And is she conscious?")).
                Add(new Sentence("Yes, she's conscious, but not responding much.")).
                Add(new Sentence("Is she able to smile?")).
                Add(new Sentence("Not really, no.")).
                Add(new Sentence("And could you get her to raise her arms above her head for me.")).
                Add(new Sentence("She tried, but she can't move her left arm at all.")).
                Add(new Sentence("Is she able to say 'The early bird catches the worm'?")).
                Add(new Sentence("Her speech is really slurred at the moment, I couldn't make out much.")).
                Add(new Sentence("OK, I'm organising help for you now.")).
                Add(new Break() { Time = "1s" }).
                Add(new Sentence("As you can see, C2s can be a lot more complex.")).
                Add(new Sentence("It's up to you to ask the right questions to properly assess the seriousness of the emergency.")).
                Add(new Sentence("I want you to see if you can remember everything by doing a call with me.")).
                Add(new Sentence("In a second, the phone will ring.")).
                Add(new Sentence("Answer it when you're ready"))),
            // 30 - C2 Tutorial, Start Call
            new CheckNextIntentNode(31, Intents.StartCallIntentName, 32, "C2_Tutorial_StartCall_Correct"),
            // 31 - C2 Tutorial, Start Call Correct
            new SpeechNode(33, new SpeechBuilder().
                Add(new Sentence("My girlfriend is having some kind of fit.  She's on the floor and can't seem to control her movements."))),
            // 32 - C2 Tutorial, Start Call Incorrect
            new SpeechNode(30, new SpeechBuilder().
                Add(new Sentence("Don't forget, always begin a call by asking the caller to tell you exactly what's happened."))),
            // 33 - C2 Tutorial, Breathing
            new CheckNextIntentNode(34, Intents.IsBreathingQuestionIntentName, 35, "C2_Tutorial_Breathing_Correct"),
            // 34 - C2 Tutorial, Breathing Correct
            new SpeechNode(36, new SpeechBuilder().
                Add(new Sentence("She doesn't seem to have any problems breathing, but she is breathing quite quickly."))),
            // 35 - C2 Tutorial, Breathing Incorrect
            new SpeechNode(33, new SpeechBuilder().
                Add(new Sentence("Always assess whether they are breathing first.  This is the same as C1 and is absolutely vital in determining the type of call category you have.  Let's get this step correct."))),
            // 36 - C2 Tutorial, Conscious
            new CheckNextIntentNode(37, Intents.IsConsciousQuestionIntentName, 38, "C2_Tutorial_Conscious_Correct"),
            // 37 - C2 Tutorial, Conscious Correct
            new SpeechNode(39, new SpeechBuilder().
                Add(new Sentence("She's thrashing around a lot and isn't responding to anything I'm saying."))),
            // 38 - C2 Tutorial, Conscious Incorrect
            new SpeechNode(39, new SpeechBuilder().
                Add(new Sentence("Checking a patient's consciousness is always the next step after their breathing.  Try again."))),
            // 39 - C2 Tutorial, Smile
            new CheckNextIntentNode(40, Intents.CanSmileQuestionIntentName, 41, "C2_Tutorial_Smile_Correct"),
            // 40 - C2 Tutorial, Smile Correct
            new SpeechNode(42, new SpeechBuilder().
                Add(new Sentence("I don't think she can understand or hear anything I'm saying."))),
            // 41 - C2 Tutorial, Smile Incorrect
            new SpeechNode(42, new SpeechBuilder().
                Add(new Sentence("In this case, it doesn't seem like a stroke, but checking facial movement is a really essential test for C2 calls.  Ask me whether the patient can smile."))),
            // 42 - C2 Tutorial, Smile

            // Now we need to start to be able to handle multiple intents
            new CheckNextIntentNode(40, Intents.CanSmileQuestionIntentName, 41, "C2_Tutorial_Smile_Correct"),

            // Have a bit in the tutorial where the handler starts pointing out some of your questions are a bit unnecessary
            // I.e. if the person follows the example, at a certain point the handler will say 'Yep, but you don't really need to ask this in this case'
            // 'Sometimes you have to realise when you've got enough information to fully assess a situation'
            // 'And sometimes you have to think on your feet to ask relevant questions'
            // At a certain point you need to tell them you're organising help and the mentor will recognise you've done that
            // At subsequent points he'll say - yep, you've got all the information you need, but you probably could have made the decision sooner

            // Before/After the player starts a proper C2 call, just point out to them that they will need to start to realise what is a C1 and what is a C2
            // and organise help at the appropriate point in the call - i.e. don't ask unnecessary questions
            // Also need to check for organising help too early

            // Need to handle valid questions but in the wrong order - people should still answer correctly?
            // If we keep the responses to incorrect questions general then maybe it will still make sense?
        };

        #endregion

        #region Runtime Functions

        public static BaseNode GetCurrentNode(Session session)
        {
            int currentNodeIndex = session.GetCurrentNodeIndex();
            return Nodes[currentNodeIndex];
        }
        
        public static SkillResponse CreateResponse(Intent intent, Session session, ILambdaContext lambdaContext)
        {
            BaseNode node = GetCurrentNode(session);
            BaseNode nextNode = node.GetNextNode(intent, session ,lambdaContext);

            return CreateResponseForNode(nextNode, intent, session, lambdaContext);
        }

        public static SkillResponse CreateResponseForNode(long nodeIndex, Intent intent, Session session, ILambdaContext lambdaContext)
        {
            return CreateResponseForNode(Nodes[(int)nodeIndex], intent, session, lambdaContext);
        }

        public static SkillResponse CreateResponseForNode(BaseNode node, Intent intent, Session session, ILambdaContext lambdaContext)
        {
            Dictionary<string, object> responseSessionAttributes = session.Attributes ?? new Dictionary<string, object>();

            while (node != null && !(node is SpeechNode))
            {
                node.ModifySessionAttributes(session.Attributes, intent, session, lambdaContext);
                node = node.GetNextNode(intent, session, lambdaContext);
            }

            SpeechNode speechNode = node as SpeechNode;
            lambdaContext.Logger.LogLine("Speech Node element count " + speechNode.Speech.Elements.Count);

            SkillResponse response = speechNode != null ? ResponseBuilder.Tell(speechNode.Speech) : ResponseBuilder.Empty();
            response.Response.ShouldEndSession = speechNode == null;

            // Update the record of the current node we are on
            response.SessionAttributes = responseSessionAttributes;

            long nextIndex = speechNode != null ? speechNode.NextNodeIndex : -1;
            lambdaContext.Logger.LogLine("Next Index: " + nextIndex);

            if (!responseSessionAttributes.ContainsKey(CurrentNodeIndexKey))
            {
                response.SessionAttributes.Add(CurrentNodeIndexKey, nextIndex);
            }
            else
            {
                response.SessionAttributes[CurrentNodeIndexKey] = nextIndex;
            }

            return response;
        }

        #endregion
    }
}