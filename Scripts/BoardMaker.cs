/* Christian Owen-Bellini - eeu223
 * Some aspects of the BoardMaker, roomGen and CorridorGen classes use an already existing cellular automata algorithm, however have been implemented by me in the way needed to create the game.
 * more info on the pre-existing algorithm can be found at: https://unity3d.com/learn/tutorials/s/scripting
 */


using System;
using System.Collections;
using UnityEngine;

using Random = UnityEngine.Random;


public class BoardMaker : MonoBehaviour
{
    //tiles that can be placed
    public enum TypeofTile
    {
        Wall, Floor, floorSpawner
    }

    public int columns = 100;       //board width
    public int rows = 100;          //board height
    public RandomIntGen numRooms = new RandomIntGen(10, 100); //number of rooms 
    public RandomIntGen roomWidth = new RandomIntGen(3, 25);  //range of room width
    public RandomIntGen roomHeight = new RandomIntGen(3, 25); //range of room height
    public RandomIntGen corridorSize = new RandomIntGen(3, 15); //size of corridors
    public RandomIntGen numVal = new RandomIntGen(0, 100);    //value for enemy generation
    public RandomIntGen playerRange = new RandomIntGen(0, 100);   //player range indicator A

    public GameObject enemies;      //game object holder for enemies
    public GameObject floorTiles; //array of floor tiles available
    public GameObject wallTiles;  //array of wall tiles available  
    public GameObject player;       //player gameobject
    public GameObject floorSpawnerTiles;  //array of spawner tiles available.

    private TypeofTile[][] tile;  //tiletype jagged array
    private RoomGen[] room_;    //room array
    private CorridorGen[] corridor_;    //corridor array
    private GameObject boardHolder; //gameobject to hold game board
    public Boolean spawned; //player spawned boolean



    private void Start()
    {
        //create gameboard
        boardHolder = new GameObject("BoardHolder");
  
        //generation methods
        TileArraySetup();
        CreateRoomsCorridors();
        SetRoomTileValues();
        SetCorridorTileValues();
        BuildTiles();
        MapOuterWalls();

        //spawning methods
        
        PlayerSpawner();
        EnemySpawner();
        
       
    }


    void TileArraySetup()
    {
        tile = new TypeofTile[columns][]; //set jagged array tile to width defined 
        for (int i = 0; i < tile.Length; i++)    //loop through all tile array
        {          
            tile[i] = new TypeofTile[rows]; //set each tile to defined height
        }
    }


    void CreateRoomsCorridors()
    {
        room_ = new RoomGen[numRooms.Random]; //create room_ array with random amount of rooms.
        corridor_ = new CorridorGen[room_.Length - 1];  //assign corridor_ array with same amount of rooms -1 
        room_[0] = new RoomGen();   //map room and corridor
        corridor_[0] = new CorridorGen(); 
        room_[0].SetupRoom(roomWidth, roomHeight, columns, rows); //map second room
        corridor_[0].CorridorSetup(room_[0], corridorSize, roomWidth, roomHeight, columns, rows, true); // map second corridor from the first room.

        for (int i = 1; i < room_.Length; i++) //loop through the room_ array
        {
            room_[i] = new RoomGen(); //create new room 
            room_[i].SetupRoom(roomWidth, roomHeight, columns, rows, corridor_[i - 1]); //set up room based on prev corridor position

            if (i < corridor_.Length)   //if current forloop iteration is less than the corridor_ value, create a corridor and set up corridor based on the room just created
            {
                corridor_[i] = new CorridorGen();
                corridor_[i].CorridorSetup(room_[i], corridorSize, roomWidth, roomHeight, columns, rows, false);
            }
        }
    }


    void SetRoomTileValues()
    {
        for (int i = 0; i < room_.Length; i++)  //loop through all rooms
        {
            RoomGen currentRoom = room_[i]; 

            for (int j = 0; j < currentRoom.roomWidth; j++) //for each room go through room width
            {
                int xCoordinate = currentRoom.xPosition + j;

                for (int k = 0; k < currentRoom.roomHeight; k++) //for each horizontal tile, go up vertically through room's height.
                {
                    int yCoordinate = currentRoom.yPosition + k;

                    if (i == 1) //if second room
                    {
                        tile[xCoordinate][yCoordinate] = TypeofTile.floorSpawner; //set tile to spawner tile
                    }
                    else
                    {
                        tile[xCoordinate][yCoordinate] = TypeofTile.Floor; //else set tile to floor.
                    }
                }
            }
        }
    }


