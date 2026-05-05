using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    public static PlayerInput playerInput;

    public static bool WasInteractPressed;

    public static bool WasBackpackPressed;

    private InputAction _interactAction;

    private InputAction _backpackAction;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        _interactAction = playerInput.actions["Interact"];
        _backpackAction = playerInput.actions["Backpack"];
    }

    private void Update()
    {
        WasInteractPressed = _interactAction.WasPressedThisFrame();
        WasBackpackPressed = _backpackAction.WasPressedThisFrame();
        if(WasBackpackPressed == true)
        {
            Debug.Log("Backpack pressed");
        }

    }



}
