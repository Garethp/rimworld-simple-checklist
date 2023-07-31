using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace SimpleChecklist;

[StaticConstructorOnStartup]
[HarmonyPatch(typeof (PlaySettings), "DoPlaySettingsGlobalControls")]
public class PlaySettingsPatch
{
    public static Texture2D MinimapToggle = ContentFinder<Texture2D>.Get("SimpleChecklist/ChecklistIcon");

    public static void Postfix(PlaySettings __instance, WidgetRow row, bool worldView)
    {
        if (worldView)
            return;
        
        row.ToggleableIcon(ref ChecklistReadout.ShowReadout, MinimapToggle, (string) "Toggle display of Simple Checklist", SoundDefOf.Mouseover_ButtonToggle);
    }
}