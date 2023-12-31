using System.Linq;
using Verse;

namespace SimpleChecklist;

public class ChecklistItem: IExposable
{
    public ChecklistItem()
    {
        Id = "";
        Label = "";
        Completed = false;
    }
    
    public ChecklistItem(string id, string label, bool completed)
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

    public string GetLabel(int number = -1)
    {
        var output = "";
        output = Completed ? Label.Aggregate(output, (current, c) => current + c + '\u0336') : Label;

        if (number >= 1) output = $"{number}. {output}";
            
        return output;
    }

    public string Id;
    public string Label;
    public bool Completed;
}