using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinControl : MonoBehaviour
{
    private MazeGenerator mazeGenerator;

    // Start is called before the first frame update
    void Start()
    {
        mazeGenerator = GameObject.FindGameObjectWithTag("Maze").GetComponent<MazeGenerator>();
        transform.position = mazeGenerator.end;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Application.LoadLevel(0);
        }
    }
}
