using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Robot Target { get; set; }
    public int Power { get; set; }
    public float Speed { get; set; }
    public float ReachTargetDisance { get; set; }

    // Start is called before the first frame update
    void Awake()
    {
        this.Speed = 5f;
        this.ReachTargetDisance = .25f;
    }

    // Update is called once per frame
    void Update()
    {
        // Remove arrow if no ennemi available
        if (this.Target == null || this.Target.IsDead())
        {
            Destroy(gameObject);
            return;
        }

        Vector3 targetPosition = this.Target.transform.position;
        Vector3 moveDir = (targetPosition - transform.position).normalized;
        transform.position += moveDir * this.Speed * Time.deltaTime;

        float angle = Utils.GetAngleFromVectorFloat(moveDir);
        transform.eulerAngles = new Vector3(0, 0, angle);

        if (Vector3.Distance(transform.position, targetPosition) < this.ReachTargetDisance)
        {
            // Damage
            this.Target.Damage(this.Power, transform.position);
            // Knockback
            //this.Target.transform.position += moveDir * .25f;

            // Destroy arrow
            Destroy(gameObject);
        }
    }
}
