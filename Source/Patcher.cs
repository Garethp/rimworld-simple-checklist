using HarmonyLib;
using RimWorld;

namespace SimpleChecklist;

[HarmonyPatch(typeof(MapInterface), "MapInterfaceOnGUI_BeforeMainTabs")]
public class Patcher
{
    private static ChecklistReadout readout = new();
    
    public static void Postfix()
    {
        readout.ReadoutOnGUI();
    }
}