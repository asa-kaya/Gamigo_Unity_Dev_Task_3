using System;
using System.Collections.Generic;
using TestTask.NonEditable;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace TestTask.Editable
{
    public class ClientMobsManager : MonoBehaviour
    {
        [SerializeField] private Image monsterImage;
        [SerializeField] private Slider healthBar;
        [SerializeField] private Sprite[] monsterSprites;

        private Dictionary<string, Sprite> monsterMap;
        private MonsterData monster;

        private void Awake()
        {
            monsterMap = new Dictionary<string, Sprite>();
            foreach (Sprite sprite in monsterSprites)
                monsterMap[sprite.name] = sprite;
        }

        public void LoadMonsterData(int id, int type, int maxHp, int currentHp)
        {
            var monsterType = MonsterNameExtensions.MonsterTypeFromId(type);
            monster = new MonsterData(id, monsterType, maxHp, currentHp);
        }

        public void SpawnMonster()
        {
            if (monster == null) return;

            monster.MonsterDamaged += OnMonsterDamageTaken;
            monster.MonsterDeath += OnMonsterDeath;

            UpdateMonsterVisuals();

            Debug.Log($"[Client] Monster spawned: id={monster.MonsterId}; type={monster.MonsterType} HP={monster.MonsterCurrentHealth}/{monster.MonsterMaxHealth}");
        }

        public void UpdateMonsterHealth(int monsterId, float newHp)
        {
            if (monster.MonsterId == monsterId)
            {
                // assume damage taken if new data has less health than current one
                float damageTaken = monster.MonsterCurrentHealth - newHp;

                if (damageTaken > 0)
                    monster.TakeDamage(damageTaken);
            }
        }

        private void OnMonsterDamageTaken(float damageAmount)
        {
            UpdateMonsterVisuals();
        }

        private void OnMonsterDeath()
        {
            // clean up and wait for server to send newly spawned monster data
            monster = null;
        }

        private void UpdateMonsterVisuals()
        {
            if (monster != null)
            {
                string typeKey = monster.MonsterType.ToFriendlyString();
                if (monsterMap.ContainsKey(typeKey))
                {
                    monsterImage.sprite = monsterMap[typeKey];
                    monsterImage.color = Color.white;
                }
                
                healthBar.value = monster.MonsterCurrentHealth/monster.MonsterMaxHealth;
                healthBar.gameObject.SetActive(true);
            }
            else
            {
                healthBar.gameObject.SetActive(false);
            }
        }

        public void DealDamageToMonster()
        {
            if (monster == null) return;

            // make up a random number between 20-50% of monster's health
            var damageAmount = Random.Range(monster.MonsterMaxHealth * 0.2f, monster.MonsterMaxHealth * 0.5f);
            ClientPacketsHandler.SendDamageMonsterRequest(monster.MonsterId, damageAmount);
        }
    }
}
