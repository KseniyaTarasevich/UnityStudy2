using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public GameObject block;

    // Start is called before the first frame update
    void Start()
    {
        block.GetComponent<Renderer>().sharedMaterial.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        CreateWall();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateWall()
    {
        StartCoroutine(DeclayBlock());
    }

    IEnumerator DeclayBlock()
    {


        for (int i = 0; i < 100; i++)
        {
            Instantiate(block, new Vector3(Random.Range(-60f, 60f), Random.Range(60f, 90f), 0),
                Quaternion.Euler(Random.Range(-90, 90), Random.Range(-90, 90), Random.Range(-90, 90)));
            yield return new WaitForSeconds(1);

        }
    }
}

