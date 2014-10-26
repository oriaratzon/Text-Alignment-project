using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace text_alignment
{
    class Letter
    {
        private int m_X;//left   
        private int m_Y;//buttom
        private int m_Width;//how much pixels to the right     
        private int m_Hight;//how much pixels to the top
        private int m_LineNumber;
        private bool m_IsApostrophe = false;

        public bool IsApostrophe
        {
            get { return m_IsApostrophe; }
            set { m_IsApostrophe = value; }
        }

        public int LineNumber
        {
            get { return m_LineNumber; }
            set { m_LineNumber = value; }
        }

        public int X
        {
            get { return m_X; }
            set { m_X = value; }
        }

        public int Y
        {
            get { return m_Y; }
            set { m_Y = value; }
        }

        public int Width
        {
            get { return m_Width; }
            set { m_Width = value; }
        }

        public int Hight
        {
            get { return m_Hight; }
            set { m_Hight = value; }
        }

    }
}
