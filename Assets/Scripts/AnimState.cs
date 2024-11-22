using UnityEngine;


public static class AnimState
{
    public static readonly int Idle = Animator.StringToHash("idle");
    public static readonly int Move = Animator.StringToHash("move");
    public static readonly int Attack = Animator.StringToHash("attack");
}