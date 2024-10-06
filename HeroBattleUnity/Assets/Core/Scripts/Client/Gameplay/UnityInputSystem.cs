
using FixMath.NET;
using HeroBattle.FixedMath;
using HeroBattleShare.Core;
using HeroBattleShare.Factory;
using UnityEngine;

public class UnityInputSystem : Singleton<UnityInputSystem>, IGameInputSystem
{

   protected PlayerActionsExample playerInput;
    protected override void OnAwake()
    {
        base.OnAwake();
        playerInput = new PlayerActionsExample();
    }
    public Vector2f GetPlayerInput()
    {
        var baseInput = playerInput.Player.Move.ReadValue<Vector2>();
        return new Vector2f((Fix64)baseInput.x, (Fix64)baseInput.y);
    }
}