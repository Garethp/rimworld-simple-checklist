using System;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace SimpleChecklist;

public class ChecklistWindow: Window
{
    public ChecklistWindow()
    {
        draggable = true;
        closeOnAccept = false;
        closeOnCancel = false;
        closeOnClickedOutside = false;
        doCloseButton = false;
        drawShadow = false;
        preventCameraMotion = false;
        onlyOneOfTypeAllowed = true;
        layer = WindowLayer.GameUI;
        doWindowBackground = false;
        drawInScreenshotMode = false;
        focusWhenOpened = false;
    }

    public override Vector2 InitialSize => new(300, internalHeight);

    protected override void SetInitialSizeAndPosition()
    {
        var initialSize = InitialSize;
        var component = Current.Game.GetComponent<SimpleChecklistGameComponent>();

        var position = new Vector2(component.WindowX, component.WindowY);
        if (position.x == 0 && position.y == 0)
        {
            position = new Vector2(UI.screenWidth - 450f, UI.screenHeight - internalHeight - 30);
        }
        
        windowRect = new Rect(position.x, position.y, initialSize.x, initialSize.y);
            
        lastInternalHeight = internalHeight;
        windowRect = windowRect.Rounded();
    }
    
    private static bool showManageItems;
    private static float internalHeight;
    private static float lastInternalHeight;
    
    public override void DoWindowContents(Rect rect1)
    {
        windowRect.height = internalHeight;

        if (Math.Abs(lastInternalHeight - internalHeight) > 0)
        {
            // If an item was added or removed, adjust the position of the window so that it doesn't move around
            var newY = windowRect.position.y - (internalHeight - lastInternalHeight);

            if (newY < 10) newY = windowRect.position.y;
            
            windowRect.position = new Vector2(windowRect.position.x , newY);
            lastInternalHeight = internalHeight;
        }
        
        // The checklist can interfere with the architect gizmos, so let's not show it when the architect menu is open
        if (
            Find.MainTabsRoot.OpenTab == MainButtonDefOf.Architect || 
            Current.ProgramState != ProgramState.Playing || 
            Find.MainTabsRoot.OpenTab == MainButtonDefOf.Menu
        ) 
            return;
        
        var gameComponent = Current.Game.GetComponent<SimpleChecklistGameComponent>();
        if (gameComponent is null) return;
        
        // There doesn't appear to be a "PostDrag" event, so let's just update the settings whenever rendered
        gameComponent.WindowX = windowRect.position.x;
        gameComponent.WindowY = windowRect.position.y;

        Text.Font = GameFont.Small;
        DoReadoutSimple(rect1, rect1.height);
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

        Text.Anchor = TextAnchor.UpperLeft;
        Widgets.EndGroup();
    }
    
    public static void UpdateItemCount(int itemCount)
    {
        internalHeight = itemCount * 24 + 70;
        showManageItems = itemCount > 0;
    }
    
    public void DrawResourceSimple(Rect rect, ChecklistItem item)
    {
        rect.y += 2f;
        Checkbox(new Rect(0f, rect.y, rect.width, rect.height), item.GetLabel(), ref item.Completed);
    }
    
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
}