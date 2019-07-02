using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 剑切开方块的脚本
/// </summary>
public class SaberCut : MonoBehaviour {

    public Material capMaterial;

    // Use this for initialization
    void Start() {

    }

    void OnCollisionEnter(Collision other)
    {
        //当Collision/Rigidbody触发另一个时调用
        GameObject victim = other.collider.gameObject;

        GameObject[] pieces = BLINDED_AM_ME.MeshCut.Cut(victim, transform.position, transform.right, capMaterial);

        if (!pieces[1].GetComponent<Rigidbody>())
            pieces[1].AddComponent<Rigidbody>();

        Destroy(pieces[1], 1);
    }
}
