using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SA;
using UnityEngine.AI;

public class ReadFromFile : MonoBehaviour
{
    [SerializeField] GameObject enemiesTypes = null;
    [SerializeField] GameObject curseTypes = null;

    string enemySavePath = "Assets/Scripts/Enemy_save/enemiesSave.txt.txt";
    string curseSavePath = "Assets/Scripts/Enemy_save/curseSave.txt.txt";
    public void Read_f() {
        Debug.Log(2332);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("New_enemy");
        foreach (GameObject enemy in enemies)
        {
            Debug.Log("Destroyed");
            Destroy(enemy);
        }

        StreamReader sr = new StreamReader(enemySavePath);

        for (int i = 0; !sr.EndOfStream; i++)
        {
            string line = sr.ReadLine();
            string[] Splitted = line.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            if (Splitted.Length > 0)
            {
                GameObject instantiatedEnemy = Instantiate(enemiesTypes, new Vector3(float.Parse(Splitted[0]), float.Parse(Splitted[1]), float.Parse(Splitted[2])), Quaternion.Euler(float.Parse(Splitted[3]), float.Parse(Splitted[4]), float.Parse(Splitted[5])));
                instantiatedEnemy.GetComponent<CapsuleCollider>().enabled = true;
                instantiatedEnemy.GetComponent<AIHandler>().enabled = true;
                instantiatedEnemy.GetComponent<NavMeshAgent>().enabled = true;
                Debug.Log(4);
            }
        }
        sr.Close();
        GameObject[] curses = GameObject.FindGameObjectsWithTag("Curse");
        foreach (GameObject curse in curses)
        {
            Debug.Log("DestroyedCurse");
            Destroy(curse);
        }

        StreamReader sr1 = new StreamReader(curseSavePath);

        for (int i = 0; !sr1.EndOfStream; i++)
        {
            string line = sr1.ReadLine();
            string[] Splitted = line.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            if (Splitted.Length > 0)
            {
                GameObject instantiatedCurse = Instantiate(curseTypes, new Vector3(float.Parse(Splitted[0]), float.Parse(Splitted[1]), float.Parse(Splitted[2])), Quaternion.Euler(0, 0, 0));
                instantiatedCurse.GetComponent<SphereCollider>().enabled = true;
                instantiatedCurse.GetComponent<CurseBehaviour>().enabled = true;
                instantiatedCurse.GetComponent<SphereCollider>().enabled = true;
                Debug.Log(4);
            }
        }
        sr1.Close();
    }
}
