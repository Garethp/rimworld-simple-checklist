using System;
using System.Collections.Generic;
using Verse;

namespace ToDoList;

public class ToDoListWorldComponent : GameComponent
{
    public ToDoListWorldComponent(Game game)
    {
    }

    public List<ToDoItem> Items;

    public void MarkItemCompleted(string id)
    {
        Items.Find(item => item.Id == id).Completed = true;
    }

    public void AddItem(string itemText)
    {
        var id = Guid.NewGuid();
        Items.Add(new (id.ToString(), itemText, false));
    }

    public void RemoveItem(string id)
    {
        var itemToRemove = Items.FirstOrDefault(item => item.Id == id);
        if (itemToRemove is null) return;

        Items.Remove(itemToRemove);
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
        Items ??= new List<ToDoItem>();
        
        base.ExposeData();
    }
}