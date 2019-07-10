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

    void OnCollisionEnter(Collision col)
    {
        //当Collision/Rigidbody触发另一个时调用
        GameObject victim = col.collider.gameObject;

        GameObject[] pieces = BLINDED_AM_ME.MeshCut.Cut(victim, col.contacts[0].point, new Vector3( col.contacts[0].normal.y, col.contacts[0].normal.x, 0), capMaterial);

        if (!pieces[1].GetComponent<Rigidbody>())
            pieces[1].AddComponent<Rigidbody>();

        pieces[0].GetComponent<Collider>().enabled = false;

        Destroy(pieces[0], 1);
        Destroy(pieces[1], 1);

        Instantiate(victim, victim.transform.position, Quaternion.identity);
    }
}
