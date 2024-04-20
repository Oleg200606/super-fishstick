﻿using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Zmeyka_555.Models;
using static System.Formats.Asn1.AsnWriter;

namespace Zmeyka_555.ViewModels
{
    internal class MainWndVM : BindableBase
    {
		private bool _continueGame;

		public bool ContinueGame
        {
			get => _continueGame;
			private set 
			{
				_continueGame = value; 
				RaisePropertyChanged(nameof(ContinueGame));
				if (_continueGame)
				{
					SnakeGo();

                }
			}
		}

		public List<List<CellVM>> AllCells { get;  } = new List<List<CellVM>>();


		public DelegateCommand StartStopCommand { get; }
		private MoveDirection _currentMoveDirection = MoveDirection.Right;

		private int _rowCount = 10;
		private int _columnCount = 10;
		private int _score ;
		private const int Speed = 400;
		private int _speed = Speed;
		private Snake _snake;
		private MainWindow _mainWnd;
		private CellVM _lastFood;


		public int Score
		{
			get => _score;
			set
			{
				_score = value;
				RaisePropertyChanged(nameof(Score));
			}
		}
		public MainWndVM(MainWindow mainWnd)
		{
			_mainWnd = mainWnd;
			StartStopCommand = new DelegateCommand(() => ContinueGame = !ContinueGame);

			for (int row = 0; row < _rowCount; row++)
			{ 
				var	rowList = new List<CellVM>();
				for (int column = 0; column < _columnCount; column++)
				{
					var cell = new CellVM(row, column);
					rowList.Add(cell);
				}
				AllCells.Add(rowList);
			}

			_snake = new Snake(AllCells, AllCells[_rowCount / 2][_columnCount / 2], CreateFood);
			CreateFood();


            _mainWnd.KeyDown += UserKeyDown;
		}

		private async Task SnakeGo()
		{
			while (ContinueGame)
			{
				await Task.Delay(_speed);

				try
				{
					_snake.Move(_currentMoveDirection);
				}
				catch(Exception ex) 
				{ 
					ContinueGame = false;
					MessageBox.Show(ex.Message);
					_speed = Speed;
					_snake.Restart();
					_lastFood.CellType = CellType.None;
					Score = 0;
                    CreateFood();


				}

			}
		}

		private void UserKeyDown(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.A:
					if(_currentMoveDirection != MoveDirection.Right)	
						_currentMoveDirection = MoveDirection.Left;
					break;
                case Key.D:
                    if (_currentMoveDirection != MoveDirection.Left)
                        _currentMoveDirection = MoveDirection.Right;
                    break;
                case Key.W:
                    if (_currentMoveDirection != MoveDirection.Down)
                        _currentMoveDirection = MoveDirection.Up;
                    break;
                case Key.S:
                    if (_currentMoveDirection != MoveDirection.Up)
                        _currentMoveDirection = MoveDirection.Down;
                    break;
                default:
					break;
			}
		}
		private void CreateFood()
		{
			var random = new Random();

			int row = random.Next(0, _rowCount);
			int column = random.Next(0, _columnCount);

			_lastFood = AllCells[row][column];

			if (_snake.SnakeCells.Contains(_lastFood))
			{
				CreateFood();
			}

			_lastFood.CellType = CellType.Food;
			_speed = (int)(_speed * 0.95);
			Score += 10;
		}



    }
}
