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
                { HowOldIsMotherIntent, 19 }
            }, 23, "WeeksPregnantOrAge_Correct"),
            // 15 - Player Asks How Many Weeks Pregnant (Weeks Pregnant Branch)
            new SpeechNode(16, new SpeechBuilder().
                Add(new Audio(Player_Asks_How_Many_Weeks_Pregnant_Weeks_Pregnant_Branch))),
            // 16 - How old is mum?
            new CheckNextIntentNode(17, HowOldIsMotherIntent, 18, "HowOldIsMother_Correct"),
            // 17 - Player Asks Mothers Age (Weeks Pregnant Branch)
            new SpeechNode(26, new SpeechBuilder().
                Add(new Audio(Player_Asks_Mothers_Age_Weeks_Pregnant_Branch)).
                Add(new Audio(Dispatcher_Where_Are_They_Now_Question))),
            // 18 - Dispatcher Corridor Mothers Age (Weeks Pregnant Branch)
            new SpeechNode(26, new SpeechBuilder().
                Add(new Audio(Dispatcher_Corridor_Mothers_Age_Weeks_Pregnant_Branch)).
                Add(new Audio(Dispatcher_Where_Are_They_Now_Question))),
            // 19 - Player Asks Mothers Age (Age Branch)
            new SpeechNode(20, new SpeechBuilder().
                Add(new Audio(Player_Asks_Mothers_Age_Age_Branch))),
            // 20 - How many weeks pregnant?
            new CheckNextIntentNode(21, HowManyWeeksPregnantIntent, 22, "HowManyWeeksPregnant_Correct"),
            // 21 - Player Asks How Many Weeks Pregnant (Age Branch)
            new SpeechNode(26, new SpeechBuilder().
                Add(new Audio(Player_Asks_How_Many_Weeks_Pregnant_Age_Branch)).
                Add(new Audio(Dispatcher_Where_Are_They_Now_Question))),
            // 22 - Dispatcher Corridor Ask How Many Weeks Pregnant (Age Branch)
            new SpeechNode(26, new SpeechBuilder().
                Add(new Audio(Dispatcher_Corridor_Ask_How_Many_Weeks_Pregnant_Age_Branch)).
                Add(new Audio(Dispatcher_Where_Are_They_Now_Question))),
            // 23 - Dispatcher Prompts To Ask Age
            new SpeechNode(24, new SpeechBuilder().
                Add(new Audio(Dispatcher_Prompts_To_Ask_Age))),
            // 24 - How old is mum?
            new CheckNextIntentNode(19, HowOldIsMotherIntent, 25, "HowOldIsMother_Correct"),
            // 25 - Dispatcher Corridor Asks Mothers Age
            new SpeechNode(20, new SpeechBuilder().
                Add(new Audio(Dispatcher_Corridor_Asks_Mothers_Age)).
                Add(new Audio(Player_Asks_Mothers_Age_Age_Branch))),
            // 26 - Where are they now?
            new CheckNextIntentNode(27, WhereAreYouIntent, 28, "WhereAreYou_Correct"),
            // 27 - Player Where Are They Now Correct
            new SpeechNode(29, new SpeechBuilder().
                Add(new Audio(Player_Where_Are_They_Now_Correct)).
                Add(new Audio(Dispatcher_Can_We_See_Baby_Question))),
            // 28 - Player Where Are They Now Incorrect
            new SpeechNode(29, new SpeechBuilder().
                Add(new Audio(Player_Where_Are_They_Now_Incorrect)).
                Add(new Audio(Dispatcher_Can_We_See_Baby_Question))),
            // 29 - Is any of the baby visible?
            new CheckNextIntentNode(30, IsBabyVisibleIntent, 31, "IsBabyVisible_Correct"),
            // 30 - Player Can We See Baby Correct
            new SpeechNode(34, new SpeechBuilder().
                Add(new Audio(Player_Can_We_See_Baby_Correct))),
            // 31 - Player Can We See Baby Incorrect
            new SpeechNode(32, new SpeechBuilder().
                Add(new Audio(Player_Can_We_See_Baby_Incorrect))),
            // 32 - Is any of the baby visible? (again)
            new CheckNextIntentNode(30, IsBabyVisibleIntent, 33, "IsBabyVisibleAgain_Correct"),
            // 33 - Dispatcher Corridors Can We See Baby
            new SpeechNode(34, new SpeechBuilder().
                Add(new Audio(Dispatcher_Corridors_Can_We_See_Baby)).
                Add(new Audio(Player_Can_We_See_Baby_Correct))),
            // 34 - Yes/No (send ambulance)
            ConditionNode.CreateYesNoChoiceNode(35, 36),
            // 35 - Player Send Ambulance Correct
            new SpeechNode(37, new SpeechBuilder().
                Add(new Audio(Player_Send_Ambulance_Correct))),
            // 36 - Player Send Ambulance Incorrect
            new SpeechNode(37, new SpeechBuilder().
                Add(new Audio(Player_Send_Ambulance_Incorrect))),
            // 37 - Stay on the line, we'll tell you what to do
            new CheckNextIntentNode(38, StayOnTheLineIntent, 39, "StayOnTheLine_Correct"),
            // 38 - Player Stay On The Line Correct
            new SpeechNode(40, new SpeechBuilder().
                Add(new Audio(Player_Stay_On_The_Line_Correct)).
                Add(new Audio(Dispatcher_Baby_Slippery_Instruction))),
            // 39 - Player Stay On The Line Incorrect
            new SpeechNode(40, new SpeechBuilder().
                Add(new Audio(Player_Stay_On_The_Line_Incorrect)).
                Add(new Audio(Dispatcher_Baby_Slippery_Instruction))),
            // 40 - Baby will be slippery
            new CheckNextIntentNode(41, BabyWillBeSlipperyIntent, 42, "BabyWillBeSlippery_Correct"),
            // 41 - Dispatcher Where To Hold Baby Instruction
            new SpeechNode(45, new SpeechBuilder().
                Add(new Audio(Dispatcher_Where_To_Hold_Baby_Instruction))),
            // 42 - Player Baby Slippery Instruction Incorrect
            new SpeechNode(43, new SpeechBuilder().
                Add(new Audio(Player_Baby_Slippery_Instruction_Incorrect))),
            // 43 - Baby will be slippery (again)
            new CheckNextIntentNode(41, BabyWillBeSlipperyIntent, 44, "BabyWillBeSlipperyAgain_Correct"),
            // 44 - Dispatcher Corridors Baby Slippery Instruction Correct
            new SpeechNode(45, new SpeechBuilder().
                Add(new Audio(Dispatcher_Corridors_Baby_Slippery_Instruction)).
                Add(new Audio(Dispatcher_Where_To_Hold_Baby_Instruction))),
            // 45 - Support head, hold hips and legs tight
            new CheckNextIntentNode(46, SupportBabyIntent, 47, "SupportBaby_Correct"),
            // 46 - Player Baby Slippery Instruction Correct
            new SpeechNode(48, new SpeechBuilder().
                Add(new Audio(Player_Where_To_Hold_Baby_Instruction_Correct))),
            // 47 - Player Baby Slippery Instruction Incorrect
            new SpeechNode(48, new SpeechBuilder().
                Add(new Audio(Player_Where_To_Hold_Baby_Instruction_Incorrect)).
                Add(new Audio(Player_Where_To_Hold_Baby_Instruction_Correct))),
            // 48 - Are you ok?
            new CheckNextIntentNode(49, CheckOkIntent, 49, "CheckOk_Correct"),
            // 49 - Player Check On Caller Shout
            new SpeechNode(50, new SpeechBuilder().
                Add(new Audio(Player_Check_On_Caller_Shout))),
            // 50 - Are you ok? (shout)
            new CheckNextIntentNode(51, CheckOkIntent, 51, "CheckOkShout_Correct"),
            // 51 - Caller Birth
            new SpeechNode(52, new SpeechBuilder().
                Add(new Audio(Caller_Birth))),
            // 52 - Is the baby crying or breathing?
            new CheckNextIntentNode(53, IsBabyCryingOrBreathingIntent, 60, "IsBabyCryingOrBreathing_Correct"),
            // 53 - Player Baby Crying Breathing Question Correct
            new SpeechNode(54, new SpeechBuilder().
                Add(new Audio(Player_Baby_Crying_Breathing_Question_Correct))),
            // 54 - Is anything obviously wrong?
            new CheckNextIntentNode(55, IsAnythingObviouslyWrongIntent, 56, "IsAnythingObviouslyWrong_Correct"),
            // 55 - Player Is Anything Wrong Correct
            new SpeechNode(57, new SpeechBuilder().
                Add(new Audio(Player_Is_Anything_Wrong_Correct)).
                Add(new Audio(Dispatcher_Rub_Babys_Back_Instruction))),
            // 56 - Callers Panic What Do We Do
            new SpeechNode(57, new SpeechBuilder().
                Add(new Audio(Callers_Panic_What_Do_We_Do)).
                Add(new Audio(Dispatcher_Rub_Babys_Back_Instruction))),
            // 57 - Rub baby's back with a towel for 30 seconds?
            new CheckNextIntentNode(58, RubBabysBackInstructionIntent, 59, "RubBabysBackInstruction_Correct"),
            // 58 - Player Rub Babys Back Instruction Correct
            new SpeechNode(61, new SpeechBuilder().
                Add(new Audio(Player_Rub_Babys_Back_Instruction_Correct)).
                Add(new Audio(Baby_Cries))),
            // 59 - Player Rub Babys Back Instruction Incorrect
            new SpeechNode(61, new SpeechBuilder().
                Add(new Audio(Player_Rub_Babys_Back_Instruction_Incorrect)).
                Add(new Audio(Baby_Cries))),
            // 60 - Baby Cries
            new SpeechNode(61, new SpeechBuilder().
                Add(new Audio(Baby_Cries))),
            // 61 - Is it a boy or a girl?
            new CheckNextIntentNode(62, IsBoyOrGirlIntent, 62, "IsBoyOrGirl_Correct"),
            // 62 - Player Boy Or Girl Question
            new SpeechNode(63, new SpeechBuilder().
                Add(new Audio(Player_Boy_Or_Girl_Question))),
            // 63 - Congratulations!
            new CheckNextIntentNode(64, CongratulationsIntent, 65, "Congratulations_Correct"),
            // 64 - Player Congratulations Correct
            new SpeechNode(66, new SpeechBuilder().
                Add(new Audio(Player_Congratulations_Correct)).
                Add(new Audio(Paramedics_Arrive))),
            // 65 - Paramedics Arrive
            new SpeechNode(66, new SpeechBuilder().
                Add(new Audio(Paramedics_Arrive))),
            // 66 - Are the paramedics with you?
            new CheckNextIntentNode(67, AreParamedicsWithYouIntent, 68, "AreParamedicsWithYou_Correct"),
            // 67 - Player Paramedics Arrived Question Correct
            new SpeechNode(69, new SpeechBuilder().
                Add(new Audio(Player_Paramedics_Arrived_Question_Correct))),
            // 68 - Player Paramedics Arrived Question Incorrect
            new SpeechNode(72, new SpeechBuilder().
                Add(new Audio(Player_Paramedics_Arrived_Question_Incorrect)).
                Add(new Audio(Outro)).
                Add(new Sentence("Would you like to hear the credits?"))),
            // 69 - Well done!
            new CheckNextIntentNode(70, WellDoneIntent, 71, "WellDone_Correct"),
            // 70 - Well Done Correct
            new SpeechNode(72, new SpeechBuilder().
                Add(new Audio(Well_Done_Correct)).
                Add(new Audio(Outro)).
                Add(new Sentence("Would you like to hear the credits?"))),
            // 71 - Well Done Incorrect
            new SpeechNode(72, new SpeechBuilder().
                Add(new Audio(Well_Done_Incorrect)).
                Add(new Audio(Outro)).
                Add(new Sentence("Would you like to hear the credits?"))),
            // 72 - Does player want to hear the credits
            ConditionNode.CreateYesNoChoiceNode(73, 74),
            // 73 - Credits
            new FinishWithCardNode("Birth Call Completed", "Thanks to you, Tim and Charlotte's baby was safely delivered!  The password 'Stork' will be useful for later on in the game, so don't forget it.  Stork Artwork: Pedro Alves.",
                "https://s3-eu-west-1.amazonaws.com/rtg-dispatcher/icons/StorkIconSmall.png",
                "https://s3-eu-west-1.amazonaws.com/rtg-dispatcher/icons/StorkIconLarge.png", new SpeechBuilder().
                Add(new Audio(Credits))),
            // 74 - No Credits
            new FinishWithCardNode("Birth Call Completed", "Thanks to you, Tim and Charlotte's baby was safely delivered!  The password 'Stork' will be useful for later on in the game, so don't forget it.  Stork Artwork: Pedro Alves.",
                "https://s3-eu-west-1.amazonaws.com/rtg-dispatcher/icons/StorkIconSmall.png",
                "https://s3-eu-west-1.amazonaws.com/rtg-dispatcher/icons/StorkIconLarge.png", new SpeechBuilder().
                Add(new Audio(Goodbye))),
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
                node.ModifySessionAttributes(responseSessionAttributes, intent, session, lambdaContext);
                node = node.GetNextNode(intent, session, lambdaContext);
            }

            SkillResponse response = node.CreateResponse();
            // ModifySessionAttributes won't be called on a node which Pauses the story, so we call it manually here
            node.ModifySessionAttributes(responseSessionAttributes, intent, session, lambdaContext);
            response.SessionAttributes = responseSessionAttributes;

            return response;
        }

        #endregion
    }
}