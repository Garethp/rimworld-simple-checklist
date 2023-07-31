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
    
    public void AddItem(string itemText)
    {
        var id = Guid.NewGuid();
        Items.Add(new (id.ToString(), itemText, false));
        
        ChecklistReadout.UpdateItemCount(Items.Count);
    }

    public void RemoveItem(string id)
    {
        var itemToRemove = Items.FirstOrDefault(item => item.Id == id);
        if (itemToRemove is null) return;

        Items.Remove(itemToRemove);
        ChecklistReadout.UpdateItemCount(Items.Count);
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
        Items ??= new List<ChecklistItem>();
        
        ChecklistReadout.UpdateItemCount(Items.Count);
        
        base.ExposeData();
    }
}