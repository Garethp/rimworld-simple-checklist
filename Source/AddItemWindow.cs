using RimWorld;
using UnityEngine;
using Verse;

namespace SimpleChecklist;

public class AddItemWindow: Window
{
    public override Vector2 InitialSize => new Vector2(640f, 80f);

    public string itemText = "";

    private bool firstFrame = true;
    
    public AddItemWindow()
    {
        forcePause = true;
        closeOnAccept = false;
        absorbInputAroundWindow = true;
    }
    
    public override void DoWindowContents(Rect rect)
    {
        Text.Font = GameFont.Small;
        var itemSubmitted = false;
        if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter))
        {
            itemSubmitted = true;
            Event.current.Use();
        }

        GUI.SetNextControlName("ChecklistItemAddInput");
        itemText = Widgets.TextField(new Rect(0.0f, 0f, (float) (rect.width / 2.0 + 70.0), 35f), itemText);
        var rect1 = new Rect((float) (rect.width / 2.0 + 90.0), 0f, (float) (rect.width / 2.0 - 90.0), 35f);

        if (firstFrame)
        {
            GUI.FocusControl("ChecklistItemAddInput");
            firstFrame = false;
        }
        
        if (Widgets.ButtonText(rect1, "Accept".Translate()))
        {
            itemSubmitted = true;
        }
        
        if (itemSubmitted)
        {
            if (itemText.Length > 0)
            {
                Current.Game.GetComponent<SimpleChecklistGameComponent>().AddItem(itemText);
            }

            Find.WindowStack.TryRemove(this);
        }
    }
}