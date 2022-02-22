using System;

using Altseed2;

namespace ExampleClient
{
    sealed class PlayerNode : TransformNode
    {
        bool isInitialized = false;
        Vector2F target;
        public Vector2F Target
        {
            get => target;
            set
            {
                if (!isInitialized)
                {
                    actualPosition = value;
                    Position = value;
                    isInitialized = true;
                }

                target = value;
            }
        }

        Vector2F actualPosition;

        const float speed = 20f;
        
        public PlayerNode()
        {
            var rectangle = new RectangleNode
            {
                RectangleSize = new Vector2F(100f, 100f),
                CenterPosition = new Vector2F(50f, 50f),
            };

            AddChildNode(rectangle);
        }

        public void Update(float deltaSecond)
        {
            var diff = Target - actualPosition;

            if (diff.Length > speed)
            {
                actualPosition += diff.Normal * speed;
            }
            else if (diff.Length > 0f)
            {
                actualPosition = target;
            }

            Position = actualPosition;
        }
    }
}