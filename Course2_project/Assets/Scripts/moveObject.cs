using UnityEngine;

[DisallowMultipleComponent]

public class moveObject : MonoBehaviour
{

    [SerializeField] Vector3 movePosition;
    [SerializeField] float moveSpeed;
    [SerializeField] [Range(0, 1.5f)] float moveProgress;
    Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        moveProgress = Mathf.PingPong(Time.time * moveSpeed, 1.5f);
        Vector3 offset = movePosition * moveProgress;
        transform.position = startPosition + offset;
    }
}
