﻿using System;
using System.Collections.Generic;
using System.Drawing;
using EPSIC_Bataille_Navale.Controllers;
using EPSIC_Bataille_Navale.Models;

namespace EPSIC_Bataille_Navale.Views
{
    public partial class Setup : GridView
    {
        // Propriété de la classe Setup
        public SetupController controller;
        public int gameType = 0;

        // Initialisation
        public Setup(int size, int gameType) : base(size)
        {
            InitializeComponent();
            this.gameType = gameType;
            controller = new SetupController(this, size);
            ClearGrid();
        }

        // Quand on clique sur une cellule
        protected override void CellClick(object sender, EventArgs e)
        {
            CustomPictureBox customPictureBox = (CustomPictureBox)sender;
            controller.Click(customPictureBox.x, customPictureBox.y);
        }

        // Rafraichissement de la grille
        public void RefreshGrid(Grid gridData, int[] clickedCell, List<int[]> possibleCells)
        {
            ClearGrid();
            for (int i = 0; i < gridData.grid.GetLength(0); i++)
            {
                for (int j = 0; j < gridData.grid.GetLength(1); j++)
                {
                    if (gridData.grid[i, j].boat != null)
                    {
                        grid[i, j].BackColor = Color.DarkBlue;
                    }
                }
            }
            for (int i = 0; i < possibleCells.Count; i++)
            {
                grid[possibleCells[i][0], possibleCells[i][1]].BackColor = Color.LightGreen;
            }
            if (clickedCell.Length == 2)
            {
                grid[clickedCell[0], clickedCell[1]].BackColor = Color.Yellow;
            }
        }

        // Le bouton qui annule le dernier bateau
        private void Btn_cancel_Click(object sender, EventArgs e)
        {
            controller.DeleteLastBoat();
        }

        // Le prochain bouton est valide
        public void EnableNextButton(bool value)
        {
            btn_next.Enabled = value;
        }

        // Le bouton cancel est valide
        public void EnableCancelButton(bool value)
        {
            btn_cancel.Enabled = value;
        }

        // Bouton pour clique
        private void Btn_next_Click(object sender, EventArgs e)
        {
            Finish();
        }

        // Quand le jeu est fini
        public void Finish()
        {
            if (gameType == 0)
            {
                Setup setup = new Setup(grid.GetLength(0), 1);
                setup.controller.AIChoise();

                Game game = new Game(grid.GetLength(0), 0);
                game.controller.grids = new Grid[] { controller.grid, setup.controller.grid };
                game.controller.playersNames = new string[] { controller.playerName, setup.controller.playerName };
                game.MakeSecondGrid();
                ((MainForm)Parent.FindForm()).LoadView(game);
            }
            else if (gameType == 2 || gameType == 3)
            {
               /* Game game = new Game(grid.GetLength(0), gameType);
                game.controller.grids = new Grid[] { controller.grid, setup.controller.grid };
                game.controller.playersNames = new string[] { controller.playerName, setup.controller.playerName };
                game.MakeSecondGrid();
                ((MainForm)Parent.FindForm()).LoadView(game);*/
            }
        }

        private void Setup_Load(object sender, EventArgs e)
        {

        }
    }
}
