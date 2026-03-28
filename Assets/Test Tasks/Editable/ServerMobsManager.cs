using System;
using TestTask.Editable;
using TestTask.NonEditable;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TestTask.Editable
{
    public class ServerMobsManager
    {
        [field: SerializeField] public MonsterData MonsterData { get; private set; }

        public Action<MonsterData> MonsterStatusChanged;

        public ServerMobsManager()
        {
            MonsterData = SpawnMonster();
        }

        public MonsterData SpawnMonster()
        {
            var monsterId = Random.Range(1, 1000);
            var monsterType = MonsterNameExtensions.MonsterTypeFromId(monsterId);
            var monsterMaxHealth = Random.Range(50, 201);
            var monsterCurrentHealth = monsterMaxHealth;

            MonsterData = new MonsterData(monsterId, monsterType, monsterMaxHealth, monsterCurrentHealth);
            MonsterData.MonsterDeath += OnMonsterDied;

            return MonsterData;
        }

        public MonsterData ApplyDamageToMonster(int id, float damageAmount)
        {
            if (MonsterData.MonsterId == id)
            {
                MonsterData.TakeDamage(damageAmount);
            }

            return MonsterData;
        }

        public void OnMonsterDied()
        {
            MonsterData.MonsterDeath -= OnMonsterDied;
            MonsterData = SpawnMonster();

            ServerPacketsHandler.SendMonsterSpawnedResponse(MonsterData);
        }
    }
}  
