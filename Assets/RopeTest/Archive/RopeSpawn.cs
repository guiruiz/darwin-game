using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSpawn : MonoBehaviour
{
    [SerializeField]
    GameObject partPrefab, parentObject, ropeTarget, fisrtPart;

    [SerializeField]
    [Range(1, 15)]
    int length = 1;

    [SerializeField]
    float partDistance = 0.21f;

    [SerializeField]
    bool reset, spawn, snapFirst, snapLast;

    // Update is called once per frame
    void Update()
    {
        if (reset)
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Rope"))
            {
                Destroy(obj);
            }

            reset = false;
        }

        if (spawn)
        {
            Spawn();
            spawn = false;
        }

        if (ropeTarget && fisrtPart)
        {
            fisrtPart.transform.position = ropeTarget.transform.position;
        }

    }

    void Spawn()
    {
        int count = (int)(length / partDistance);

        for (int x = 0; x < count; x++)
        {
            GameObject tmp;

            tmp = Instantiate(partPrefab, new Vector3(transform.position.x, transform.position.y + partDistance * (x + 1), transform.position.z), Quaternion.identity, parentObject.transform);
            tmp.transform.eulerAngles = new Vector3(180, 0, 0);

            tmp.name = parentObject.transform.childCount.ToString();

            if (x == 0)
            {
                Destroy(tmp.GetComponent<CharacterJoint>());
                if (snapFirst)
                {
                    tmp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                }
                tmp.transform.localEulerAngles = new Vector3(-180, -90, 0);
            }
            else
            {
                tmp.GetComponent<CharacterJoint>().connectedBody = parentObject.transform.Find((parentObject.transform.childCount - 1).ToString()).GetComponent<Rigidbody>();
            }

            if (x == count - 1)
            {
                //tmp.transform.localPosition = new Vector3(-0.14f, 2.48f, 3.86f);
                tmp.transform.localEulerAngles = new Vector3(90, 0, 90);
                fisrtPart = tmp;
            }
        }

        if (snapLast)
        {
            parentObject.transform.Find((parentObject.transform.childCount).ToString()).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }

}
