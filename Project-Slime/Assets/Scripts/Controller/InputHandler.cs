using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{
    public class InputHandler : MonoBehaviour
    {

        float vertical;
        float horizontal;
        bool a_input;
        bool b_input;
        bool x_input;
        bool y_input;

        bool rb_input;
        float rt_axis;
        bool rt_input;
        bool lb_input;
        float lt_axis;
        bool lt_input;

        bool leftAxis_down;
        bool rightAxis_down;

        float b_timer;
        float rt_timer;
        float lt_timer;


        float jump_delta;



        StateManager states;
        CameraManager camManager;

        float delta;
        float l_delta;

        public List<AudioSource> steps = new List<AudioSource>();

        void Start()
        {
            jump_delta = Time.deltaTime;

            states = GetComponent<StateManager>();
            states.Init();

            camManager = CameraManager.singleton;
            camManager.Init(states);
        }

        void FixedUpdate()
        {
            GetInput();

            delta = Time.fixedDeltaTime;
            UpdateStates();
            states.FixedTick(Time.deltaTime);
            
        }

        void Update()
        {
            camManager.Tick(delta);
            GetInputTick();
            delta = Time.deltaTime;
            l_delta += delta;
            states.Tick(delta);

            ResetInputNStates();
        }

        void GetInput()
        {
            vertical = Input.GetAxis(StaticStrings.Vertical);
            horizontal = Input.GetAxis(StaticStrings.Horizontal);

            b_input = Input.GetButton(StaticStrings.B);
            a_input = Input.GetButton(StaticStrings.A);
            if (b_input)
                b_timer += Time.deltaTime;

            x_input = Input.GetButton(StaticStrings.X);
            y_input = Input.GetButton(StaticStrings.Y);
                

            rt_input = Input.GetButton(StaticStrings.RT);
            rt_axis = Input.GetAxis(StaticStrings.RT);
            if (rt_axis != 0)
                rt_input = true;

            lt_input = Input.GetButton(StaticStrings.LT);
            lt_axis = Input.GetAxis(StaticStrings.LT);
            if (lt_axis != 0)
                lt_input = true;

            rb_input = Input.GetButton(StaticStrings.RB);
            lb_input = Input.GetButton(StaticStrings.LB);


            
            
        }

        void GetInputTick()
        {
            jump_delta += Time.deltaTime;

            leftAxis_down = Input.GetButton("L");

            rightAxis_down = (Input.GetButtonDown(StaticStrings.R) || Input.GetKeyDown(KeyCode.L));
        }


        void UpdateStates()
        {
            states.horizontal = horizontal;
            states.vertical = vertical;

            Vector3 v = vertical * camManager.transform.forward;
            Vector3 h = horizontal * camManager.transform.right;

            states.moveDir = (v + h).normalized;
            float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            states.moveAmount = Mathf.Clamp01(m);

            if (x_input)
                b_input = false;

            if (b_input && b_timer > 0.5f)
            {
                states.run = (states.moveAmount > 0);
            }

            

            if (states.run)
            {
                if (leftAxis_down && states.onGround)
                {
                    jump_delta = 0;
                    states.Jump();
                }    
            }    

            if (b_input == false && b_timer > 0 && b_timer < 0.5f)
            {
                states.rollInput = true;
            }

            states.itemInput = x_input;
            states.rt = rt_input;
            states.lt = lt_input;
            states.rb = rb_input;
            states.lb = lb_input;

            if (y_input)
            {
                states.isTwoHanded = !states.isTwoHanded;
                states.HandleTwoHamded();
            }

            if (states.lockOnTarget != null)
            {
                if (states.lockOnTarget.eStates.enabled == false)
                {
                    states.lockOn = false;
                    states.lockOnTarget = null;
                    states.lockOnTransform = null;
                    camManager.lockonTarget = null;
                    camManager.lockon = false;
                }
            }

            if (rightAxis_down && l_delta > .7f)
            {
                l_delta = 0;
                states.lockOn = !states.lockOn;

                states.lockOnTarget = EnemyManager.singleton.GetEnemy(transform.position);
                if (states.lockOnTarget == null)
                    states.lockOn = false;

                camManager.lockonTarget = states.lockOnTarget;
                states.lockOnTransform = states.lockOnTarget.GetTarget();
                camManager.lockonTransform = states.lockOnTransform;
                camManager.lockon = states.lockOn;

                
            }
        }  


        void ResetInputNStates()
        {
            if (b_input == false)
                b_timer = 0;

            if (states.rollInput)
                states.rollInput = false;

            if (states.run)
                states.run = false;
        }
    }
}