    void SetCorridorTileValues()
    {
        for (int i = 0; i < corridor_.Length; i++) // loop through every corridor
        {
            CorridorGen currentCorridor = corridor_[i]; //assign current corridor index to currentcorridor object

            for (int j = 0; j < currentCorridor.corridorLength; j++) //loop through corridor array length
            {
                int xCoord = currentCorridor.startXPos; //coordinates at start of the corridor
                int yCoord = currentCorridor.startYPos;

                switch (currentCorridor.dir) //depending on corridor direction, add or take away coord based on how far through  the length of the loop is.
                {
                    case Dir.North:
                        yCoord += j;
                        break;
                    case Dir.East:
                        xCoord += j;
                        break;
                    case Dir.South:
                        yCoord -= j;
                        break;
                    case Dir.West:
                        xCoord -= j;
                        break;
                }

                tile[xCoord][yCoord] = TypeofTile.Floor; //set tile at coords to floor.
            }
        }
    }


    void BuildTiles()
    {
        for (int i = 0; i < tile.Length; i++) //iterate through all tiles in jagged array
        {
            for (int j = 0; j < tile[i].Length; j++)
            {
                if (tile[i][j] == TypeofTile.floorSpawner)
                {
                    InstantiateTile(floorSpawnerTiles, i, j); //instantiate floorspawn tile if tiletype is floorspawner
                }
                if (tile[i][j] == TypeofTile.Floor)
                {
                    InstantiateTile(floorTiles, i, j);  //instantiate floor tile if tiletype is floor
                }
               
                if(tile[i][j] == TypeofTile.Wall)
                {
                    InstantiateTile(wallTiles, i, j);  //instantiate wall tile if tiletype is wall
                }
            }
        }
    }


    void MapOuterWalls()
    {
        float leftEdgeX = -1f; //make outer walls one unit to the left, right up down etc.
        float rightEdgeX = columns + 0f;
        float bottomEdgeY = -1f;
        float topEdgeY = rows + 0f;

        BuildVerticalOuterWall(leftEdgeX, bottomEdgeY, topEdgeY); //instantiate horizontal walls, both sides.
        BuildVerticalOuterWall(rightEdgeX, bottomEdgeY, topEdgeY);

        BuildHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, bottomEdgeY); //instantiate horizontal walls, both side.
        BuildHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, topEdgeY);
    }


    void BuildVerticalOuterWall(float xCoord, float startingY, float endingY)
    {
        float currentY = startingY; //start loop at starting value for y

        while (currentY <= endingY) //while value for y is less than end value
        {
            InstantiateTile(wallTiles, xCoord, currentY); //build outer walla tile at y coord and current y coord.
            currentY++;
        }
    }


    void BuildHorizontalOuterWall(float startingX, float endingX, float yCoord)
    {
        float currentX = startingX; //start loop at starting value x

        while (currentX <= endingX) //while value for x is less then end value
        {
            InstantiateTile(wallTiles, currentX, yCoord); //build outer wall tile at y coord.
            currentX++;
        }
    }


    void InstantiateTile(GameObject prefabs, float xCoord, float yCoord)
    {
        //int randomIndex = UnityEngine.Random.Range(0, prefabs.Length); //create random index for array
        Vector3 position = new Vector3(xCoord, yCoord, 0f); //position to instante based on coord
        GameObject tileInstance = Instantiate(prefabs, position, Quaternion.identity) as GameObject; //position to be instantiated based on coord
        tileInstance.transform.parent = boardHolder.transform; // set tile parent to boardholder.
    }

    void EnemySpawner()
    {
        int enemyRange; //create range variable
        Vector3 playerpos = GameObject.FindWithTag("Player").transform.position; //find player
        int spawnrange = 5; //define minimum range enemy must be from player.

        for (int i = 0; i < tile.Length; i++) //loop through tile
        {
            for (int j = 0; j < tile[i].Length; j++)
            {
                enemyRange = numVal.Random; //assign range variable to random value.

                if (tile[i][j] == TypeofTile.Floor && enemyRange <= 10) //if tiletype is floorSpawnerTiles and range is less than or equal too 10
                {
                    Vector3 enemyPos = new Vector3(i, j, 0);    //set enemy position
                    if (Vector3.Distance(playerpos, enemyPos) > spawnrange) // if enemy position is greater than spawn range
                    {
                        Instantiate(enemies, enemyPos, Quaternion.identity);    //instantiate enemy in pre-defined tile.
                    }
                }
            }
        }

    }

    void PlayerSpawner()
    {
        int playerS;
        if(spawned == false)
        {  
            for (int i = 0; i < tile.Length; i++)    //loop through all tiles.
            {           
                for (int j = 0; j < tile[i].Length; j++)
                {
                    if (tile[i][j] == TypeofTile.floorSpawner && spawned == false)
                    { //if tiletype is floordpawner and bool false

                        playerS = playerRange.Random; //player range set to random value

                        if (playerS <= 50 ) //if playerS is less than 75 assign playerposition and instantiate player and set bool to true.
                        {
                            Vector3 playerpos = new Vector3(i, j, 0);
                            Instantiate(player, playerpos, Quaternion.identity);
                            spawned = true;
                        }

                    }
                }
            }
        }
    }
    

}