using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Alexa.NET.Response.Ssml;
using Amazon.Lambda.Core;
using Dispatcher.IntentHandlers;
using Dispatcher.SpeechUtilities;
using Dispatcher.StorySystem.Nodes;
using System;
using System.Collections.Generic;
using System.Text;
using static BirthCallAudioConsts;
using static Dispatcher.Intents;

namespace Dispatcher.StorySystem
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
            // 0 - Dispatcher Ready Question
            new SpeechNode(1, new SpeechBuilder().
                Add(new Audio(Dispatcher_Ready_Question))),
            // 1 - Check if player ready
            ConditionNode.CreateYesNoChoiceNode(2, 3),
            // 2 - Player Ready Yes
            new SpeechNode(4, new SpeechBuilder().
                Add(new Audio(Player_Ready_Yes))),
            // 3 - Player Ready No
            new SpeechNode(4, new SpeechBuilder().
                Add(new Audio(Player_Ready_No))),
            // 4 - AA Emergency, tell me exactly what's happened
            new CheckNextIntentNode(5, TellMeWhatsHappenedIntent, 6, "TellMeWhatsHappened_Correct"),
            // 5 - Player Tell Me Whats Happened Correct
            new SpeechNode(9, new SpeechBuilder().
                Add(new Audio(Player_Tell_Me_Whats_Happened_Correct)).
                Add(new Audio(Dispatcher_Interrupts_For_Information))),
            // 6 - Player Tell Me Whats Happened Incorrect
            new SpeechNode(7, new SpeechBuilder().
                Add(new Audio(Player_Tell_Me_Whats_Happened_Incorrect))),
            // 7 - AA Emergency, tell me exactly what's happened
            new CheckNextIntentNode(5, TellMeWhatsHappenedIntent, 8, "TellMeWhatsHappened_Correct"),
            // 8 - Dispatcher Corridor Tell Me Whats Happened
            new SpeechNode(9, new SpeechBuilder().
                Add(new Audio(Dispatcher_Corridor_Tell_Me_Whats_Happened)).
                Add(new Audio(Player_Tell_Me_Whats_Happened_Correct)).
                Add(new Audio(Dispatcher_Interrupts_For_Information))),
            // 9 - Are you with her now?
            new CheckNextIntentNode(10, WithThemNowIntent, 11, "WithThemNowIntent_Correct"),
            // 10 - Player Caller With Partner Correct
            new SpeechNode(14, new SpeechBuilder().
                Add(new Audio(Player_Caller_With_Partner_Correct)).
                Add(new Audio(Dispatcher_Interrupts_Caller_Again))),
            // 11 - Player Caller With Partner Incorrect
            new SpeechNode(12, new SpeechBuilder().
                Add(new Audio(Player_Caller_With_Partner_Incorrect))),
            // 12 - Are you with her now?
            new CheckNextIntentNode(10, WithThemNowIntent, 13, "WithThemNowIntent_Correct"),
            // 13 - Dispatcher Corridor Caller With Partner
            new SpeechNode(14, new SpeechBuilder().
                Add(new Audio(Dispatcher_Corridor_Caller_With_Partner)).
                Add(new Audio(Player_Tell_Me_Whats_Happened_Correct)).
                Add(new Audio(Dispatcher_Interrupts_For_Information))),
            // 14 - How many weeks pregnant/How old is the mother
            new CheckNextIntentNode(new Dictionary<string, int>()
            {
                { HowManyWeeksPregnantIntent, 15 },
                { HowOldIsMotherIntent, 21 }
            }, 27, "WeeksPregnantOrAge_Correct"),
            // 15 - Player Asks How Many Weeks Pregnant (Weeks Pregnant Branch)
            new SpeechNode(16, new SpeechBuilder().
                Add(new Audio(Player_Asks_How_Many_Weeks_Pregnant_Weeks_Pregnant_Branch))),
            // 16 - How old is mum?
            new CheckNextIntentNode(17, HowOldIsMotherIntent, 18, "HowOldIsMother_Correct"),
            // 17 - Player Asks Mothers Age (Weeks Pregnant Branch)
            new SpeechNode(30, new SpeechBuilder().
                Add(new Audio(Player_Asks_How_Many_Weeks_Pregnant_Weeks_Pregnant_Branch)).
                Add(new Audio(Dispatcher_Where_Are_They_Now_Question))),
            // 18 - Dispatcher Prompts To Ask Mothers Age (Weeks Pregnant Branch)
            new SpeechNode(19, new SpeechBuilder().
                Add(new Audio(Dispatcher_Prompts_To_Ask_Mothers_Age_Weeks_Pregnant_Branch))),
            // 19 - How old is mum?
            new CheckNextIntentNode(17, HowOldIsMotherIntent, 20, "HowOldIsMother_Correct"),
            // 20 - Dispatcher Corridor Mothers Age (Weeks Pregnant Branch)
            new SpeechNode(30, new SpeechBuilder().
                Add(new Audio(Dispatcher_Corridor_Mothers_Age_Weeks_Pregnant_Branch)).
                Add(new Audio(Player_Asks_How_Many_Weeks_Pregnant_Weeks_Pregnant_Branch)).
                Add(new Audio(Dispatcher_Where_Are_They_Now_Question))),
            // 21 - Player Asks Mothers Age (Age Branch)
            new SpeechNode(22, new SpeechBuilder().
                Add(new Audio(Player_Asks_Mothers_age_Age_Branch))),
            // 22 - How many weeks pregnant?
            new CheckNextIntentNode(23, HowManyWeeksPregnantIntent, 24, "HowManyWeeksPregnant_Correct"),
            // 23 - Player Asks Mothers Age (Age Branch)
            new SpeechNode(30, new SpeechBuilder().
                Add(new Audio(Player_Asks_How_Many_Weeks_Pregnant_Age_Branch)).
                Add(new Audio(Dispatcher_Where_Are_They_Now_Question))),
            // 24 - Dispatcher Prompts To Ask How Many Weeks Pregnant (Age Branch)
            new SpeechNode(25, new SpeechBuilder().
                Add(new Audio(Dispatcher_Prompts_To_Ask_How_Many_Weeks_Pregnant_Age_Branch))),
            // 25 - How many weeks pregnant?
            new CheckNextIntentNode(23, HowManyWeeksPregnantIntent, 26, "HowManyWeeksPregnant_Correct"),
            // 26 - Dispatcher Corridor Ask How Many Weeks Pregnant (Age Branch)
            new SpeechNode(30, new SpeechBuilder().
                Add(new Audio(Dispatcher_Corridor_Ask_How_Many_Weeks_Pregant_Age_Branch)).
                Add(new Audio(Player_Asks_How_Many_Weeks_Pregnant_Age_Branch)).
                Add(new Audio(Dispatcher_Where_Are_They_Now_Question))),
            // 27 - Dispatcher Prompts To Ask Age
            new SpeechNode(28, new SpeechBuilder().
                Add(new Audio(Dispatcher_Prompts_To_Ask_Age))),
            // 28 - How old is mum?
            new CheckNextIntentNode(23, HowOldIsMotherIntent, 29, "HowOldIsMother_Correct"),
            // 29 - Dispatcher Corridor Asks Mothers Age
            new SpeechNode(23, new SpeechBuilder().
                Add(new Audio(Dispatcher_Corridor_Asks_Mothers_Age)).
                Add(new Audio(Player_Asks_Mothers_age_Age_Branch))),
            // 30 - Where are they now?
            new CheckNextIntentNode(31, WhereAreYouIntent, 32, "WhereAreYou_Correct"),
            // 31 - Player Where Are They Now Correct
            new SpeechNode(33, new SpeechBuilder().
                Add(new Audio(Player_Where_Are_They_Now_Correct)).
                Add(new Audio(Dispatcher_Can_We_See_Baby_Question))),
            // 32 - Player Where Are They Now Incorrect
            new SpeechNode(33, new SpeechBuilder().
                Add(new Audio(Player_Where_Are_They_Now_Correct)).
                Add(new Audio(Dispatcher_Can_We_See_Baby_Question))),
            // 33 - Is any of the baby visible?
            new CheckNextIntentNode(34, IsBabyVisibleIntent, 35, "IsBabyVisible_Correct"),
            // 34 - Player Where Are They Now Correct
            new SpeechNode(38, new SpeechBuilder().
                Add(new Audio(Player_Can_We_See_Baby_Correct)).
                Add(new Audio(Dispatcher_Send_Ambulance_Question))),
            // 35 - Player Where Are They Now Incorrect
            new SpeechNode(36, new SpeechBuilder().
                Add(new Audio(Player_Can_We_See_Baby_Incorrect))),
            // 36 - Is any of the baby visible? (again)
            new CheckNextIntentNode(34, IsBabyVisibleIntent, 37, "IsBabyVisibleAgain_Correct"),
            // 37 - Dispatcher Corridors Can We See Baby
            new SpeechNode(38, new SpeechBuilder().
                Add(new Audio(Dispatcher_Corridors_Can_We_See_Baby)).
                Add(new Audio(Player_Can_We_See_Baby_Correct)).
                Add(new Audio(Dispatcher_Send_Ambulance_Question))),
            // 38 - Yes/No (send ambulance)
            ConditionNode.CreateYesNoChoiceNode(39, 40),
            // 39 - Player Send Ambulance Correct
            new SpeechNode(41, new SpeechBuilder().
                Add(new Audio(Player_Send_Ambulance_Correct))),
            // 40 - Player Send Ambulance Incorrect
            new SpeechNode(41, new SpeechBuilder().
                Add(new Audio(Player_Send_Ambulance_Incorrect))),
            // 41 - Stay on the line, we'll tell you what to do
            new CheckNextIntentNode(42, StayOnTheLineIntent, 43, "StayOnTheLine_Correct"),
            // 42 - Player Stay On The Line Correct
            new SpeechNode(44, new SpeechBuilder().
                Add(new Audio(Player_Stay_On_The_Line_Correct)).
                Add(new Audio(Dispatcher_Baby_Slippery_Instruction))),
            // 43 - Player Stay On The Line Incorrect
            new SpeechNode(44, new SpeechBuilder().
                Add(new Audio(Player_Stay_On_The_Line_Incorrect)).
                Add(new Audio(Dispatcher_Baby_Slippery_Instruction))),
            // 44 - Baby will be slippery
            new CheckNextIntentNode(45, BabyWillBeSlipperyIntent, 46, "BabyWillBeSlippery_Correct"),
            // 45 - Player Baby Slippery Instruction Correct
            new SpeechNode(49, new SpeechBuilder().
                Add(new Audio(Player_Baby_Slippery_Instruction_Correct)).
                Add(new Audio(Dispatcher_Where_To_Hold_Baby_Instruction))),
            // 46 - Player Baby Slippery Instruction Incorrect
            new SpeechNode(47, new SpeechBuilder().
                Add(new Audio(Player_Baby_Slippery_Instruction_Incorrect))),
            // 47 - Baby will be slippery (again)
            new CheckNextIntentNode(45, BabyWillBeSlipperyIntent, 48, "BabyWillBeSlipperyAgain_Correct"),
            // 48 - Dispatcher Corridors Baby Slippery Instruction Correct
            new SpeechNode(49, new SpeechBuilder().
                Add(new Audio(Dispatcher_Corridors_Baby_Slippery_Instruction)).
                Add(new Audio(Player_Baby_Slippery_Instruction_Correct)).
                Add(new Audio(Dispatcher_Where_To_Hold_Baby_Instruction))),
            // 49 - Support head, hold hips and legs tight
            new CheckNextIntentNode(50, SupportBabyIntent, 51, "SupportBaby_Correct"),
            // 50 - Player Baby Slippery Instruction Correct
            new SpeechNode(52, new SpeechBuilder().
                Add(new Audio(Player_Where_To_Hold_Baby_Instruction_Correct)).
                Add(new Audio(Dispatcher_Check_On_Caller_Instruction))),
            // 51 - Player Baby Slippery Instruction Incorrect
            new SpeechNode(52, new SpeechBuilder().
                Add(new Audio(Player_Where_To_Hold_Baby_Instruction_Incorrect)).
                Add(new Audio(Player_Where_To_Hold_Baby_Instruction_Correct)).
                Add(new Audio(Dispatcher_Check_On_Caller_Instruction))),
            // 52 - Are you ok?
            new CheckNextIntentNode(53, CheckOkIntent, 53, "CheckOk_Correct"),
            // 53 - Player Check On Caller Shout
            new SpeechNode(54, new SpeechBuilder().
                Add(new Audio(Player_Check_On_Caller_Shout))),
            // 54 - Are you ok? (shout)
            new CheckNextIntentNode(55, CheckOkIntent, 55, "CheckOkShout_Correct"),
            // 55 - Caller Birth
            new SpeechNode(56, new SpeechBuilder().
                Add(new Audio(Caller_Birth)).
                Add(new Audio(Dispatcher_Baby_Crying_Breathing_Question))),
            // 56 - Is the baby crying or breathing?
            new CheckNextIntentNode(57, IsBabyCryingOrBreathingIntent, 64, "IsBabyCryingOrBreathing_Correct"),
            // 57 - Player Baby Crying Breathing Question Correct
            new SpeechNode(58, new SpeechBuilder().
                Add(new Audio(Player_Baby_Crying_Breathing_Question_Correct))),
            // 58 - Is anything obviously wrong?
            new CheckNextIntentNode(59, IsAnythingObviouslyWrongIntent, 60, "IsAnythingObviouslyWrong_Correct"),
            // 59 - Player Is Anything Wrong Correct
            new SpeechNode(61, new SpeechBuilder().
                Add(new Audio(Player_Is_Anything_Wrong_Correct)).
                Add(new Audio(Dispatcher_Rub_Babys_Back_Instruction))),
            // 60 - Callers Panic What Do We Do
            new SpeechNode(61, new SpeechBuilder().
                Add(new Audio(Callers_Panic_What_Do_We_Do)).
                Add(new Audio(Dispatcher_Rub_Babys_Back_Instruction))),
            // 61 - Rub baby's back with a towel for 30 seconds?
            new CheckNextIntentNode(62, RubBabysBackInstructionIntent, 63, "RubBabysBackInstruction_Correct"),
            // 62 - Player Rub Babys Back Instruction Correct
            new SpeechNode(65, new SpeechBuilder().
                Add(new Audio(Player_Rub_Babys_Back_Instruction_Correct)).
                Add(new Audio(Baby_Cries)).
                Add(new Audio(Dispatcher_Final_Instructions_Instruction))),
            // 63 - Player Rub Babys Back Instruction Incorrect
            new SpeechNode(65, new SpeechBuilder().
                Add(new Audio(Player_Rub_Babys_Back_Instruction_Incorrect)).
                Add(new Audio(Baby_Cries)).
                Add(new Audio(Dispatcher_Final_Instructions_Instruction))),
            // 64 - Baby Cries
            new SpeechNode(65, new SpeechBuilder().
                Add(new Audio(Baby_Cries)).
                Add(new Audio(Dispatcher_Final_Instructions_Instruction))),
            // 65 - Is it a boy or a girl?
            new CheckNextIntentNode(66, IsBoyOrGirlIntent, 66, "IsBoyOrGirl_Correct"),
            // 66 - Player Boy Or Girl Question
            new SpeechNode(67, new SpeechBuilder().
                Add(new Audio(Player_Boy_Or_Girl_Question)).
                Add(new Audio(Dispatcher_Say_Congratulations_Instruction))),
            // 67 - Congratulations!
            new CheckNextIntentNode(68, CongratulationsIntent, 69, "Congratulations_Correct"),
            // 68 - Player Congratulations Correct
            new SpeechNode(70, new SpeechBuilder().
                Add(new Audio(Player_Congratulations_Correct)).
                Add(new Audio(Paramedics_Arrive)).
                Add(new Audio(Dispatcher_Paramedics_Arrived_Question))),
            // 69 - Paramedics Arrive
            new SpeechNode(70, new SpeechBuilder().
                Add(new Audio(Paramedics_Arrive)).
                Add(new Audio(Dispatcher_Paramedics_Arrived_Question))),
            // 70 - Are the paramedics with you?
            new CheckNextIntentNode(71, AreParamedicsWithYouIntent, 72, "AreParamedicsWithYou_Correct"),
            // 71 - Player Paramedics Arrived Question Correct
            new SpeechNode(73, new SpeechBuilder().
                Add(new Audio(Player_Paramedics_Arrived_Question_Correct))),
            // 72 - Player Paramedics Arrived Question Incorrect
            new SpeechNode(/*End*/1, new SpeechBuilder().
                Add(new Audio(Player_Paramedics_Arrived_Question_Incorrect))),
            // 73 - Well done!
            new CheckNextIntentNode(74, WellDoneIntent, 75, "WellDone_Correct"),
            // 74 - Well Done Correct
            new SpeechNode(/*End*/1, new SpeechBuilder().
                Add(new Audio(Well_Done_Correct))),
            // 75 - Well Done Incorrect
            new SpeechNode(/*End*/1, new SpeechBuilder().
                Add(new Audio(Well_Done_Incorrect))),
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