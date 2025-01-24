using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

[RequireComponent(typeof(ObiSolver))]
public class LineCollisionDetector : MonoBehaviour
{

    ObiSolver solver;
    public PuzzleManager_first puzzleManager;

    public GameObject fuzeObject;
  

    void Awake()
    {
        solver = GetComponent<Obi.ObiSolver>();
    }

    void OnEnable()
    {
        solver.OnCollision += Solver_OnCollision;
    }

    void OnDisable()
    {
        solver.OnCollision -= Solver_OnCollision;
    }

    void Solver_OnCollision(object sender, ObiNativeContactList e)
    {
        var colliderWorld = ObiColliderWorld.GetInstance();

        for (int i = 0; i < e.count; ++i)
        {
            Oni.Contact c = e[i];
            // make sure this is an actual contact:
            if (c.distance < 0.01f)
            {
                // get the collider:
                var col = colliderWorld.colliderHandles[c.bodyB].owner;

                if (col.gameObject.Equals(fuzeObject))
                {
                    puzzleManager.lineAttached = true;
                }
                else
                {
                    puzzleManager.lineAttached = false;
                }
            }
        }
    }
}


