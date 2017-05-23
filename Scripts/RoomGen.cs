/* Christian Owen-Bellini - eeu223
 * Some aspects of the BoardMaker, roomGen and CorridorGen classes use an already existing cellular automata algorithm, however have been implemented by me in the way needed to create the game.
 * more info on the pre-existing algorithm can be found at: https://unity3d.com/learn/tutorials/s/scripting
 */


using UnityEngine;

public class RoomGen
{
    public int xPosition;       //x coord of lower left tile in each room                
    public int yPosition;       //y coord of lower left tile in each room
    public int roomWidth;  // how wide the room is                   
    public int roomHeight; //how tall the room is                   
    public Dir CorridorEntry;  //direction the corridor is heading.  


   //used for the first room.
    public void SetupRoom(RandomIntGen widthRng, RandomIntGen heightRng, int columns, int rows)
    {
        
        roomWidth = widthRng.Random; //set a width&height to random values
        roomHeight = heightRng.Random;
        xPosition = Mathf.RoundToInt(columns / 2f - roomWidth / 2f); //set x&y coord so the room is around the middle of the board.
        yPosition = Mathf.RoundToInt(rows / 2f - roomHeight / 2f);
    }


  //overload of setuproom with corridor parameter. used for all other rooms except first.
    public void SetupRoom(RandomIntGen widthRng, RandomIntGen heightRng, int columns, int rows, CorridorGen corridor)
    {
        
        CorridorEntry = corridor.dir; //entery corridor direction
        roomWidth = widthRng.Random; //set width&height to random values
        roomHeight = heightRng.Random;

        switch (corridor.dir)
        {
            // depending on corridor enetering room direction
            case Dir.North:       //height of room cant go beyond the board so its clamped. On the height of board, end of corridor leads to room
                roomHeight = Mathf.Clamp(roomHeight, 1, rows - corridor.EndPositionY);
                yPosition = corridor.EndPositionY;  //y coord of room must be at end of the corridor
                //xposition can be rand but left-most possibility no further than width and right-most possiblity is end of the corridor is at the pos of the room
                xPosition = Random.Range(corridor.EndPositionX - roomWidth + 1, corridor.EndPositionX);
                //clamped to ensure room isnt off board.            
                xPosition = Mathf.Clamp(xPosition, 0, columns - roomWidth);
                break;
            case Dir.East:
                roomWidth = Mathf.Clamp(roomWidth, 1, columns - corridor.EndPositionX);
                xPosition = corridor.EndPositionX;

                yPosition = Random.Range(corridor.EndPositionY - roomHeight + 1, corridor.EndPositionY);
                yPosition = Mathf.Clamp(yPosition, 0, rows - roomHeight);
                break;
            case Dir.South:
                roomHeight = Mathf.Clamp(roomHeight, 1, corridor.EndPositionY);
                yPosition = corridor.EndPositionY - roomHeight + 1;

                xPosition = Random.Range(corridor.EndPositionX - roomWidth + 1, corridor.EndPositionX);
                xPosition = Mathf.Clamp(xPosition, 0, columns - roomWidth);
                break;
            case Dir.West:
                roomWidth = Mathf.Clamp(roomWidth, 1, corridor.EndPositionX);
                xPosition = corridor.EndPositionX - roomWidth + 1;

                yPosition = Random.Range(corridor.EndPositionY - roomHeight + 1, corridor.EndPositionY);
                yPosition = Mathf.Clamp(yPosition, 0, rows - roomHeight);
                break;
        }
    }
}