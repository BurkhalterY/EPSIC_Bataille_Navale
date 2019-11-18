﻿using EPSIC_Bataille_Navale.Controllers;
using EPSIC_Bataille_Navale.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EPSIC_Bataille_Navale.Views
{
    /// <summary>
    /// Logique d'interaction pour Setup.xaml
    /// </summary>
    public partial class Setup : Page
    {
        // Propriété de la classe Setup
        public SetupController controller;
        private CustomButton[,] grid;
        public int size = 10;
        public int gameType = 0;

        // Initialisation
        public Setup(int gameType) : base()
        {
            InitializeComponent();
            controller = new SetupController(this, size);
            MakeGrid();
            ClearGrid();
        }


        // Le bouton qui annule le dernier bateau
        private void Btn_cancel_Click(object sender, EventArgs e)
        {
            controller.DeleteLastBoat();
        }

        // Le prochain bouton est valide
        public void EnableNextButton(bool value)
        {
            btn_next.IsEnabled = value;
        }

        // Le bouton cancel est valide
        public void EnableCancelButton(bool value)
        {
            btn_cancel.IsEnabled = value;
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
                Setup setup = new Setup(1);
                setup.controller.AIChoise();

                Game game = new Game(grid.GetLength(0), 0);
                game.controller.grids = new Models.GridModel[] { controller.grid, setup.controller.grid };
                game.controller.playersNames = new string[] { controller.playerName, setup.controller.playerName };
                Window.GetWindow(this).Content = game;
                game.RefreshGrid();
            }
            else if (gameType == 2 || gameType == 3)
            {
                /*Game game = new Game(grid.GetLength(0), gameType);
                game.controller.grids = new Models.Grid[] { controller.grid, setup.controller.grid };
                game.controller.playersNames = new string[] { controller.playerName, setup.controller.playerName };
                game.MakeSecondGrid();
                Window.GetWindow(this).Content = game;*/
            }
        }

        protected void MakeGrid()
        {
            grid = new CustomButton[size, size];
            Grid gridView = Content as Grid;
            int cellSize = 450 / size;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    grid[i, j] = new CustomButton(i, j);
                    grid[i, j].HorizontalAlignment = HorizontalAlignment.Left;
                    grid[i, j].VerticalAlignment = VerticalAlignment.Top;
                    grid[i, j].Margin = new Thickness(i * cellSize + 25, j * cellSize + 25, 0, 0);
                    grid[i, j].Width = cellSize;
                    grid[i, j].Height = cellSize;
                    grid[i, j].Click += new RoutedEventHandler(CellClick);
                    gridView.Children.Add(grid[i, j]);
                }
            }
        }

        protected void ClearGrid()
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j].Background = Brushes.White;
                }
            }
        }

        protected void CellClick(object sender, EventArgs e)
        {
            CustomButton customButton = (CustomButton)sender;
            controller.Click(customButton.x, customButton.y);
        }

        // Rafraichissement de la grille
        public void RefreshGrid(GridModel gridData, int[] clickedCell, List<int[]> possibleCells)
        {
            ClearGrid();
            for (int i = 0; i < gridData.grid.GetLength(0); i++)
            {
                for (int j = 0; j < gridData.grid.GetLength(1); j++)
                {
                    if (gridData.grid[i, j].boat != null)
                    {
                        grid[i, j].Background = Brushes.DarkBlue;
                    }
                }
            }
            for (int i = 0; i < possibleCells.Count; i++)
            {
                grid[possibleCells[i][0], possibleCells[i][1]].Background = Brushes.LightGreen;
            }
            if (clickedCell.Length == 2)
            {
                grid[clickedCell[0], clickedCell[1]].Background = Brushes.Yellow;
            }
        }

        private void Btn_back_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Content = new Home();
        }
    }
}
