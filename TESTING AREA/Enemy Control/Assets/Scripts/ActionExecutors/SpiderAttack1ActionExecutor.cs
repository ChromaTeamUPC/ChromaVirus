using UnityEngine;
using System.Collections;

public class SpiderAttack1ActionExecutor: BaseExecutor
{
    SpiderAttack1Action spiderAttack1Action;

    public override void SetAction(Action act)
    {
        base.SetAction(act);
        spiderAttack1Action = (SpiderAttack1Action)act;

        // state.agent.Stop();  // comprobar si es necesario

        // si (TIEMPO_DESDE_ULTIMO_ATAQUE > delay)
        //      trigger animación de ataque1
        // sino
        //      ATAQUE_DESECHADO = true // ¿podría hacerse un "return spiderAttack1Action.nextAction;" desde aquí?
    }

    public override int Execute()
    {
        // si (ATAQUE_DESECHADO o ANIMACION_TERMINADA)
        //      return spiderAttack1Action.nextAction;
        // sino
        //    return Action.ACTION_NOT_FINISHED;
        //      

        
        return Action.ACTION_NOT_FINISHED;  // dummy, para retornar algo de momento y que no dé error
    }
}
