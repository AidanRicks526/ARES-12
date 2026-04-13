using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    public static PlayerInput playerInput;

    public static bool WasInteractPressed;

    private InputAction _interactAction;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        _interactAction = playerInput.actions["Interact"];
    }

    private void Update()
    {
        WasInteractPressed = _interactAction.WasPressedThisFrame();

    }



}
