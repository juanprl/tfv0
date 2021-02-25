using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;


public class SujetarObjeto : MonoBehaviour
{
    SpeedForce m_speedForce;
    ParentConstraint m_parentConstraint;
    Rigidbody m_rigidbody;
    ConstraintSource constraintSource;

    float weight;
    public float f_weight_read_only;


    //Si es solo copiar la posición del objeto invisble se puede hacer con un copiar Transform (Translate.position/rotation = target.position/rotation),
    //pero que es más óptimo y correcto, script o ParentConstraint?

    //Para que sea cogido por algo su weight tiene que estar en 0


    void Start()
    {
        m_speedForce = GetComponent<SpeedForce>();
        m_parentConstraint = GetComponent<ParentConstraint>();
        m_rigidbody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        f_weight_read_only = m_parentConstraint.weight;

        /*Si m_parentConstraint ha sido asignado ha alguien.*/
        //sino m_parentConstraintenabled = true;
        if (m_parentConstraint.weight == 1 && !m_speedForce.itMoved )
        {
           // m_rigidbody.isKinematic = true;
            m_rigidbody.useGravity = false;
            //boxcollider is trigger
        }

        if (m_speedForce.itMoved)
        {
            m_parentConstraint.RemoveSource(0); //Estamos considerando que solo hay un constraint
            //m_parentConstraint.enabled = false;
            m_parentConstraint.weight = 0;
            m_rigidbody.isKinematic = false;
            //boxcollider is trigger
            m_rigidbody.useGravity = true;

            //Desactivar el arma asociada a dicho personaje que le sujeta.
            //Si weight == 0 gameobjectX = null
            //
        }

        //if (x.activeSelf == false && x) print("hello");
    }

    public void SetConstraint(GameObject x)
    {
        //m_parentConstraint.enabled = true;
        
        m_parentConstraint.RemoveSource(0); //Quita el anterior constraint si lo tuviese, suponemos que solo trabajamos con uno.
        m_parentConstraint.weight = 1;

        constraintSource.sourceTransform = x.transform;     //Añadimos el nuevo constraint. //Indicamos con la influencia con la que empieza, del 0 al 1.
        constraintSource.weight = 1;
        m_parentConstraint.AddSource(constraintSource);
    }

    public void SetOffConstraint()
    {
        m_parentConstraint.RemoveSource(0);
    }
}
