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
            Log.Error(string.Concat(new string[]
            {
                        "Could not generate a pawn after ",
                        70.ToString(),
                        " tries. Last error: ",
                        text,
                        " Ignoring scenario requirements."
            }));
            flag = true;
        }
        if (i == 100)
        {
            Log.Error(string.Concat(new string[]
            {
                        "Could not generate a pawn after ",
                        100.ToString(),
                        " tries. Last error: ",
                        text,
                        " Ignoring validator."
            }));
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
        string[] array = new string[6];
        array[0] = "Pawn generation error: ";
        array[1] = text;
        array[2] = " Too many tries (";
        array[3] = 120.ToString();
        array[4] = "), returning null. Generation request: ";
        int num = 5;
        PawnGenerationRequest pawnGenerationRequest2 = request;
        array[num] = pawnGenerationRequest2.ToString();
        Log.Error(string.Concat(array));
        return null;
    }
    return pawn;
}