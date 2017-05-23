/* Christian Owen-Bellini - eeu223
 * Some aspects of the BoardMaker, roomGen and CorridorGen classes use an already existing cellular automata algorithm, however have been implemented by me in the way needed to create the game.
 * more info on the pre-existing algorithm can be found at: https://unity3d.com/learn/tutorials/s/scripting
 */


using UnityEngine;

// Enum to specify the direction.
public enum Dir
{
    North, East, South, West,
}

public class CorridorGen
{
    public int startXPos;         // x coord for the start of the corridor.
    public int startYPos;         // y coord for the start of the corridor.
    public int corridorLength;    // How many units long the corridor is.
    public Dir dir;   // direction the corridor is heading from room.


    // Get end position of the corridor based on it's start position and direction.
    public int EndPositionX
    {
        get
        {
            if (dir == Dir.North || dir == Dir.South)       //if direction = north/south return starting xposition
                return startXPos;
            if (dir == Dir.East)                                  //if direction - east return starting xposition plus length of the coridor - 1..this makes it so the corridor tiles dont overlap with rooms..
                return startXPos + corridorLength - 1;
            return startXPos - corridorLength + 1;                      //if direction IS NOT north/south/east it must be west, and does the same with east except adds rather than subtracts.
        }
    }


    public int EndPositionY
    {
        get
        {
            if (dir == Dir.East || dir == Dir.West)
                return startYPos;
            if (dir == Dir.North)
                return startYPos + corridorLength - 1;
            return startYPos - corridorLength + 1;
        }
    }


    public void CorridorSetup(RoomGen room, RandomIntGen length, RandomIntGen roomWidth, RandomIntGen roomHeight, int columns, int rows, bool firstCorridor)
    {
        // Set a random direction (random index from 0-3, cast to Direction).
        dir = (Dir)Random.Range(0, 4);

        // Find direction opposite to one entering room current corridor is leaving from.
        // Cast prev corridor direction to int between 0 and 3 then add 2, then find remainder
        Dir oppositeDirection = (Dir)(((int)room.CorridorEntry + 2) % 4);

        // If this is not the first corridor and the randomly selected direction is opposite to the previous corridor's direction...
        if (!firstCorridor && dir == oppositeDirection)
        {
            // Rotate the direction 90 degrees clockwise (North becomes East, East becomes South.)
            int directionVal = (int)dir;
            directionVal++;
            directionVal = directionVal % 4;
            dir = (Dir)directionVal;

        }

        // Set random length.
        corridorLength = length.Random;

        // Create cap from randomvalue script for how long it can be..
        int max_Length = length.maximumVal;

        switch (dir)
        {
            // If the choosen direction is North
            case Dir.North:
                // starting position in the x axis can be random but within the width of the room.
                startXPos = Random.Range(room.xPosition, room.xPosition + room.roomWidth - 1);

                // starting position in the y axis must be the top of the room.
                startYPos = room.yPosition + room.roomHeight;

                // maximum length the corridor can be is the height of the board (rows) but from the top of the room (y pos + height).
                max_Length = rows - startYPos - roomHeight.minimumvVal;
                break;
            case Dir.East:
                startXPos = room.xPosition + room.roomWidth;
                startYPos = Random.Range(room.yPosition, room.yPosition + room.roomHeight - 1);
                max_Length = columns - startXPos - roomWidth.minimumvVal;
                break;
            case Dir.South:
                startXPos = Random.Range(room.xPosition, room.xPosition + room.roomWidth);
                startYPos = room.yPosition;
                max_Length = startYPos - roomHeight.minimumvVal;
                break;
            case Dir.West:
                startXPos = room.xPosition;
                startYPos = Random.Range(room.yPosition, room.yPosition + room.roomHeight);
                max_Length = startXPos - roomWidth.minimumvVal;
                break;
        }

        // clamp the corridor length to make sure it doesn't go off the board.
        corridorLength = Mathf.Clamp(corridorLength, 1, max_Length);
    }
}