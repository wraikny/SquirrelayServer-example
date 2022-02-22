using System;

using Altseed2;

using SquirrelayServer.Client;
using SquirrelayServer.Common;

namespace ExampleClient
{
    class Program
    {
        public static void Main()
        {
            // SquirrelayServer設定ファイルの読み込み
            const string configPath = @"config.json";
            var config = Config.LoadFromFile(configPath);

            if (config is null)
            {
                throw new Exception("failed to load the SquirrelayServer config file.");
            }

            // Altseed2初期化
            Engine.Initialize("test", 800, 600);

            // GameNode登録
            var gameNode = new GameNode();
            Engine.AddNode(gameNode);

            // Listener作成
            var listener = new EventBasedClientListener<PlayerStatus, RoomMessage, GameMessage>();
            listener.OnGameMessageReceived += gameNode.OnGameMessageReceived;

            // ClientNode作成
            var clientNode = new ClientNode(config.NetConfig, listener);
            Engine.AddNode(clientNode);

            gameNode.SendGameMessage += clientNode.Send;

            _ = clientNode.Start();

            while (Engine.DoEvents())
            {
                Engine.Update();
            }

            Engine.Terminate();
        }
    }
}
