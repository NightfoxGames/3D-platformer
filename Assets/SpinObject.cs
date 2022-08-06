using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinObject : MonoBehaviour
{
    public AnimationCurve curve;
    public Animator anim;
    public GameObject theObject;
    Vector3 sPos;
    public Vector3 newPos;
    public float time;
    
    float elapledTime;
    void Start() {
        sPos  = theObject.transform.position;
    }
    public IEnumerator Spined()
    {
        elapledTime += Time.deltaTime;
        float p = elapledTime / time;
        anim.SetBool("IsSpinning",true);
        theObject.transform.position = Vector3.Lerp(sPos, newPos,curve.Evaluate(p));
        yield return new WaitForSeconds(0.8f);
        anim.SetBool("IsSpinning",false);
    }
    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player"){
            anim.SetBool("IsSpinning",false);
        }
    }
}
