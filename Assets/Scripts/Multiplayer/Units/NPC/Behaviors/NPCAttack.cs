/* edited by: SWT-P_WS_2018_Holo */
using Multiplayer;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

///<summary>
/// This class manages the attacks NPCs perform against players.
/// The NPCs choose this behaviour after chasing a player down and being close enough to attack.
/// After a NPC has attacked it waits a certain time until it will attack the player again.
/// The amount of damage a NPC deals to a player is determined by which role the player and NPC represent.
/// Every playertype will receive little damage by NPCs of the same role and there is one NPCrole that will deal lots of damage.
/// Special enemy types are handled in this script. The huge, slow enemy will deal very much damage and the small kamikaze enemies deal a considerable amount of damage, too.
/// Another special enemy is the pyro enemy, which will deal damage over time to the players but no instant damage on the hit.
/// The overall amount of damage can be manipulated by changing the value of parameter mul. It is set to 3 by standard to ensure balanced gameplay.
/// </summary>

namespace NPC
{
	public class NPCAttack : NPCBehavior
	{
		private float maxDistance;
		private float attackDelay;
		private bool canAttack;
		public MonoBehaviour monoInstance;

		///<summary>
		/// In this function the damagetable is initialised as a 2 dimensional array.
		/// The first index resembles the players role as an int value. The second index is the NPCs role as an int value.
		/// <param name="maxDistance"> Distance between NPC and player in which NPCs are able to deal damage to players</param>
		/// <param name="attackDelay"> Amount of time NPCs take to regain their ability to attack </param>
		///</summary>
		public NPCAttack(float maxDistance, float attackDelay, MonoBehaviour monoInstance)
		{
			this.maxDistance = maxDistance;
			this.attackDelay = attackDelay;
			this.monoInstance = monoInstance;
			canAttack = true;

		}

		///<summary>
		/// In the Act method NPCs deal damage to players after calculating the amount of damage.
		/// To calculate the damage this script multiplies the entry in the damagetable according to player and NPC role with the overall multiplier
		/// After dealing damage, the NPC will be unable to attack and a coroutine to reset this ability
		///</summary>
		public override void Act(Transform npc, Transform target)
		{
			Enemy self = npc.GetComponent<Enemy>();
			Unit player = target.GetComponent<Unit>();

			if (!player.isDead())
			{
				self.hit(player);
			} else
			{
				NPCManager.Instance.RemoveTarget(player.transform);
				if (player.tag.Equals("Player"))
				{
					GameOverManager.Instance.RemoveProf(player.GetComponent<Player>());
				}
				GameObject[] npcs = GameObject.FindGameObjectsWithTag("Enemy");
				foreach (GameObject go in npcs)
				{
					go.GetComponent<NPC>().againLive();
				}
			}
		}

		///<summary>
		/// This method determines wether the player is still within maximum attacking distance after attacking
		///</summary>
		public override void Reason(Transform npc, Transform target, NPCController controller)
		{
			{
				return;
			}
			if (Vector3.Distance(npc.position, target.position) > maxDistance)
			{
				controller.SetTransition(Transition.LostTarget);
			}

		}
	}
}
