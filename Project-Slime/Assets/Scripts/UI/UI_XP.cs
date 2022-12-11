using SA;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_XP : MonoBehaviour
{
    [SerializeField] StateManager controller;
    [SerializeField] RectTransform health_back;
    [SerializeField] RectTransform damdage_moment;
    [SerializeField] public DamageCollider dam_coll;
    [SerializeField] GameObject HP;
    [SerializeField] GameObject DM;
    [SerializeField] GameObject HPc;
    [SerializeField] GameObject DMc;
    private int xp;
    public int hp_lvl = 0;
    public int dam_lvl = 0;

    public void lvl_Update()
    {
        HP.GetComponent<TextMeshProUGUI>().text = (hp_lvl + 1).ToString() + " уровень";
        DM.GetComponent<TextMeshProUGUI>().text = (dam_lvl + 1).ToString() + " уровень";
        HPc.GetComponent<TextMeshProUGUI>().text = (30 + hp_lvl * 30).ToString() + " монет";
        DMc.GetComponent<TextMeshProUGUI>().text = (30 + dam_lvl * 30).ToString() + " монет";
        if (controller.characterStats.xp >= (30 + hp_lvl * 30) && hp_lvl < 4)
        {
            transform.GetChild(3).GetComponent<Button>().enabled = true;
            transform.GetChild(3).GetComponent<Image>().color = new Color(144, 144, 144);
        }
        else
        {
            transform.GetChild(3).GetComponent<Button>().enabled = false;
            transform.GetChild(3).GetComponent<Image>().color = new Color(144, 144, 144);
        }
        if (controller.characterStats.xp >= (30 + dam_lvl * 30) && dam_lvl < 5)
        {
            transform.GetChild(4).GetComponent<Button>().enabled = true;
            transform.GetChild(4).GetComponent<Image>().color = new Color(144, 144, 144);
        }
        else
        {
            transform.GetChild(4).GetComponent<Button>().enabled = false;
            transform.GetChild(4).GetComponent<Image>().color = new Color(144, 144, 144);
        }
    }
    public void Add_hp()
    {
            controller.characterStats.max_hp += 20;
            controller.characterStats.hp = controller.characterStats.hp * controller.characterStats.max_hp / (controller.characterStats.max_hp - 20);
            health_back.sizeDelta = new Vector2(controller.characterStats.max_hp, 35);
            damdage_moment.sizeDelta = new Vector2(controller.characterStats.max_hp, 35);
            controller.characterStats.xp -= 30;
            hp_lvl++;
    }
    public void Add_dm()
    {
            dam_coll.add_damage += 20;
            controller.characterStats.xp -= 30;
            dam_lvl++;
    }

    void Update()
    {
        lvl_Update();
    }
}
