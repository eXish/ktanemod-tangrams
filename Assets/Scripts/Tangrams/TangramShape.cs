using UnityEngine;

public enum TangramShape
{
    LargeTriangle,
    MediumTriangle,
    SmallTriangle,
    Square,
    Parallelogram
}

public static class TangramShapeExtensions
{
    public static Vector2[] GetShape(this TangramShape shape)
    {
        switch (shape)
        {
            case TangramShape.LargeTriangle:
                return new Vector2[]
                {
                    new Vector2(0.0f, 0.0f),
                    new Vector2(0.5f, 0.5f),
                    new Vector2(0.0f, 1.0f),
                };

            case TangramShape.MediumTriangle:
                return new Vector2[]
                {
                    new Vector2(0.0f, 0.0f),
                    new Vector2(0.5f, 0.0f),
                    new Vector2(0.5f, 0.5f),
                };

            case TangramShape.SmallTriangle:
                return new Vector2[]
                {
                    new Vector2(0.0f, 0.0f),
                    new Vector2(0.25f, 0.25f),
                    new Vector2(0.0f, 0.5f),
                };

            case TangramShape.Square:
                return new Vector2[]
                {
                    new Vector2(0.25f, 0.0f),
                    new Vector2(0.5f, 0.25f),
                    new Vector2(0.25f, 0.5f),
                    new Vector2(0.0f, 0.25f),
                };

            case TangramShape.Parallelogram:
                return new Vector2[]
                {
                    new Vector2(0.75f, 0.0f),
                    new Vector2(0.5f, 0.25f),
                    new Vector2(0.0f, 0.25f),
                    new Vector2(0.25f, 0.0f),
                };

            default:
                return null;
        }
    }

    public static Vector2[] GetConnectionPoints(this TangramShape shape)
    {
        switch (shape)
        {
            case TangramShape.LargeTriangle:
                return new Vector2[]
                {
                    new Vector2(0.125f, 0.125f),
                    new Vector2(0.375f, 0.375f),
                    new Vector2(0.375f, 0.625f),
                    new Vector2(0.125f, 0.875f),
                    new Vector2(0.0f, 0.875f),
                    new Vector2(0.0f, 0.625f),
                    new Vector2(0.0f, 0.375f),
                    new Vector2(0.0f, 0.125f),
                };

            case TangramShape.MediumTriangle:
                return new Vector2[]
                {
                    new Vector2(0.125f, 0.0f),
                    new Vector2(0.375f, 0.0f),
                    new Vector2(0.5f, 0.125f),
                    new Vector2(0.5f, 0.375f),
                    new Vector2(0.375f, 0.375f),
                    new Vector2(0.125f, 0.125f),
                };

            case TangramShape.SmallTriangle:
                return new Vector2[]
                {
                    new Vector2(0.125f, 0.125f),
                    new Vector2(0.375f, 0.375f),
                    new Vector2(0.0f, 0.375f),
                    new Vector2(0.0f, 0.125f),
                };

            case TangramShape.Square:
                return new Vector2[]
                {
                    new Vector2(0.375f, 0.125f),
                    new Vector2(0.375f, 0.375f),
                    new Vector2(0.125f, 0.375f),
                    new Vector2(0.125f, 0.125f),
                };

            case TangramShape.Parallelogram:
                return new Vector2[]
                {
                    new Vector2(0.625f, 0.125f),
                    new Vector2(0.375f, 0.25f),
                    new Vector2(0.125f, 0.25f),
                    new Vector2(0.125f, 0.125f),
                    new Vector2(0.375f, 0.0f),
                    new Vector2(0.625f, 0.0f),
                };

            default:
                return null;
        }
    }
}
