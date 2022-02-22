using System;
using System.Collections.Generic;

using Altseed2;

namespace ExampleClient
{
    sealed class GameNode : Node
    {
        Dictionary<ulong, PlayerNode> players;

        IMessenger<GameMessage> messenger;

        public GameNode(IMessenger<GameMessage> messenger)
        {
            players = new Dictionary<ulong, PlayerNode>();

            this.messenger = messenger;
        }

        protected override void OnUpdate()
        {
            if (Engine.Mouse.GetMouseButtonState(MouseButton.ButtonLeft) == ButtonState.Push)
            {
                messenger.Send(new GameMessage { Position = Engine.Mouse.Position });
            }
        }

        // `通常はOnUpdate`で記述する「毎フレームの処理」も、サーバーからのTickメッセージで行う。
        float lastElapsedSeconds = 0f;
        public void OnTicked(float elapsedSeconds)
        {
            var deltaSecond = elapsedSeconds - lastElapsedSeconds;

            foreach (var kvp in players)
            {
                kvp.Value.Update(deltaSecond);
            }
        }

        // `GameMessage`（プレイヤーの入力）を反映する
        public void OnGameMessageReceived(ulong id, float elapsedSeconds, GameMessage msg)
        {
            if (players.TryGetValue(id, out var player))
            {
                player.Target = msg.Position;
            }
        }

        public void OnPlayerEntered(ulong id, PlayerStatus? _status)
        {
            Console.WriteLine($"Entered {id}");
            var player = new PlayerNode();
            AddChildNode(player);
            players.Add(id, player);
        }

        public void OnPlayerExited(ulong id)
        {
            var player = players[id];
            RemoveChildNode(player);
            players.Remove(id);
        }
    }
}