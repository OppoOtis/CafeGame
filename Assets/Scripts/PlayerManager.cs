using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public enum playerState
{
    freeControl,
    noControl
}

public class PlayerManager : MonoBehaviour
{
    playerState currentState;

    public PlayerCharacterController movement;
    public PlayerInteraction interaction;

    InputSystem_Actions inputSystemActions;
    InputManager inputManager;

    public TextMeshProUGUI PromptText;
    bool textPromptRunning;

    private void Awake()
    {
        inputSystemActions = new InputSystem_Actions();
        Blackboard.inputSystemActions = inputSystemActions;
        inputManager = new InputManager(new InputAction[] {
                                            inputSystemActions.Player.Move,
                                            inputSystemActions.Player.Jump,
                                            inputSystemActions.Player.InteractLeft,
                                            inputSystemActions.Player.InteractRight,
                                            inputSystemActions.Player.Look,
                                            inputSystemActions.Player.LeftUse,
                                            inputSystemActions.Player.LeftUseHold,
                                            inputSystemActions.Player.RightUse,
                                            inputSystemActions.Player.RightUseHold,
                                            inputSystemActions.Player.Cancel,
                                            inputSystemActions.Player.DropLeft,
                                            inputSystemActions.Player.DropRight,
                                            inputSystemActions.Player.Throw
                                            });
        Blackboard.inputManager = inputManager;

        SetPlayerState(playerState.freeControl);
        Blackboard.playerManager = this;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SetPlayerState(playerState _state)
    {
        currentState = _state;

        switch (currentState)
        {
            case playerState.freeControl:
                movement.enabled = true;
                interaction.enabled = true;
                break;
                case playerState.noControl: 
                movement.enabled = false;
                interaction.enabled = false;
                break;

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

    public IEnumerator ShowTextPrompt(string _text)
    {
        if (textPromptRunning)
            yield return null;

        textPromptRunning = true;
        PromptText.text = _text;

        yield return new WaitForSeconds(3);

        PromptText.text = "";
        textPromptRunning = false;
    }
}
