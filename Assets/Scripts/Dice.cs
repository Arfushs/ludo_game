using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dice : MonoBehaviour
{
    [SerializeField] private float _torqueMin = 0.1f;
    [SerializeField] private float _torqueMax = 2f;
    [SerializeField] private float _throwStrengthMin = .3f;
    [SerializeField] private float _throwStrengthMax = .45f;
    [SerializeField] private float _horizontalThrowStrength = .2f;
    [SerializeField] private Vector3 _throwVector;
    private Vector3 initialPos;
    private Rigidbody _rb;
    private MeshRenderer _mesh;
    private int diceValue = -1;
    [Header("Colliders")] 
    [SerializeField] private LayerMask boardLayer;
    [SerializeField] private float _colliderRadius;
    [SerializeField] private Transform _value3Transform;
    [SerializeField] private Transform _value2Transform;
    [SerializeField] private Transform _value1Transform;
    [SerializeField] private Transform _value4Transform;
    [SerializeField] private Transform _value5Transform;
    [SerializeField] private Transform _value6Transform;

    private void Awake()
    {
        _mesh = GetComponentInChildren<MeshRenderer>();
        _mesh.enabled = false;
        initialPos = transform.position;
        _rb = GetComponent<Rigidbody>();
    }
    

    [Button("Roll Dice")]
    public void RollTheDice()
    {
        _mesh.enabled = true;
        diceValue = -1;
        StopAllCoroutines();
        //TODO pozisyon işlemini burda yapma
        transform.position = initialPos;
        _rb.isKinematic = false;
        Vector3 horizontalVector = new Vector3(0, 0, 1) * Random.Range(-_horizontalThrowStrength, _horizontalThrowStrength);
        _rb.AddForce(_throwVector * Random.Range(_throwStrengthMin,_throwStrengthMax) + horizontalVector,ForceMode.Impulse);
        _rb.AddTorque(transform.forward * Random.Range(_torqueMin,_torqueMax) + transform.up * Random.Range(_torqueMin,_torqueMax) + transform.right * Random.Range(_torqueMin,_torqueMax));
        StartCoroutine(WaitForStop());
        
    }

    IEnumerator WaitForStop()
    {
        yield return new WaitForFixedUpdate();
        while (_rb.angularVelocity.sqrMagnitude > 0.1 || _rb.velocity.sqrMagnitude > 0.1)
        {
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(.5f);
        CheckAndSeteDiceValue();
        Debug.Log(GetDiceValue());
    }

    public int GetDiceValue() => diceValue;

    public void CheckAndSeteDiceValue()
    {
        if (Physics.CheckSphere(_value1Transform.position, _colliderRadius, boardLayer))
            diceValue = 1;
        else if (Physics.CheckSphere(_value2Transform.position, _colliderRadius, boardLayer))
            diceValue = 2;
        else if (Physics.CheckSphere(_value3Transform.position, _colliderRadius, boardLayer))
            diceValue = 3;
        else if (Physics.CheckSphere(_value4Transform.position, _colliderRadius, boardLayer))
            diceValue = 4;
        else if (Physics.CheckSphere(_value5Transform.position, _colliderRadius, boardLayer))
            diceValue = 5;
        else if (Physics.CheckSphere(_value6Transform.position, _colliderRadius, boardLayer))
            diceValue = 6;
        
    }

    [Button("Pickup Dice")]
    public void PickUpDiceFromTable()
    {
        _mesh.enabled = false;
        _rb.isKinematic = true;
        transform.position = new Vector3(10, 10, 10);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_value3Transform.position,_colliderRadius);
        Gizmos.DrawWireSphere(_value2Transform.position,_colliderRadius);
        Gizmos.DrawWireSphere(_value1Transform.position,_colliderRadius);
        Gizmos.DrawWireSphere(_value4Transform.position,_colliderRadius);
        Gizmos.DrawWireSphere(_value5Transform.position,_colliderRadius);
        Gizmos.DrawWireSphere(_value6Transform.position,_colliderRadius);
    }
}
