using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisPiece : MonoBehaviour
{
    private Grid grid;
    private float dropInterval = 1;
    private float dropTimer;
    private bool isLocked = false;
    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<Grid>();
        dropTimer = dropInterval;
    }

    // Update is called once per frame
    void Update()
    {
        HandleAutomaticDrop();
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { Move(Vector3.left); }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { Move(Vector3.right); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { Move(Vector3.down); }
        if (Input.GetKeyDown(KeyCode.UpArrow)) { RotatePiece(); }
        if (Input.GetKeyDown(KeyCode.Space)) { HardDrop(); }
    }

    public void Move(Vector3 direction)
    {
        transform.position += direction;

        if(!IsValidPosition())
        {
           transform.position -= direction; //go back if not valid

           if(direction == Vector3.down)
            {
                LockPiece(); //lock the piece in place
            }
        }
    }

    void RotatePiece()
    {
        //store the original position and rotation for rollback
        Vector3 originalPosition = transform.position;
        Quaternion originalRotation = transform.rotation;
        transform.Rotate(0, 0, 90);

        if (!IsValidPosition())
        {
            if(!TryWallKick(originalPosition, originalRotation))
            {
                //revert if no wallkick works
                transform.position = originalPosition;
                transform.rotation = originalRotation;
                Debug.Log("roataion invalid, reverting back to original position/rotation");

            }
            else
            {
                Debug.Log("rotation/position adjusted with wallkick");
            }
        }
    }

    private bool TryWallKick(Vector3 originalPosition, Quaternion originalRotation)
    {
        //define wall kicks (srs guidelines)
        Vector2Int[] wallKickOffsets = new Vector2Int[]
            {
             new Vector2Int(1,0), //move right by 1
             new Vector2Int(-1,0), //move left by 1
             new Vector2Int(0,-1), //move down 
             new Vector2Int(1,-1), //move diagonally right-down
             new Vector2Int(-1,-1), //move diagonally left down

             new Vector2Int(2,0), //move right by 2
             new Vector2Int(-2,0), //move left by 2
             new Vector2Int(0,-2), //move down 
             new Vector2Int(2,-1), //move diagonally right-down
             new Vector2Int(-2,-1), //move diagonally left down
             new Vector2Int(2,2), //move diagonally right-down
             new Vector2Int(-2,-2), //move diagonally left down


             new Vector2Int(3,0), //move right by 3
             new Vector2Int(-3,0), //move left by 3
             new Vector2Int(0,-3), //move down 
             new Vector2Int(3,-1), //move diagonally right-down
             new Vector2Int(-3,-1), //move diagonally right-down
             new Vector2Int(3,-2), //move diagonally left down
             new Vector2Int(-3,-2), //move diagonally right-down
             new Vector2Int(3,3), //move diagonally left down
             new Vector2Int(-3,-3), //move diagonally left down

            };

        foreach(Vector2Int offset in wallKickOffsets)
        {
            //apply offset to piece
            transform.position += (Vector3Int)offset;
            //check if the new position is valid
            if (IsValidPosition())
            {
                return true;
            }
            //revert posotion if invalid
            transform.position -= (Vector3Int)offset;
        }

        return false;
    }

    private void HardDrop()
    {
        do
        {
            Move(Vector3.down);

        } while (!isLocked);
    }

        private bool IsValidPosition()
        {
            foreach (Transform block in transform)
            {
                Vector2Int position = Vector2Int.RoundToInt(block.position);

                if (grid.IsCellOccupied(position))
                {
                    return false;
                }
            }
            return true;
        }

    private void HandleAutomaticDrop()
    {
       dropTimer -= (Time.deltaTime * grid.numberPiecesPlaced);
       
       if (dropTimer <= 0)
        {
            Move(Vector3.down);
            dropTimer = dropInterval; //reset the timer
        }

    }

    private void LockPiece()
    {
        grid.numberPiecesPlaced += 1;
        isLocked = true;
        foreach(Transform block in transform)
        {
            Vector2Int position = Vector2Int.RoundToInt(block.position);
            grid.AddBlockToGrid(block, position); //adds block to grid for line clearing checks
        }

        grid.ClearFullLines(); //check and clear full lines
        if (FindObjectOfType<TetrisSpawner>())
        {
            FindObjectOfType<TetrisSpawner>().SpawnPiece(); //spawn a new piece
        }
        Destroy(this); //remove the script only
    }
    
}
