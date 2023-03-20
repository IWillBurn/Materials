using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RoomService : MonoBehaviour
{

    [SerializeField] List<List<int>> room;
    [SerializeField] List<List<GameObject>> roomObjects;
    [SerializeField] GameObject block;
    [SerializeField] float blockSize;

    List<List<int>> GenerateRoom(int width, int height) {
        List<List<int>> generatedRoom = new List<List<int>>();
        for (int i = 0; i < width; i++) {
            List<int> line = new List<int>();
            for (int j = 0; j < height; j++)
            {
                line.Add(1);
            }
            generatedRoom.Add(line);
        }
        int type = Random.Range(0, 2);
        if (type == 0) {
            return generatedRoom;
        }
        if (type == 1)
        {
            int angle = Random.Range(0, 3);
            int sizeX = Random.Range(1, (int)width / 4);
            int sizeY = Random.Range(1, (int)height / 4);
            int startX = 0;
            int endX = 0;
            int startY = 0;
            int endY = 0;
            int deltaX = 0;
            int deltaY = 0;
            if (Random.Range(0, 2) == 0)
            {
                sizeX /= 2;
                sizeY /= 2;
                if (3 <= width - sizeX - 4) { deltaX = Random.Range(3, width - sizeX - 4); }
                if (4 <= height - sizeY - 5) { deltaY = Random.Range(4, height - sizeY - 5); }
            }
            else
            {
                if (Random.Range(0, 2) == 0)
                {
                    sizeX /= 2;
                    if (3 <= width - sizeX - 4) { deltaX = Random.Range(3, width - sizeX - 4); }
                }
                else
                {
                    sizeY /= 2;
                    if (4 <= height - sizeY - 5) { deltaY = Random.Range(4, height - sizeY - 5); }
                }
            }
            Debug.Log(deltaX.ToString() + " " + deltaY.ToString());
            if (angle == 0)
            {
                startX = 0 + deltaX;
                endX = sizeX + deltaX;
                startY = 0 + deltaY;
                endY = sizeY + deltaY;
            }
            if (angle == 1)
            {
                startX = width - 1 - sizeX - deltaX;
                endX = width - 1 - deltaX;
                startY = height - 1 - sizeY - deltaY;
                endY = height - 1 - deltaY;
            }
            if (angle == 2)
            {
                startX = 0 + deltaX;
                endX = sizeX + deltaX;
                startY = height - 1 - sizeY - deltaY;
                endY = height - 1 - deltaY;
            }
            if (angle == 3)
            {
                startX = width - 1 - sizeX - deltaX;
                endX = width - 1 - deltaX;
                startY = 0 + deltaY;
                endY = sizeY + deltaY;
            }

            for (int i = startX; i <= endX; i++)
            {
                for (int j = startY; j <= endY; j++)
                {
                    generatedRoom[i][j] = 0;
                }
            }
            return generatedRoom;
        }
        return generatedRoom;
    }

    List<List<int>> GenerateTestRoom() {
        List<List<int>> generatedRoom = new List<List<int>>();
        for (int i = 0; i < 14; i++)
        {
            List<int> line = new List<int>();
            for (int j = 0; j < 8; j++)
            {
                line.Add(1);
            }
            generatedRoom.Add(line);
        }

        generatedRoom[5][4] = 0;

        return generatedRoom;
    }

    bool OutOfRange(int x, int y, int sizeX, int sizeY) {
        return (x >= sizeX) || (y >= sizeY) || (x < 0) || (y < 0);
    }

    bool IsEmpty(List<List<int>> room, int x, int y, int sizeX, int sizeY)
    {
        return OutOfRange(x, y, sizeX, sizeY) || (room[x][y] == 0); 
    }

    bool HasEmpty(List<List<int>> room, int x, int y, int sizeX, int sizeY) 
    {
        return IsEmpty(room, x + 1, y, sizeX, sizeY) || IsEmpty(room, x, y + 1, sizeX, sizeY) || 
               IsEmpty(room, x - 1, y, sizeX, sizeY) || IsEmpty(room, x, y - 1, sizeX, sizeY) || 
               IsEmpty(room, x, y, sizeX, sizeY);
    }

    bool HasEmptyAround(List<List<int>> room, int x, int y, int sizeX, int sizeY)
    {
        return IsEmpty(room, x + 1, y, sizeX, sizeY) || IsEmpty(room, x, y + 1, sizeX, sizeY) ||
               IsEmpty(room, x - 1, y, sizeX, sizeY) || IsEmpty(room, x, y - 1, sizeX, sizeY) ||
               IsEmpty(room, x + 1, y + 1, sizeX, sizeY) || IsEmpty(room, x - 1, y - 1, sizeX, sizeY) ||
               IsEmpty(room, x - 1, y + 1, sizeX, sizeY) || IsEmpty(room, x + 1, y - 1, sizeX, sizeY) ||
               IsEmpty(room, x, y, sizeX, sizeY);
    }

    bool isUppest(List<List<int>> room, int x, int y, int sizeX, int sizeY, bool uppest) {
        bool wasEmpty = false;
        for (int i = y - 1; i >= 0; i--)
        {
            if (IsEmpty(room, x, i, sizeX, sizeY)) {
                wasEmpty = true;
            }
            if (wasEmpty && !IsEmpty(room, x, i, sizeX, sizeY)) {
                uppest = false;
                break;
            }
            if (!HasEmpty(room, x, i, sizeX, sizeY))
            {
                uppest = false;
                break;
            }
        }
        return uppest;
    }

    bool isLowest(List<List<int>> room, int x, int y, int sizeX, int sizeY, bool lowest)
    {
        for (int i = y + 1; i < sizeY; i++)
        {
            if (!HasEmpty(room, x, i, sizeX, sizeY))
            {
                lowest = false;
                break;
            }
        }
        return lowest;
    }


    bool CalculateBlockType(List<List<int>> room, int x, int y) {
        if (room[x][y] != 1) {
            return false;
        }
        int width = room.Count;
        int height = room[0].Count;
        int result = 0;
        bool emptyAround;
        bool uppest = false;
        bool lowest = false;
        bool up;
        bool down;
        bool right;
        bool left;

        emptyAround = HasEmptyAround(room, x, y, width, height);
        if (emptyAround) {
            uppest = true;
            lowest = true;
        }
        uppest = isUppest(room, x, y, width, height, uppest);
        lowest = isLowest(room, x, y, width, height, lowest);

        up = IsEmpty(room, x, y - 1, width, height);
        down = IsEmpty(room, x, y + 1, width, height);
        left = IsEmpty(room, x - 1, y, width, height);
        right = IsEmpty(room, x + 1, y, width, height);

        //Debug.Log("debug " + x.ToString() + " " + y.ToString() + " " + uppest.ToString() + " " + lowest.ToString() + " " + up.ToString() + " " + down.ToString() + " " + right.ToString() + " " + left.ToString());

        if (uppest && lowest) {
            if (down && right)
            {
                room[x][y] = 9;
                room[x][y - 1] = 22;
            }
            if (down && left)
            {
                room[x][y] = 11;
                room[x][y - 1] = 23;
            }
        }

        if (uppest)
        {
            if (up && left) {
                room[x][y] = 17;
                room[x][y + 1] = 25;
            }
            if (up && !right && !left) {
                room[x][y] = 18;
                room[x][y + 1] = 3;
            }
            if (up && right) {
                room[x][y] = 19;
                room[x][y + 1] = 20;
            }
            if (right && !up && !down) { room[x][y] = 21; }
            if (left && !up && !down) { room[x][y] = 24; }
            if (IsEmpty(room, x - 1, y - 1, width, height) && !down && !up && !left && !right){
                room[x][y] = 27;
                room[x][y + 1] = 4;
            }
            if (IsEmpty(room, x + 1, y - 1, width, height) && !down && !up && !left && !right)
            {
                room[x][y] = 26;
                room[x][y + 1] = 2;
            }
            return true;
        }
        if (lowest)
        {
            if (left && up) { room[x][y] = 5; }
            if (right && up) { room[x][y] = 7; }
            if (right && !down && !up) { room[x][y] = 8; }
            if (down && right) { room[x][y] = 9; }
            if (down && !right && !left) { room[x][y] = 10; }
            if (down && left) { room[x][y] = 11; }
            if (left && !down && !up) { room[x][y] = 12; }
            if (IsEmpty(room, x + 1, y + 1, width, height) && !down && !up && !left && !right) { room[x][y] = 13; }
            if (IsEmpty(room, x - 1, y + 1, width, height) && !down && !up && !left && !right) { room[x][y] = 14; }
            return true;
        }

        if (emptyAround && room[x][y] != 0) {
            if (!down && !up && !left && !right) {
                if (IsEmpty(room, x + 1, y + 1, width, height)) { room[x][y] = 13; }
                if (IsEmpty(room, x - 1, y + 1, width, height)) { room[x][y] = 14; }
                if (IsEmpty(room, x - 1, y - 1, width, height)) { room[x][y] = 15; }
                if (IsEmpty(room, x + 1, y - 1, width, height)) { room[x][y] = 16; }
            }
            if (right) { room[x][y] = 8; }
            if (down) { room[x][y] = 10; }
            if (left) { room[x][y] = 12; }
            if (up) { room[x][y] = 6; }
        }

        return true;
    }

    List<List<int>> UpdateRoom(List<List<int>> room)
    {
        int width = room.Count;
        int height = room[0].Count;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (room[i][j] != 0) { room[i][j] = 1; }
            }

        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                CalculateBlockType(room, i, j);
            }

        }
        return room;
    }

    List<List<GameObject>> RenderRoom(List<List<int>> room, float centerX, float centerY, float blockSize) {
        List<List<GameObject>> roomObjects = new List<List<GameObject>>();
        Vector3 position;
        float positionX;
        float positionY;
        int blockType;
        int width = room.Count;
        int height = room[0].Count;
        UpdateRoom(room);
        for (int i = 0; i < width; i++)
        {
            List<GameObject> line = new List<GameObject>();
            for (int j = 0; j < height; j++)
            {
                positionX = i * blockSize - (width - 1) / 2f * blockSize;
                positionY = - j * blockSize + (height - 1) / 2f * blockSize;
                position = new Vector3(positionX, positionY, 0);
                blockType = room[i][j];
                GameObject roomBlock = Instantiate(block, position, new Quaternion());
                RoomBlockParametrs roomBlockParametrs = roomBlock.GetComponent<RoomBlockParametrs>();
                roomBlockParametrs.setSpiteType(blockType);
                roomBlockParametrs.setPositionInRoom(i, j);
                line.Add(roomBlock);
            }
            roomObjects.Add(line);
        }
        return roomObjects;
    }

    void ClearRoom(List<List<GameObject>> roomObjects)
    {
        int width = roomObjects.Count;
        int height = roomObjects[0].Count;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Destroy(roomObjects[i][j]);
            }
        }
    }

    void Reroll() {
        ClearRoom(roomObjects);
        room = GenerateRoom(16, 12);
        roomObjects = RenderRoom(room, 0, 0, blockSize);
    }

    void Start()
    {
        Random.InitState(System.DateTime.Now.Second);
        room = GenerateRoom(16, 12);
        roomObjects = RenderRoom(room, 0, 0, blockSize);
    }

    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            Reroll();
        }

        if (Input.GetKeyDown("u"))
        {
            UpdateRoom(room);
            ClearRoom(roomObjects);
            roomObjects = RenderRoom(room, 0, 0, blockSize);
        }
    }
}
