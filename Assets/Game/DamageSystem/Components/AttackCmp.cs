using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;
using UnityEditor;
using UnityEngine.Events;
using System;

public class AttackCmp : ComponentBase, ICustomAwake
{
	
    public Attack[] attackList;
	public Action<int, Attack> OnAttack;
	public int currentAttackType;
	public Animator animator;

	public void OnAwake()
	{
		animator = GetComponent<Animator>();
	}

	public void Kick()
    {

		OnAttack(entity, attackList[currentAttackType]);
	}

	private void OnDrawGizmos()
    {

        for (int i = 0; i < attackList.Length; i++)
        {
			if (attackList[i].attackZone.showZone)
			{
				Quaternion rotation = Quaternion.Euler(0, 0, -(attackList[i].attackZone.angleOffset - GetComponent<MoverCmp>().rotation));
				Gizmos.color = attackList[i].attackZone.color;

				Vector3 pos = attackList[i].attackZone.cubeSize;
				if (pos.x < 0) pos.x = 0;
				if (pos.y < 0) pos.y = 0;
				if (pos.z < 0) pos.z = 0;
				attackList[i].attackZone.cubeSize = pos;

				Gizmos.DrawWireMesh(CreateCube(), transform.position + (Vector3)((Vector2)transform.right * attackList[i].attackZone.distance)
					.Rotate(-rotation.eulerAngles.z), rotation, attackList[i].attackZone.cubeSize);
			}
		}
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

	//public UnityEvent unityEvent;
	public float timeBetweenAttacks;
	[HideInInspector]
	public float timeAfterLastAttack;

	public float pushForce;

	public EffectBase[] effects;
	public AttackZone attackZone;
}

[System.Serializable]
public class AttackZone
{
	public bool showZone;
	public Color color = Color.blue;

	public LayerMask layerMask;
	public float distance;
	public float angleOffset;
	public Vector3 cubeSize;

}
