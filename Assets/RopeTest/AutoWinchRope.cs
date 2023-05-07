using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class AutoWinchRope : MonoBehaviour
{
    [SerializeField]
    GameObject boomTarget;

    [SerializeField]
    GameObject partPrefab;


    [SerializeField]
    bool spawn = false;

    public float partHeight = 0.24f;
    public float distanceDivisor = 0.24f;
    public float yOffsetModifier = 0;
    public int partCountModifier = 0;
    public bool shouldFollowBoom = false;
    public bool readyToFollow = false;

    void Start()
    {
        //partHeight = partPrefab.transform.localScale.y * 2;
    }

    void Update()
    {
        Debug.DrawLine(transform.position, boomTarget.transform.position, Color.green, 0.01f);
        FollowBoom();


        if (Input.GetKeyDown(KeyCode.P) || spawn)
        {
            DestroyRope();
            SpawnRope();
            spawn = false;
        }
    }

    float GetDistance()
    {
        return (transform.position - boomTarget.transform.position).magnitude;
    }

    Transform[] RetrieveRopeParts()
    {
        Transform[] childTransforms = GetComponentsInChildren<Transform>();
        return childTransforms.Where(t => t.tag == "Rope").ToArray();
    }

    void FollowBoom()
    {
        Transform[] ropeParts = RetrieveRopeParts();
        if (boomTarget && ropeParts.Length >= 2 && readyToFollow && shouldFollowBoom)
        {
            Transform part = ropeParts[ropeParts.Length - 1];
            part.transform.position = boomTarget.transform.position;
            part.transform.localEulerAngles = new Vector3(0, 0, -90);
        }
    }

    void DestroyRope()
    {
        foreach (var p in RetrieveRopeParts())
        {
            Destroy(p.gameObject);
        }
    }

    void SpawnRope()
    {
        readyToFollow = false;
        float length = GetDistance();
        int count = (int)(length / distanceDivisor) + partCountModifier;
        Debug.Log($"dist {length} / x {length / distanceDivisor} / count {count}");

        GameObject prevObj = null;
        for (int x = 0; x < count; x++)
        {
            bool snap = x == 0 || x == count - 1;
            GameObject obj = SpawnNode(x, snap, prevObj);

            if (x == 0)
            {
                obj.transform.localEulerAngles = new Vector3(0, 0, -90);
                Destroy(obj.GetComponent<CharacterJoint>());
                Destroy(obj.GetComponent<CapsuleCollider>());
                Destroy(obj.GetComponent<RopeNodeCollider>());
            }

            prevObj = obj;
        }
        readyToFollow = true;
    }

    GameObject SpawnNode(int i, bool snap, GameObject parent)
    {
        GameObject obj;
        Vector3 position = (parent) ? parent.transform.position : transform.position;

        float xOffset = (i == 0) ? -(partHeight / 2) : 0;
        float yOffset = (i == 0) ? 0 : partHeight;
        obj = Instantiate(
            partPrefab,
            new Vector3(
                position.x + xOffset,
                position.y + yOffset,
                position.z
            ),
            Quaternion.identity,
            transform
        );
        obj.name = i.ToString();

        if (parent)
        {
            obj.GetComponent<CharacterJoint>().connectedBody = parent.GetComponent<Rigidbody>();
        }

        if (snap)
        {
            //obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            obj.GetComponent<Rigidbody>().isKinematic = true;
        }

        return obj;
    }

}