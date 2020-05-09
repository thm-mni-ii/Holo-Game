﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents a projectile which jumps to the next closest target
/// after hitting its first target.
/// Targets that were already hit are stored in hitEnemies, so they can not be
/// hit twice.
/// </summary>
public class BouncingProjectile : Projectile
{
	[SerializeField] private int maxBounces;
	[SerializeField] private LayerMask enemyLayer;

	private int currBounces;

	private GameObject closestTarget;

	private List<Collider> hitEnemies;

	/// <summary>
	/// Start is called before the first frame update.
	/// </summary>
    void Start()
    {
	    hitEnemies = new List<Collider>();
	    currBounces = 0;
    }

	/// <summary>
	/// Update is called once per frame.
	/// If the projectile hit an enemy, it moves towards the closestTarget.
	/// If the target dies before it is reached, the projectile looks for a new target.
	/// </summary>
    private void Update()
    {
	    if (currBounces > 0)
	    {
		    if (closestTarget == null)
		    {
			    updateTarget();
		    }
		    Vector3 dir = (closestTarget.transform.position - this.transform.position).normalized * (speed * 2);
		    GetComponent<Rigidbody>().velocity = dir;

		    transform.LookAt(closestTarget.transform);
	    }
    }

	/// <summary>
	/// Hits the target and looks for the next target.
	/// Is called by parent class, when a trigger was hit.
	/// If the projectile has reached maxBounces it is destroyed.
	/// </summary>
	/// <param name="hit">Target that was hit</param>
    protected override void onTriggerHit(Collider hit)
    {
	    onHit(hit.GetComponent<Unit>());
		currBounces++;
		hitEnemies.Add(hit);

		if (currBounces < maxBounces)
		{
			updateTarget();
		}
		else
		{
			Destroy(gameObject);
		}
    }

	/// <summary>
	/// Looks for the closest enemy in range.
	/// Ignories enemies that were already hit once.
	/// If there is no closer target found, the projectile destroys itself.
	/// </summary>
    private void updateTarget()
    {
	    Collider[] enemiesInRange = Physics.OverlapSphere(this.transform.position, 20f, enemyLayer);

	    float minDist = Mathf.Infinity;
	    foreach (Collider enemy in enemiesInRange)
	    {
		    if (!hitEnemies.Contains(enemy))
		    {
			    float distToEnemy = Vector3.Distance(this.transform.position, enemy.transform.position);
			    if (distToEnemy < minDist)
			    {
				    closestTarget = enemy.gameObject;
				    minDist = distToEnemy;
			    }
		    }
	    }

	    if (float.IsPositiveInfinity(minDist))
	    {
		    Destroy(gameObject);
	    }

    }

}
