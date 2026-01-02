using System;

namespace Saper
{
    public class GameCell
    {
        private bool hasMine;
        private int minesAround;
        private int state;

        public int X { get; private set; }
        public int Y { get; private set; }

        public bool HasMine
        {
            get { return hasMine; }
            set { hasMine = value; }
        }

        public int MinesAround
        {
            get { return minesAround; }
            set { minesAround = value; }
        }

        public int State
        {
            get { return state; }
            set { state = value; }
        }

        public bool IsOpened()
        {
            return state == 1;
        }

        public bool IsFlagged()
        {
            return state == 2;
        }

        public bool HasQuestion()
        {
            return state == 3;
        }

        public bool IsClosed()
        {
            return state == 0;
        }

        public GameCell(int x, int y)
        {
            X = x;
            Y = y;
            hasMine = false;
            minesAround = 0;
            state = 0; 
        }

        public void Open()
        {
            if (state == 0) 
            {
                state = 1;
            }
        }

        public void ToggleMark()
        {
            if (state == 0)
            {
                state = 2;
            }
            else if (state == 2)
            {
                state = 3;
            }
            else if (state == 3)
            {
                state = 0;
            }
        }

        public void Reset()
        {
            hasMine = false;
            minesAround = 0;
            state = 0;
        }
        public void ResetState()
        {
            state = 0;
        }
    }
}