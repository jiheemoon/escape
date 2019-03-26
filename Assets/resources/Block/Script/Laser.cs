using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public bool Enable = true;
    public char Direction = 'U';
    public float MaxLength = 100;

    void Start()
    {

    }
    // Update is called once per frame
    void Update ()
    {
        if (Enable)
        {
            transform.GetComponent<LineRenderer>().enabled = true;

            switch (Direction)
            {
                case 'U':
                    StartCoroutine(LaserSet(0));
                    break;

                case 'R':
                    StartCoroutine(LaserSet(1));
                    break;

                case 'D':
                    StartCoroutine(LaserSet(2));
                    break;

                case 'L':
                    StartCoroutine(LaserSet(3));
                    break;
            }
        }
        else transform.GetComponent<LineRenderer>().enabled = false;
    }

    IEnumerator LaserSet(int index)
    {
        RaycastHit2D HitObject;
        Vector3[] Pos = new Vector3[2];
        Vector2[] PosVector = new Vector2[] { Vector2.up, Vector2.right, Vector2.down, Vector2.left };

        Pos[0] = transform.GetChild(index).position;
        int LayerMask = 1 << 8;
        HitObject = Physics2D.Raycast(transform.GetChild(index).position, transform.GetChild(index).TransformDirection(PosVector[index]), MaxLength, LayerMask);

        if (HitObject.collider != null)
        {
            Pos[1] = HitObject.point;
            transform.GetComponent<LineRenderer>().SetPositions(Pos);
            OnLaserEnter(HitObject);
        }
        else
        {
            float Dx = transform.GetChild(index).TransformDirection(PosVector[index]).x;
            float Dy = transform.GetChild(index).TransformDirection(PosVector[index]).y;
            float z = Mathf.Sqrt(Mathf.Pow(Dx, 2) + Mathf.Pow(Dy, 2));
            float Cos = Dx / z;
            float Sin = Dy / z;

            Pos[1] = new Vector2(transform.GetChild(index).position.x + (Cos * MaxLength), transform.GetChild(index).position.y + (Sin * MaxLength));
            transform.GetComponent<LineRenderer>().SetPositions(Pos);
        }
        yield return null;
    }

    void OnLaserEnter(RaycastHit2D HitObject)
    {
        if(HitObject.transform.name == "Player")
        {
            GameObject.Find("Player").GetComponent<Player>().Helth = 0;
        }
    }
}
