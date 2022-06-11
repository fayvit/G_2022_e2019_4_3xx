using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FayvitUI
{
    public abstract class BaseGridMenu : InteractiveUiBase
    {
        protected System.Action<int> ThisAction;
        protected int rowCellCount = 0;
        protected int colCellCount = 0;

        public void ChangeOption(int Vval, int Hval)
        {

            int quanto = -rowCellCount * Vval;

            if (quanto == 0)
                quanto = Hval;

            ChangeOptionWithVal(quanto, rowCellCount);

        }

        public int ColCellCount()
        {
            GridLayoutGroup grid = variableSizeContainer.GetComponent<GridLayoutGroup>();

            Debug.Log("grid lengths: " + grid.cellSize + " : " + grid.spacing.x);

            return
                (int)((variableSizeContainer.rect.width ) / (grid.cellSize.x + grid.spacing.x));
        }

        public int RowCellCount()
        {
            GridLayoutGroup grid = variableSizeContainer.GetComponent<GridLayoutGroup>();

            return
                (int)(variableSizeContainer.rect.height / (grid.cellSize.y + grid.spacing.y));
        }

        public void SetLineRowLength()
        {
            rowCellCount = ColCellCount();
            colCellCount = RowCellCount();

            Debug.Log("Celulas do grid: " + rowCellCount + " : " + colCellCount);
        }

        protected override void AfterFinisher()
        {
            ThisAction = null;
        }
    }
}
