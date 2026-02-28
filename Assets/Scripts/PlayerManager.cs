using UnityEngine;
using UnityEngine.InputSystem;

public enum playerState
{
    normal,
    minigame
}

public class PlayerManager : MonoBehaviour
{
    playerState currentState;

    public PlayerCharacterController movement;
    public PlayerInteraction interaction;

    InputSystem_Actions inputSystemActions;
    InputManager inputManager;

    private void Awake()
    {
        inputSystemActions = new InputSystem_Actions();
        Blackboard.inputSystemActions = inputSystemActions;
        inputManager = new InputManager(new InputAction[] {
                                            inputSystemActions.Player.Move,
                                            inputSystemActions.Player.Jump,
                                            inputSystemActions.Player.Interact,
                                            inputSystemActions.Player.Look,
                                            inputSystemActions.Player.Use,
                                            inputSystemActions.Player.SecondUse,
                                            inputSystemActions.Player.Cancel,
                                            inputSystemActions.Player.Drop,
                                            inputSystemActions.Player.Throw
                                            });
        Blackboard.inputManager = inputManager;

        SetPlayerState(playerState.normal);
        Blackboard.playerManager = this;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SetPlayerState(playerState _state)
    {
        currentState = _state;

        if(currentState == playerState.normal) 
        {
            movement.enabled = true;
            interaction.enabled = true;
        }
        else
        {
            movement.enabled = false;
            interaction.enabled = false;
        }
    }


    private void OnEnable()
    {
        inputManager.WhenEnabled();
    }

    private void OnDisable()
    {
        inputManager.WhenDisabled();
    }
}
