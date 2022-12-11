using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SA
{
    public class CurseBehaviour : MonoBehaviour
    {
        private MeshRenderer _material;
        [SerializeField] private float _tp_radius;
        [SerializeField] private float _tp_time;
        private GameObject _core;
        private GameObject _heal;

        public float curse_strength = 0;

        private Vector3 dis;
        private Vector3 h_dis;
        private Vector3 c_dis;
        private Vector3 heal_spawn;
        private Vector3 new_pos;
        private float deltaTime = 100;
        private float radius;
        //private float self_hp;
        private Outline outline;
        private Outline outline2;
        public bool act = false;
        public bool glow = true;


        //Daniil's changes
        //public Transform core;
        public UnityEngine.AI.NavMeshAgent agent;
        UnityEngine.AI.NavMeshPath nav_mesh_path;
        Vector3 random_point;
        bool ray_blocked;
        UnityEngine.AI.NavMeshHit hit;
        public Animator goblinAnim;
        public GetDamage goblinDmg;
        public Transform player;
        public bool activated = false;
        public float TP_RANGE = 0f;

        private void Teleport_core()
        {
            /*new_pos = transform.position + (Random.insideUnitSphere * radius);
            c_dis = transform.position - GameObject.Find("Controller").transform.position;
            while (c_dis.magnitude < 7f)
            {
                    new_pos = transform.position + (Random.insideUnitSphere * radius);
                    c_dis = GameObject.Find("Controller").transform.position - new Vector3(new_pos.x, transform.position.y, new_pos.z);
            }
            transform.position = new Vector3(new_pos.x, transform.position.y, new_pos.z);*/
            bool get_correct_point = false;

            int cnt = 0;

            UnityEngine.AI.NavMeshHit navmesh_hit;
            UnityEngine.AI.NavMesh.SamplePosition(Random.insideUnitSphere * TP_RANGE + transform.position, out navmesh_hit, 3 * TP_RANGE, UnityEngine.AI.NavMesh.AllAreas);
            random_point = navmesh_hit.position;

            while (!get_correct_point || (random_point - player.transform.position).magnitude < 10f)
            {
                if (cnt > 1000)
                {
                    Debug.Log("Core CRUSDF");
                    break;
                }
                UnityEngine.AI.NavMesh.SamplePosition(Random.insideUnitSphere * TP_RANGE + transform.position, out navmesh_hit, 3 * TP_RANGE, UnityEngine.AI.NavMesh.AllAreas);
                random_point = navmesh_hit.position;

                //get_correct_point = true;
                agent.CalculatePath(random_point, nav_mesh_path);
                if (nav_mesh_path.status == UnityEngine.AI.NavMeshPathStatus.PathComplete) get_correct_point = true;
            }

            //path_complete = true;

            goblinDmg.enabled = false;
            goblinAnim.Play("teleport");
            StartCoroutine(waitTeleport());


            //new_pos = transform.position + Random.insideUnitSphere * _tp_radius; - Nikita

            /*RaycastHit hit;
            Vector3 origin = transform.position;
            origin.y -= _tp_radius;
            if (Physics.Raycast(origin, transform.up, out hit, 5 * _tp_radius))
            {
                Debug.Log("Found");
                transform.position = hit.point;
            }*/
        }

        IEnumerator waitTeleport()
        {
            yield return new WaitForSeconds(1.2f);
            Debug.Log("Teleported");
            goblinDmg.enabled = true;
            transform.position = random_point;
            Teleport_heal();
        }

        private void Teleport_heal()
        {
            Debug.Log("Heal_TP");
            /*heal_spawn = transform.position + (Random.insideUnitSphere * radius) ;
            h_dis = gameObject.transform.position - new Vector3(heal_spawn.x, _heal.transform.position.y, heal_spawn.z);
            while (h_dis.magnitude < 7f)
            {
                    heal_spawn = transform.position + (Random.insideUnitSphere * radius);
                    h_dis = gameObject.transform.position - new Vector3(heal_spawn.x, _heal.transform.position.y, heal_spawn.z);
            }
            _heal.transform.position = new Vector3(heal_spawn.x, _heal.transform.position.y, heal_spawn.z);*/

            bool get_correct_point = false;
            UnityEngine.AI.NavMeshHit navmesh_hit;

            UnityEngine.AI.NavMesh.SamplePosition(Random.insideUnitSphere * TP_RANGE + transform.position, out navmesh_hit, 3 * TP_RANGE, UnityEngine.AI.NavMesh.AllAreas);
            random_point = navmesh_hit.position;

            int cnt = 0;

            //get_correct_point = true;
            agent.CalculatePath(random_point, nav_mesh_path);
            if (nav_mesh_path.status == UnityEngine.AI.NavMeshPathStatus.PathComplete) get_correct_point = true;

            while (!get_correct_point || (random_point - player.transform.position).magnitude < 10f || (random_point - _core.transform.position).magnitude < 7f)
            {
                if (cnt > 1000)
                {
                    Debug.Log("Heal CRUSH");
                }

                UnityEngine.AI.NavMesh.SamplePosition(Random.insideUnitSphere * TP_RANGE + transform.position, out navmesh_hit, 3 * TP_RANGE, UnityEngine.AI.NavMesh.AllAreas);
                random_point = navmesh_hit.position;

                //get_correct_point = true;
                agent.CalculatePath(random_point, nav_mesh_path);
                if (nav_mesh_path.status == UnityEngine.AI.NavMeshPathStatus.PathComplete) get_correct_point = true;
            }

            //path_complete = true;

            _heal.transform.position = random_point;

            /*RaycastHit hit;
            Vector3 origin = transform.position;
            origin.y -= _tp_radius;
            if (Physics.Raycast(origin, transform.up, out hit, 5 * _tp_radius))
            {
                Debug.Log("Found");
                transform.position = hit.point;
            }*/

        }
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "Controller" && !act)
            {
                outline.enabled = true;
                outline2.enabled = true;
                _material.enabled = true;
                //Teleport_heal();
                _heal.SetActive(true);
                act = true;
                activated = true;

                Vector3 v = new Vector3();
                v = transform.position - new Vector3(0.5f, 0, 0.5f) - player.transform.position;
                /*heal_spawn = transform.position - v;
                h_dis = gameObject.transform.position - new Vector3(heal_spawn.x, _heal.transform.position.y, heal_spawn.z);*/

                UnityEngine.AI.NavMeshHit navmesh_hit;

                UnityEngine.AI.NavMesh.SamplePosition(transform.position + v, out navmesh_hit, 3 * TP_RANGE, UnityEngine.AI.NavMesh.AllAreas);
                random_point = navmesh_hit.position;

                _heal.transform.position = random_point;

                //transform.GetChild(1).Find("Curse_hp").gameObject.GetComponent<TextMeshProUGUI>().enabled = true;
            }

        }

        private void OnTriggerStay(Collider other)
        {

            if (other.gameObject.name == "Controller" && activated)
            {
                curse_strength = 0;
                dis = transform.position - GameObject.Find("Controller").transform.position;
                if (deltaTime >= _tp_time && dis.magnitude <= _tp_radius)
                {
                    Debug.Log(dis);
                    Teleport_core();
                    deltaTime = Time.deltaTime;
                }
                curse_strength = ((int)(radius - dis.magnitude + 1));
                deltaTime += Time.deltaTime;
                if (dis.magnitude > 50)
                {
                    outline.OutlineWidth = 0;
                    outline2.OutlineWidth = 0;

                }
                else
                {
                    if (glow)
                    {
                        outline.OutlineWidth = 5f;
                        outline2.OutlineWidth = 5f;
                    }
                    else
                    {
                        outline.OutlineWidth = 0;
                        outline2.OutlineWidth = 0;
                    }
                }
            }
        }

        //void Check_hp()
        //{
        //self_hp = transform.GetChild(1).GetComponent<GetDamage>().hp;
        //transform.GetChild(1).Find("Curse_hp").GetComponent<TextMeshProUGUI>().text = self_hp.ToString();
        //}

        void Update()
        {
            if (activated)
            {
                Quaternion turn = Quaternion.LookRotation(player.transform.position - _core.transform.position);

                _core.transform.rotation = Quaternion.Slerp(_core.transform.rotation, turn, 0.1f);
            }
        }

        void Start()
        {
            //Daniil's changes
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            nav_mesh_path = new UnityEngine.AI.NavMeshPath();
            goblinAnim = transform.GetChild(1).GetComponent<Animator>();
            goblinDmg = transform.GetChild(1).GetComponent<GetDamage>();
            player = GameObject.Find("Controller").transform;



            //Check_hp();
            _heal = transform.GetChild(0).gameObject;
            _core = transform.GetChild(1).gameObject;
            if (_core.GetComponent<Outline>() == null)
                outline = _core.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = Color.yellow;
            outline.OutlineWidth = 5f;
            outline.enabled = false;
            outline2 = _heal.AddComponent<Outline>();
            outline2.OutlineMode = Outline.Mode.OutlineAll;
            outline2.OutlineColor = Color.yellow;
            outline2.OutlineWidth = 5f;
            outline2.enabled = false;
            radius = transform.localScale.x / 2;
            Debug.Log(radius);
            _material = gameObject.GetComponent<MeshRenderer>();
            _material.enabled = false;
            //transform.GetChild(1).Find("Curse_hp").GetComponent<TextMeshProUGUI>().enabled = false;
        }

        /*void Nav_random_point()
        {
            bool get_correct_point = false;
            while (!get_correct_point)
            {
                NavMeshHit navmesh_hit;
                NavMesh.SamplePosition(Random.insideUnitSphere * _tp_radius, out navmesh_hit, _tp_radius, NavMesh.AllAreas);
                random_point = navmesh_hit.position;

                get_correct_point = true;
                *//*agent.CalculatePath(random_point, nav_mesh_path);
                if (nav_mesh_path.status == NavMeshPathStatus.PathComplete) get_correct_point = true;*//*
            }

            //path_complete = true;

            transform.position = random_point;


        }*/
    }

}