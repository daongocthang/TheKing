using System;
using Troops;
using UnityEngine;

public class Warrior : Troop
{
    [SerializeField]
    private float moveSpeed = 1f;

    protected override void OnCreateStates()
    {
        throw new System.NotImplementedException();
    }
}