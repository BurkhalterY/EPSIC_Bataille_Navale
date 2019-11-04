﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EPSIC_Bataille_Navale.Controllers;
using EPSIC_Bataille_Navale.Models;

namespace EPSIC_Bataille_Navale.Views
{
    public partial class Setup : GridView
    {
        private SetupController controller;

        public Setup(int size) : base(size)
        {
            InitializeComponent();
            controller = new SetupController(this, size);
            ClearGrid();
        }

        protected override void CellClick(object sender, EventArgs e)
        {
            CustomPictureBox customPictureBox = (CustomPictureBox)sender;
            controller.Click(customPictureBox.x, customPictureBox.y);
        }

       
        public void RefreshGrid(Grid gridData, int[] clickedCell, List<int[]> possibleCells)
        {
            ClearGrid();
            for (int i = 0; i < gridData.grid.GetLength(0); i++)
            {
                for (int j = 0; j < gridData.grid.GetLength(1); j++)
                {
                    if(gridData.grid[i, j].boat != null)
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

        private void Btn_cancel_Click(object sender, EventArgs e)
        {
            controller.DeleteLastBoat();
        }

        public void EnableNextButton(bool value)
        {
            btn_next.Enabled = value;
        }

        public void EnableCancelButton(bool value)
        {
            btn_cancel.Enabled = value;
        }

        private void Btn_next_Click(object sender, EventArgs e)
        {
            Finish();
        }

        public void Finish()
        {
            Setup setup = new Setup(grid.GetLength(0));
            setup.controller.AIChoise();

            Game game = new Game(10, 0);
            game.controller.SetGrids(new Grid[] { controller.grid, setup.controller.grid });
            ((MainForm)Parent.FindForm()).LoadView(game);
        }
    }
}