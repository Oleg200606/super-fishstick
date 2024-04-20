﻿using Prism.Mvvm;
using Zmeyka_555.Models;

namespace Zmeyka_555.ViewModels
{
    internal class CellVM : BindableBase
    {
        public int Row {  get; }

        public int Column { get; }

        private CellType _cellType = CellType.None;

        public CellType CellType
        {
            get => _cellType;
            set 
            {
                _cellType = value; 
                RaisePropertyChanged(nameof(CellType));
            }
        }

        public CellVM(int row, int column)
        {
            Row = row;
            Column = column;
        }


    }
}
