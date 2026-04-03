namespace SaneSkills
{
    using HarmonyLib;
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using Verse;
    using System.Linq;

    [HarmonyPatch(typeof(PawnGenerator), "GenerateNewPawnInternal")]
    public static class GenerateNewPawnInternalPatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator ilGenerator)
        {
            var codes = new List<CodeInstruction>(instructions);
            int patchedCount = 0;

            var patches = new[]
            {
                // Iterations of the loop to try to generate a pawn
                (index: 95, originalValue: 120, newValue: 12000),
                (index: 115, originalValue: 120, newValue: 12000),

                // The index of the loop at which the scenario requirements are ignored and an error log is printed.
                (index: 12, originalValue: 70, newValue: 7000),
                (index: 22, originalValue: 70, newValue: 7000),

                // The index of the loop at which the validator is ignored and an error log is printed.
                (index: 44, originalValue: 100, newValue: 10000),
                (index: 54, originalValue: 100, newValue: 10000)
            };

            foreach (var patch in patches)
            {
                patchedCount += TryPatchNumberInstruction(codes, patch.index, patch.originalValue, patch.newValue) ? 1 : 0;
            }


            int expectedPatchedCount = patches.Length;
            if (patchedCount != expectedPatchedCount)
            {
                Log.Error($"[Sane-Skills] failed to patch GenerateNewPawnInternal, expected patches: {expectedPatchedCount}, actual patches: {patchedCount}");
            }

            return codes.AsEnumerable();
        }

        private static bool TryPatchNumberInstruction(List<CodeInstruction> codes, int index, int originalValue, int newValue)
        {
            if (index < 0 || index >= codes.Count)
            {
                return false;
            }

            CodeInstruction code = codes[index];
            if (code != null && code.opcode == OpCodes.Ldc_I4_S && ((sbyte)code.operand) == originalValue)
            {
                code.operand = newValue;
                code.opcode = OpCodes.Ldc_I4;
                return true;
            }

            return false;
        }

    }
}