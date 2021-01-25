using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "SO/GameState")]
public class GameState : ScriptableObject
{
    [SerializeField] private int freezeDuration = 15;
    private int birdShot = 0;

    public int BirdShot { get => birdShot; }
    public int FreezeDuration { get => freezeDuration; set => freezeDuration = value; }

    public void ShotBird()
    {
        birdShot += 1;
    }

    public void Init()
    {
        birdShot = 0;
    }
}
