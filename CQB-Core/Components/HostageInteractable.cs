using EFT;
using EFT.Interactive;

namespace CQB.Components;

public class HostageInteractable : InteractableObject
{
    private Player _targetPlayer;

    public void OnHostageTake()
    {
        _targetPlayer = transform.root.gameObject.GetComponent<Player>();
        
        Plugin.LogSource.LogInfo(_targetPlayer + " Grabbed");

        
    }

    public ActionsReturnClass GetActions()
    {
        ActionsReturnClass actionsReturnClass = new ActionsReturnClass();
        actionsReturnClass.Actions.Add(new ActionsTypesClass()
        {
            Action = OnHostageTake,
            Name = "Take Hostage",
            Disabled = false,
        });
        
        return actionsReturnClass;
    }
}