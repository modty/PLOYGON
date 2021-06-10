using System;
using System.Collections;
using System.Collections.Generic;
using ActionPool;
using Managers;
using Scripts;
using UnityEngine;
using UnityEngine.UI;

public class StatScript : MonoBehaviour
{
    private PlayerAttribute _player;
    private CombatTextManager _combatTextManager;
    public PlayerAttribute ControlledControlledChaState
    {
        get => _player;
        set => _player = value;
    }

    private static StatScript instance;
    public static StatScript Instance => instance;
}
