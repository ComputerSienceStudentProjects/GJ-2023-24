using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/FloorStats")]
public class FloorStatScriptableObject : ScriptableObject
{
    [Header("Values")] 
    [SerializeField] private int numberOfMoves = 0;
    [SerializeField] private int score = 0;
    [SerializeField] private float time = 0f;
    
    public int GetNumberOfMoves => numberOfMoves;
    public int GetScore => score;
    public float GetTime => time;
    
    public void SetTime(float newTime)
    {
        time = newTime;
    }
    
    public void SetScore(int newScore)
    {
        score = newScore;
    }
    
    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }
    
    public void SetMoves(int newMoves)
    {
        numberOfMoves = newMoves;
    }

    public void AddMoves(int movesToAdd)
    {
        numberOfMoves += movesToAdd;
    }
    

    public void Reset()
    {
        SetMoves(0);
        SetTime(0);
        SetScore(0);
    }
}
