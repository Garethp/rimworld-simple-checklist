using HarmonyLib;
using RimWorld;

namespace ToDoList;

[HarmonyPatch(typeof(MapInterface), "MapInterfaceOnGUI_BeforeMainTabs")]
public class Patcher
{
    private static ToDoListReadout readout = new();
    
    public static void Postfix()
    {
        readout.ReadoutOnGUI();
    }
}