using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleLib.Console;
using XRL;
using XRL.UI;
using XRL.World.AI;
using XRL.Wish;

namespace XRL.World.Parts
{
	[HasWishCommand]
	[Serializable]
	public class ModJamPetSpawner : IPart
	{
		public override bool WantEvent(int ID, int Cascade) =>
			base.WantEvent(ID, Cascade)
			|| ID == EnteredCellEvent.ID;
		
		public override bool HandleEvent(EnteredCellEvent E)
		{
			if (The.Player?.CurrentCell is Cell startingCell)
			{			
				foreach (var petBP in GetPets())
				{					
					GameObject pet = GameObject.Create(petBP);
					
					MakePet(pet, startingCell);
				}
			}
		
			return base.HandleEvent(E);
		}
			
		
		public static IEnumerable<GameObjectBlueprint> GetPets()
		{
			foreach(var pet in GameObjectFactory.Factory.GetBlueprintsWithTag("StartingPet"))
			{
				if (pet.HasTag("ExcludeFromModJamPetSpawner")) continue;

				yield return pet;
			}
		}
		
		public static void MakePet(GameObject pet, Cell startingCell)
		{
			// borrowed from XRL.CharacterBuilds.Qud.QudSpecificBootHandlersModule
			pet.SetIntProperty("NoXP", 1);
			pet.SetAlliedLeader<AllyPet>(The.Player);
			pet.MakeActive();
			pet.Brain?.GoToPartyLeader();
			if (pet.CurrentCell == null)
			{
				Cell petCell =
						startingCell.GetFirstEmptyAdjacentCell(1, 1)
						?? startingCell.GetFirstEmptyAdjacentCell(1, 2)
						?? startingCell.GetFirstEmptyAdjacentCell(1, 3)
						?? startingCell.GetFirstEmptyAdjacentCell(1, 4)
						?? startingCell.GetFirstEmptyAdjacentCell(1, 5)
						?? startingCell.GetFirstEmptyAdjacentCell(1, 6)
						?? startingCell.GetRandomLocalAdjacentCell()
						?? startingCell
					;
				pet.SystemLongDistanceMoveTo(petCell);
			}
		}
		
		[WishCommand("jamaddpet")]
		public static void AddPet()
		{
			var pets = GetPets().Prepend(GameObjectFactory.Factory.GetBlueprint("ModJam_All Pet Spawner")).ToList();
			
			int n = Popup.PickOption(
				Title: "Choose pet",
				Options: pets.Select(pet => pet.GetTag("PetName", pet.DisplayName())).ToArray(),
				Icons: pets.Select(pet => new Renderable(pet)).ToArray(),
				AllowEscape: true
			);
			
			if (n < 0) return;
			
			var pet = GameObject.Create(pets[n]);
			MakePet(pet, The.Player.CurrentCell);
		}
	}
}
