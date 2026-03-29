using System.Collections.Generic;
using TestTask.NonEditable;
using UnityEngine;

namespace TestTask.Editable
{
    public static class ServerPacketsHandler
    {
        #region Packet Handlers
        public static void LoginRequest(Packet packet)
        {
            var clientLogInResponse = ServerMock.Instance.TryConnectClient(out var clientId);
            SendLoginResponse(clientLogInResponse, clientId);

            // TODO: probably want this somewhere else since it gets received by the client faster before login response sometimes
            if (clientLogInResponse == LoginResponse.Success)
                SendMonsterSpawnedResponse(ServerMock.Instance.ServerMobsManager.SpawnMonster());
        }

        public static void DamageMonsterRequest(Packet packet)
        {
            int monsterId = packet.ReadInt();
            float damageAmount = packet.ReadFloat();
            MonsterData monsterData = ServerMock.Instance.ServerMobsManager.ApplyDamageToMonster(monsterId, damageAmount);

            // no need to send anything if a different monster has already spawned
            if (monsterId == monsterData.MonsterId)
                SendMonsterHealthChangedResponse(monsterData.MonsterId, monsterData.MonsterCurrentHealth);
        }

        public static void ColorSetRequest(Packet packet)
        {
            // maybe return 5 - 10 random colors idk
            int colorCount = Random.Range(5, 11);
            List<Color32> colorSet = new List<Color32>(colorCount);
            
            for (int i = 0; i < colorCount; i++)
            {
                colorSet.Add(new Color32(
                    (byte)Random.Range(0, 256),
                    (byte)Random.Range(0, 256),
                    (byte)Random.Range(0, 256),
                    (byte)Random.Range(0, 256)
                )); // generate random color
            }

            SendColorSetResponse(colorSet);
        }

        #endregion

        #region Packet Senders
        public static void SendLoginResponse(LoginResponse response, int clientId)
        {
            using (Packet packet = new Packet(1))
            {
                packet.Write((int)response);
                packet.Write(clientId);

                ServerMock.Instance.PacketSenderServer.SendToClient(packet);
            }
        }

        public static void SendMonsterSpawnedResponse(MonsterData monsterData)
        {
            using (Packet packet = new Packet(2))
            {
                packet.Write(monsterData.MonsterId);
                packet.Write((int)monsterData.MonsterType);
                packet.Write((int)monsterData.MonsterMaxHealth);
                packet.Write((int)monsterData.MonsterCurrentHealth);

                ServerMock.Instance.PacketSenderServer.SendToClient(packet);
            }
        }

        public static void SendMonsterHealthChangedResponse(int monsterId, float newHealth)
        {
            using (Packet packet = new Packet(3))
            {
                packet.Write(monsterId);
                packet.Write(newHealth);

                ServerMock.Instance.PacketSenderServer.SendToClient(packet);
            }
        }

        public static void SendColorSetResponse(IList<Color32> colorSet)
        {
            using (Packet packet = new Packet(4))
            {
                packet.Write(colorSet.Count);

                foreach (Color32 color in colorSet)
                    packet.Write(color.ToInt());

                ServerMock.Instance.PacketSenderServer.SendToClient(packet);
            }
        }
        #endregion
    }
}

public enum LoginResponse
{
    Success = 0,
    Failure = 1,
}