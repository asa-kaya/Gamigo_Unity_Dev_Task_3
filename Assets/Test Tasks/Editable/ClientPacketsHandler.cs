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

        public static void MobDataReceived(Packet packet)
        {
            int monsterId = packet.ReadInt();
            int monsterType = packet.ReadInt();
            int monsterMaxHp = packet.ReadInt();
            int monsterCurrentHp = packet.ReadInt();

            ClientManager.Instance.ClientMobsManager.SpawnMonster(monsterId, monsterType, monsterMaxHp, monsterCurrentHp);
        }
        #endregion

        #region Packet Senders
        public static void SendLoginRequest()
        {
            Packet packet = new Packet(1);
            ClientManager.Instance.PacketSenderClient.SendToServer(packet);
        }
        #endregion
    }
}
