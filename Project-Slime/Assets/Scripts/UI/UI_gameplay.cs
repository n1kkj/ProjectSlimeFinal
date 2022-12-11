using SA;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_gameplay : MonoBehaviour
{
    public float curse;
    public float bar = 0;
    public CurseBehaviour[] l;
    private float max_curse = 200;
    private float darkness;
    private float health;
    public bool paused = false;
    //private Rect rect;
    [SerializeField] public Image image;
    [SerializeField] public RectTransform strength_rect;
    [SerializeField] public RectTransform bar_rect;
    [SerializeField] public RectTransform health_back;
    [SerializeField] public RectTransform actual_healh;
    [SerializeField] public RectTransform damdage_moment;
    [SerializeField] public StateManager controller;
    [SerializeField] public DamageCollider damage_coll;
    [SerializeField] public TextMeshProUGUI damage_text;
    [SerializeField] public TextMeshProUGUI xp_text;
    [SerializeField] public TextMeshProUGUI xp2_text;
    [SerializeField] public CharacterStats stats;


    void Curse_update()
    {
        l = FindObjectsOfType<CurseBehaviour>();

        foreach (CurseBehaviour c in l)
        {
            if (c.act)
            {
                bar_rect.gameObject.SetActive(true);
            }
        }

        curse = 0;
        for (int i = 0; i < l.Length; i++) curse += l[i].curse_strength;
        bar = Mathf.Min(max_curse, bar + (curse) / 220);
        darkness = (0.95f * bar) / 200;
        damage_coll.damage = (int)Mathf.Ceil(Mathf.Max(10, 0.25f * (max_curse - bar))) + damage_coll.add_damage;
        if (bar >= max_curse / 2) foreach (CurseBehaviour j in l)j.glow = false;
        if (bar >= max_curse)
        {
            strength_rect.sizeDelta = new Vector2(bar, 0);
            bar_rect.sizeDelta = new Vector2(max_curse, 0);
            image.color = new Color(0, 0, 0, darkness);
        }
        else
        {
            strength_rect.sizeDelta = new Vector2(bar, 20);
            bar_rect.sizeDelta = new Vector2(max_curse, 20);
            image.color = new Color(0, 0, 0, darkness);
        }
    }

    void Health_update()
    {
        health = controller.characterStats.hp;
        actual_healh.sizeDelta = new Vector2(health, 35);
        damdage_moment.sizeDelta = new Vector2(controller.characterStats.max_hp, 35);
        health_back.sizeDelta = new Vector2(controller.characterStats.max_hp, 35);
        //rect = damdage_moment.rect;
        //rect.width = Mathf.Abs(Vector2.Lerp(damdage_moment.position, actual_healh.position - new Vector3(actual_healh.rect.width, 0, 0), Time.deltaTime).x);
    }

    void Damage_update()
    {
        damage_text.text = damage_coll.damage.ToString();
    }

    void XP_update()
    {
        xp_text.text = controller.characterStats.xp.ToString();
        xp2_text.text = controller.characterStats.xp.ToString();
    }

    void Update()
    {
        if (!paused)
        {
            Curse_update();
        }
        Damage_update();
        Health_update();
        XP_update();
    }
}
