using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUI : MonoBehaviour
{
    public Transform follow;
    public GameObject icon;
    private GameManager gmScript;

    private bool isKing;

    // Start is called before the first frame update
    void Start()
    {
        isKing = transform.parent.gameObject.GetComponent<Piece>().isKing;
        transform.parent = null;

        gmScript = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float damping = 5f;
        var target = GameObject.Find("Main Camera");

        var lookPos = target.transform.position - transform.position;
        if (gmScript.isOverhead == false)
        {
            lookPos.y = 0;
            transform.localScale = new Vector3(1, 1, 1);

            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);

            if (isKing == false) icon.SetActive(false);
        }
        else
        {
            transform.localScale = new Vector3(3, 3, 3);
            transform.eulerAngles = new Vector3(90f, 0, 0);

            icon.SetActive(true);
        }
        

        transform.position = follow.position;

    }
}
