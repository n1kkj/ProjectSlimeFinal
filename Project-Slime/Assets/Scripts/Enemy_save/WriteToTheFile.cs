using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WriteToTheFile : MonoBehaviour
{
    string[] savesList;

    /*[SerializeField] GameObject[] enemiesTypes = null;
    [SerializeField] GameObject[] curseTypes = null;*/

    string enemySavePath = "Assets/Scripts/Enemy_save/enemiesSave.txt.txt";
    string curseSavePath = "Assets/Scripts/Enemy_save/curseSave.txt.txt";

    void Start()
    {
        SaveTheProgress();
    }

    public void SaveTheProgress()
    {
            savesList = null;
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("New_enemy");

            int enemiesQuantity = enemies.Length;

            savesList = new string[enemiesQuantity];
            //savesList[0] = enemiesQuantity.ToString();

            for (int j = 0; j < enemiesQuantity; j++)
            {
                Debug.Log("WrittenEnemy");
                savesList[j] = enemies[j].transform.position.x.ToString() + " " + enemies[j].transform.position.y.ToString() + " " + enemies[j].transform.position.z.ToString() + " " + enemies[j].transform.eulerAngles.x.ToString() + " " + enemies[j].transform.eulerAngles.y.ToString() + " " + enemies[j].transform.eulerAngles.z.ToString();
            }
            
        File.WriteAllLines(enemySavePath, savesList);

            savesList = null;
            GameObject[] curses = GameObject.FindGameObjectsWithTag("Curse");

            int cursesQuantity = curses.Length;

            savesList = new string[cursesQuantity];
            //savesList[0] = cursesQuantity.ToString();

            for (int j = 0; j < cursesQuantity; j++)
            {
                Debug.Log("WrittenCurse");
                savesList[j] = curses[j].transform.position.x.ToString() + " " + curses[j].transform.position.y.ToString() + " " + curses[j].transform.position.z.ToString();
            }
                
        File.WriteAllLines(curseSavePath, savesList);
    }
}
