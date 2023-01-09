using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{
    public class WeaponHook : MonoBehaviour
    {
        public GameObject[] damageCollider;
        
        public void OpenDamageCollidersWeapon()
        {
            for (int i = 0; i < damageCollider.Length; i++)
            {
                damageCollider[i].SetActive(true);
            }
        }

        public void CloseDamageCollidersWeapon()
        {
            for (int i = 0; i < damageCollider.Length; i++)
            {
                damageCollider[i].SetActive(false);
            }
        }
    }
}

