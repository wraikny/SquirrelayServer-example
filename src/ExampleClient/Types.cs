using System;

using Altseed2;

using MessagePack;

namespace ExampleClient
{
    [MessagePackObject]
    public sealed class PlayerStatus
    {

    }

    [MessagePackObject]
    public sealed class RoomMessage
    {

    }

    [MessagePackObject]
    public sealed class GameMessage
    {
        [Key(0)]
        public Vector2F Position { get; set; }
    }
}
