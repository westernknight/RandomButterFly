using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ParticleController : MonoBehaviour
{
    public static ParticleController instance;


    List<ParticleButterfly> particles = new List<ParticleButterfly>();

    public float duration = 5;
    public bool isLooping = true;
    public float startLife = 5;
    public float startSpeed = 5;
    [Range(0.01f,1000)]
    public float rate = 1;//1miao 1ge 

    //shape
    public float sideAngle = 25;
    public float upAngle = 25;
    public float scale = 1;
    public float playbackSpeed = 1;
    public float playbackTime = 0;
    public GameObject butterflyPrefab;

    List<GameObject> particlePool = new List<GameObject>();
    public List<GameObject> activeParticles = new List<GameObject>();

    bool isStoped = false;

    void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < 1000; i++)
        {
            particlePool.Add(GameObject.Instantiate(butterflyPrefab) as GameObject);
            particlePool[i].transform.parent = transform;
            particlePool[i].transform.localScale = Vector3.one * scale; 
            particlePool[i].SetActive(false);
        }
        isStoped = true;
    }
    GameObject Initantiate()
    {
        for (int i = 0; i < 1000; i++)
        {
            if (particlePool[i].activeSelf == false)
            {
                particlePool[i].SetActive(true);
                activeParticles.Add(particlePool[i]);
                return particlePool[i];
            }
        }
        return null;
    }
    void Destory(GameObject go)
    {
        activeParticles.Remove(go);
        go.SetActive(false);
    }
    int debug = 0;

    public void StopEmit()
    {
        isStoped = true;
        playbackTime = 0;
    }
    public void PlayEmit()
    {
        isStoped = false;
    }
    public void Clean()
    {
        for (int i = 0; i < 1000; i++)
        {
          
                particlePool[i].SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
       
        

        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].transform.position = particles[i].transform.position + particles[i].transform.forward * Time.deltaTime * startSpeed * playbackSpeed;
            particles[i].life -= Time.deltaTime;
            if (particles[i].life<0)
            {


                Destory(particles[i].gameObject);
                particles.RemoveAt(i);
                i--;
            }
        }
        if (isStoped)
        {
            return;
        }
        playbackTime += Time.deltaTime;
        if (playbackTime > 1/rate && butterflyPrefab != null )
        {
            GameObject go = Initantiate();
            if (go)
            {
             
                go.transform.parent = transform;
                go.transform.position = transform.position;
                Quaternion q = transform.rotation;
                go.transform.rotation = Quaternion.Euler(/*transform.forward.x*/ q.eulerAngles.x + Random.Range(-upAngle, upAngle), q.eulerAngles.y + Random.Range(-sideAngle, sideAngle), q.eulerAngles.z);
                ParticleButterfly pb = go.GetComponent<ParticleButterfly>();
                pb.life = startLife;
                particles.Add(go.GetComponent<ParticleButterfly>());
                playbackTime -= 1/rate;
            }
           
        }


        
    }
}
