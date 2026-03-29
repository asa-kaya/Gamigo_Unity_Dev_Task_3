using System.Collections;
using System.Collections.Generic;
using TestTask.Editable;
using TestTask.NonEditable;
using UnityEngine;
using UnityEngine.UI;

public class MonsterUI : MonoBehaviour
{
    [SerializeField] private Image monsterImage;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Sprite[] monsterSprites;

    // NOTE: make sure Asset's file name is equal to Monster Name for this to work
    private Dictionary<string, Sprite> spriteMap;

    private void Awake()
    {
        spriteMap = new Dictionary<string, Sprite>();
        foreach (Sprite sprite in monsterSprites)
            spriteMap[sprite.name] = sprite;
    }

    private void OnEnable()
    {
        ClientMobsManager.MonsterStatusUpdated += UpdateMonsterVisuals;
    }

    private void OnDisable()
    {
        ClientMobsManager.MonsterStatusUpdated -= UpdateMonsterVisuals;
    }

    private void UpdateMonsterVisuals(MonsterData monster)
    {
        if (monster != null)
        {
            string typeKey = monster.MonsterType.ToFriendlyString();
            if (spriteMap.ContainsKey(typeKey))
            {
                monsterImage.sprite = spriteMap[typeKey];
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
