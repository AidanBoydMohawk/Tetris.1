using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TetrisManager : MonoBehaviour
{
    private Grid grid;
    public GameObject gameOverText;
    [SerializeField]
    TextMeshProUGUI scoreText;
    public int score;
    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckGameOver();
        scoreText.text = "Score: " + score;
    }

    public void CalculateScore(int linesCleared)
    {
        switch (linesCleared)
        {

            case 1:
                score += 100;
                break;
            case 2:
                score += 300;
                break;
            case 3:
                score += 600;
                break;
            case 4:
                score += 800;
                break; 
            default:
                score += 100; 
                break;
        
        }

    }

    public void CheckGameOver()
    {
        for(int i = 0; i < grid.width; i++)
        {
          if (grid.IsCellOccupied(new Vector2Int(i, grid.height - 1)))
          {
                Debug.Log("Game Over");
                gameOverText.SetActive(true);
                Invoke("ReloadScene", 5);
                
          }
        }
        
    }public void ReloadScene()
    {
        SceneManager.LoadScene("Tetris");
    }
}
