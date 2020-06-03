using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private PaddleMover paddleMover;
    
    //For shared controls
    private PaddleMover paddle1;
    private PaddleMover paddle2;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        var movers = FindObjectsOfType<PaddleMover>();
        var index = playerInput.playerIndex;
        paddleMover = movers.FirstOrDefault(m => m.playerIndex == index);

        paddle1 = movers[0];
        if(movers[1] != null)
            paddle2 = movers[1];
    }

    public void MoveP1(CallbackContext context)
    {
        if(paddleMover != null)
            paddle1.SetInputVector(context.ReadValue<Vector2>());
    }

    public void MoveP2(CallbackContext context)
    {
        if(paddleMover != null)
            paddle2.SetInputVector(context.ReadValue<Vector2>());
    }
}
