using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{
    public class GetDamage : MonoBehaviour
    {
        public float hp = 150;

        public void DestroyCore()
        {
            GameObject.Find("Gameplay").GetComponent<UI_gameplay>().bar = 0;
            Destroy(gameObject.transform.parent.gameObject);
            Destroy(gameObject.transform.parent.GetChild(0).gameObject);
            Destroy(gameObject.transform.parent.GetChild(1).gameObject);
            GameObject.Find("Controller").GetComponent<StateManager>().characterStats.xp += 50;
            GameObject.Find("Gameplay").GetComponent<UI_gameplay>().bar_rect.gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 9) // Damage Colliders
            {
                Debug.Log(other.gameObject.GetComponent<DamageCollider>());
                hp -= other.gameObject.GetComponent<DamageCollider>().damage;

                if (hp <= 0)
                    DestroyCore();

            }
        }
    }
}

