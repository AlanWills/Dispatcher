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
                Add(new Audio(Player_Tell_Me_Whats_Happened_Correct))),
            // 6 - Player Tell Me Whats Happened Incorrect
            new SpeechNode(7, new SpeechBuilder().
                Add(new Audio(Player_Tell_Me_Whats_Happened_Incorrect))),
            // 7 - AA Emergency, tell me exactly what's happened
            new CheckNextIntentNode(5, TellMeWhatsHappenedIntent, 8, "TellMeWhatsHappened_Correct"),
            // 8 - Dispatcher Corridor Tell Me Whats Happened
            new SpeechNode(9, new SpeechBuilder().
                Add(new Audio(Dispatcher_Corridor_Tell_Me_Whats_Happened)).
                Add(new Audio(Player_Tell_Me_Whats_Happened_Correct))),
            // 9 - Are you with her now?
            new CheckNextIntentNode(10, WithThemNowIntent, 11, "WithThemNowIntent_Correct"),
            // 10 - Player Caller With Partner Correct
            new SpeechNode(14, new SpeechBuilder().
                Add(new Audio(Player_Caller_With_Partner_Correct))),
            // 11 - Player Caller With Partner Incorrect
            new SpeechNode(12, new SpeechBuilder().
                Add(new Audio(Player_Caller_With_Partner_Incorrect))),
            // 12 - Are you with her now?
            new CheckNextIntentNode(10, WithThemNowIntent, 13, "WithThemNowIntent_Correct"),
            // 13 - Dispatcher Corridor Caller With Partner
            new SpeechNode(14, new SpeechBuilder().
                Add(new Audio(Dispatcher_Corridor_Caller_With_Partner)).
                Add(new Audio(Player_Caller_With_Partner_Correct))),
            // 14 - How many weeks pregnant/How old is the mother
            new CheckNextIntentNode(new Dictionary<string, int>()
            {
                { HowManyWeeksPregnantIntent, 15 },
                { HowOldIsMotherIntent, 21 }
            }, 25, "WeeksPregnantOrAge_Correct"),
            // 15 - Player Asks How Many Weeks Pregnant (Weeks Pregnant Branch)
            new SpeechNode(16, new SpeechBuilder().
                Add(new Audio(Player_Asks_How_Many_Weeks_Pregnant_Weeks_Pregnant_Branch))),
            // 16 - How old is mum?
            new CheckNextIntentNode(17, HowOldIsMotherIntent, 18, "HowOldIsMother_Correct"),
            // 17 - Player Asks Mothers Age (Weeks Pregnant Branch)
            new SpeechNode(28, new SpeechBuilder().
                Add(new Audio(Player_Asks_Mothers_Age_Weeks_Pregnant_Branch)).
                Add(new Audio(Dispatcher_Where_Are_They_Now_Question))),
            // 18 - Dispatcher Corridor Mothers Age (Weeks Pregnant Branch)
            new SpeechNode(28, new SpeechBuilder().
                Add(new Audio(Dispatcher_Corridor_Mothers_Age_Weeks_Pregnant_Branch)).
                Add(new Audio(Dispatcher_Where_Are_They_Now_Question))),
            // 21 - Player Asks Mothers Age (Age Branch)
            new SpeechNode(22, new SpeechBuilder().
                Add(new Audio(Player_Asks_Mothers_Age_Age_Branch))),
            // 22 - How many weeks pregnant?
            new CheckNextIntentNode(23, HowManyWeeksPregnantIntent, 24, "HowManyWeeksPregnant_Correct"),
            // 23 - Player Asks How Many Weeks Pregnant (Age Branch)
            new SpeechNode(28, new SpeechBuilder().
                Add(new Audio(Player_Asks_How_Many_Weeks_Pregnant_Age_Branch)).
                Add(new Audio(Dispatcher_Where_Are_They_Now_Question))),
            // 24 - Dispatcher Corridor Ask How Many Weeks Pregnant (Age Branch)
            new SpeechNode(28, new SpeechBuilder().
                Add(new Audio(Dispatcher_Corridor_Ask_How_Many_Weeks_Pregnant_Age_Branch)).
                Add(new Audio(Dispatcher_Where_Are_They_Now_Question))),
            // 25 - Dispatcher Prompts To Ask Age
            new SpeechNode(26, new SpeechBuilder().
                Add(new Audio(Dispatcher_Prompts_To_Ask_Age))),
            // 26 - How old is mum?
            new CheckNextIntentNode(21, HowOldIsMotherIntent, 27, "HowOldIsMother_Correct"),
            // 27 - Dispatcher Corridor Asks Mothers Age
            new SpeechNode(25, new SpeechBuilder().
                Add(new Audio(Dispatcher_Corridor_Asks_Mothers_Age)).
                Add(new Audio(Player_Asks_Mothers_Age_Age_Branch))),
            // 28 - Where are they now?
            new CheckNextIntentNode(29, WhereAreYouIntent, 30, "WhereAreYou_Correct"),
            // 29 - Player Where Are They Now Correct
            new SpeechNode(31, new SpeechBuilder().
                Add(new Audio(Player_Where_Are_They_Now_Correct)).
                Add(new Audio(Dispatcher_Can_We_See_Baby_Question))),
            // 30 - Player Where Are They Now Incorrect
            new SpeechNode(31, new SpeechBuilder().
                Add(new Audio(Player_Where_Are_They_Now_Incorrect)).
                Add(new Audio(Dispatcher_Can_We_See_Baby_Question))),
            // 31 - Is any of the baby visible?
            new CheckNextIntentNode(32, IsBabyVisibleIntent, 33, "IsBabyVisible_Correct"),
            // 32 - Player Can We See Baby Correct
            new SpeechNode(36, new SpeechBuilder().
                Add(new Audio(Player_Can_We_See_Baby_Correct))),
            // 33 - Player Can We See Baby Incorrect
            new SpeechNode(32, new SpeechBuilder().
                Add(new Audio(Player_Can_We_See_Baby_Incorrect))),
            // 34 - Is any of the baby visible? (again)
            new CheckNextIntentNode(36, IsBabyVisibleIntent, 35, "IsBabyVisibleAgain_Correct"),
            // 35 - Dispatcher Corridors Can We See Baby
            new SpeechNode(36, new SpeechBuilder().
                Add(new Audio(Dispatcher_Corridors_Can_We_See_Baby)).
                Add(new Audio(Player_Can_We_See_Baby_Correct))),
            // 36 - Yes/No (send ambulance)
            ConditionNode.CreateYesNoChoiceNode(37, 38),
            // 37 - Player Send Ambulance Correct
            new SpeechNode(39, new SpeechBuilder().
                Add(new Audio(Player_Send_Ambulance_Correct))),
            // 38 - Player Send Ambulance Incorrect
            new SpeechNode(39, new SpeechBuilder().
                Add(new Audio(Player_Send_Ambulance_Incorrect))),
            // 39 - Stay on the line, we'll tell you what to do
            new CheckNextIntentNode(40, StayOnTheLineIntent, 41, "StayOnTheLine_Correct"),
            // 40 - Player Stay On The Line Correct
            new SpeechNode(42, new SpeechBuilder().
                Add(new Audio(Player_Stay_On_The_Line_Correct)).
                Add(new Audio(Dispatcher_Baby_Slippery_Instruction))),
            // 41 - Player Stay On The Line Incorrect
            new SpeechNode(42, new SpeechBuilder().
                Add(new Audio(Player_Stay_On_The_Line_Incorrect)).
                Add(new Audio(Dispatcher_Baby_Slippery_Instruction))),
            // 42 - Baby will be slippery
            new CheckNextIntentNode(43, BabyWillBeSlipperyIntent, 44, "BabyWillBeSlippery_Correct"),
            // 43 - Dispatcher Where To Hold Baby Instruction
            new SpeechNode(47, new SpeechBuilder().
                Add(new Audio(Dispatcher_Where_To_Hold_Baby_Instruction))),
            // 44 - Player Baby Slippery Instruction Incorrect
            new SpeechNode(45, new SpeechBuilder().
                Add(new Audio(Player_Baby_Slippery_Instruction_Incorrect))),
            // 45 - Baby will be slippery (again)
            new CheckNextIntentNode(47, BabyWillBeSlipperyIntent, 46, "BabyWillBeSlipperyAgain_Correct"),
            // 46 - Dispatcher Corridors Baby Slippery Instruction Correct
            new SpeechNode(47, new SpeechBuilder().
                Add(new Audio(Dispatcher_Corridors_Baby_Slippery_Instruction)).
                Add(new Audio(Dispatcher_Where_To_Hold_Baby_Instruction))),
            // 47 - Support head, hold hips and legs tight
            new CheckNextIntentNode(48, SupportBabyIntent, 49, "SupportBaby_Correct"),
            // 48 - Player Baby Slippery Instruction Correct
            new SpeechNode(50, new SpeechBuilder().
                Add(new Audio(Player_Where_To_Hold_Baby_Instruction_Correct))),
            // 49 - Player Baby Slippery Instruction Incorrect
            new SpeechNode(50, new SpeechBuilder().
                Add(new Audio(Player_Where_To_Hold_Baby_Instruction_Incorrect)).
                Add(new Audio(Player_Where_To_Hold_Baby_Instruction_Correct))),
            // 50 - Are you ok?
            new CheckNextIntentNode(51, CheckOkIntent, 51, "CheckOk_Correct"),
            // 51 - Player Check On Caller Shout
            new SpeechNode(52, new SpeechBuilder().
                Add(new Audio(Player_Check_On_Caller_Shout))),
            // 52 - Are you ok? (shout)
            new CheckNextIntentNode(53, CheckOkIntent, 53, "CheckOkShout_Correct"),
            // 53 - Caller Birth
            new SpeechNode(54, new SpeechBuilder().
                Add(new Audio(Caller_Birth))),
            // 54 - Is the baby crying or breathing?
            new CheckNextIntentNode(55, IsBabyCryingOrBreathingIntent, 62, "IsBabyCryingOrBreathing_Correct"),
            // 55 - Player Baby Crying Breathing Question Correct
            new SpeechNode(56, new SpeechBuilder().
                Add(new Audio(Player_Baby_Crying_Breathing_Question_Correct))),
            // 56 - Is anything obviously wrong?
            new CheckNextIntentNode(57, IsAnythingObviouslyWrongIntent, 58, "IsAnythingObviouslyWrong_Correct"),
            // 57 - Player Is Anything Wrong Correct
            new SpeechNode(59, new SpeechBuilder().
                Add(new Audio(Player_Is_Anything_Wrong_Correct)).
                Add(new Audio(Dispatcher_Rub_Babys_Back_Instruction))),
            // 58 - Callers Panic What Do We Do
            new SpeechNode(59, new SpeechBuilder().
                Add(new Audio(Callers_Panic_What_Do_We_Do)).
                Add(new Audio(Dispatcher_Rub_Babys_Back_Instruction))),
            // 59 - Rub baby's back with a towel for 30 seconds?
            new CheckNextIntentNode(60, RubBabysBackInstructionIntent, 61, "RubBabysBackInstruction_Correct"),
            // 60 - Player Rub Babys Back Instruction Correct
            new SpeechNode(63, new SpeechBuilder().
                Add(new Audio(Player_Rub_Babys_Back_Instruction_Correct)).
                Add(new Audio(Baby_Cries))),
            // 61 - Player Rub Babys Back Instruction Incorrect
            new SpeechNode(63, new SpeechBuilder().
                Add(new Audio(Player_Rub_Babys_Back_Instruction_Incorrect)).
                Add(new Audio(Baby_Cries))),
            // 62 - Baby Cries
            new SpeechNode(63, new SpeechBuilder().
                Add(new Audio(Baby_Cries))),
            // 63 - Is it a boy or a girl?
            new CheckNextIntentNode(64, IsBoyOrGirlIntent, 64, "IsBoyOrGirl_Correct"),
            // 64 - Player Boy Or Girl Question
            new SpeechNode(65, new SpeechBuilder().
                Add(new Audio(Player_Boy_Or_Girl_Question))),
            // 65 - Congratulations!
            new CheckNextIntentNode(66, CongratulationsIntent, 67, "Congratulations_Correct"),
            // 66 - Player Congratulations Correct
            new SpeechNode(68, new SpeechBuilder().
                Add(new Audio(Player_Congratulations_Correct)).
                Add(new Audio(Paramedics_Arrive))),
            // 67 - Paramedics Arrive
            new SpeechNode(68, new SpeechBuilder().
                Add(new Audio(Paramedics_Arrive))),
            // 68 - Are the paramedics with you?
            new CheckNextIntentNode(69, AreParamedicsWithYouIntent, 70, "AreParamedicsWithYou_Correct"),
            // 69 - Player Paramedics Arrived Question Correct
            new SpeechNode(71, new SpeechBuilder().
                Add(new Audio(Player_Paramedics_Arrived_Question_Correct))),
            // 70 - Player Paramedics Arrived Question Incorrect
            new FinishWithCardNode("Birth Call", "Completed", new SpeechBuilder().
                Add(new Audio(Player_Paramedics_Arrived_Question_Incorrect))),
            // 71 - Well done!
            new CheckNextIntentNode(72, WellDoneIntent, 73, "WellDone_Correct"),
            // 72 - Well Done Correct
            new FinishWithCardNode("Birth Call", "Completed", new SpeechBuilder().
                Add(new Audio(Well_Done_Correct))),
            // 73 - Well Done Incorrect
            new FinishWithCardNode("Birth Call", "Completed", new SpeechBuilder().
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

            while (node != null && !node.PausesStory)
            {
                node.ModifySessionAttributes(session.Attributes, intent, session, lambdaContext);
                node = node.GetNextNode(intent, session, lambdaContext);
            }

            SkillResponse response = node.CreateResponse();
            // ModifySessionAttributes won't be called on a node which Pauses the story, so we call it manually here
            node.ModifySessionAttributes(session.Attributes, intent, session, lambdaContext);
            response.SessionAttributes = responseSessionAttributes;

            return response;
        }

        #endregion
    }
}