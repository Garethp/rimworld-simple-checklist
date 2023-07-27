using RimWorld;
using Verse;

namespace ToDoList;

public class ToDoItem: IExposable
{
    public ToDoItem()
    {
        Id = "";
        Label = "";
        Completed = false;
    }
    
    public ToDoItem(string id, string label, bool completed)
    {
        Id = id;
        Label = label;
        Completed = completed;
    }

    public void ExposeData()
    {
        Scribe_Values.Look(ref Id, "Id");
        Scribe_Values.Look(ref Label, "Label");
        Scribe_Values.Look(ref Completed, "Completed");
    }

    public string Id;
    public string Label;
    public bool Completed;
}