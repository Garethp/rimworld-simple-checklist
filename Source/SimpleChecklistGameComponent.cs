using System;
using System.Collections.Generic;
using Verse;

namespace SimpleChecklist;

public class SimpleChecklistGameComponent : GameComponent
{
    public SimpleChecklistGameComponent(Game game)
    {
    }

    public List<ChecklistItem> Items = new ();
    public float WindowX = 0;
    public float WindowY = 0;
    public bool ShowBackground = false;
    
    public void AddItem(string itemText)
    {
        var id = Guid.NewGuid();
        Items.Add(new (id.ToString(), itemText, false));
        
        ChecklistWindow.UpdateItemCount(Items.Count);
    }

    public void RemoveItem(string id)
    {
        var itemToRemove = Items.FirstOrDefault(item => item.Id == id);
        if (itemToRemove is null) return;

        Items.Remove(itemToRemove);
        ChecklistWindow.UpdateItemCount(Items.Count);
    }

    public void MoveUp(string id)
    {
        var index = Items.FindIndex(item => item.Id == id);
        if (index == 0) return;

        var item = Items[index];
        Items.RemoveAt(index);
        Items.Insert(index - 1, item);
    }
    
    public void MoveDown(string id)
    {
        var index = Items.FindIndex(item => item.Id == id);
        if (index == Items.Count - 1) return;

        var item = Items[index];
        Items.RemoveAt(index);
        Items.Insert(index + 1, item);
    }
    
    public override void ExposeData()
    {
        Scribe_Collections.Look(ref Items, "Items", LookMode.Deep);
        Scribe_Values.Look(ref WindowX, "WindowX");
        Scribe_Values.Look(ref WindowY, "WindowY");
        Scribe_Values.Look(ref ShowBackground, "ShowBackground");
        
        Items ??= new List<ChecklistItem>();
        
        ChecklistWindow.UpdateItemCount(Items.Count);
        
        base.ExposeData();
    }

    public override void FinalizeInit()
    {
        ChecklistWindow.UpdateItemCount(Items.Count);
        Find.WindowStack.Add(new ChecklistWindow());

        base.FinalizeInit();
    }
}