using DataModel.StaticData.Component;
using System.Numerics;

namespace DataModel.Components.Implementations
{
    public sealed class TranssformComponent : Component<ITransformComponentStaticData>, ITransformComponent
    {
        public Vector2 Position { get; set; }

        private float rotation;
        public float Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                if (rotation != value)
                {
                    rotation = value;
                    RecalculateOrientation();
                }
            }
        }

        private void RecalculateOrientation()
        {
            Forward = RotateVector(Vector2.UnitY, Rotation);
            Back = RotateVector(-Vector2.UnitY, Rotation);
            Right = RotateVector(Vector2.UnitX, Rotation);
            Left = RotateVector(-Vector2.UnitX, Rotation);
        }

        private Vector2 RotateVector(Vector2 vector, float rotation)
        {
            float sin = (float)System.Math.Sin(rotation);
            float cos = (float)System.Math.Cos(rotation);

            float tx = vector.X;
            float ty = vector.Y;
            return new Vector2((cos * tx) + (ty * sin), (cos * ty) - (sin * tx));
        }

        public Vector2 Forward { get; private set; } = Vector2.UnitY;
        public Vector2 Back { get; private set; } = -Vector2.UnitY;
        public Vector2 Right { get; private set; } = Vector2.UnitX;
        public Vector2 Left { get; private set; } = -Vector2.UnitX;
    }
}
