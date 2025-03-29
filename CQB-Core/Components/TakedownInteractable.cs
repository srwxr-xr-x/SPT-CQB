using EFT;
using EFT.Interactive;


namespace CQB.Components;

public class TakedownInteractable : InteractableObject
{
    private Player _targetPlayer;

    public void OnTakedown()
    {
        _targetPlayer = transform.root.gameObject.GetComponent<Player>();
        Plugin.LogSource.LogInfo(_targetPlayer + " Takedown");
    }

    public ActionsReturnClass GetActions()
    {
        ActionsReturnClass actionsReturnClass = new ActionsReturnClass();
        actionsReturnClass.Actions.Add(new ActionsTypesClass()
        {
            Action = OnTakedown,
            Name = "Takedown",
            Disabled = false,
        });
        
        return actionsReturnClass;
    }
}