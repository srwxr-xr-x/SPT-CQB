using EFT;
using EFT.Interactive;
using EFT.InventoryLogic;
using Fika.Core.Coop.Players;
using ShoulderCQB.Common;
using ShoulderCQB.Fika;
using UnityEngine;

namespace ShoulderCQB.Components;

public class ShoulderInteractable : InteractableObject
{
    Transform _moveTarget;
    private Player _targetPlayer;

    private bool _disableLetGo = true;
    private bool _disableHold;
    private bool _disableShoulderTap;
    
    public void OnFollow()
    {
        _disableHold = true;
        _disableLetGo = false;
        _disableShoulderTap = false;
        _targetPlayer = transform.root.gameObject.GetComponent<Player>();
        
        transform.root.gameObject.GetComponent<CapsuleCollider>().radius = 0.175f;
        Utils.MainPlayer.gameObject.transform.root.gameObject.GetComponent<CapsuleCollider>().radius = 0.175f;
        
        // Now, find the location to move to
        if (name.Contains("Left"))
        {
            // transition gun to shoulder left
            Utils.MainPlayer.MovementContext.LeftStanceController.SetLeftStanceForce(true);
            _moveTarget = transform.root.gameObject.transform.GetChild(0).GetChild(4).GetChild(0).GetChild(4);
            
        } else if (name.Contains("Right"))
        {
            // transition gun to right shoulder (normal)
            _moveTarget = transform.root.gameObject.transform.GetChild(0).GetChild(4).GetChild(0).GetChild(5);
        }

        InvokeRepeating("MovePlayer", 0f, 0.01f);
        // Pull out pistol
        Utils.MainPlayer.SetSlotItem(EquipmentSlot.Holster, null);
        
    }
    void MovePlayer()
    {
        Vector2 heading = new Vector2(_moveTarget.position.x - Utils.MainPlayer.gameObject.transform.position.x, _moveTarget.position.z - Utils.MainPlayer.gameObject.transform.position.z);
        
        // Main player needs to move fast, as fast as possible, based on sqrMagnitude
        // target Player needs to move at half speed
        Utils.MainPlayer.MovementContext.SetCharacterMovementSpeed((heading.sqrMagnitude * 2) * (Utils.MainPlayer.MovementContext.MaxSpeed*2), true);
        FikaMethods.SendCQBPacket(Utils.MainPlayer.Id, _targetPlayer.Id, false, 0, (Utils.MainPlayer.MovementContext.MaxSpeed/1.75f), false);
        
        // Cancel out rotation of the player when moving to prevent weird shit happening (literal witchcraft dont ask me)
        Vector3 rotationHandle = Quaternion.Euler(0f, 0f, Utils.MainPlayer.Rotation.x) * heading;
        if (!(heading.sqrMagnitude < 0.05f))
        {
            Utils.MainPlayer.Move(rotationHandle.normalized);
        }
        // If to far, disconnect from, not worth it.
        if (heading.sqrMagnitude > 10f)
        {
            CancelInvoke("MovePlayer");
        }
        // Handle death of target
        if (!_targetPlayer.HealthController.IsAlive)
        {
            CancelInvoke("MovePlayer");
        }
    }
    
    public void OnLetGo()
    {
        CancelInvoke("MovePlayer");
        _disableShoulderTap = false;
        _disableHold = false;
        _disableLetGo = true;
        FikaMethods.SendCQBPacket(Utils.MainPlayer.Id,_targetPlayer.Id,false, 0, Utils.MainPlayer.MovementContext.MaxSpeed, false);
        Utils.MainPlayer.MovementContext.SetCharacterMovementSpeed(Utils.MainPlayer.MovementContext.MaxSpeed, true);
        
    }

    public void OnTapShoulder()
    {       
        _targetPlayer = transform.root.gameObject.GetComponent<Player>();

        if (this.name.Contains("Left"))
        {
            Utils.MainPlayer.SetInteractInHands(EInteraction.ContainerOpenDefault);
            Utils.MainPlayer.gameObject.GetComponent<GamePlayerOwner>().ClearInteractionState();
            // Lean in direction slightly to simulate tap
            FikaMethods.SendCQBPacket(Utils.MainPlayer.Id,_targetPlayer.Id,true, 0, 0, true);
        }
        else
        {
            Utils.MainPlayer.SetInteractInHands(EInteraction.ContainerOpenDefault);
            Utils.MainPlayer.gameObject.GetComponent<GamePlayerOwner>().ClearInteractionState(); 
            // Lean in direction slightly to simulate tap
            FikaMethods.SendCQBPacket(Utils.MainPlayer.Id,_targetPlayer.Id,true, 1, 0, true);
        }
    }
    
    public ActionsReturnClass GetActions()
    {
        ActionsReturnClass actionsReturnClass = new ActionsReturnClass();
        actionsReturnClass.Actions.Add(new ActionsTypesClass()
        {
            Action = OnTapShoulder,
            Name = "Tap Shoulder",
            Disabled = _disableShoulderTap,
        });
        actionsReturnClass.Actions.Add(new ActionsTypesClass()
        {
            Action = OnFollow,
            Name = "Follow Side",
            Disabled = _disableHold,
        });
        actionsReturnClass.Actions.Add(new ActionsTypesClass()
        {
            Action = OnLetGo,
            Name = "Stop Following",
            Disabled = _disableLetGo,
        });
        
        return actionsReturnClass;
    }
}