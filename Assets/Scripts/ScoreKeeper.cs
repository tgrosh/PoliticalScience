using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour {
    public int currentScore;
    public TMPro.TextMeshProUGUI textMesh;
    
    public void AddScore(int amount)
    {
        currentScore += amount;
        textMesh.text = currentScore.ToString("N0");
    }
}
