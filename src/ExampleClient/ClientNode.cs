using System;
using System.Linq;
using System.Threading.Tasks;

using Altseed2;

using MessagePack;
using MessagePack.Resolvers;
using MessagePack.Altseed2;

using SquirrelayServer.Client;
using SquirrelayServer.Common;

namespace ExampleClient
{
    sealed class ClientNode : Node
    {
        Client<PlayerStatus, RoomMessage, GameMessage> client;

        public Client<PlayerStatus, RoomMessage, GameMessage> Client => client;


        public ClientNode(NetConfig netConfig, IClientListener<PlayerStatus, RoomMessage, GameMessage> listener)
        {
            var serverOpt = Options.DefaultOptions;

            var resolver = CompositeResolver.Create(
                Altseed2Resolver.Instance,
                StandardResolver.Instance
            );

            var clientsOpt = MessagePackSerializerOptions.Standard.WithResolver(resolver);

            client = new Client<PlayerStatus, RoomMessage, GameMessage>(netConfig, serverOpt, clientsOpt, listener);
        }

        protected override void OnUpdate()
        {
            // クライアント更新
            client.Update();
        }
        
        public async Task Start()
        {
#if DEBUG
            const string host = @"localhost";
#else
            const string host = @"example.com";
#endif
            if (!await client.Start(host))
            {
                // on failed to connect
                return;
            }

            // 普通のゲームならルーム選択UIなど出したい。
            await EnterRoom();
        }

        public async Task EnterRoom()
        {
            var roomList = await client.RequestGetRoomListAsync();

            if (roomList.Count == 0)
            {
                // ルーム作成
                var createRoomResp = await client.RequestCreateRoomAsync();

                // ゲームスタート
                var gameStartResp = await client.RequestStartPlayingAsync();
            }
            else
            {
                // ルーム入室
                var enterRoomResp = await client.RequestEnterRoomAsync(roomList.First().Id);
            }
        }
    }
}