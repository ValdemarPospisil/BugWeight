using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyTypeData", menuName = "Enemy/EnemyTypeData")]
public class EnemyTypeData : ScriptableObject
{
    public string typeName;
    public GameObject prefab;
    public float moveSpeed;
    public float attackDamage;
    public float attackSpeed;
    public float xpDrop;
    public int enemyCurentHP;
    public int enemyMaxHP;
}
