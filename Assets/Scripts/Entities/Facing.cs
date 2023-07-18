using UnityEngine;

public class Facing : MonoBehaviour
{
    public Vector2 Direction { get; private set; }

    protected SpriteRenderer _spriteRenderer { get; set; }
    [field: SerializeField] protected bool UsesLeftFacingSprite { get; set; }

    protected virtual void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual Vector2 GetNewFacingDirection()
    {
        return Vector2.right;
    }

    protected virtual void FlipSprite()
    {
        // Convention: sprites are drawn facing the right by default
        if (Direction.x < 0)
        {
            _spriteRenderer.flipX = UsesLeftFacingSprite ? false : true;
        }
        else if (Direction.x > 0)
        {
            _spriteRenderer.flipX = UsesLeftFacingSprite ? true : false;
        }
    }

    protected void Update()
    {
        Direction = GetNewFacingDirection();
        FlipSprite();
    }
}
