using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBlockParametrs : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites;
    [SerializeField] int spriteType;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Vector2 positionInRoom;

    public void setSpiteType(int _spriteType) {
        spriteType = _spriteType;
        spriteRenderer.sprite = sprites[spriteType];
    }

    public void setPositionInRoom(int x, int y)
    {
        positionInRoom.x = x;
        positionInRoom.y = y;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
