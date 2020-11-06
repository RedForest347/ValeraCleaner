using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;
using UnityEditor;
using UnityEngine.Events;

public class AttackCmp : ComponentBase, ICustomAwake
{
	public UnityEvent unityEvent;
    public Attack[] attackList;


	public void OnAwake()
	{

	}

	private void OnDrawGizmos()
    {

        for (int i = 0; i < attackList.Length; i++)
        {
			if (attackList[i].attackZone.showZone)
			{
				Quaternion rotation = Quaternion.Euler(0, 0, -attackList[i].attackZone.angleOffset);
				Gizmos.color = attackList[i].attackZone.color;

				Vector3 pos = attackList[i].attackZone.cubeSize;
				if (pos.x < 0) pos.x = 0;
				if (pos.y < 0) pos.y = 0;
				if (pos.z < 0) pos.z = 0;
				attackList[i].attackZone.cubeSize = pos;

				Gizmos.DrawWireMesh(CreateCube(), transform.position + (Vector3)((Vector2)transform.right * attackList[i].attackZone.distance)
					.Rotate(attackList[i].attackZone.angleOffset), rotation, attackList[i].attackZone.cubeSize);
			}
		}
	}

    private void OnTriggerStay(Collider other)
    {

    }

    private Mesh CreateCube()
	{
		Vector3[] vertices = {
			new Vector3 (-0.5f, -0.5f, -0.5f),
			new Vector3 (0.5f, -0.5f, -0.5f),
			new Vector3 (0.5f, 0.5f, -0.5f),
			new Vector3 (-0.5f, 0.5f, -0.5f),
			new Vector3 (-0.5f, 0.5f, 0.5f),
			new Vector3 (0.5f, 0.5f, 0.5f),
			new Vector3 (0.5f, -0.5f, 0.5f),
			new Vector3 (-0.5f, -0.5f, 0.5f),
		};

		int[] triangles = {
			0, 2, 1, //face front
			0, 3, 2,
			2, 3, 4, //face top
			2, 4, 5,
			1, 2, 5, //face right
			1, 5, 6,
			0, 7, 4, //face left
			0, 4, 3,
			5, 4, 7, //face back
			5, 7, 6,
			0, 6, 7, //face bottom
			0, 1, 6
		};

		Mesh mesh = new Mesh();
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.Optimize();
		mesh.RecalculateNormals();

		return mesh;
	}

}

[System.Serializable]
public struct Attack
{
	public string attackName;

	public float damage;
	public float timeBetweenAttacks;
	public float timeAfterLastAttack;

	public float pushForce;

	public EffectBase[] effects;
	public AttackZone attackZone;
}

[System.Serializable]
public class AttackZone
{
	public bool showZone;
	public Color color;

	public LayerMask layerMask;
	public float distance;
	public float angleOffset;
	public Vector3 cubeSize;

}
