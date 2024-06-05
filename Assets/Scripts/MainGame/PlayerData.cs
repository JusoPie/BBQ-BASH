using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerData : INetworkInput
{
    public float HorizontalInput;
    public NetworkButtons NetworkButtons;
}
