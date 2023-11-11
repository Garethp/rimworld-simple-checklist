using System.IO;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace SimpleChecklist;

public class SimpleChecklist : Mod
{
    public static DirectoryInfo SettingsDir;

    public SimpleChecklist(ModContentPack content) : base(content)
    {
        new Harmony("Garethp.rimworld.SimpleChecklist.main").PatchAll();
    }

    public override void DoSettingsWindowContents(Rect canvas)
    {
        base.DoSettingsWindowContents(canvas);
        GetSettings<Settings>().DoWindowContents(canvas);
    }

    public override string SettingsCategory()
    {
        return "Simple Checklist";
    }
}