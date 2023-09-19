using Dotted;
using Dotted.Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class ParticleManager : MonoBehaviour
{
    private static ParticleManager _instance;

    public ParticleSystem circleShatter;

    private ObjectPool<ParticleSystem> _circleShatterPool;

    public static ParticleManager Instance => _instance;

    private void Awake()
    {
        if(_instance == null)
            _instance = this;

    }

    private void Start()
    {
        _circleShatterPool = PoolManager.CreateObjectPool<ParticleSystem>(circleShatter);
        _circleShatterPool.CreateObjects(8);
    }

    //Zaten sahnede olanı oynatıyoruz
    public void Play(ParticleSystem particle, bool loop)
    {
        if (particle != null)
        {
            particle.gameObject.SetActive(true);
            ParticleSystem.MainModule main = particle.main;

            main.loop = loop;
            main.stopAction = ParticleSystemStopAction.Destroy;
            particle.Play();
        }

    }


    public void Play(ParticleSystem particle, bool loop,Vector3 pos)
    {
        if (particle != null)
        {
            particle.gameObject.SetActive(true);
            particle.gameObject.transform.position = pos;
            ParticleSystem.MainModule main = particle.main;

            main.loop = loop;
            main.stopAction = ParticleSystemStopAction.Destroy;
            particle.Play();
        }

    }


    public void Stop(ParticleSystem particle,bool destroy)
    {
        ParticleSystem.MainModule main = particle.main;

        if(destroy)
            main.stopAction = ParticleSystemStopAction.Destroy;
        else
            main.stopAction = ParticleSystemStopAction.None;

        particle.Stop();
    }

    //Yeni obje oluşturup pozisyon verip başlatıyoruz
    public ParticleSystem CreateAndPlay(ParticleSystem particle,Transform parent,Vector3 position,bool loop = false,bool isLocalPosition = false)
    {
        ParticleSystem returnPart = null;
        //Eğer particle atanmışsa
        if(particle != null)
        {
            ParticleSystem newParticle = _circleShatterPool.Pop();
            newParticle.AddComponent<ParticleStatus>();
            newParticle.GetComponent<ParticleStatus>().OnDisableEvent += PushParticle;

            if (parent != null)
                newParticle.transform.SetParent(parent);

            newParticle.gameObject.SetActive(true);
            

            ParticleSystem.MainModule main = newParticle.main;
            main.loop = loop;
            //main.stopAction = ParticleSystemStopAction.Destroy;

            if(isLocalPosition)
                newParticle.gameObject.transform.localPosition = position;
            else
                newParticle.gameObject.transform.position = position;

            newParticle.Play();

            returnPart = newParticle;
        }
        return returnPart;
    }

    public GameObject CreateAndPlayRandom(List<ParticleSystem> particles, Transform parent, Vector3 position, bool loop)
    {
        int rand = UnityEngine.Random.Range(0, particles.Count);
        GameObject returnPart = null;
        //Eğer particle atanmışsa
        if (particles[rand] != null)
        {
            ParticleSystem newParticle = _circleShatterPool.Pop();

            if (parent != null)
                newParticle.transform.SetParent(parent);

            newParticle.gameObject.SetActive(true);

            ParticleSystem.MainModule main = newParticle.main;
            main.loop = loop;
            //main.stopAction = ParticleSystemStopAction.Destroy;

            newParticle.gameObject.transform.position = position;

            newParticle.Play();
            returnPart = newParticle.gameObject;
        }
        else
            Debug.LogWarning("Particle list element is null. Element: " + rand);
        
        return returnPart;

    }

    //Belirli bir süree sonra oynatıyoruz
    public IEnumerator CreateAndPlay(ParticleSystem particle, GameObject parent, Vector3 position, bool loop,float time)
    {
        yield return new WaitForSeconds(time);
        //Eğer particle atanmışsa
        if (particle != null)
        {
            ParticleSystem newParticle = Instantiate(particle, parent.transform);
            newParticle.gameObject.SetActive(true);

            ParticleSystem.MainModule main = newParticle.main;
            main.loop = loop;
            main.stopAction = ParticleSystemStopAction.Destroy;

            newParticle.gameObject.transform.position = position;

            newParticle.Play();
        }
    }

    private void PushParticle(ParticleSystem particleSystem)
    {
        _circleShatterPool.Push(particleSystem);
        particleSystem.GetComponent<ParticleStatus>().OnDisableEvent -= PushParticle;
    }

}





