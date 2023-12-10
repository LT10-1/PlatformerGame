using UnityEngine;

public class Trap_Fire : Trap
{
    public bool isWorking;
    private Animator anim;

    [SerializeField] private float repeateRate;
    private void Start()
    {
        anim = GetComponent<Animator>();
        InvokeRepeating("FireSwitch", 0, repeateRate);
    }

    private void Update()
    {

        anim.SetBool("isWorking", isWorking);
    }

    private void FireSwitch()
    {
        isWorking = !isWorking;
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (isWorking)
        {
            base.OnTriggerStay2D(collision);
            
        }
        else
            isWorking = false;
    }
}
