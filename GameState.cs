using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private GameObject player;
    private MazeGenerator mazeGenerator;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        mazeGenerator = GameObject.FindGameObjectWithTag("Maze").GetComponent<MazeGenerator>();
        player.transform.position = mazeGenerator.start;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Home))
        {
            Application.Quit();
        }
    }
}
