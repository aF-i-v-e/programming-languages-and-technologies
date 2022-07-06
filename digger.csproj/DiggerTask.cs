using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digger
{
    public class Player : ICreature
    {
        public string GetImageFileName()
        {
            return "Digger.png";
        }

        public int GetDrawingPriority()
        {
            return 1;
        }

        public bool IsOk(int x, int y, int deltaX, int deltaY)
        {
            bool okBorder;
            var borderX = Game.MapWidth - 1;
            var borderY = Game.MapHeight - 1;
            if (deltaX > 0)
                okBorder = x < borderX;
            else if (deltaX < 0)
                okBorder = x > 0;
            else if (deltaY > 0)
                okBorder = y < borderY;
            else
                okBorder = y > 0;
            return okBorder && !(Game.Map[x + deltaX, y + deltaY] is Sack);
        }

        public CreatureCommand Act(int x, int y)
        {
            var key = Game.KeyPressed.ToString();
            var creatureCommand = new CreatureCommand();
            if (key == "Right" && IsOk(x, y, 1, 0))
                creatureCommand.DeltaX = 1;

            if (key == "Down" && IsOk(x, y, 0, 1))
                creatureCommand.DeltaY = 1;

            if (key == "Left" && IsOk(x, y, -1, 0))
                creatureCommand.DeltaX = -1;

            if (key == "Up" && IsOk(x, y, 0, -1))
                creatureCommand.DeltaY = -1;

            return creatureCommand;
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return conflictedObject is Sack || conflictedObject is Monster;
        }
    }

    public class Gold : ICreature
    {
        public string GetImageFileName()
        {
            return "Gold.png";
        }

        public int GetDrawingPriority()
        {
            return 2;
        }

        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject is Player)
            {
                Game.Scores += 10;
                return true;
            }
            else if (conflictedObject is Monster)
                return true;
            return false;
        }
    }

    public class Monster : ICreature
    {
        public bool DoYouFoundDigger;
        public string GetImageFileName()
        {
            return "Monster.png";
        }

        public int GetDrawingPriority()
        {
            return 3;
        }

        public int[] FoundDigger()
        {
            DoYouFoundDigger = false;
            var coord = new int[2];
            for (int x = 0; x < Game.MapWidth; x++)
            {
                for (int y = 0; y < Game.MapHeight; y++)
                {
                    if (Game.Map[x, y] is Player && Game.Map[x, y] != null)
                    {
                        coord[0] = x;
                        coord[1] = y;
                        DoYouFoundDigger = true;
                        break;
                    }
                }
                if (DoYouFoundDigger)
                    break;
            }
            return coord;
        }

        public bool CanMove(int x, int y, int deltaX, int deltaY)
        {
            bool okBorder;
            if (deltaX > 0)
                okBorder = x < Game.MapWidth - 1;
            else if (deltaX < 0)
                okBorder = x > 0;
            else if (deltaY > 0)
                okBorder = y < Game.MapHeight - 1;
            else
                okBorder = y > 0;
            var reason1 = Game.Map[x + deltaX, y + deltaY] is Player;
            var reason2 = Game.Map[x + deltaX, y + deltaY] is Gold;
            var reason3 = Game.Map[x + deltaX, y + deltaY] == null;
            return okBorder && (reason1 || reason2 || reason3);
        }

        public CreatureCommand Move(int x, int y, int dX, int dY)
        {
            var creatureCommand = new CreatureCommand();
            if (dX > x && CanMove(x, y, 1, 0))
                creatureCommand.DeltaX = 1;

            if (dY > y && CanMove(x, y, 0, 1))
                creatureCommand.DeltaY = 1;

            if (dX < x && CanMove(x, y, -1, 0))
                creatureCommand.DeltaX = -1;

            if (dY < y && CanMove(x, y, 0, -1))
                creatureCommand.DeltaY = -1;
            return creatureCommand;
        }

        public CreatureCommand Act(int x, int y)
        {
            var creatureCommand = new CreatureCommand();
            var coordDigger = FoundDigger();
            if (!DoYouFoundDigger)
                return creatureCommand;
            return Move(x, y, coordDigger[0], coordDigger[1]);
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return conflictedObject is Monster || conflictedObject is Sack;
        }
    }

    public class Sack : ICreature
    {
        public int Tact;
        public bool StartFall;

        public string GetImageFileName()
        {
            return "Sack.png";
        }

        public int GetDrawingPriority()
        {
            return 4;
        }

        public bool SolveSituationWithPlayerAndMonster(int x, int y)
        {
            if (StartFall)
            {
                Tact++;
                return true;
            }
            else
            {
                Tact = 0;
                return false;
            }
        }

        public bool SolveSituationWhenTransform(int x, int y)
        {
            var reason1 = Tact > 1;
            var reason2 = (y == Game.MapHeight - 1) || (y < Game.MapHeight - 1 && Game.Map[x, y + 1] is Terrain);
            var reason3 = y < Game.MapHeight - 1 && (Game.Map[x, y + 1] is Sack || Game.Map[x, y + 1] is Gold);
            return reason1 && (reason2 || reason3);
        }

        public CreatureCommand Act(int x, int y)
        {
            var creatureCommand = new CreatureCommand();
            if (y < Game.MapHeight - 1 && Game.Map[x, y + 1] == null)
            {
                Tact++;
                StartFall = true;
                creatureCommand.DeltaY = 1;
            }
            else if (y < Game.MapHeight - 1 && (Game.Map[x, y + 1] is Player || Game.Map[x, y + 1] is Monster))
            {
                if (SolveSituationWithPlayerAndMonster(x, y))
                    creatureCommand.DeltaY = 1;
            }
            else if (SolveSituationWhenTransform(x, y))
            {
                Tact = 0;
                creatureCommand.TransformTo = new Gold();
            }
            else
                Tact = 0;
            return creatureCommand;
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return false;
        }
    }

    public class Terrain : ICreature
    {
        public string GetImageFileName()
        {
            return "Terrain.png";
        }

        public int GetDrawingPriority()
        {
            return 5;
        }

        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return true;
        }
    }
}
