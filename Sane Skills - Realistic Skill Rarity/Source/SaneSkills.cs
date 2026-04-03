namespace SaneSkills
{
    using UnityEngine;
    using Verse;

    // Token: 0x02000009 RID: 9
    [StaticConstructorOnStartup]
	public class SaneSkills : Mod
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000D RID: 13 RVA: 0x00002D65 File Offset: 0x00000F65
		public static SaneSkills Instance
		{
			get
			{
				return SaneSkills._instance;
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002D6C File Offset: 0x00000F6C
		public SaneSkills(ModContentPack content) : base(content)
		{
			SaneSkills._instance = this;
			SaneSkills.settings = base.GetSettings<SaneSkillsSettings>();

			SaneSkillsSettings.ApplySettings();
        }

		// Token: 0x0600000F RID: 15 RVA: 0x00002DA0 File Offset: 0x00000FA0
		public override string SettingsCategory()
		{
			return "Sane Skills";
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002DB7 File Offset: 0x00000FB7
		public override void DoSettingsWindowContents(Rect inRect)
		{
			SaneSkillsSettings.DoWindowContents(inRect);
		}

		// Token: 0x04000009 RID: 9
		private static SaneSkills _instance;

		// Token: 0x0400000A RID: 10
		public static SaneSkillsSettings settings;
	}
}
