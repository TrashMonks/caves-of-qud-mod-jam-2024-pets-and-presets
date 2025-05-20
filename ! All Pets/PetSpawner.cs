using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleLib.Console;
using XRL.UI;
using XRL.Wish;
using XRL.World.AI;

namespace XRL.World.Parts
{
	[HasWishCommand]
	[Serializable]
	public class ModJamPetSpawner : IPart
	{
		public override bool WantEvent(int ID, int Cascade) =>
			base.WantEvent(ID, Cascade) || ID == EnteredCellEvent.ID;

		public override bool HandleEvent(EnteredCellEvent E)
		{
			if (The.Player?.CurrentCell is Cell startingCell)
			{
				foreach (var petBP in GetPets())
				{
					var pet = GameObject.Create(petBP);

					MakePet(pet, startingCell);
				}
			}

			return base.HandleEvent(E);
		}

		public static IEnumerable<GameObjectBlueprint> GetPets()
		{
			foreach (var pet in GameObjectFactory.Factory.GetBlueprintsWithTag("StartingPet"))
			{
				if (pet.HasTag("ExcludeFromModJamPetSpawner"))
				{
					continue;
				}

				yield return pet;
			}
		}

		public static void MakePet(GameObject Pet, Cell StartingCell)
		{
			// borrowed from XRL.CharacterBuilds.Qud.QudSpecificBootHandlersModule
			Pet.SetIntProperty("NoXP", 1);
			Pet.SetAlliedLeader<AllyPet>(The.Player);
			Pet.MakeActive();
			Pet.Brain?.GoToPartyLeader();
			if (Pet.CurrentCell == null)
			{
				Pet.SystemLongDistanceMoveTo(StartingCell.GetConnectedSpawnLocation());
			}
		}

		[WishCommand("jamaddpet")]
		public static void AddPet()
		{
			var pets = GetPets()
				.Prepend(GameObjectFactory.Factory.GetBlueprint("ModJam_All Pet Spawner"))
				.ToList();

			var n = Popup.PickOption(
				Title: "Choose pet",
				Options: pets.Select(Pet => Pet.GetTag("PetName", Pet.DisplayName())).ToArray(),
				Icons: pets.Select(Pet => new Renderable(Pet)).ToArray(),
				AllowEscape: true
			);

			if (n < 0)
			{
				return;
			}

			var pet = GameObject.Create(pets[n]);
			MakePet(pet, The.Player.CurrentCell);
		}
	}
}
