using System.Collections.Generic;
using TestTask.NonEditable;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

        public void SpawnMonster(int id, int type, int maxHp, int currentHp)
        {
            var monsterType = MonsterNameExtensions.MonsterTypeFromId(type);

            monster = new MonsterData(id, monsterType, maxHp, currentHp);

            UpdateMonsterVisuals();

            Debug.Log($"[Client] Monster spawned: id={id}; type={monsterType} HP={currentHp}/{maxHp}");
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
    }
}
