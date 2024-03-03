using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : Soliton<PlayerControl>
{
    public Vector2 crntMove { get; private set; }
    public float crntGas { get; private set; }
    public float crntBreak { get; private set; }
    public float crntHandBreak { get; private set; }

    public Action OnPauseButtonPressed;
    
    public void OnMove(InputAction.CallbackContext context) {
        crntMove = context.ReadValue<Vector2>();
    }

    public void OnGas(InputAction.CallbackContext context) {
        crntGas = context.ReadValue<float>();
    }

    public void OnBreak(InputAction.CallbackContext context)
    {
        crntBreak = context.ReadValue<float>();
    }

    public void OnHandBreak(InputAction.CallbackContext context) {

        float cont = context.ReadValue<float>();
        crntHandBreak = cont;
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.gameState != GameState.Gameplay) return;
        OnPauseButtonPressed();
    }

    public void OnMoveVal(Vector2 value)
    {
        crntMove = value;
    }

    public void OnGasVal(float value) 
    {
        crntGas = value;
    }

    public void OnBreakVal(float value)
    {
        crntBreak = value;
    }

    public void OnHandBreakVal(float value)
    {
        crntHandBreak = value;
    }

    public void OnDestroy()
    {
        OnPauseButtonPressed -= Foo;
    }

    public void Foo() { 
    
    }
}
