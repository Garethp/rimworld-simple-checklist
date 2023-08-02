using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace SimpleChecklist;

[StaticConstructorOnStartup]
[HarmonyPatch(typeof (PlaySettings), "DoPlaySettingsGlobalControls")]
public class PlaySettingsPatch
{
    public static Texture2D ChecklistToggle = ContentFinder<Texture2D>.Get("SimpleChecklist/ChecklistIcon");
    private static bool ChecklistOpenState = true;
    private static bool LastChecklistOpenState = true;

    public static void Postfix(PlaySettings __instance, WidgetRow row, bool worldView)
    {
        if (worldView)
            return;

        if (ChecklistOpenState != LastChecklistOpenState)
        {
            LastChecklistOpenState = ChecklistOpenState;
            
            switch (ChecklistOpenState)
            {
                case true when !Find.WindowStack.IsOpen(typeof(ChecklistWindow)):
                    Find.WindowStack.Add(new ChecklistWindow());
                    break;
                case false when Find.WindowStack.IsOpen(typeof(ChecklistWindow)):
                    Find.WindowStack.TryRemove(typeof(ChecklistWindow));
                    break;
            }
        }
        
        row.ToggleableIcon(ref ChecklistOpenState, ChecklistToggle, (string) "Toggle display of Simple Checklist", SoundDefOf.Mouseover_ButtonToggle);
    }
}