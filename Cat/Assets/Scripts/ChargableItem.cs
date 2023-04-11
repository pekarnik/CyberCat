using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(MeshFilter), typeof(MeshRenderer))]
public class ChargableItem : MonoBehaviour
{
    
    public bool isCorruptionTimerOn = false;
    
    private float corruptionValue = 0f;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private AudioSource audioSource;
    private ItemState state = ItemState.Default;

    [SerializeField] private float chargedActionTimer = 0;

    [Header("Normal")]
    [SerializeField] private Mesh normalMesh;
    [SerializeField] private Material normalMaterial;
    [SerializeField] private AudioClip defaultedAudio;

    [Header("Corrupted")]
    [SerializeField] private Mesh corruptedMesh;
    [SerializeField] private Material corruptedMaterial;
    [SerializeField] private AudioClip corruptedAudio;

    [Header("Charged")]
    [SerializeField] private Mesh chargedMesh;
    [SerializeField] private Material chargedMaterial;
    [SerializeField] private AudioClip chargedAudio;
    [SerializeField] private float timeToChargedAttack;

    public void Awake()
    {
        meshFilter = gameObject.GetComponent<MeshFilter>();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public ItemState GetState()
    {
        return state;
    }

    public void SwitchCharge()
    {
        switch (state)
        {
            case ItemState.Default:
                {
                    state = ItemState.Charged;
                    Debug.Log("object charged");
                    ChangeState(chargedMesh, chargedMaterial, chargedAudio);
                }; break;
            case ItemState.Charged:
                {
                    state = ItemState.Default;
                    Debug.Log("object defaulted");
                    ChangeState(normalMesh, normalMaterial, defaultedAudio);
                }; break;
            case ItemState.Corrupted: {
                    Debug.Log("can't change object state");
                }; break;
        }
        chargedActionTimer = 0;
    }

    private void Update()
    {
        if (state == ItemState.Charged)
        {
            chargedActionTimer += Time.deltaTime;
            if (chargedActionTimer >= timeToChargedAttack) {
                Debug.Log("Do charged action");
                chargedActionTimer = 0;
            }
        }
    }

    private void ChangeState(Mesh newMesh, Material newMaterial, AudioClip audio)
    {
        meshFilter.mesh = newMesh;
        meshRenderer.material = newMaterial;
        if (audio != null)
        {
            audioSource.clip = audio;
            audioSource.Play();
        }
    }

    public enum ItemState
    {
        Default,
        Charged,
        Corrupted
    }

}
