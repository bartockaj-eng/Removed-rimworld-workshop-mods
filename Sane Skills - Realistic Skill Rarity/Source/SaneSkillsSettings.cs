namespace SaneSkills
{
    using HarmonyLib;
    using RimWorld;
    using UnityEngine;
    using Verse;

    public class SaneSkillsSettings : ModSettings
    {
        private static bool pawnXpForSkillLevelUpCurve = true;
        private static Traverse<SimpleCurve> xpForLevelUpCurveField = Traverse.Create(typeof(SkillRecord)).Field<SimpleCurve>("XpForLevelUpCurve");

        private static bool pawnFinalSkillCurve = true;
        private static Traverse<SimpleCurve> generatedPawnSkillLevelFinalAdjustmentCurveField = Traverse.Create(typeof(PawnGenerator)).Field<SimpleCurve>("LevelFinalAdjustmentCurve");
        private static Traverse<SimpleCurve> ageSkillMaxFactorCurveField = Traverse.Create(typeof(PawnGenerator)).Field<SimpleCurve>("AgeSkillMaxFactorCurve");

        public static float level0To1CurvePoint = 1000f;
        public static float level6To7CurvePoint = 10000f;
        public static float level11To12CurvePoint = 20000f;
        public static float level15To16CurvePoint = 32000;
        public static float level19To20CurvePoint = 48000;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<bool>(ref SaneSkillsSettings.pawnXpForSkillLevelUpCurve, "pawnXpForSkillLevelUpCurve", true, true);

            Scribe_Values.Look<float>(ref SaneSkillsSettings.level0To1CurvePoint, "level0To1CurvePoint", 1000f, true);
            Scribe_Values.Look<float>(ref SaneSkillsSettings.level6To7CurvePoint, "level6To7CurvePoint", 10000f, true);
            Scribe_Values.Look<float>(ref SaneSkillsSettings.level11To12CurvePoint, "level11To12CurvePoint", 20000f, true);
            Scribe_Values.Look<float>(ref SaneSkillsSettings.level15To16CurvePoint, "level15To16CurvePoint", 32000f, true);
            Scribe_Values.Look<float>(ref SaneSkillsSettings.level19To20CurvePoint, "level19To20CurvePoint", 48000f, true);

            Scribe_Values.Look<bool>(ref SaneSkillsSettings.pawnFinalSkillCurve, "pawnFinalSkillCurve", true, true);
        }

        public static void DoWindowContents(Rect inRect)
        {
            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(inRect);
            listing_Standard.Gap(12f);

            listing_Standard.CheckboxLabeled("Modify curve of XP required to level up", ref pawnXpForSkillLevelUpCurve, null);

            if (pawnXpForSkillLevelUpCurve)
            {
                listing_Standard.Label(label: $"XP required to level up from level 0 to level 1 (Barely heard of it to Ultra Beginner): {SaneSkillsSettings.level0To1CurvePoint}", maxHeight: -1f, tooltip: "Default is 1000, Vanila is 1000.");
                level0To1CurvePoint = listing_Standard.Slider(level0To1CurvePoint, min: 1f, max: 2000f);

                listing_Standard.Label(label: $"XP required to level up from level 6 to level 7 (Capable Ameteur to Weak Professional): {SaneSkillsSettings.level6To7CurvePoint}", maxHeight: -1f, tooltip: "Default is 10000, Vanila is 7000.");
                level6To7CurvePoint = listing_Standard.Slider(level6To7CurvePoint, min: level0To1CurvePoint + 1f, max: level0To1CurvePoint + ((10000f - level0To1CurvePoint) * 2));

                listing_Standard.Label(label: $"XP required to level up from level 11 to level 12 (Very Skilled Professional to Expert): {SaneSkillsSettings.level11To12CurvePoint}", maxHeight: -1f, tooltip: "Default is 20000, Vanila is 14000.");
                level11To12CurvePoint = listing_Standard.Slider(level11To12CurvePoint, min: level6To7CurvePoint + 1f, max: level6To7CurvePoint + ((20000f - level6To7CurvePoint) * 2));

                listing_Standard.Label(label: $"XP required to level up from level 15 to level 16 (Strong Master to Region-Known Master): {SaneSkillsSettings.level15To16CurvePoint}", maxHeight: -1f, tooltip: "Default is 32000, Vanila is 22000.");
                level15To16CurvePoint = listing_Standard.Slider(level15To16CurvePoint, min: level11To12CurvePoint + 1f, max: level11To12CurvePoint + ((32000f - level11To12CurvePoint) * 2));

                listing_Standard.Label(label: $"XP required to level up from level 19 to level 20 (Planet-Leading Master to Legendary Master): {SaneSkillsSettings.level19To20CurvePoint}", maxHeight: -1f, tooltip: "Default is 48000, Vanila is 30000.");
                level19To20CurvePoint = listing_Standard.Slider(level19To20CurvePoint, min: level15To16CurvePoint + 1f, max: level15To16CurvePoint + ((48000f - level15To16CurvePoint) * 2));
            }

            listing_Standard.CheckboxLabeled("High level skills are much rarer and depend more on age", ref pawnFinalSkillCurve, null);

            listing_Standard.End();
            Rect rect = GenUI.LeftPart(GenUI.BottomPart(inRect, 0.1f), 0.1f);
            if (Widgets.ButtonText(rect, "Reset Settings", true, true, true))
            {
                SaneSkillsSettings.ResetFactor();
            }

            ApplySettings();
        }

        public static void ResetFactor()
        {
            pawnXpForSkillLevelUpCurve = true;

            SaneSkillsSettings.level0To1CurvePoint = 1000f;
            SaneSkillsSettings.level6To7CurvePoint = 10000f;
            SaneSkillsSettings.level11To12CurvePoint = 20000f;
            SaneSkillsSettings.level15To16CurvePoint = 32000;
            SaneSkillsSettings.level19To20CurvePoint = 48000f;

            pawnFinalSkillCurve = true;
        }

        public static void ApplySettings()
        {
            if (pawnXpForSkillLevelUpCurve)
            {
                xpForLevelUpCurveField.Value.Points.Clear();
                xpForLevelUpCurveField.Value.Add(0.0f, level0To1CurvePoint);
                xpForLevelUpCurveField.Value.Add(6f, level6To7CurvePoint);
                xpForLevelUpCurveField.Value.Add(11f, level11To12CurvePoint);
                xpForLevelUpCurveField.Value.Add(14f, level15To16CurvePoint);
                xpForLevelUpCurveField.Value.Add(19f, level19To20CurvePoint);
            }
            else
            {
                // Vanilla
                xpForLevelUpCurveField.Value.Points.Clear();
                xpForLevelUpCurveField.Value.Add(0.0f, 1000f);
                xpForLevelUpCurveField.Value.Add(9f, 10000f);
                xpForLevelUpCurveField.Value.Add(19f, 30000f);
            }

            if (pawnFinalSkillCurve)
            {
                generatedPawnSkillLevelFinalAdjustmentCurveField.Value.Points.Clear();
                generatedPawnSkillLevelFinalAdjustmentCurveField.Value.Add(0.0f, 0.0f);
                generatedPawnSkillLevelFinalAdjustmentCurveField.Value.Add(5f, 4f);
                generatedPawnSkillLevelFinalAdjustmentCurveField.Value.Add(10f, 7.5f);
                generatedPawnSkillLevelFinalAdjustmentCurveField.Value.Add(15f, 10.5f);
                generatedPawnSkillLevelFinalAdjustmentCurveField.Value.Add(22f, 12f);
                generatedPawnSkillLevelFinalAdjustmentCurveField.Value.Add(27f, 13f);
                generatedPawnSkillLevelFinalAdjustmentCurveField.Value.Add(33f, 14f);
                generatedPawnSkillLevelFinalAdjustmentCurveField.Value.Add(40f, 20f);

                ageSkillMaxFactorCurveField.Value.Points.Clear();
                ageSkillMaxFactorCurveField.Value.Add(0.0f, 0.0f);
                ageSkillMaxFactorCurveField.Value.Add(10f, 0.2f);
                ageSkillMaxFactorCurveField.Value.Add(15f, 0.6f);
                ageSkillMaxFactorCurveField.Value.Add(18f, 0.8f);
                ageSkillMaxFactorCurveField.Value.Add(22f, 1.2f);
                ageSkillMaxFactorCurveField.Value.Add(25f, 1.4f);
                ageSkillMaxFactorCurveField.Value.Add(33f, 1.6f);
                ageSkillMaxFactorCurveField.Value.Add(50f, 2.2f);
                ageSkillMaxFactorCurveField.Value.Add(75f, 2.6f);
                ageSkillMaxFactorCurveField.Value.Add(113f, 3f);
            }
            else
            {
                // Vanilla
                generatedPawnSkillLevelFinalAdjustmentCurveField.Value.Points.Clear();
                generatedPawnSkillLevelFinalAdjustmentCurveField.Value.Add(0.0f, 0.0f);
                generatedPawnSkillLevelFinalAdjustmentCurveField.Value.Add(10f, 10f);
                generatedPawnSkillLevelFinalAdjustmentCurveField.Value.Add(20f, 16f);
                generatedPawnSkillLevelFinalAdjustmentCurveField.Value.Add(27f, 20f);

                ageSkillMaxFactorCurveField.Value.Points.Clear();
                ageSkillMaxFactorCurveField.Value.Add(0.0f, 0.0f);
                ageSkillMaxFactorCurveField.Value.Add(10f, 0.7f);
                ageSkillMaxFactorCurveField.Value.Add(35f, 1.0f);
                ageSkillMaxFactorCurveField.Value.Add(60f, 1.6f);
            }
        }
    }
}
