public enum PacketType
{
    UNKNOWN = 0x00,

    Ping = 0x01,
    SendMessage = 0x04,
    GetMessages = 0x05,
    UpdatePubKey = 0x06,
    GetPubKey = 0x07,
    GetMessageCount = 0x08,
    ResolveAlias = 0x09,

    Pong = 0x10,
    Timeout = 0x20,
    ACK = 0x30,
    MessageSent = 0x40,
    Rejected = 0x50,
    GetMessagesReponse = 0x60,
    GetPubKeyResponse = 0x70,
    GetMessageCountResponse = 0x80,
    ResolveAliasResponse = 0x90,

    ERROR = 0xFF
}