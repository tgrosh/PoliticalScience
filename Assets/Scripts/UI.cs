using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour {
    public static UI instance;
    public GameObject gameOver;
    public GameObject livesPanel;
    public GameObject livesIndicatorPrefab;

    private void Awake()
    {
        if (UI.instance == null)
        {
            instance = this;
        }
    }

    public void UpdateLives(int lives)
    {
        foreach(Transform livesIndicator in livesPanel.transform)
        {
            Destroy(livesIndicator.gameObject); 
        }
        for (int x = 0; x < lives; x++)
        {
            Instantiate(livesIndicatorPrefab, livesPanel.transform);
        }
    }

    public void ShowGameOver()
    {
        gameOver.SetActive(true);
    }
}
