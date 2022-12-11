using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{
    public class InventoryManager : MonoBehaviour
    {
        public Weapon rightHandWeapon;
        public bool hasLeftHandWeapon = true;
        public Weapon leftHandWeapon;

        StateManager states;

        public void Init(StateManager st)
        {
            states = st;
            EquipWeapon(rightHandWeapon, false);
            EquipWeapon(leftHandWeapon, true);
            InitAllDamageColliders(states);
            CloseAllDamageColliders();


        }

        public void EquipWeapon(Weapon w, bool isLeft = false)
        {
            string targetIdle = w.oh_idle;
            targetIdle += (isLeft) ? "_l" : "_r";
            states.anim.SetBool("mirror", isLeft);
            states.anim.Play("changeWeapon");
            states.anim.Play(targetIdle);
        }

        public Weapon GetCurrentWeapon(bool isLeft)
        {
            if (isLeft)
                return leftHandWeapon;
            else
                return rightHandWeapon;
        }

        public void InitAllDamageColliders(StateManager states)
        {
            if (rightHandWeapon.w_hook != null)
                rightHandWeapon.w_hook.InitDamageColliders(states);

            if (leftHandWeapon.w_hook != null)
                leftHandWeapon.w_hook.InitDamageColliders(states);
        }

        public void OpenAllDamageColliders()
        {
            if (rightHandWeapon.w_hook != null)
                rightHandWeapon.w_hook.OpenDamageCollidersWeapon();

            if (leftHandWeapon.w_hook != null)
                leftHandWeapon.w_hook.OpenDamageCollidersWeapon();
        }

        public void CloseAllDamageColliders()
        {
            if (rightHandWeapon.w_hook != null)
                rightHandWeapon.w_hook.CloseDamageCollidersWeapon();

            if (leftHandWeapon.w_hook != null)
                leftHandWeapon.w_hook.CloseDamageCollidersWeapon();
        }

    }

    [System.Serializable]
    public class Weapon
    {
        public string oh_idle;
        public string th_idle;

        public List<Action> actions;
        public List<Action> two_handedActions;
        public WeaponStats parryStats;
        public WeaponStats backstabbStats;
        public bool LeftHandMirror;

        public GameObject weaponModel;
        public WeaponHook w_hook;

        public Action GetAction(List<Action> l, ActionInput inp)
        {
            for (int i = 0; i < l.Count; i++)
            {
                if (l[i].input == inp)
                    return l[i];
            }

            return null;
        }
    }
}

