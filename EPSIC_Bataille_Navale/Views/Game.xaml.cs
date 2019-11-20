﻿using EPSIC_Bataille_Navale.Controllers;
using EPSIC_Bataille_Navale.Models;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using Brushes = System.Windows.Media.Brushes;

namespace EPSIC_Bataille_Navale.Views
{
    /// <summary>
    /// Logique d'interaction pour Game.xaml
    /// </summary>
    public partial class Game : Page
    {
        public GameController controller;
        private CustomButton[,] grid;
        private CustomButton[,] gridSecond;
        public RichTextBox history;
        private int size = 10;
        public int gameType = 0;

        public Game(int size, int code) : base()
        {
            InitializeComponent();
            this.size = size;
            switch (code)
            {
                case 0: controller = new SoloGameController(this); break;
                case 3: controller = new DemoGameController(this); break;
            }
            MakeGrid();
        }

        public void RefreshGrid()
        {
            for (int i = 0; i < controller.grids[1].grid.GetLength(0); i++)
            {
                for (int j = 0; j < controller.grids[1].grid.GetLength(1); j++)
                {
                    Sprite sprite = new Sprite(Properties.Resources.water);
                    switch (controller.grids[1].grid[i, j].state)
                    {
                        case State.noBoat:
                            sprite.AddSprite(Properties.Resources.miss);
                            break;
                        case State.boat:
                            sprite.AddSprite(Properties.Resources.touch);
                            break;
                        case State.fullBoat:
                            Boat boat = controller.grids[1].grid[i, j].boat;
                            sprite.AddSprite((Bitmap)Properties.Resources.ResourceManager.GetObject("boat_" + boat.cells.Count), boat.orientation == Directions.Right || boat.orientation == Directions.Down ? boat.cells.IndexOf(controller.grids[1].grid[i, j]) : boat.cells.Count - boat.cells.IndexOf(controller.grids[1].grid[i, j]) - 1, 0);
                            sprite.RotateSprite(controller.grids[1].grid[i, j].boat.orientation);
                            break;
                    }
                    grid[i, j].Background = sprite.ToBrush();

                    sprite = new Sprite(Properties.Resources.water);
                    if (controller.grids[0].grid[i, j].boat != null)
                    {
                        Boat boat = controller.grids[0].grid[i, j].boat;
                        sprite.AddSprite((Bitmap)Properties.Resources.ResourceManager.GetObject("boat_" + boat.cells.Count), boat.orientation == Directions.Right || boat.orientation == Directions.Down ? boat.cells.IndexOf(controller.grids[0].grid[i, j]) : boat.cells.Count - boat.cells.IndexOf(controller.grids[0].grid[i, j]) - 1, 0);
                        sprite.RotateSprite(controller.grids[0].grid[i, j].boat.orientation);
                    }
                    switch (controller.grids[0].grid[i, j].state)
                    {
                        case State.noActivity:
                            sprite.AddSprite(Properties.Resources.hide);
                            break;
                        case State.noBoat:
                            sprite.AddSprite(Properties.Resources.miss);
                            break;
                        case State.boat:
                            sprite.AddSprite(Properties.Resources.touch);
                            break;
                    }
                    gridSecond[i, j].Background = sprite.ToBrush();
                }
            }
        }

        public void Finish(string winnerName)
        {
            Home home = new Home();
            Window.GetWindow(this).Content = home;
            home.SetTitle(winnerName + " a gagné !");
        }

        protected void MakeGrid()
        {
            grid = new CustomButton[size, size];
            Grid gridView = Content as Grid;
            int cellSize = 450 / size;

            gridSecond = new CustomButton[size, size];
            int cellSizeSecond = 450 / 3 / size;

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

                    gridSecond[i, j] = new CustomButton(i, j);
                    gridSecond[i, j].HorizontalAlignment = HorizontalAlignment.Left;
                    gridSecond[i, j].VerticalAlignment = VerticalAlignment.Top;
                    gridSecond[i, j].Margin = new Thickness(i * cellSizeSecond + 475, j * cellSizeSecond + 25, 0, 0);
                    gridSecond[i, j].Width = cellSizeSecond;
                    gridSecond[i, j].Height = cellSizeSecond;
                    gridView.Children.Add(gridSecond[i, j]);
                }
            }

            history = new RichTextBox();
            history.HorizontalAlignment = HorizontalAlignment.Left;
            history.VerticalAlignment = VerticalAlignment.Top;
            history.Margin = new Thickness(475, size * cellSizeSecond + 25, 0, 0);
            history.Width = 450 / 3;
            history.Height = 450 / 3 - 50;
            history.IsReadOnly = true;
            history.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            gridView.Children.Add(history);
        }

        protected void CellClick(object sender, EventArgs e)
        {
            CustomButton customButton = (CustomButton)sender;
            controller.Click(customButton.x, customButton.y);
        }

    }
}
