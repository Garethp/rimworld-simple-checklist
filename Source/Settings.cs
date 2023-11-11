using UnityEngine;
using Verse;

namespace SimpleChecklist;

public class Settings : ModSettings
{
    public void DoWindowContents(Rect canvas)
    {
        var redrawWindow = false;

        var listing = new Listing_Standard();
        listing.Begin(canvas);
        
        if (Current.ProgramState != ProgramState.Playing)
        {
            Widgets.Label(listing.GetRect(34f), "There are no settings until you've loaded a game");
            listing.End();
            return;
        }
        

        var gameComponent = Current.Game.GetComponent<SimpleChecklistGameComponent>();

        var resetPositionLine = listing.GetRect(34f);
        Widgets.Label(resetPositionLine.LeftHalf().ContractedBy(2f), "Reset Checklist Position");
        if (Widgets.ButtonText(resetPositionLine.RightHalf().ContractedBy(2f), "Reset"))
        {
            gameComponent.WindowX = 0;
            gameComponent.WindowY = 0;

            redrawWindow = true;
        }

        listing.Gap();
        var showBackgroundLine = listing.GetRect(34f);
        Widgets.Label(showBackgroundLine.LeftHalf().ContractedBy(2f), "Show Checklist Background");
        if (Widgets.ButtonText(showBackgroundLine.RightHalf().ContractedBy(2f),
                gameComponent.ShowBackground ? "Hide" : "Show"))
        {
            gameComponent.ShowBackground =
                !gameComponent.ShowBackground;

            redrawWindow = true;
        }
        listing.Gap();
        Widgets.Label(listing.GetRect(34f), "Having this on may look weird when menus are open. Only use this to help you see where to drag for repositioning");

        if (redrawWindow && Find.WindowStack.IsOpen(typeof(ChecklistWindow)))
        {
            Find.WindowStack.TryRemove(typeof(ChecklistWindow));
            Find.WindowStack.Add(new ChecklistWindow());
        }
        
        listing.End();
    }
}