using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour {
    public int currentScore;
    
    public void AddScore(int amount)
    {
        currentScore += amount;
    }
}
