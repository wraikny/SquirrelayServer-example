using System;
using System.Threading.Tasks;

using Altseed2;

using SquirrelayServer.Client;
using SquirrelayServer.Common;

namespace ExampleClient
{
    class Program
    {
        public static void Main()
        {
            // Altseed2初期化
            Engine.Initialize("test", 800, 600);

            // SquirrelayServer設定ファイルの読み込み
            const string configPath = @"config.json";
            var config = Config.LoadFromFile(configPath);

            if (config is null)
            {
                throw new Exception("failed to load the SquirrelayServer config file.");
            }

            // Listener作成
            var listener = new EventBasedClientListener<PlayerStatus, RoomMessage, GameMessage>();
            

            // ClientNode作成
            var clientNode = new ClientNode(config.NetConfig, listener);
            Engine.AddNode(clientNode);

            _ = clientNode.Start().ContinueWith(_task => {
                // GameNode登録
                var gameNode = new GameNode(clientNode);
                Engine.AddNode(gameNode);

                gameNode.OnPlayerEntered(clientNode.Client.Id.Value, null);

                var currentRoom = clientNode.Client.CurrentRoom;
                foreach(var kvp in currentRoom.PlayerStatuses)
                {
                    Console.WriteLine(kvp.Key);
                    gameNode.OnPlayerEntered(kvp.Key, kvp.Value);
                }
                
                listener.OnGameMessageReceived += gameNode.OnGameMessageReceived;
                listener.OnTicked += gameNode.OnTicked;
                listener.OnPlayerEntered += gameNode.OnPlayerEntered;
                listener.OnPlayerExited += gameNode.OnPlayerExited;
            });

            while (Engine.DoEvents())
            {
                Engine.Update();
            }

            Engine.Terminate();
        }
    }
}
