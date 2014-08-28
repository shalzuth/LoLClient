using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ENet;

namespace LoLClient
{
    public partial class Form1 : Form
    {
        private BlowFish blowfish;
        private Peer server;
        private Host client;
        private string IPAddr;
        private ushort port;
        private string key;
        private UInt64 playerId;
        private UInt32 netId = 0;
        private Boolean pastLoadScreen = false;
        private Boolean startedGame = false;
        private uint comp = 2;
        private int k = 0;
        bool first = false;
        bool logPackets = false;

        public Form1(string[] args)
        {
            InitializeComponent();
            //args[0] = "192.64.170.20 5108 LuPEhb0bZ95QqcqwsbF6Ag== 50473085";
            String[] arguments = args[0].Split(' ');
            IPAddr = arguments[0];
            port = Convert.ToUInt16(arguments[1]);
            key = arguments[2];
            playerId = Convert.ToUInt64(arguments[3]);
            /*
            l = new Lua();
            l.RegisterFunction("dostuff", this, this.GetType().GetMethod("dostuff"));
            l.DoFile("test.lua");*/
            //string hex = "C4-00-00-00-00-10-00-00-00-11-22-B5-7D-F9-FF-03-00-00-00-05-00-E0-AB-45-01-03-00-00-00-05-00-80-80-80-0C-22-0F-2C-F0-FF-03-00-00-00-05-00-E0-AB-45-01-03-00-00-00-05-00-80-80-80-0C-22-E1-03-93-FF-03-00-00-00-05-00-00-7A-45-01-03-00-00-00-05-00-80-80-80-0C-22-F1-20-4A-FF-03-00-00-00-05-00-00-7A-45-01-03-00-00-00-05-00-80-80-80-0C-22-3E-3C-D2-FF-03-00-00-00-05-00-00-7A-45-01-03-00-00-00-05-00-80-80-80-0C-22-1F-8F-FF-FF-03-00-00-00-05-00-00-7A-45-01-03-00-00-00-05-00-80-80-80-0C-22-0F-AC-26-FF-03-00-00-00-05-00-00-7A-45-01-03-00-00-00-05-00-80-80-80-0C-22-D0-93-67-FF-03-00-00-00-05-00-00-7A-45-01-03-00-00-00-05-00-80-80-80-0C-22-36-B8-53-FF-03-00-00-00-05-20-BC-BE-4C-01-03-00-00-00-05-00-80-80-80-0C-22-E8-00-BA-FF-03-00-00-00-05-20-BC-BE-4C-01-03-00-00-00-05-00-80-80-80-0C-22-D5-47-E6-FF-03-00-00-00-05-20-BC-BE-4C-01-03-00-00-00-05-00-80-80-80-0C-22-71-71-B7-FF-03-00-00-00-05-20-BC-BE-4C-01-03-00-00-00-05-00-80-80-80-0C-22-4C-36-EB-FF-03-00-00-00-05-20-BC-BE-4C-01-03-00-00-00-05-00-80-80-80-0C-22-AF-C9-5E-FF-03-00-00-00-05-20-BC-BE-4C-01-03-00-00-00-05-00-80-80-80-0C-2A-01-00-00-40-FF-7F-00-00-18-FF-FF-87-80-88-02-00-01-00-01-00-00-C8-41-FF-FF-00-00-80-3F-FF-FF-FF-FF-3F-00-00-00-0F-00-80-A2-44-00-80-A2-44-FF-FF-FF-00-00-80-3F-03-00-00-00-05-00-80-80-80-0C-2A-02-00-00-40-FF-7F-00-00-18-FF-FF-87-80-88-02-00-01-00-01-00-00-C8-41-FF-FF-00-00-80-3F-FF-FF-FF-FF-3F-00-00-00-0F-00-80-A2-44-00-80-A2-44-FF-FF-FF-00-00-80-3F-03-00-00-00-05-00-80-80-80-0C-2A-03-00-00-40-FF-7F-00-00-18-FF-FF-87-80-88-02-00-01-00-01-00-00-C8-41-FF-FF-00-00-80-3F-FF-FF-FF-FF-3F-00-00-00-0F-00-80-A2-44-00-80-A2-44-FF-FF-FF-00-00-80-3F-03-00-00-00-05-00-80-80-80-0C";
            //string hex = "61-00-00-00-00-F7-02-00-00-01-00-04-19-00-00-40-01-36-F2-9D-F2-20-15-F2";
            //Update up = new Update(bytes);
            //Waypoints wp = new Waypoints(bytes);
        }
        public static void dostuff()
        {
            Console.WriteLine("rawrd!");
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
            //HandleMoveReq(ConvertStringToBytes("61-00-00-00-00-46-10-00-00-01-00-04-1A-00-00-40-00-BE-F3-2E-F3-47-F6-CC-F4"));
            //HandleMoveReq(ConvertStringToBytes("61-00-00-00-00-DD-11-00-00-01-00-02-1A-00-00-40-05-F5-FF-F3"));
        }
        private byte[] ConvertStringToBytes(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new Byte[NumberChars / 2];
            hex = hex.Replace("-", "");
            NumberChars = hex.Length;
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
        private void Log(string str)
        {
            Console.WriteLine(str);
            richTextBox1.Invoke(new MethodInvoker(delegate()
            {
                richTextBox1.Text = DateTime.Now + " : " + str + "\n" + richTextBox1.Text;
            }));
        }
        private Boolean SendPacket(byte[] data, Channel channelNum)
        {
            String val = ((PacketCmd)data[0]).ToString();
            if (logPackets)
            {
                if (val.All(c => char.IsDigit(c)))
                    Log("send : " + String.Format("{0:X}", data[0]));
                else
                    Log("send : " + ((PacketCmd)data[0]).ToString());
            }
            if (data.Length >= 8 && data[0] != (byte)PacketCmd.PKT_KeyCheck)
            {
                data = blowfish.Encrypt_ECB(data);
            }
            return server.Send((byte)channelNum, data);
        }
        private void HandleEncryptedPacket(Byte[] data, int channel)
        {
            if (data.Length >= 8 && (data[0] != (byte)PacketCmd.PKT_KeyCheck || startedGame))
                data = blowfish.Decrypt_ECB(data);
            HandlePacket(data, channel);
        }
        private void HandlePacket(Byte[] data, int channel)
        {
            //l["packetdatawrapper"] = data.ToList();//BitConverter.ToString(data);
            //l.DoString("OnRecvPacket(packetdatawrapper)");
            String val = ((PacketCmd)data[0]).ToString();
            if (logPackets)
            {
                if (val.All(c => char.IsDigit(c)))
                    Log("recv : " + String.Format("{0:X}", data[0]));
                else
                    Log("recv : " + ((PacketCmd)data[0]).ToString());
            }
            switch ((PacketCmd)data[0])
            {
                case PacketCmd.PKT_Batch: HandleBatchPacket(data); break;
                case PacketCmd.PKT_KeyCheck: HandleKeyCheck(data); break;
                case PacketCmd.PKT_World_SendGameNumber: HandleGameNumber(data); break;
                case PacketCmd.PKT_S2C_QueryStatusAns: HandleQuery(data); break;
                case PacketCmd.PKT_S2C_SynchVersion: HandleSynch(data); break;
                case PacketCmd.PKT_S2C_LoadScreenInfo: HandleScreenInfo(data); break;
                case PacketCmd.PKT_S2C_LoadName: HandleName(data); break;
                case PacketCmd.PKT_S2C_LoadHero: HandleHero(data); break;
                case PacketCmd.PKT_S2C_Ping_Load_Info: HandlePing(data); break;
                case PacketCmd.PKT_S2C_StartSpawn: HandleStartSpawn(data); break;
                case PacketCmd.PKT_S2C_HeroSpawn: HandleHeroSpawn(data); break;
                case PacketCmd.PKT_S2C_MinionSpawn: HandleMinionSpawn(data); break;
                case PacketCmd.PKT_S2C_SetHealth: HandleSetHealth(data); break;
                case PacketCmd.PKT_S2C_TurretSpawn: HandleTurretSpawn(data); break;
                case PacketCmd.PKT_S2C_LevelPropSpawn: HandleLevelPropSpawn(data); break;
                case PacketCmd.PKT_S2C_EndSpawn: HandleEndSpawn(data); break;
                case PacketCmd.PKT_S2C_BuyItemAns: HandleBuyItem(data); break;
                case PacketCmd.PKT_S2C_GameTimer: HandleGameTimer(data); break;
                case PacketCmd.PKT_S2C_GameTimerUpdate: HandleGameTimerUpdate(data); break;
                case PacketCmd.PKT_S2C_StartGame: HandleGameStart(data); break;
                case PacketCmd.PKT_S2C_CharStats: HandleUpdateUnit(data); break;
                case PacketCmd.PKT_S2C_MoveAns: HandleMoveReq(data); break;
                    /*case (PacketCmd)0x10:
                    case PacketCmd.PKT_S2C_SkillUp:
                    case (PacketCmd)0x17:
                    case PacketCmd.PKT_S2C_AutoAttack:
                    case (PacketCmd)0x1c:
                    case (PacketCmd)0x1f:
                    case PacketCmd.PKT_S2C_FogUpdate2:
                    case PacketCmd.PKT_S2C_PlayerInfo:
        registerHandler(&PacketHandler::HandleNull,            (PacketCmd)0x34, CHL_S2C);
        registerHandler(&PacketHandler::HandleNull,            (PacketCmd)0x45, CHL_S2C);
        registerHandler(&PacketHandler::HandleNull,            (PacketCmd)0x6b, CHL_S2C);
        registerHandler(&PacketHandler::HandleNull,            (PacketCmd)0x6e, CHL_S2C);
        registerHandler(&PacketHandler::HandleNull,            (PacketCmd)0x76, CHL_GAMEPLAY);
        registerHandler(&PacketHandler::HandleNull,            (PacketCmd)0x7b, CHL_S2C);
        registerHandler(&PacketHandler::HandleNull,            (PacketCmd)0x7f, CHL_S2C);
        registerHandler(&PacketHandler::HandleNull,            (PacketCmd)0x87, CHL_S2C);
        registerHandler(&PacketHandler::HandleNull,            (PacketCmd)0x93, CHL_S2C);
        registerHandler(&PacketHandler::HandleNull,            (PacketCmd)0x9e, CHL_S2C);
        registerHandler(&PacketHandler::HandleNull,            (PacketCmd)0xb0, CHL_S2C);
        registerHandler(&PacketHandler::HandleNull,            (PacketCmd)0xb7, CHL_S2C);
        registerHandler(&PacketHandler::HandleNull,            (PacketCmd)0xe9, CHL_S2C);
        registerHandler(&PacketHandler::HandleNull,            (PacketCmd)0xf0, CHL_S2C);
        registerHandler(&PacketHandler::HandleNull,            (PacketCmd)0xfe, CHL_S2C);registerHandler(&PacketHandler::HandleNull,            (PacketCmd)0xfe, CHL_S2C);
        */
                    break;
            }
        }
        private void HandleBatchPacket(Byte[] data)
        {
            int offset = 0;
            Byte[] pkt = data;
            Byte numPackets = pkt[1];
            Byte firstPacketSize = pkt[2];
            offset += 3;
            Byte previousCmd = pkt[offset + 0];
            offset += firstPacketSize;
            for (int i = 2; i < numPackets + 1; ++i)
            {
                Byte flagsAndLength = pkt[offset + 0]; // 6 first bits = size (if not 0xFC), 2 last bits = flags
                Byte size = (Byte)(flagsAndLength >> 2);
                Byte additionalByte = pkt[offset + 1]; // Only preset if flagsAndLength & FLAG_UNK
                Byte command;
                Byte[] buffer = new Byte[8192];
                offset++;
                if ((flagsAndLength & 0x01) > 0)
                { // additionnal byte, skip command   
                    offset++;
                    command = previousCmd;
                }
                else
                {
                    command = pkt[offset + 0];
                    offset++;
                    if ((flagsAndLength & 0x02) > 0)
                    { // looks like when this is set, we keep the same netId, else we use a new one
                        offset++;
                    }
                    else
                    {
                        offset += 4;
                    }
                }
                if (size == 0x3F)
                { // size is too big to be on 6 bits, so instead it's stored later
                    size = pkt[offset + 0];
                    offset++;
                }
                buffer[0] = command;
                for (int j = 0; j < size; j++)
                {
                    buffer[j + 1] = pkt[offset + j];
                }
                HandlePacket(buffer, 2);
                offset += size;
            }
        }
        private void SendConnect(UInt64 userId)
        {
            KeyCheck keyCheck = KeyCheck.Create(playerId, blowfish.Encrypt_ECB(playerId));
            SendPacket(GetBytes<KeyCheck>(keyCheck), Channel.CHL_HANDSHAKE);
        }
        private void HandleKeyCheck(Byte[] data)
        {
            KeyCheck keyCheck;
            keyCheck = GetStruct<KeyCheck>(data);
            if (first){
                //SendPacket(GetBytes<KeyCheck>(keyCheck), Channel.CHL_HANDSHAKE);
            }
            else
            {
                first = true;
            }
            if (keyCheck.userId == playerId)
            {
                //netId = keyCheck.playerNo;// +1;
                Log("netid : " + netId.ToString());
            }
        }
        private void HandleGameNumber(Byte[] data)
        {
            SendQuery();
        }
        private void SendQuery()
        {
            PacketHeader queryStatus = PacketHeader.Create(PacketCmd.PKT_C2S_QueryStatusReq, 0);
            SendPacket(GetBytes(queryStatus), Channel.CHL_C2S);
        }
        private void HandleQuery(Byte[] data)
        {
            SendSynch();
        }
        private void SendSynch()
        {
            SynchVersion synch = SynchVersion.Create();
            SendPacket(GetBytes(synch), Channel.CHL_C2S);
        }
        private void HandleSynch(Byte[] data)
        {
            SynchVersionAns synch = GetStruct<SynchVersionAns>(data);
            ClientReady ready = ClientReady.Create();
            SendPacket(GetBytes(ready), Channel.CHL_LOADING_SCREEN);
            //SendPing(0.0f, 666.6f, 0x000F0001);
            new Thread(() =>
            {
                float i = 5.4f;
                while (true)
                {
                    Thread.Sleep(1000);
                    i += 23.1f;
                    comp += 2;
                    if (i >= 99.5f)
                    {
                        i = 100.0f;
                        if (pastLoadScreen == false && i > 99)
                        {
                            pastLoadScreen = true;
                            SendPing(100.0f, 0.0f, 0x80100040 | comp);
                            SendCharLoaded();
                        }
                        k = k + 1;
                        if (k < 7)
                        {
                            SendPing(i, 0.0f, 0x80100040 | comp);
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        SendPing(i, 15.4f, 0x000F0000 | comp);
                    }
                }
            });
        }
        private void HandleScreenInfo(Byte[] data)
        {
            LoadScreenInfo info = GetStruct<LoadScreenInfo>(data);
        }
        private void HandleName(Byte[] data)
        {
            LoadScreenPlayer info = GetStruct<LoadScreenPlayer>(data);
        }
        private void HandleHero(Byte[] data)
        {
            LoadScreenPlayer info = GetStruct<LoadScreenPlayer>(data);
        }
        private void SendPing(float loaded, float ping, UInt32 unk)
        {
            PingLoadInfo pingPacket = PingLoadInfo.Create();
            pingPacket.userId = 0xFFFFFFFFFFFFFFFF;
            pingPacket.loaded = loaded;
            pingPacket.ping = ping;
            pingPacket.unk2 = unk;
            SendPacket(GetBytes(pingPacket), Channel.CHL_C2S);
        }
        private void HandlePing(Byte[] data)
        {
            PingLoadInfo ping = GetStruct<PingLoadInfo>(data);
            //if (ping.userId == playerId)
            {
                new Thread(() =>
                {
                    Thread.Sleep(1750);
                    float i = ping.loaded;
                    i += 9.1f;
                    comp += 2;
                    if (i >= 99.5f)
                    {
                        i = 100.0f;
                        if (pastLoadScreen == false && i > 99)
                        {
                            pastLoadScreen = true;
                            SendPing(100.0f, 0.0f, 0x80100040 | comp);
                            SendCharLoaded();
                        }
                        k = k + 1;
                        if (k < 7)
                        {
                            SendPing(i, 0.0f, 0x80100040 | comp);
                        }
                    }
                    else
                    {
                        SendPing(i, 15.4f, 0x000F0000 | comp);
                    }
                }).Start();
            }
        }
        private void SendCharLoaded()
        {
            SendPacket(GetBytes(PacketHeader.Create(PacketCmd.PKT_C2S_CharLoaded, 0)), Channel.CHL_C2S);
        }
        private void HandleStartSpawn(Byte[] data)
        {
        }
        private void HandleHeroSpawn(Byte[] data)
        {
            SendGameStart();
        }
        private void HandleMinionSpawn(Byte[] data)
        {
        }
        private void HandleSetHealth(Byte[] data)
        {
        }
        private void HandleTurretSpawn(Byte[] data)
        {
        }
        private void HandleLevelPropSpawn(Byte[] data)
        {
        }
        private void HandleEndSpawn(Byte[] data)
        {
            SendGameStart();
        }
        private void HandleBuyItem(Byte[] data)
        {
        }
        private void HandleGameTimer(Byte[] data)
        {
            GameTimer timer = GetStruct<GameTimer>(data);
            SendGameTime(timer);
        }
        private void HandleGameTimerUpdate(Byte[] data)
        {
            GameTimer timer = GetStruct<GameTimer>(data);
            SendGameTime(timer);
        }
        private void SendGameTime(GameTimer timer)
        {
            GameTime time = GameTime.Create(timer.tick1);
            SendPacket(GetBytes(time), Channel.CHL_GAMEPLAY);
        }
        private void SendGameStart()
        {
            if (!startedGame)
            {
                SendPing(100.0f, 0.0f, 0x80100040);
                SendPacket(GetBytes(PacketHeader8.Create(PacketCmd.PKT_C2S_unkstart)), Channel.CHL_C2S);
                startedGame = SendPacket(GetBytes(StartGame.Create()), Channel.CHL_C2S);
                SendPing(100.0f, 0.0f, 0x80100070);
                SendPing(100.0f, 0.0f, 0x80100072);
            }
        }
        private void HandleGameStart(Byte[] data)
        {
        }
        private void HandleUpdateUnit(Byte[] data)
        {
            Update up = new Update(data);
            GameHeader updateUnit = GetStruct<GameHeader>(data);
            SendUpdateUnitConfirm(updateUnit);
        }
        private void SendUpdateUnitConfirm(GameHeader stats)
        {
            GameHeader realStats = GameHeader.Create(PacketCmd.PKT_C2S_StatsConfirm, 0, stats.tick);
            SendPacket(GetBytes(realStats), Channel.CHL_C2S);
        }
        private void HandleMoveReq(Byte[] data)
        {
            //Waypoints waypoints = new Waypoints(data);
            MoveConfirm move = GetStruct<MoveConfirm>(data);
            move.header.header.cmd = (byte)PacketCmd.PKT_C2S_MoveConfirm;
            SendPacket(GetBytes(move), Channel.CHL_C2S);
        }
        private void SendMove(float x, float y)
        {
            Byte[] netIdBytes = new Byte[4] { 0x19, 0, 0, 0x40 };
            //netIdBytes[0] += (byte)netId;
            Log("move id : " + BitConverter.ToString(netIdBytes));
            MovementReq move = MovementReq.Create(x, y, BitConverter.ToUInt32(netIdBytes, 0));
            SendPacket(GetBytes(move), Channel.CHL_C2S);
        }
        public static Byte[] GetBytes<T>(T msg) where T : struct
        {
            int objsize = Marshal.SizeOf(typeof(T));
            Byte[] ret = new Byte[objsize];
            IntPtr buff = Marshal.AllocHGlobal(objsize);
            Marshal.StructureToPtr(msg, buff, true);
            Marshal.Copy(buff, ret, 0, objsize);
            Marshal.FreeHGlobal(buff);
            return ret;
        }
        public static T GetStruct<T>(Byte[] data) where T : struct
        {
            int objsize = Marshal.SizeOf(typeof(T));
            IntPtr buff = Marshal.AllocHGlobal(objsize);
            Marshal.Copy(data, 0, buff, objsize);
            T retStruct = (T)Marshal.PtrToStructure(buff, typeof(T));
            Marshal.FreeHGlobal(buff);
            return retStruct;
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            client = new Host();
            Library.Initialize();
            client.Create(null, 1);

            var address = new Address();
            address.SetHost(IPAddr);
            address.Port = port;

            server = client.Connect(address, 8);

            byte[] blowfishkey = Convert.FromBase64String(key);
            blowfish = new BlowFish(blowfishkey);
            while (client.Service(1) >= 0)
            {
                Event @event;
                while (client.CheckEvents(out @event) > 0)
                {
                    switch (@event.Type)
                    {
                        case EventType.Connect:
                            SendConnect(playerId);
                            break;
                        case EventType.Receive:
                            HandleEncryptedPacket(@event.Packet.GetBytes(), @event.ChannelID);
                            @event.Packet.Dispose();
                            break;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            server.Disconnect(0);
            server.DisconnectNow(0);
            startedGame = false;
            pastLoadScreen = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var address = new Address();
            address.SetHost(IPAddr);
            address.Port = port;
            server = client.Connect(address, 8);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SendMove(Convert.ToSingle(textBox2.Text), Convert.ToSingle(textBox3.Text));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SendPacket(GetBytes(PacketHeader8.Create(PacketCmd.PKT_C2S_Surrender)), Channel.CHL_C2S);
        }
    }
}
