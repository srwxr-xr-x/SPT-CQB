using EFT;
using EFT.Interactive;
using CQB.Common;
using CQB.Fika;

namespace CQB.Components;

public class ThighInteractable : InteractableObject
{
    private Player _targetPlayer;

    public void OnTapThigh()
    {
        _targetPlayer = transform.root.gameObject.GetComponent<Player>();

        if (this.name.Contains("Left"))
        {
            Utils.MainPlayer.SetInteractInHands(EInteraction.ContainerOpenDefault);
            Utils.MainPlayer.gameObject.GetComponent<GamePlayerOwner>().ClearInteractionState();
            // Lean in direction slightly to simulate tap
            FikaBridge.SendCQBPacket(Utils.MainPlayer.Id, _targetPlayer.Id, true, 2, 0, true);
        }
        else
        {
            Utils.MainPlayer.SetInteractInHands(EInteraction.ContainerOpenDefault);
            Utils.MainPlayer.gameObject.GetComponent<GamePlayerOwner>().ClearInteractionState(); 
            // Lean in direction slightly to simulate tap
            FikaBridge.SendCQBPacket(Utils.MainPlayer.Id,_targetPlayer.Id, true, 3, 0, true);
        }
    }

    public ActionsReturnClass GetActions()
    {
        ActionsReturnClass actionsReturnClass = new ActionsReturnClass();
        actionsReturnClass.Actions.Add(new ActionsTypesClass()
        {
            Action = OnTapThigh,
            Name = "Tap Thigh",
            Disabled = false,
        });
        
        return actionsReturnClass;
    }
}