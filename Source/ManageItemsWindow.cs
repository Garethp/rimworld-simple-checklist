﻿using System.Linq;
using UnityEngine;
using Verse;

namespace SimpleChecklist;

public class ManageItemsWindow: Window
{
    public override void DoWindowContents(Rect rect)
    {
        var gameComponent = Current.Game.GetComponent<SimpleChecklistGameComponent>();
        
        var y = 0;
        
        Widgets.Label(new Rect(0.0f, y, rect.width, rect.height), "Manage Items");

        y += 48;

        var numberItemsRect = new Rect(0, rect.height - 35f, 100, 35f);
        ChecklistWindow.Checkbox(numberItemsRect, "Number Items", ref gameComponent.NumberItems);
        
        var okRect = new Rect(rect.width - 100, rect.height - 35f, 100, 35f);
        if (Widgets.ButtonText(okRect, "Ok"))
        {
            Find.WindowStack.TryRemove(this);
        }
        
        var addRect = new Rect(rect.width - 210, rect.height - 35f, 100, 35f);
        if (Widgets.ButtonText(addRect, "Add Item"))
        {
            Find.WindowStack.Add(new AddItemWindow());
        }
        
        var items = gameComponent.Items.ToArray();

        if (items.Length == 0) return;
        for (var i = 0; i < items.Length; i++)
        {
            var checklistItem = items[i];
            
            if (i != 0)
            {
                var moveUpRect = new Rect(0, y, 24f, 24f);
                if (Widgets.ButtonImage(moveUpRect, TexButton.ReorderUp, Color.white))
                {
                    gameComponent.MoveUp(checklistItem.Id);
                }
                
                if (Mouse.IsOver(moveUpRect))
                {
                    TooltipHandler.TipRegion(rect, "Move Up");
                }
            }
            
            if (i != items.Length - 1)
            {
                var moveDownRect = new Rect(30, y, 24f, 24f);
                if (Widgets.ButtonImage(moveDownRect, TexButton.ReorderDown, Color.white))
                {
                    gameComponent.MoveDown(checklistItem.Id);
                }
                
                if (Mouse.IsOver(moveDownRect))
                {
                    TooltipHandler.TipRegion(rect, "Move Down");
                }
            }

            var removeButtonRect = new Rect(60f, y, 24, 24);
            Widgets.DrawTextureFitted(removeButtonRect, TexButton.Minus, 1f);
            if (Widgets.ButtonInvisible(removeButtonRect))
            {
                gameComponent.RemoveItem(checklistItem.Id);
            }

            if (Mouse.IsOver(removeButtonRect))
            {
                TooltipHandler.TipRegion(rect, "Delete".Translate());
            }
            
            var editButtonRect = new Rect(90f, y, 24, 24);
            Widgets.DrawTextureFitted(editButtonRect, TexButton.Rename, 1f);
            if (Widgets.ButtonInvisible(editButtonRect))
            {
                Find.WindowStack.Add(new EditItemWindow(i, checklistItem.Label));
            }

            if (Mouse.IsOver(editButtonRect))
            {
                TooltipHandler.TipRegion(rect, "Rename".Translate());
            }

            Widgets.Label(new Rect(120f, y, rect.width, 24), checklistItem.GetLabel(gameComponent.NumberItems ? i + 1 : -1));
            y += 30;
        }
    }
}