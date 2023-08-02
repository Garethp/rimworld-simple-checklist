using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace SimpleChecklist;

public class ChecklistReadout
{
    private Vector2 scrollPosition;
    private float lastDrawnHeight;

    float x = (float) UI.screenWidth - 450f;
    static float initialY = (float) UI.screenHeight - 70f;

    private static float y = initialY;
    private static bool showManageItems = false;
    
    // float x = (float) UI.screenWidth - 130;
    // float y = (float) UI.screenHeight - 7; 

    public static bool ShowReadout = true;

    public static bool Checkbox(Rect rect, string s, ref bool checkOn)
    {
        bool flag = checkOn;
        if (Widgets.ButtonInvisible(rect))
        {
            checkOn = !checkOn;
            if (checkOn)
                SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera();
            else
                SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera();
        }
        TextAnchor anchor = Text.Anchor;
        Text.Anchor = TextAnchor.MiddleLeft;
        Widgets.DrawTextureFitted(rect.LeftPartPixels(30f), checkOn ? Widgets.CheckboxOnTex : (Texture) Widgets.CheckboxOffTex, 0.5f);
        rect.x += 30f;
        Widgets.Label(rect, s);
        Text.Anchor = anchor;
        return checkOn != flag;
    }
    
    public void ReadoutOnGUI()
    {
        if (Find.UIRoot.screenshotMode.Active || !ShowReadout) return;
        
        var gameComponent = Current.Game.GetComponent<SimpleChecklistGameComponent>();
        if (gameComponent is null) return;
        
        if (
            // Event.current.type == EventType.Layout || 
            Current.ProgramState != ProgramState.Playing || Find.MainTabsRoot.OpenTab == MainButtonDefOf.Menu)
            return;
        GenUI.DrawTextWinterShadow(new Rect(256f, 512f, -256f, -512f));
        Text.Font = GameFont.Small;
        Rect rect1 = new Rect(x, y, 300f, UI.screenHeight - 7 - 200f);
        Rect rect2 = new Rect(0.0f, 0.0f, rect1.width, lastDrawnHeight);
        int num = rect2.height > (double) rect1.height ? 1 : 0;
        if (num != 0)
        {
            Widgets.BeginScrollView(rect1, ref scrollPosition, rect2, false);
        }
        else
        {
            scrollPosition = Vector2.zero;
            Widgets.BeginGroup(rect1);
        }
        DoReadoutSimple(rect2, rect1.height);
        if (num != 0)
            Widgets.EndScrollView();
        else
            Widgets.EndGroup();
    }
    
    private void DoReadoutSimple(Rect rect, float outRectHeight)
    {
        var component = Current.Game.GetComponent<SimpleChecklistGameComponent>();
        
        Widgets.BeginGroup(rect);
        Text.Anchor = TextAnchor.MiddleLeft;
        var y = 0.0f;
        
        foreach (var item in component.Items)
        {
            var rect1 = new Rect(0.0f, y, 999f, 24f);
            DrawResourceSimple(rect1, item);
            y += 24f;
        }

        y += 5;
        if (Widgets.ButtonText(new Rect(0, y, 70, 24), "Add Item"))
        {
            Find.WindowStack.Add(new AddItemWindow());
        }
        
        if (showManageItems && Widgets.ButtonText(new Rect(75, y, 110, 24), "Manage Items"))
        {
            Find.WindowStack.Add(new ManageItemsWindow());
        }

        y += 24;
        
        Text.Anchor = TextAnchor.UpperLeft;
        lastDrawnHeight = y;
        Widgets.EndGroup();
    }
    
    public void DrawResourceSimple(Rect rect, ChecklistItem item)
    {
        var output = "";
        output = item.Completed ? item.Label.Aggregate(output, (current, c) => current + c + '\u0336') : item.Label;

        rect.y += 2f;
        Checkbox(new Rect(0f, rect.y, rect.width, rect.height), output, ref item.Completed);
    }

    public static void UpdateItemCount(int itemCount)
    {
        y = initialY - itemCount * 24;
        showManageItems = itemCount > 0;
    }
}