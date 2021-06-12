using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_player_mover : MonoBehaviour
{

    public float speed;

    private Transform trans;
    

    // Start is called before the first frame update
    void Start()
    {
        trans = this.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float z_move = Input.GetAxis("Vertical");
        float x_move = Input.GetAxis("Horizontal");

        Vector3 move_vec = new Vector3(x_move, z_move, 0);

        trans.position = trans.position + (move_vec * speed * Time.deltaTime);
    }
}
