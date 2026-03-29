using System;
using System.Collections.Generic;
using TestTask.NonEditable;
using UnityEngine;

namespace TestTask.Editable
{
    public static class ClientPacketsHandler
    {
        #region Packet Handlers
        public static void LoginDataReceived(Packet packet)
        {
            int responseCode = packet.ReadInt();
            int clientId = packet.ReadInt();

            ClientManager.Instance.SetClientLogInStatus(responseCode, clientId);
        }

        public static void MonsterSpawnDataReceived(Packet packet)
        {
            int monsterId = packet.ReadInt();
            int monsterType = packet.ReadInt();
            int monsterMaxHp = packet.ReadInt();
            int monsterCurrentHp = packet.ReadInt();

            ClientManager.Instance.ClientMobsManager.SpawnMonster(monsterId, monsterType, monsterMaxHp, monsterCurrentHp);
        }

        public static void MonsterHealthDataReceived(Packet packet)
        {
            int monsterId = packet.ReadInt();
            float monsterCurrentHp = packet.ReadFloat();

            ClientManager.Instance.ClientMobsManager.UpdateMonsterHealth(monsterId, monsterCurrentHp);
        }

        public static void ColorSetDataReceived(Packet packet)
        {
            int count = packet.ReadInt();

            List<Color32> colors = new List<Color32>(count);
            for (int i = 0; i < count; i++)
            {
                int colorAsInt = packet.ReadInt();
                colors.Add(colorAsInt.ToColor32());
            }

            ClientManager.Instance.ClientColorManager.SetColors(colors);
        }
        #endregion

        #region Packet Senders
        public static void SendLoginRequest()
        {
            Packet packet = new Packet(1);
            ClientManager.Instance.PacketSenderClient.SendToServer(packet);
        }

        public static void SendDamageMonsterRequest(int monsterId, float damageAmount)
        {
            Packet packet = new Packet(2);
            packet.Write(monsterId);
            packet.Write(damageAmount);
            ClientManager.Instance.PacketSenderClient.SendToServer(packet);
        }

        public static void SendNewColorSetRequest()
        {
            Packet packet = new Packet(3);
            ClientManager.Instance.PacketSenderClient.SendToServer(packet);
        }
        #endregion
    }
}
