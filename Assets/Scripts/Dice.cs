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
    private int diceValue = -1;

    private void Awake()
    {
        initialPos = transform.position;
        _rb = GetComponent<Rigidbody>();
    }
    

    [Button("Roll Dice")]
    public void RollTheDice()
    {
        StopAllCoroutines();
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
        _rb.isKinematic = true;
    }

    public int GetDiceValue() => diceValue;
}
