using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "SO/GameState")]
public class GameState : ScriptableObject
{
    private int birdShot = 0;

    public int BirdShot { get => birdShot; }

    public void ShotBird()
    {
        birdShot += 1;
    }

    public void Init()
    {
        birdShot = 0;
    }
}
