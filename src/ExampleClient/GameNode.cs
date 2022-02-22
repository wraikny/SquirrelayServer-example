using System;
using System.Collections.Generic;

using Altseed2;

namespace ExampleClient
{
    sealed class GameNode : Node
    {
        Dictionary<ulong, RectangleNode> players;

        public event Action<GameMessage> SendGameMessage = delegate { };

        public GameNode()
        {
            players = new Dictionary<ulong, RectangleNode>();
        }

        protected override void OnUpdate()
        {
            SendGameMessage(new GameMessage { Position = Engine.Mouse.Position });
        }

        public void OnGameMessageReceived(ulong id, float elapsedSeconds, GameMessage msg)
        {
            if (!players.TryGetValue(id, out var player))
            {
                player = new RectangleNode
                {
                    RectangleSize = new Vector2F(100f, 100f),
                    CenterPosition = new Vector2F(50f, 50f),
                };

                AddChildNode(player);

                players.Add(id, player);
            }

            player.Position = msg.Position;
        }
    }
}