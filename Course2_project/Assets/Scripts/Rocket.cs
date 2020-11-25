using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Rocket : MonoBehaviour
{
    [SerializeField] Text energyText;
    [SerializeField] int energyTotal = 800;
    [SerializeField] int energyApply = 30;
    [SerializeField] float rotSpeed = 100f;
    [SerializeField] float flySpeed = 100f;

    [SerializeField] AudioClip flySound;
    [SerializeField] AudioClip boomSound;
    [SerializeField] AudioClip finishSound;

    [SerializeField] ParticleSystem flyParticles;
    [SerializeField] ParticleSystem finishParticles;
    [SerializeField] ParticleSystem boomParticles;

    bool collisionOff = false;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Playing, Dead, NextLevel };
    State state = State.Playing;

    // Start is called before the first frame update
    void Start()
    {
        energyText.text = energyTotal.ToString();
        state = State.Playing;
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Playing && energyTotal > 5)
        {
            Launch();
            Rotation();

            if (Debug.isDebugBuild)
            {
                DebugKeys();
            }
        }
    }

    void DebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }

        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionOff = !collisionOff;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Playing || collisionOff)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;

            case "Finish":
                Finish();
                break;

            case "Battery":
                PlusEnergy(500, collision.gameObject);
                break;

            default:
                Lose();
                break;
        }
    }

    void PlusEnergy(int energytoAdd, GameObject batteryObject)
    {
        batteryObject.GetComponent<BoxCollider>().enabled = false;
        energyTotal += energytoAdd;
        energyText.text = energyTotal.ToString();
        Destroy(batteryObject);
    }

    void LoadNextLevel() //finish
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        int nextLevelIndex = currentLevelIndex + 1;

        if (nextLevelIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextLevelIndex = 0;
        }

        SceneManager.LoadScene(nextLevelIndex);
    }

    void LoadFirstLevel() //lose
    {
        SceneManager.LoadScene(1);
    }

    void Launch()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            energyTotal -= Mathf.RoundToInt(energyApply * Time.deltaTime);
            energyText.text = energyTotal.ToString();
            rigidBody.AddRelativeForce(Vector3.up * flySpeed * Time.deltaTime);

            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(flySound);
                flyParticles.Play();
            }

            else
            {
                audioSource.Pause();
                flyParticles.Stop();
            }
        }
    }

    void Rotation()
    {
        float rotationSpeed = rotSpeed * Time.deltaTime;

        rigidBody.freezeRotation = true;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed);
        }

        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }

        rigidBody.freezeRotation = false;
    }

    void Finish()
    {
        state = State.NextLevel;
        audioSource.Stop();
        audioSource.PlayOneShot(finishSound);
        finishParticles.Play();
        Invoke("LoadNextLevel", 2f);
    }

    void Lose()
    {
        print("RocketBoom!");
        state = State.Dead;
        audioSource.Stop();
        audioSource.PlayOneShot(boomSound);
        boomParticles.Play();
        Invoke("LoadFirstLevel", 2f);
    }
}
