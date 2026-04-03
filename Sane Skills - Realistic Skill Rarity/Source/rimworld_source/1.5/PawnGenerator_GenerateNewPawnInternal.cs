using Verse;

private static Pawn GenerateNewPawnInternal(ref PawnGenerationRequest request)
{
    Pawn pawn = null;
    string text = null;
    bool flag = false;
    bool flag2 = false;
    for (int i = 0; i < 120; i++)
    {
        if (i == 70)
        {
            Log.Error(string.Concat(new object[] { "Could not generate a pawn after ", 70, " tries. Last error: ", text, " Ignoring scenario requirements." }));
            flag = true;
        }
        if (i == 100)
        {
            Log.Error(string.Concat(new object[] { "Could not generate a pawn after ", 100, " tries. Last error: ", text, " Ignoring validator." }));
            flag2 = true;
        }
        PawnGenerationRequest pawnGenerationRequest = request;
        pawn = PawnGenerator.TryGenerateNewPawnInternal(ref pawnGenerationRequest, out text, flag, flag2);
        if (pawn != null)
        {
            request = pawnGenerationRequest;
            break;
        }
    }
    if (pawn == null)
    {
        Log.Error(string.Concat(new object[] { "Pawn generation error: ", text, " Too many tries (", 120, "), returning null. Generation request: ", request }));
        return null;
    }
    return pawn;
}