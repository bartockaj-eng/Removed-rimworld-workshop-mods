namespace SaneSkills
{
    using System;
    using System.Reflection;
    using HarmonyLib;
    using Verse;

    // Token: 0x02000008 RID: 8
    [StaticConstructorOnStartup]
	internal class Main
	{
		// Token: 0x0600000B RID: 11 RVA: 0x00002D0C File Offset: 0x00000F0C
		static Main()
		{
			try
			{
				Harmony harmony = new Harmony("SaneSkills");
				harmony.PatchAll(Assembly.GetExecutingAssembly());
			}
			catch (Exception e)
			{
				Log.Error(string.Format("Error during startup:\n{0}", e));
			}
		}
	}
}
