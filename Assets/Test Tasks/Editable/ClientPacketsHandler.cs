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
        #endregion
    }
}
