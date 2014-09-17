using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LoLClient
{
    public enum PacketCmd : byte
    {
        PKT_KeyCheck = 0x00,

        PKT_C2S_InGame = 0x08,
        PKT_S2C_EndSpawn = 0x11,
        PKT_C2S_QueryStatusReq = 0x14,
        PKT_S2C_SkillUp = 0x15,
        PKT_C2S_Ping_Load_Info = 0x16,
        PKT_S2C_AutoAttack = 0x1A,

        PKT_S2C_FogUpdate2 = 0x23,
        PKT_S2C_PlayerInfo = 0x2A,

        PKT_S2C_ViewAns = 0x2C,
        PKT_C2S_ViewReq = 0x2E,

        PKT_C2S_SkillUp = 0x39,
        PKT_S2C_SpawnProjectile = 0x3B,
        PKT_S2C_AttentionPing = 0x40,

        PKT_S2C_Emotion = 0x42,
        PKT_C2S_unkstart = 0x47,
        PKT_C2S_Emotion = 0x48,
        PKT_S2C_HeroSpawn = 0x4C,
        PKT_S2C_Announce = 0x4D,

        PKT_C2S_StartGame = 0x52,
        PKT_S2C_SynchVersion = 0x54,
        PKT_C2S_ScoreBoard = 0x56,
        PKT_C2S_AttentionPing = 0x57,
        PKT_S2C_SpellSet = 0x5A,
        PKT_S2C_StartGame = 0x5C,

        PKT_S2C_MoveAns = 0x61,
        PKT_S2C_StartSpawn = 0x62,
        PKT_C2S_ClientReady = 0x64,
        PKT_S2C_LoadHero = 0x65,
        PKT_S2C_LoadName = 0x66,
        PKT_S2C_LoadScreenInfo = 0x67,
        PKT_ChatBoxMessage = 0x68,
        PKT_S2C_SetTarget = 0x6A,
        PKT_S2C_BuyItemAns = 0x6F,

        PKT_C2S_MoveReq = 0x72,
        PKT_C2S_MoveConfirm = 0x77,

        PKT_C2S_LockCamera = 0x81,
        PKT_C2S_BuyItemReq = 0x82,
        PKT_S2C_QueryStatusAns = 0x88,
        PKT_C2S_Exit = 0x8F,

        PKT_World_SendGameNumber = 0x92,
        PKT_S2C_Ping_Load_Info = 0x95,
        PKT_C2S_CastSpell = 0x9A,
        PKT_S2C_TurretSpawn = 0x9D,

        PKT_C2S_Surrender = 0xA4,
        PKT_C2S_StatsConfirm = 0xA8,
        PKT_S2C_SetHealth = 0xAE,
        PKT_C2S_Click = 0xAF,

        PKT_S2C_CastSpellAns = 0xB5,
        PKT_S2C_MinionSpawn = 0xBA,
        PKT_C2S_SynchVersion = 0xBD,
        PKT_C2S_CharLoaded = 0xBE,

        PKT_S2C_GameTimer = 0xC1,
        PKT_S2C_GameTimerUpdate = 0xC2,

        PKT_S2C_CharStats = 0xC4,
        PKT_S2C_LevelPropSpawn = 0xD0,


        PKT_Batch = 0xFF
    };
    enum Channel : byte
    {
        CHL_HANDSHAKE = 0,
        CHL_C2S = 1,
        CHL_GAMEPLAY = 2,
        CHL_S2C = 3,
        CHL_LOW_PRIORITY = 4,
        CHL_COMMUNICATION = 5,
        CHL_LOADING_SCREEN = 7,
    };
    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public struct KeyCheck
    {
        public Byte cmd;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public Byte[] partialKey;
        public UInt32 playerNo;
        public UInt64 userId;
        public UInt32 trash;
        public UInt64 checkId;
        public UInt32 trash2;
        public static KeyCheck Create(UInt64 userId, UInt64 checkId)
        {
            KeyCheck keyCheck;
            keyCheck.cmd = (Byte)PacketCmd.PKT_KeyCheck;
            keyCheck.userId = userId;
            keyCheck.checkId = checkId;
            keyCheck.playerNo = keyCheck.trash = keyCheck.trash2 = 0;
            keyCheck.partialKey = new byte[3] { 0xcc, 0xcc, 0xcc };
            return keyCheck;
        }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketHeader
    {
        public Byte cmd;
        public UInt32 netId;
        public static PacketHeader Create(PacketCmd cmd, UInt32 netId)
        {
            PacketHeader pktHeader;
            pktHeader.cmd = (Byte)cmd;
            pktHeader.netId = netId;
            return pktHeader;
        }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketHeader8
    {
        public PacketHeader header;
        public Byte unk;
        public static PacketHeader8 Create(PacketCmd cmd)
        {
            PacketHeader8 pkt;
            pkt.header = PacketHeader.Create(cmd, 0);
            pkt.unk = 1;
            return pkt;
        }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct StartGame
    {
        public PacketHeader header;
        public UInt64 unk1;
        public UInt64 unk2;
        public static StartGame Create()
        {
            StartGame pkt;
            pkt.header = PacketHeader.Create(PacketCmd.PKT_C2S_StartGame, 0);
            pkt.unk1 = pkt.unk2 = 0;
            return pkt;
        }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GameHeader
    {
        public PacketHeader header;
        public UInt32 tick;
        public static GameHeader Create(PacketCmd cmd, UInt32 netId, UInt32 tick)
        {
            GameHeader gameHeader = new GameHeader();
            gameHeader.header = PacketHeader.Create(cmd, netId);
            gameHeader.tick = tick;
            return gameHeader;
        }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GameTime
    {
        public PacketHeader header;
        public float tick1;
        public float tick2;
        public static GameTime Create(float tick)
        {
            GameTime gameHeader = new GameTime();
            gameHeader.header = PacketHeader.Create(PacketCmd.PKT_C2S_InGame, 0);
            gameHeader.tick1 = gameHeader.tick2 = tick;
            return gameHeader;
        }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GameTimer
    {
        public PacketHeader header;
        public float tick1;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SynchVersion
    {
        public PacketHeader header;
        public UInt32 unk1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public Byte[] version; //Dunno how big and when usefull data begins
        public static SynchVersion Create()
        {
            SynchVersion synch;
            int i = 0;
            synch.header = PacketHeader.Create(PacketCmd.PKT_C2S_SynchVersion, 0);
            synch.unk1 = 0;
            synch.version = new Byte[265]; synch.version[i++] = 0x56; synch.version[i++] = 0x65; synch.version[i++] = 0x72; synch.version[i++] = 0x73; synch.version[i++] = 0x69;
            synch.version[i++] = 0x6f; synch.version[i++] = 0x6e; synch.version[i++] = 0x20; synch.version[i++] = 0x34; synch.version[i++] = 0x2e;
            synch.version[i++] = 0x31; synch.version[i++] = 0x36; synch.version[i++] = 0x2e; synch.version[i++] = 0x30; synch.version[i++] = 0x2e;
            synch.version[i++] = 0x32; synch.version[i++] = 0x33; synch.version[i++] = 0x38; synch.version[i++] = 0x20; synch.version[i++] = 0x5b;
            synch.version[i++] = 0x50; synch.version[i++] = 0x55; synch.version[i++] = 0x42; synch.version[i++] = 0x4c; synch.version[i++] = 0x49;
            synch.version[i++] = 0x43; synch.version[i++] = 0x5d; synch.version[i++] = 0x00; synch.version[i++] = 0xfc; synch.version[i++] = 0x97;
            synch.version[i++] = 0x03; synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00;
            synch.version[i++] = 0x0f; synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0xc0; synch.version[i++] = 0x00;
            synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0xb8; synch.version[i++] = 0x59;
            synch.version[i++] = 0x66; synch.version[i++] = 0x05; synch.version[i++] = 0xc8; synch.version[i++] = 0x0e; synch.version[i++] = 0x02;
            synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00;
            synch.version[i++] = 0xb4; synch.version[i++] = 0xf6; synch.version[i++] = 0x22; synch.version[i++] = 0x00; synch.version[i++] = 0xcd;
            synch.version[i++] = 0x76; synch.version[i++] = 0x23; synch.version[i++] = 0x77; synch.version[i++] = 0x00; synch.version[i++] = 0x00;
            synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x2c; synch.version[i++] = 0xf7; synch.version[i++] = 0x22;
            synch.version[i++] = 0x00; synch.version[i++] = 0x82; synch.version[i++] = 0x2d; synch.version[i++] = 0x27; synch.version[i++] = 0x75;
            synch.version[i++] = 0xc8; synch.version[i++] = 0x0e; synch.version[i++] = 0x02; synch.version[i++] = 0x00; synch.version[i++] = 0x81;
            synch.version[i++] = 0x02; synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00;
            synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x0f; synch.version[i++] = 0x00; synch.version[i++] = 0x00;
            synch.version[i++] = 0xc0; synch.version[i++] = 0xc8; synch.version[i++] = 0x0e; synch.version[i++] = 0x02; synch.version[i++] = 0x00;
            synch.version[i++] = 0x45; synch.version[i++] = 0x0e; synch.version[i++] = 0xc8; synch.version[i++] = 0x09; synch.version[i++] = 0x00;
            synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x45; synch.version[i++] = 0x0e;
            synch.version[i++] = 0xc8; synch.version[i++] = 0x09; synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00;
            synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00;
            synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00;
            synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x0f; synch.version[i++] = 0x00;
            synch.version[i++] = 0x00; synch.version[i++] = 0xc0; synch.version[i++] = 0x34; synch.version[i++] = 0xf7; synch.version[i++] = 0x22;
            synch.version[i++] = 0x00; synch.version[i++] = 0x2f; synch.version[i++] = 0x26; synch.version[i++] = 0x27; synch.version[i++] = 0x75;
            synch.version[i++] = 0xb8; synch.version[i++] = 0x59; synch.version[i++] = 0x66; synch.version[i++] = 0x05; synch.version[i++] = 0x00;
            synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x98; synch.version[i++] = 0xf9;
            synch.version[i++] = 0x22; synch.version[i++] = 0x00; synch.version[i++] = 0x67; synch.version[i++] = 0xe3; synch.version[i++] = 0x23;
            synch.version[i++] = 0x77; synch.version[i++] = 0x6f; synch.version[i++] = 0xe3; synch.version[i++] = 0x23; synch.version[i++] = 0x77;
            synch.version[i++] = 0xc8; synch.version[i++] = 0x0e; synch.version[i++] = 0x02; synch.version[i++] = 0x00; synch.version[i++] = 0x45;
            synch.version[i++] = 0x0e; synch.version[i++] = 0xc8; synch.version[i++] = 0x09; synch.version[i++] = 0x45; synch.version[i++] = 0x0e;
            synch.version[i++] = 0xc8; synch.version[i++] = 0x09; synch.version[i++] = 0xc8; synch.version[i++] = 0x0e; synch.version[i++] = 0x02;
            synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00;
            synch.version[i++] = 0xaf; synch.version[i++] = 0xdc; synch.version[i++] = 0x23; synch.version[i++] = 0x77; synch.version[i++] = 0x18;
            synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x87; synch.version[i++] = 0x02;
            synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0xc2; synch.version[i++] = 0xdc; synch.version[i++] = 0x23;
            synch.version[i++] = 0x77; synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00;
            synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x38;
            synch.version[i++] = 0x8f; synch.version[i++] = 0x37; synch.version[i++] = 0x05; synch.version[i++] = 0x50; synch.version[i++] = 0xfa;
            synch.version[i++] = 0x97; synch.version[i++] = 0x03; synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00;
            synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0xfc; synch.version[i++] = 0x97; synch.version[i++] = 0x03;
            synch.version[i++] = 0xbc; synch.version[i++] = 0x02; synch.version[i++] = 0x18; synch.version[i++] = 0x00; synch.version[i++] = 0x00;
            synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0xfc;
            synch.version[i++] = 0x97; synch.version[i++] = 0x03; synch.version[i++] = 0xc8; synch.version[i++] = 0xf7; synch.version[i++] = 0x22;
            synch.version[i++] = 0x00; synch.version[i++] = 0x14; synch.version[i++] = 0xa9; synch.version[i++] = 0x23; synch.version[i++] = 0x77;
            synch.version[i++] = 0x50; synch.version[i++] = 0xfa; synch.version[i++] = 0x97; synch.version[i++] = 0x03; synch.version[i++] = 0x00;
            synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0xfc;
            synch.version[i++] = 0x97; synch.version[i++] = 0x03; synch.version[i++] = 0x00; synch.version[i++] = 0x00; synch.version[i++] = 0x00;
            synch.version[i++] = 0x00; synch.version[i++] = 0xb7; synch.version[i++] = 0xfd; synch.version[i++] = 0x76; synch.version[i++] = 0xa7;
            synch.version[i++] = 0xc8; synch.version[i++] = 0xfd; synch.version[i++] = 0xfd; synch.version[i++] = 0xfd; synch.version[i++] = 0xfd;
            synch.version[i++] = 0xab; synch.version[i++] = 0xab; synch.version[i++] = 0xab; synch.version[i++] = 0xab; synch.version[i++] = 0xab;
            return synch;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SynchBlock
    {
        UInt64 userId;
        UInt16 netId;
        UInt32 skill1;
        UInt32 skill2;
        Byte flag;
        UInt32 teamId;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        Byte[] data1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        Byte[] data2;
        UInt32 unk2;
        UInt32 unk3;
        public static SynchBlock Create()
        {
            SynchBlock synch = new SynchBlock();
            synch.userId = 0xFFFFFFFFFFFFFFFF;
            synch.netId = 0x1E;
            synch.teamId = 0x64;
            synch.flag = 0; //1 for bot?
            synch.unk3 = 0x19;
            return synch;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SynchVersionAns
    {
        public PacketHeader header;
        public Byte ok;
        public UInt32 mapId;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public SynchBlock[] players;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 27)]
        public Byte[] version;      //Ending zero so size 26+0x00
        public Byte ok2;              //1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 228)]
        public Byte[] unknown;     //Really strange shit
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public Byte[] gameMode;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2232)]
        public Byte[] zero;
        public UInt16 end1;            //0xE2E0
        public Byte end2;             //0xA0 || 0x08
        public static SynchVersionAns Create()
        {
            SynchVersionAns synch = new SynchVersionAns();
            synch.header = PacketHeader.Create(PacketCmd.PKT_S2C_SynchVersion, 0);
            synch.ok = synch.ok2 = 1;
            /*memcpy(version, "Version 4.5.0.264 [PUBLIC]", 27);
            memcpy(gameMode, "CLASSIC", 8);
            memset(zero, 0, 2232);*/
            /*synch.version = new Byte[27];
            synch.unknown = new Byte[228];
            synch.gameMode = new Byte[8];
            synch.zero = new Byte[2232];*/
            synch.end1 = 0xE2E0;
            synch.end2 = 0xA0;
            return synch;
        }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ClientReady
    {
        public PacketHeader header;
        public UInt32 teamId;
        public static ClientReady Create()
        {
            ClientReady client = new ClientReady();
            client.header = PacketHeader.Create(PacketCmd.PKT_C2S_ClientReady, 0);
            client.teamId = client.header.cmd;
            return client;
        }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LoadScreenInfo
    {

        public Byte cmd;
        public UInt32 blueMax;
        public UInt32 redMax;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public UInt64[] bluePlayerIds; //Team 1, 6 players max
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 144)]
        public Byte[] blueData;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public UInt64[] redPlayersIds; //Team 2, 6 players max
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 144)]
        public Byte[] redData;
        public UInt32 bluePlayerNo;
        public UInt32 redPlayerNo;
        public static LoadScreenInfo Create()
        {
            LoadScreenInfo loadScreenInfo = new LoadScreenInfo();
            loadScreenInfo.cmd = (Byte)PacketCmd.PKT_S2C_LoadScreenInfo;
            return loadScreenInfo;
        }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LoadScreenPlayer
    {
        public Byte cmd;
        public UInt64 userId;
        public UInt32 skinId;
        public UInt32 length;
        public Byte description;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PingLoadInfo
    {
        public PacketHeader header;
        public UInt32 unk1;
        public UInt64 userId;//all F's?
        public float loaded;
        public float ping;
        public UInt32 unk2;
        public static PingLoadInfo Create()
        {
            PingLoadInfo ping = new PingLoadInfo();
            ping.header = PacketHeader.Create(PacketCmd.PKT_C2S_Ping_Load_Info, 0);
            return ping;
        }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MoveConfirm
    {
        public GameHeader header;
        public Byte zero;
        public static MoveConfirm Create(UInt32 tick)
        {
            MoveConfirm move = new MoveConfirm();
            move.header = GameHeader.Create(PacketCmd.PKT_C2S_MoveConfirm, 0, tick);
            return move;
        }
    }
    enum MoveType : byte
    {
        EMOTE = 1,
        MOVE = 2,
        ATK_MOVE = 7,
        STOP = 10,
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MovementReq
    {
        PacketHeader header;
        MoveType type;
        float x;
        float y;
        UInt32 zero;
        Byte vectorNo;
        UInt32 netId;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        Byte[] moveData;
        public static MovementReq Create(float x, float y, UInt32 netId)
        {
            MovementReq move = new MovementReq();
            move.header = PacketHeader.Create(PacketCmd.PKT_C2S_MoveReq, netId);
            move.x = x;
            move.y = y;
            move.type = MoveType.ATK_MOVE;
            move.vectorNo = 2;
            Byte[] xbytes = BitConverter.GetBytes(x);
            Byte[] ybytes = BitConverter.GetBytes(y);
            move.moveData = new Byte[7]{
                xbytes[0], xbytes[1],
                ybytes[0], ybytes[1],
                0xff, 0xff, 0xff
            };
            return move;
        }
    }
    public class Update
    {
        public class Unit
        {
            public class Mask
            {
                public UInt32 subMask;
                public Byte sectionLen;
                public List<float> values;
                public Mask() { values = new List<float>(); }
            }
            public Byte masterMask;
            public float netId;
            public List<Mask> masks;
            public Unit() { masks = new List<Mask>(); }
        }
        GameHeader header;
        Byte updateCount;
        List<Unit> units;
        public Update(byte[] data)
        {
            List<List<String>> masterMask = new List<List<String>>();
            for (int i = 0; i < 6; i++)
            {
                List<String> mask = new List<String>();
                for (int j = 0; j < 32; j++)
                {
                    mask.Add("null");
                }
                masterMask.Add(mask);
            }
            masterMask[2][7] = "duration";
            masterMask[2][12] = "mp5";
            masterMask[4][1] = "hp";
            masterMask[4][2] = "mp";
            masterMask[4][5] = "xp";
            int offset = 9;
            updateCount = data[offset++];
            units = new List<Unit>();
            for (int i = 0; i < updateCount; i++)
            {
                Unit unit = new Unit();
                unit.masterMask = data[offset++];
                //unit.netId = BitConverter.ToSingle(new byte[4] { data[offset++], data[offset++], data[offset++], data[offset++] }, 0);
                String netId2 = BitConverter.ToString(new byte[4] { data[offset++], data[offset++], data[offset++], data[offset++] }, 0);
                UInt32 temp = unit.masterMask;
                UInt32 temp2 = temp;
                int count = 0;
                while (temp > 0){
                    if ((temp & 1) == 1)
                    {
                        temp2 -= temp;
                        int startPos = offset;
                        Unit.Mask subMask = new Unit.Mask();
                        subMask.subMask = BitConverter.ToUInt32(new byte[4] { data[offset++], data[offset++], data[offset++], data[offset++] }, 0);
                        subMask.sectionLen = data[offset++];
                        UInt32 temp3 = subMask.subMask;
                        int count2 = 0;
                        while (temp3 > 0)
                        {
                            if ((temp3 & 1) == 1)
                            {
                                try
                                {
                                    float val = BitConverter.ToSingle(new byte[4] { data[offset++], data[offset++], data[offset++], data[offset++] }, 0);
                                    subMask.values.Add(val);
                                }
                                catch { }
                            }
                            count2++;
                            temp3 >>= 1;
                            if (offset >= startPos + 5 + subMask.sectionLen)
                                break;
                        }
                        offset = startPos + 5 + subMask.sectionLen;
                    }
                    count++;
                    temp >>= 1;
                }
            }
        }
    }
    public class Waypoints
    {
        public class Unit
        {
            public Byte waypointCount;
            public UInt32 netId;
            public List<Byte> bitMasks;
            public System.Drawing.Point waypoint;
            public Unit() { bitMasks = new List<Byte>(); }
        }
        GameHeader header;
        UInt16 unitCount;
        List<Unit> units;
        public Waypoints(byte[] data)
        {
            int offset = 9;
            unitCount = BitConverter.ToUInt16(new Byte[2] { data[offset++], data[offset++] }, 0);
            units = new List<Unit>();
            for (int i = 0; i < unitCount; i++)
            {
                Unit unit = new Unit();
                unit.waypointCount = (byte)(data[offset++] >> 1);
                unit.netId = BitConverter.ToUInt32(new Byte[4] { data[offset++], data[offset++], data[offset++], data[offset++] }, 0);
                List<Byte> modifierBits = new List<Byte>();
                for (int j = 0; j < Math.Ceiling((double)(unit.waypointCount - 1) / 4); j++)
                {
                    Byte bitMask = data[offset++];
                    unit.bitMasks.Add(bitMask);
                    for (int k = 0; k < 8; k++)
                    {
                        modifierBits.Add((byte)(bitMask & 1));
                        bitMask >>= 1;
                    }
                }
                for (int j = 0; j < unit.waypointCount; j++)
                {
                    Byte popX = modifierBits[modifierBits.Count - 1];
                    modifierBits.RemoveAt(modifierBits.Count - 1);
                    Byte popY = modifierBits[modifierBits.Count - 1];
                    modifierBits.RemoveAt(modifierBits.Count - 1);
                    try
                    {
                        Byte[] x;
                        if (popX == 1)
                            x = new Byte[4] { data[offset++], 0, 0, 0 };
                        else
                            x = new Byte[4] { data[offset++], data[offset++], 0, 0 };
                        Byte[] y;
                        if (popY == 1)
                            y = new Byte[4] { data[offset++], 0, 0, 0 };
                        else
                            y = new Byte[4] { data[offset++], data[offset++], 0, 0 };
                        unit.waypoint = new System.Drawing.Point(BitConverter.ToInt32(x, 0), BitConverter.ToInt32(y, 0));
                    }
                    catch { }
                }
            }
        }
    }
}
