﻿using System.Collections.Generic;

namespace EPSIC_Bataille_Navale.Models
{
    /// <summary>
    /// Classe servant au passage des données d'un Player par le réseau
    /// </summary>
    public class SendablePlayer
    {
        public string playerName;
        public List<int[]> boats = new List<int[]>();

        public SendablePlayer()
        {

        }

        public SendablePlayer(Player player)
        {
            playerName = player.playerName;
            foreach (Boat boat in player.grid.boats)
            {
                boats.Add(new int[] { boat.startCell[0], boat.startCell[1], (int)boat.orientation, boat.cells.Count });
            }
        }

        /// <summary>
        /// Reconstruit le player original
        /// </summary>
        /// <returns>Le player</returns>
        public Player ToPlayer()
        {
            GridModel grid = new GridModel(Properties.Settings.Default.size);

            foreach(int[] array in boats)
            {
                Boat boat = new Boat();
                boat.startCell = new int[] { array[0], array[1] };
                
                if (array[3] != 1)
                {
                    boat.orientation = (Direction)array[2];

                    for (int i = array[0]; i < array[0] + (boat.orientation == Direction.Left || boat.orientation == Direction.Right ? array[3] : 1); i++)
                    {
                        for (int j = array[1]; j < array[1] + (boat.orientation == Direction.Up || boat.orientation == Direction.Down ? array[3] : 1); j++)
                        {
                            grid.grid[i, j].boat = boat;
                            boat.cells.Add(grid.grid[i, j]);
                        }
                    }
                    grid.boats.Add(boat);
                }
                else
                {
                    boat.touchedCell = -1;
                    grid.grid[array[0], array[1]].boat = boat;
                    boat.cells.Add(grid.grid[array[0], array[1]]);
                }
            }
            
            return new Player(grid, playerName);
        }
    }
}
