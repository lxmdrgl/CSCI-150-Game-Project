using UnityEngine;

[CreateAssetMenu(fileName = "FireStatus", menuName = "Data/Status Data/Fire Status")]
public class D_FireStatus : ScriptableObject
{
    [field: SerializeField] public float Amount { get; private set; }
    [field: SerializeField] public float Damage { get; protected set;}
    [field: SerializeField] public float Stun { get; protected set;}
    [field: SerializeField] public int Ticks { get; private set;}
    [field: SerializeField] public float Delay { get; private set;}
    [field: SerializeField] public float Mult { get; private set;}
}
