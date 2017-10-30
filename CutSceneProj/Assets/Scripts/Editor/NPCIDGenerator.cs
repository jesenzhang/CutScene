using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[CustomEditor(typeof(NPCUnitInfo))]
public class NPCIDGenerator// : Editor 
{
	private static int limit = 1;
	private static List<int> idleIDs = new List<int>();

	private static List<bool> idPool = new List<bool>();

	private static void CollectIdleIDs()
	{
		for(int i = 1; i < idPool.Count; i++)
			idPool[i] = false;
		for(int i = idPool.Count; i < limit; i++)
			idPool.Add(false);

		NPCUnitInfo[] npcs = (NPCUnitInfo[])Object.FindObjectsOfType(typeof(NPCUnitInfo));
		foreach(NPCUnitInfo npc in npcs)
		{
			if(npc.id >= 1)
				idPool[npc.id] = true;
		}

		idleIDs.Clear();
		for(int i = 1; i < idPool.Count; i++)
		{
			if(!idPool[i])
				idleIDs.Add(i);
		}
	}

	private static void ResetTotalID()
	{
		limit = 1;
		NPCUnitInfo[] npcs = (NPCUnitInfo[])Object.FindObjectsOfType(typeof(NPCUnitInfo));
		foreach(NPCUnitInfo npc in npcs)
		{
			if(npc.id >= limit)
				limit = npc.id + 1;
		}

		idleIDs.Clear();
		idPool.Clear();
	}

	//void OnEnable()
	public static void OnNewNPC(NPCUnitInfo npcEntity)
	{
		/*
		NPCUnitInfo npcEntity = null;
		try
		{
			npcEntity = (NPCUnitInfo)target;
		}
		catch(System.Exception)
		{
			return;
		}
		*/
		if(npcEntity == null)
			return;

		bool isInScene = false;
		NPCUnitInfo[] npcs = (NPCUnitInfo[])Object.FindObjectsOfType(typeof(NPCUnitInfo));
		foreach(NPCUnitInfo npc in npcs)
		{
			if(npc.gameObject == npcEntity.gameObject)
			{
				isInScene = true;
				break;
			}
		}

		if(!isInScene)
			return;

		if(npcEntity.id >= 0)
			return;

		ResetTotalID();
		CollectIdleIDs();

		if(idleIDs.Count > 0)
		{
			npcEntity.id = idleIDs[0];
			idleIDs.RemoveAt(0);
		}
		else
		{
			npcEntity.id = limit++;
		}

		//In order to refresh script.
		npcEntity.enabled = false;
		npcEntity.enabled = true;
	}
}
