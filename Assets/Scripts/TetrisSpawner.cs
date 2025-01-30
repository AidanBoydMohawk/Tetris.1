using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisSpawner : MonoBehaviour
{
    //public List<GameObject> tetrominoes;
    public GameObject[] tetrominoPrefabs;
    private Grid grid;
    private GameObject nextpiece;

    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<Grid>();
        if(grid == null)
        {
            Debug.LogError("No Grid");
            return;
        }

        //spawn initial piece
        SpawnPiece();

    }

    public void SpawnPiece()
    {
        // calculate centerr of grid based on dimensions
        Vector3 spawnPosition = new Vector3(
            Mathf.Floor(grid.width / 2), //x
            grid.height - 3,             //y
            0);                          //z

        if(nextpiece != null)
        {
            nextpiece.SetActive(true);
            nextpiece.transform.position = spawnPosition;
        }
        else
        {
            //spawn random piece
            nextpiece = InstantiateRandomPiece();
            nextpiece.transform.position = spawnPosition;
            
        }

        //prepare next piece
        //spawn next piece here
        nextpiece = InstantiateRandomPiece();
        nextpiece.SetActive(false); //deactivate until its time to use it
            
    }

    private GameObject InstantiateRandomPiece()
    {
        int index = Random.Range(0, tetrominoPrefabs.Length);
        return Instantiate(tetrominoPrefabs[index]);
    }
}
