using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    private Rigidbody rigidbody;
    private AudioSource audioSource;
    private State state;
    private bool CollisionsEnabled= true;

    [SerializeField] AudioClip MainEngine;
    [SerializeField] AudioClip Dead;
    [SerializeField] AudioClip Success;
    [SerializeField] int levelLoadDelay;

    [SerializeField] ParticleSystem MainEngineParticles;
    [SerializeField] ParticleSystem SuccessParticles;
    [SerializeField] ParticleSystem DeathParticles;

    public float pushForce = 1;
    public float rotationSpeed = 1;

    enum State
    {
        Dying,
        Transcending,
        Alive
    };

    // Start is called before the first frame update
    void Start()
    {
        state = State.Alive;
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
        if (Debug.isDebugBuild)
        {
            ListenDebugKeys();
        }

    }

    private void ListenDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            NextLevel();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            CollisionsEnabled = !CollisionsEnabled;
        }
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidbody.AddRelativeForce(Vector3.up * pushForce * Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(MainEngine);
            }
            MainEngineParticles.Play();
        }
        else
        {
            audioSource.Stop();
            MainEngineParticles.Stop();
        }
    }

    private void Rotate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateManually(rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateManually(-rotationSpeed * Time.deltaTime);
        }
    }

    private void RotateManually(float rotationThisFrame)
    {
        rigidbody.freezeRotation = true; // Take control of rotation
        transform.Rotate(Vector3.forward * rotationThisFrame);
        rigidbody.freezeRotation = false; //Resume physics
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (state != State.Alive || !CollisionsEnabled) return;

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                {
                    break;
                }
            case "Finish":
                {
                    audioSource.Stop();
                    audioSource.PlayOneShot(Success);
                    state = State.Transcending;
                    SuccessParticles.Play();
                    Invoke("NextLevel", levelLoadDelay);
                    
                    break;
                }
            default:
                {
                    audioSource.Stop();
                    audioSource.PlayOneShot(Dead);
                    state = State.Dying;
                    DeathParticles.Play();
                    Invoke("Die", levelLoadDelay);
                    break;
                }
        }
    }

    private void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }

        state = State.Alive;
    }


    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        state = State.Alive;
    }


}
