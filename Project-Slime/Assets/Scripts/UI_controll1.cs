using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class UI_controll1 : MonoBehaviour
    {
        public float timer;
        public UI_gameplay game;
        public bool ispause = false;
        public bool in_skills = false;
        public CurseBehaviour[] curse_script;
        public CameraManager camManager;
        [SerializeField] GameObject gameplay;
        [SerializeField] GameObject pause;
        [SerializeField] GameObject skills;

        public void UnPause()
        {
            timer = 1f;
            ispause = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            pause.SetActive(false);
            camManager.enabledTurning = true;
            curse_script = FindObjectsOfType<CurseBehaviour>();
            foreach (CurseBehaviour i in curse_script)
            {
                i.enabled = true;
            }
            game = gameplay.GetComponent<UI_gameplay>();
            game.paused = false;
        }

        public void Skills()
        {
            timer = 0f;
            ispause = true;
            pause.SetActive(false);
            skills.SetActive(true);
            camManager.enabledTurning = true;
            in_skills = true;
        }

        public void from_Skills()
        {
            timer = 0f;
            ispause = true;
            pause.SetActive(true);
            skills.SetActive(false);
            camManager.enabledTurning = true;
            in_skills = false;
        }

        public void Pause()
        {
            camManager.enabledTurning = false;
            timer = 0;
            curse_script = FindObjectsOfType<CurseBehaviour>();
            foreach (CurseBehaviour i in curse_script)
            {
                i.enabled = false;
            }
            game = gameplay.GetComponent<UI_gameplay>();
            game.paused = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        void Update()
        {
            Time.timeScale = timer;
            if (Input.GetKeyDown(KeyCode.Escape) && ispause == false) ispause = true;
            else if (Input.GetKeyDown(KeyCode.Escape) && ispause == true) ispause = false;
            if (ispause == true)
            {
                Pause();
                pause.SetActive(true);
            }
            else if (ispause == false && !in_skills)
            {
                UnPause();
            }
            else if (in_skills)
            {
                from_Skills();
                in_skills = false;
            }
        }

    }

}