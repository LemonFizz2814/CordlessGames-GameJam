using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIProperties : MonoBehaviour
{
    public GangController.Gangs gang;

    public float accuracy;
    public float firerate;
    public float speed;
    public float damage;
    public float maxHealth;
    public float attackDelayMin;
    public float attackDelayMax;
    public float shootDelay;

    public int burstMin;
    public int burstMax;
}
