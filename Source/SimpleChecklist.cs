using System.IO;
using HarmonyLib;
using Verse;

namespace SimpleChecklist;

public class SimpleChecklist : Mod
{
    public static DirectoryInfo SettingsDir;

    public SimpleChecklist(ModContentPack content) : base(content)
    {
        new Harmony("Garethp.rimworld.SimpleChecklist.main").PatchAll();
    }
}