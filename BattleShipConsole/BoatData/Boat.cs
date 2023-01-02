using System.Numerics;
using BattleShipConsole.Enums;
using BattleShipConsole.Interfaces;

namespace BattleShipConsole.BoatData;

public class Boat
{
   public readonly int Length;
   public readonly Vector2 Cords;
   
   public readonly BoatPart[] Parts;
   public readonly IField Field;
   public Facing Direction { get; private set; } = Facing.North;
   

   public Boat(IField field, int length, int x, int y)
   {
      Field = field;
      Cords = new Vector2(x, y);
      Length = length;
      Parts = new BoatPart[Length];
      for (int i = 0; i < Length; i++)
         Parts[i] = new BoatPart(this, i);
   }

   public Boat(IField field, int length, Vector2 cords)
   {
      Field = field;
      Length = length;
      Cords = cords;

      Parts = new BoatPart[Length];
      for (int i = 0; i < Length; i++)
         Parts[i] = new BoatPart(this, i);
   }

   public bool Rotate(Facing direction)
   {
      Facing oldDir = Direction;

      Direction = direction;

      bool successful = SetParts();
      

      if (!successful)
         Direction = oldDir;
      SetParts();
      
      return successful;
   }

   private bool SetParts()
   {
      for (int i = 0; i < Length; i++)
         if (!Parts[i].SetPosition())
            return false;

      return true;
   }
}