using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Represents an item with an effect.
/// Implements the Slotable interface so it can be sloted in the ItemQuickAccess and the Inventory.
/// </summary>
public class Item : NetworkBehaviour , Slotable
{
	[SerializeField]
	private	Effect effect;

	[SerializeField]
	private SpriteRenderer spriteRenderer;

	private bool hasBeenPickedUp;

	private void Start()
	{
		hasBeenPickedUp = false;
	}

	public void activate(Player player) {
		player.CmdItemWasUsed(this.gameObject);
	}

	public Sprite getIcon()
	{
		return spriteRenderer.sprite;
	}

	public GameObject getInstance()
	{
		return this.gameObject;
	}

	public bool wasPickedUp()
	{
		return hasBeenPickedUp;
	}

	public Effect getEffect()
	{
		return effect;
	}

	public void pickUpItem()
	{
		spriteRenderer.enabled = false;
		hasBeenPickedUp = true;
		Debug.Log("TRUE");
	}

}
