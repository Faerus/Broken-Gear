using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DestroyOnRightClick : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // Check remaining life
            float proportion = 1;
            if(HealthSystem.TryGetHealthSystem(gameObject, out HealthSystem healthSystem))
            {
                proportion = healthSystem.GetHealthPercent();
            }

            // Refund proportional gears
            if (this.TryGetComponent<Robot>(out Robot robot))
            {
                GameManager.RegisterRobotDead(robot.TeamColor);
                GameManager.AddGears(robot.TeamColor, Mathf.RoundToInt(GameManager.PRICE_ROBOT * proportion));
            }
            else if (this.TryGetComponent<Drill>(out Drill drill))
            {
                GameManager.AddGears(drill.TeamColor, Mathf.RoundToInt(GameManager.PRICE_DRILL* proportion));
            }
            else if (this.TryGetComponent<Factory>(out Factory factory))
            {
                GameManager.AddGears(factory.TeamColor, Mathf.RoundToInt(GameManager.PRICE_FACTORY * proportion));
            }
            else if (this.TryGetComponent<Turret>(out Turret turret))
            {
                GameManager.AddGears(turret.TeamColor, Mathf.RoundToInt(GameManager.PRICE_TURRET * proportion));
            }

            // And remove
            Destroy(gameObject);
        }
    }
}
