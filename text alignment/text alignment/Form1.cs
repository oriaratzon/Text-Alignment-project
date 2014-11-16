using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using text_alignment.Properties;
using render_letters;
using System.Drawing.Drawing2D;
using MathWorks.MATLAB.NET.Arrays;
using Alignment;
//using dlltest;

namespace text_alignment
{
    enum EType
    {
        Filled,
        Border
    }

    enum EMoveMode
    {
        line,
        col
    }

    enum ELineChangeType
    {
        buttomLineMove,
        topLineMove,
        verticalLineMove,
        none
    }

    enum EDirection
    {
        left,
        right
    }

    public partial class textBox : Form
    {
        private char[] m_Letters = new char[746];
        private Letter[] m_LettersMatrix = new Letter[746];
        private char[][] m_Indexes = new char[646][];//that's actually letters. must be fixed to indexes in the future
        private Label m_LetterLabel;
        private Image m_OriginalImage;
        private string m_ManuFileName;
        private Color[] m_ManuscriptColorsArray = new Color[2] {Color.BlueViolet, Color.Orange};
        private const int m_HandleRadius = 6;
        private ELineChangeType m_PointMoveInProgress = ELineChangeType.none;// 1- top line is changed, 2 - buttom line is changed, 3- vertical line is changed, 0 - no change
        private Point m_ButtomLeftLinePoint= new Point();
        private Point m_ButtomRightLinePoint = new Point();
        private Point m_TopLeftLinePoint = new Point();
        private Point m_TopRightLinePoint = new Point();
        private Point m_TopCorrectionPoint = new Point();
        private Point m_ButtomCorrectionPoint = new Point();
        private int m_CurrentManuscriptLineIndex = -1;
        private int m_CurrentTranscriptLineIndex = 0;
        //private Dictionary<int, int> m_StartEndOfLinesPixels = new Dictionary<int, int>();//{{0, 20},{20, 45},{45, 88},{88, 132},{132, 151},{151, 174},{174, 213}, {233, 257},{257, 275},{275, 295},{295, 317},{317, 338},{338, 357},{357, 378},{378, 398},{398, 418},{418, 435},{435, 455},{455, 478},{478, 496}, {496,515},{515, 539},{539, 563},{563, 584},{584, 611},{611, 632}};//for the demo
        private bool m_FindLineClicked = false;
        private bool m_ManuPicBoxContainAPic = false;
        private EMoveMode m_CurrMode = EMoveMode.line;
        private List<AlignmentRectangle> m_LinesRectangles = new List<AlignmentRectangle>();
        private bool m_SetLineWasPressedForTheFirstTime = false;
        private string m_FontName = "David";
        private int m_FontSize = 18;
        //private AlignmentRectangle m_AlignmentRectangleToSend = new AlignmentRectangle();
        private string m_FirstTranscriptColor = "#CCCCFF";
        private string m_SecondTranscriptColor = "#FFD699";
        private AlignmentRectangle[] m_CurrLineAlignment;
        private string m_AlignReturnedMatrixFilePath = Directory.GetCurrentDirectory() + "\\indext_img.csv";
        private Bitmap m_Cropped;
        private PixelDetails[,] m_PixelDetailsMatrix;
        private Bitmap m_ManuPicBoxBackUp = null;
        private string m_BackupPath = null;
        private EDirection m_FixDirection = EDirection.left;
        private Color m_FixButtonChosenColor = Color.LightSkyBlue;
        private bool m_LineWasDetected = false;
        private int m_OffsetInLine;// in case of running the algorithm on a part of a line + the matched text that was selected manually, we need to save the offset of the index
        private Dictionary<int, int> m_FinalStartEndOfLinesPixels = new Dictionary<int, int>();
        private KeyValuePair<int, int> m_CurrentStartEndLine = new KeyValuePair<int, int>();
        private int m_NumOfRecognizedLinesInTranscript = 0;
        private int m_NumOfCharsInCurrLine;


        internal class PixelDetails
        {
            private int m_LineNumber = -1;
            private int m_IndexInLine = -1;

            public PixelDetails()
            { }

            public void SetPixelDetails(int i_LineNumber,int i_IndexInLine)
            {
                m_LineNumber = i_LineNumber;
                m_IndexInLine = i_IndexInLine;
            }
            public int LineNumber
            {
              get { return m_LineNumber; }
              set { m_LineNumber = value; }
            }            

            public int IndexInLine
            {
              get { return m_IndexInLine; }
              set { m_IndexInLine = value; }
            }
        }

        internal class AlignmentRectangle
        {
            private int m_X;
            private int m_Y;
            private int m_W;
            private int m_H;

            public AlignmentRectangle(){}

            public AlignmentRectangle(int i_X, int i_Y, int i_W, int i_H)
            {
                m_X = i_X;
                m_Y = i_Y;
                m_W = i_W;
                m_H = i_H;
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

            public int W
            {
                get { return m_W; }
                set { m_W = value; }
            }

            public int H
            {
                get { return m_H; }
                set { m_H = value; }
            }
        }

        public textBox()
        {
            InitializeComponent();
            //Position winform in the top left corner of the screen
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            initLettersMatrix();
            initLetters();
            initIndexes();
            initLetterLabel();
            leftDirButton.Visible = false;
            rightDirButton.Visible = false;
        }

        private void initIndexes()//fix efficiency
        {
            int i = 0;
            int j = 0;
            int k;

            for(i = 0; i < 646; i++)//rows
            {
                m_Indexes[i] = new char [268];
                for(j=0; j < 268; j++)//cols
                {
                    m_Indexes[i][j] = '\0';
                }
            }

            k = 0;

            foreach (Letter le in m_LettersMatrix)
            {
                for (i = le.Y - le.Hight; i < le.Y; i++)
                {
                    for(j = le.X; j<le.Width + le.X ; j++)
                    {
                        m_Indexes[i][j] = m_Letters[k];
                    }
                }
                k++;
            }
        }

        private void getHorizlePixelOfTheRectangle(out int o_Y1,out int o_Y2, bool i_NewLine)//send i_NewLine = true if we try this line for the first time
        {
            MWArray croppedImage = getCroppedImageForFindLines();
            if (croppedImage != null)
            {
                AlignText align = new AlignText();
                MWArray lineOffsetsMWArray = align.findLines(croppedImage);
                double[,] ad = (double[,]) ((MWNumericArray) lineOffsetsMWArray).ToArray(MWArrayComponent.Real);
                fillStartEndOfLinesPixels(ad);
            }
            o_Y1 = o_Y2 = -1;
            if (m_CurrentManuscriptLineIndex < m_NumOfRecognizedLinesInTranscript+1)
            {
                if (i_NewLine == true)
                {
                    m_CurrentManuscriptLineIndex++;
                }
                //KeyValuePair<int, int> startEnd = m_StartEndOfLinesPixels.ElementAt(Math.Max(m_CurrentManuscriptLineIndex,0));
                o_Y1 = m_CurrentStartEndLine.Key;
                o_Y2 = m_CurrentStartEndLine.Value;          
            }
        }

        private void fillStartEndOfLinesPixels(double[,] i_StartIndexesArray)
        {
            int y = 0;
            if (m_ButtomLeftLinePoint.Y > 0)
            {
                y = m_ButtomLeftLinePoint.Y + 1;
            }
            m_CurrentStartEndLine = new KeyValuePair<int, int>(y+(int)i_StartIndexesArray[0,0],
            y + (int)i_StartIndexesArray[1,0] );
            
            //Dictionary<int, int> newStartEndOfLinesPixels = new Dictionary<int, int>();
            //for (int i = 0; i < i_StartIndexesArray.Length-2; i++)
            //{
            //    if (i < m_CurrentTranscriptLineIndex)
            //    {
            //        newStartEndOfLinesPixels.Add(m_StartEndOfLinesPixels.ElementAt(i).Key, m_StartEndOfLinesPixels.ElementAt(i).Value);
            //    }
            //    else
            //    {
            //        newStartEndOfLinesPixels.Add((int)i_StartIndexesArray[i - m_CurrentTranscriptLineIndex, 0], (int)i_StartIndexesArray[i - m_CurrentTranscriptLineIndex + 1, 0]);
            //    }
            //}
            //m_StartEndOfLinesPixels = newStartEndOfLinesPixels;
        }

        private void initLetters()
        {
            int i = 0;
            string text = string.Empty;
            var filePath = System.Reflection.Assembly.GetExecutingAssembly().Location + "\\..\\..\\letters.txt";

            using (StreamReader streamReader = new StreamReader(filePath, Encoding.GetEncoding(862)))
            {
                text = streamReader.ReadLine();
                while (text != null)
                {
                    //string bla = @"'";
                    char apostrophe = '\'';// Apostrophe
                    if (text[0] != apostrophe)
                        m_Letters[i] = text[0];
                    else
                        m_Letters[i] = '\0';
                    i++;
                    text = streamReader.ReadLine();
                }
            }
        }

        private void initLettersMatrix()
        {
            var filePath = System.Reflection.Assembly.GetExecutingAssembly().Location + "\\..\\..\\matrix.csv";
            CsvFileReader matrixCsv = new CsvFileReader(filePath);
            CsvRow currRow = new CsvRow();
            int i = 0;

            while (matrixCsv.ReadRow(currRow))
            {
                m_LettersMatrix[i] = new Letter();
                m_LettersMatrix[i].X = Int32.Parse(currRow[0]);
                m_LettersMatrix[i].Y = Int32.Parse(currRow[1]);
                m_LettersMatrix[i].Width = Int32.Parse(currRow[2]);
                m_LettersMatrix[i].Hight = Int32.Parse(currRow[3]);
                m_LettersMatrix[i].LineNumber = Int32.Parse(currRow[4]);
                if (m_LettersMatrix[i].X == 0)// if true there is a -'-
                {
                    m_LettersMatrix[i].IsApostrophe = true;
                }
                i++;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox_Load_1(object sender, EventArgs e)
        {

        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void onSetParamsClick(object sender, EventArgs e)
        {
            using (var dlg = new FontDialog())
            {
                dlg.Font = new Font("Arial Black", 16);
                DialogResult result = dlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    // Get Font.
                    Font font = dlg.Font;
                    // Set richTextBox properties.                    
                    richTextBox1.Font = font;
                    richTextBox1.SelectionFont = new Font(font.FontFamily, font.Size);
                }
            }
        }
        // [MAP,text_image] = render_letters(file_name, direction, font_name)
        private void onLoadTextClick(object sender, EventArgs e)
        {
            if (m_ManuFileName == null)
            {
                MessageBox.Show("Please load the manuscript file first");
            }
            else
            {
                loadTranscript();
            }
        }

        private void loadTranscript()
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Text file";
                dlg.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string text = string.Empty;
                    using (StreamReader streamReader = new StreamReader(dlg.FileName, Encoding.GetEncoding("windows-1255")))
                    {
                        text = streamReader.ReadToEnd();
                    }
                    richTextBox1.Text = text;
                    richTextBox1.Font = new Font("Arial", 22);
                    richTextBox1.BackColor = Color.White;
                    richTextBox1.RightToLeft = RightToLeft.Yes;

                }
            }
        }

        private void onLoadManuscriptClick(object sender, EventArgs e)
        {
            loadManuscript();            
        }

        private void loadManuscript()
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Manuscript Image";
                dlg.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                if (dlg.ShowDialog() == DialogResult.OK)
                {

                    // Create a new Bitmap object from the picture file on disk,
                    // and assign that to the PictureBox.Image property
                    m_ManuFileName = dlg.FileName;
                    initManuImage(dlg.FileName);
                    m_OriginalImage = new Bitmap(dlg.FileName);
                }

                if (manuPicBox.Image != null)
                {
                    m_ManuPicBoxContainAPic = true;
                    manuPicBox.BackColor = Color.White;
                    initPixelDetailMatrix();
                    //drawRectangle(0, 0, manuPicBox.Image.Size.Width, 100);
                }
            }
        }

        private void initPixelDetailMatrix()
        {
            m_PixelDetailsMatrix = new PixelDetails[manuPicBox.Image.Size.Height, manuPicBox.Image.Size.Width];

            for (int i=0; i<manuPicBox.Image.Size.Height; i++)
            {
                for(int j=0; j<manuPicBox.Image.Size.Width; j++)
                {
                    m_PixelDetailsMatrix[i, j] = new PixelDetails();
                }
            }
        }

        private void initManuImage(string i_FileName)
        {      
            manuPicBox.Image = new Bitmap(i_FileName);
            setManuPicBoxSizeAndRefresh();      
        }

        private void setManuPicBoxSizeAndRefresh()
        {
            manuPicBox.SizeMode = PictureBoxSizeMode.AutoSize;
            manuPicBox.Refresh();
        }

        private void initLetterLabel()
        {
            m_LetterLabel = new Label();
            m_LetterLabel.Parent = manuPicBox;
            m_LetterLabel.Visible = false;
        }

        private void drawRectangle(int i_X, int i_Y, int i_Width, int i_Height, EType i_Type, Color i_Color)//make a rectangle around the line detected
        {
            //refresh the picture box
            manuPicBox.Refresh();
            //create a new Bitmap object
            //Bitmap map = (Bitmap)manuPicBox.Image;
            Bitmap map = (Bitmap)manuPicBox.Image;
            //create a graphics object
            Graphics g = Graphics.FromImage(map);
            if (EType.Border == i_Type)
            {
                //create a pen object and setting the color and width for the pen
                using (Pen p = new Pen(Color.Black, 2))
                {
                    //draw line between  point p1 and p2
                    g.DrawRectangle(p, i_X, i_Y, i_Width, i_Height);
                }
            }
            else
            {
                using (Brush brush = new SolidBrush(Color.FromArgb(128, i_Color)))
                {
                    g.FillRectangle(brush, i_X, i_Y, i_Width, i_Height);
                }
            }
            manuPicBox.Image = map;
            //dispose graphics object
            g.Dispose();
        }

        private void onGoClick()
        {
            int index = -1;
            
            int indexToText;
            bool textFound = false;
            //String temp = richTextBox1.Text;
            richTextBox1.SelectAll();
            richTextBox1.SelectionBackColor = Color.White;

            while (index < richTextBox1.Text.LastIndexOf(searchTextBox.Text))
            {
                if (index == -1)
                    index = 0;
                indexToText = richTextBox1.Find(searchTextBox.Text, index, richTextBox1.TextLength, RichTextBoxFinds.MatchCase);
                if (textFound == false && indexToText >= 0 && richTextBox1.Text.Substring(indexToText, searchTextBox.Text.Length) == searchTextBox.Text)
                {
                    textFound = true;
                }
                //richTextBox1.SelectionBackColor = Color.Yellow;
                int currLength = richTextBox1.SelectionLength;
                int currOffset = richTextBox1.SelectionStart;
                paintTarnscriptFromCurrOffset(currLength, currOffset);
                index = richTextBox1.Text.IndexOf(searchTextBox.Text, index)+1;                
            }

            if (textFound)
            {
                drawRectanglesOnSearch();
            }
        }

        private void paintTarnscriptFromCurrOffset(int i_CurrLength, int i_CurrOffset)
        {
            int colorIndex = 0;
            for (int i = 0; i < i_CurrLength; i++)
            {
                richTextBox1.Select(i_CurrOffset + i, 1);
                if (colorIndex == 0)
                {
                    richTextBox1.SelectionBackColor = System.Drawing.ColorTranslator.FromHtml(m_FirstTranscriptColor);
                }
                else
                {
                    richTextBox1.SelectionBackColor = System.Drawing.ColorTranslator.FromHtml(m_SecondTranscriptColor);
                }
                colorIndex = (colorIndex + 1) % 2;
            }
        }

        private void drawRectanglesOnSearch()
        {            
            initManuImage(m_ManuFileName);
            string lettersStr = new string (m_Letters);
            int i = 0;
            
            while ((i = lettersStr.IndexOf(searchTextBox.Text, i)) != -1)
	        {
                int currentColorIndex = 0;
                //
                int lineNum = richTextBox1.GetLineFromCharIndex(i);
                int offset = richTextBox1.Lines[lineNum].IndexOf(searchTextBox.Text);
                
                for(int j=0; j < searchTextBox.Text.Length; j++)
                {
                    //look for the pixels of this index in m_PixelDetailsMatrix and color it
                    for (int h = 0; h < manuPicBox.Image.Size.Height; h++)
                    {
                        for (int w = 0; w < manuPicBox.Image.Size.Width; w++)
                        {
                            if (m_PixelDetailsMatrix[h, w].IndexInLine == offset && m_PixelDetailsMatrix[h, w].LineNumber == lineNum)
                            {

                                //drawRectangle(m_LettersMatrix[i + j].X, m_LettersMatrix[i + j].Y - m_LettersMatrix[i + j].Hight,
                                //m_LettersMatrix[i + j].Width, m_LettersMatrix[i + j].Hight, EType.Filled, m_ManuscriptColorsArray[currentColorIndex]);
                                drawRectangle(w, h, 1, 1, EType.Filled, m_ManuscriptColorsArray[currentColorIndex]);                                
                            }
                        }
                    }
                    offset++;
                    currentColorIndex = (currentColorIndex + 1) % 2;
                }
                i++;
            }
        }

        private void paintManuscript()
        {
            int currentColorIndex = 0;
            for (int j = 0; j < m_CurrLineAlignment.Length; j++)
            {
                drawRectangle(m_CurrLineAlignment[j].X, m_CurrLineAlignment[j].Y - m_CurrLineAlignment[j].H,
                    m_CurrLineAlignment[j].W, m_CurrLineAlignment[j].H, EType.Filled, m_ManuscriptColorsArray[currentColorIndex]);
                currentColorIndex = (currentColorIndex + 1) % 2;
            }
        }
        class CMR// Character Measures Recognition- the proper input is a string that is ends with '\n'
        {
            private EMoveMode m_MoveMode;
            private EDirection m_PartialTextDir;

            public CMR(EMoveMode i_MoveMode, EDirection i_PartialTextDir)
            {
                m_MoveMode = i_MoveMode;
                m_PartialTextDir = i_PartialTextDir;
            }

            int[,] m_PixelMatrix;//each cell in this matrix will contain the character that appears in the pixel as it's index in the 
            double[,] m_IndexMatrix;
            int m_IndexMatrixRows;
            int m_IndexMatrixCols;
            Bitmap m_Image;
            public int IndexMatrixRows
            {
                get { return m_IndexMatrixRows; }
            }            

            public int IndexMatrixCols
            {
                get { return m_IndexMatrixCols; }
            }
            public double[,] IndexMatrix
            {
                get { return m_IndexMatrix; }
            }

            public int[,] PixelMatrix
            {
                get { return m_PixelMatrix; }
            }

            

            public Bitmap Image
            {
                get { return m_Image; }
            }

            public static Bitmap convert_text_to_image(string txt, string fontname, int fontsize)
            {
                //creating bitmap image
                Bitmap bmp = new Bitmap(1, 1);

                //fromimage method creates a new graphics from the specified image.
                Graphics graphics = Graphics.FromImage(bmp);
                // create the font object for the image text drawing.
                Font font = new Font(fontname, fontsize);
                // instantiating object of bitmap image again with the correct size for the text and font.
                SizeF stringsize = graphics.MeasureString(txt, font);
                bmp = new Bitmap(bmp, (int)stringsize.Width, (int)stringsize.Height);
                graphics = Graphics.FromImage(bmp);

                /* it can also be a way
               bmp = new bitmap(bmp, new size((int)graphics.measurestring(txt, font).width, (int)graphics.measurestring(txt, font).height));*/

                //draw specified text with specified format 
                SolidBrush drawBrush = new SolidBrush(Color.Black);
                graphics.DrawString(txt, font, drawBrush, 0, 0);
                font.Dispose();
                graphics.Flush();
                graphics.Dispose();
                drawBrush.Dispose();
                return bmp;     //return bitmap image 
            }

            private Bitmap ConvertCharToImage(char charToConvert, string fontname, int fontsize)
            {
                //creating bitmap image
                Bitmap retBmp = new Bitmap(1, 1);
                //convert the char to string
                String charAsString = charToConvert.ToString();
                //FromImage method creates a new Graphics from the specified Image.
                Graphics graphics = Graphics.FromImage(retBmp);
                // Create the Font object for the image text drawing.
                Font font = new Font(fontname, fontsize);
                // Instantiating object of Bitmap image again with the correct size for the text and font.
                SizeF stringSize = graphics.MeasureString(charAsString, font);
                retBmp = new Bitmap(retBmp, (int)stringSize.Width, (int)stringSize.Height);
                graphics = Graphics.FromImage(retBmp);
                graphics.Clear(Color.White);
                //Draw Specified text with specified format 
                graphics.DrawString(charAsString, font, Brushes.Black, 0, 0);// making transctipt image

                font.Dispose();
                graphics.Flush();
                graphics.Dispose();

                return retBmp;     //return Bitmap Image 
            }

            private void AppendImage(int i_Direction, Bitmap imageToAppend)
            {
                Bitmap res = new Bitmap(m_Image.Size.Width + imageToAppend.Size.Width,
                    Math.Max(m_Image.Size.Height, imageToAppend.Size.Height));

                if (i_Direction == 1)
                {
                    for (int y = 0; y < m_Image.Size.Height; y++)//copy original Image
                    {
                        for (int x = 0; x < m_Image.Width; x++)
                            res.SetPixel(x, y, m_Image.GetPixel(x, y));
                    }
                }

                for (int y = 0; y < imageToAppend.Size.Height; y++)//copy imageToAppend
                {
                    for (int x = m_Image.Width; x < imageToAppend.Size.Width + m_Image.Width; x++)
                        res.SetPixel(x, y, imageToAppend.GetPixel(x - m_Image.Width, y));
                }

                if (i_Direction == 0)
                {
                    for (int y = 0; y < m_Image.Size.Height; y++)//copy original Image
                    {
                        for (int x = 0; x < m_Image.Width; x++)
                            res.SetPixel(x, y, m_Image.GetPixel(x, y));
                    }
                }
                m_Image = res;
                //return res;
            }
            /// <summary>
            /// dierection: 0- right to left, 1- left to right
            /// </summary>
            /// <param name="direction"></param>
            /// <param name="i_Str"></param>
            /// <param name="i_Fontname"></param>
            /// <param name="i_Fontsize"></param>
            public void StringToImage(int i_Direction, String i_Str, string i_Fontname, int i_Fontsize)
            {
                int widthOffset, x;
                if (i_Str.Length == 0)
                {
                    throw new NullReferenceException("Empty string");
                }
                initPixelAndIndexMatrix(i_Str, i_Fontname, i_Fontsize);
                Bitmap imageToAppend;
                
                
                if (i_Direction == 1)//L to R
                {
                    m_Image = ConvertCharToImage(i_Str[0], i_Fontname, i_Fontsize);
                    updatePixelMatrix(0, 0);
                    updateIndexMatrix(0, 0, m_Image.Size.Height-1, m_Image.Size.Width, m_Image.Size.Height);
                    for (int i = 1; i < i_Str.Length-1; i++)
                    {
                        widthOffset = m_Image.Size.Width;
                        x = widthOffset;
                        imageToAppend = ConvertCharToImage(i_Str[i], i_Fontname, i_Fontsize);
                        AppendImage(i_Direction, imageToAppend);
                        updatePixelMatrix(widthOffset, i);
                        updateIndexMatrix(i, x, m_Image.Height, m_Image.Size.Width-x, m_Image.Height);
                    }
                }
                else // R to L
                {
                    //if (m_PartialTextDir == EDirection.right && m_MoveMode == EMoveMode.col)
                    //{
                    //    m_Image = ConvertCharToImage(i_Str[i_Str.Length - 1], i_Fontname, i_Fontsize);
                    //    updatePixelMatrix(0, 0);
                    //    updateIndexMatrix(i_Str.Length - 1, m_Image.Size.Width - 1, m_Image.Size.Height, m_Image.Size.Width, m_Image.Size.Height);
                    //    for (int i = i_Str.Length - 2; i >= 0; i--)
                    //    {
                    //        widthOffset = m_Image.Size.Width;
                    //        x = widthOffset;
                    //        imageToAppend = ConvertCharToImage(i_Str[i], i_Fontname, i_Fontsize);
                    //        AppendImage(i_Direction, imageToAppend);
                    //        updatePixelMatrix(widthOffset, i);
                    //        updateIndexMatrix(i, x, m_Image.Height, m_Image.Size.Width - x, m_Image.Height);
                    //    }
                    //}
                    //else
                    //{
                    //    m_Image = ConvertCharToImage(i_Str[i_Str.Length - 2], i_Fontname, i_Fontsize);
                    //    updatePixelMatrix(0, 0);
                    //    updateIndexMatrix(i_Str.Length - 2, m_Image.Size.Width - 1, m_Image.Size.Height, m_Image.Size.Width, m_Image.Size.Height);
                    //    for (int i = i_Str.Length - 3; i >= 0; i--)
                    //    {
                    //        widthOffset = m_Image.Size.Width;
                    //        x = widthOffset;
                    //        imageToAppend = ConvertCharToImage(i_Str[i], i_Fontname, i_Fontsize);
                    //        AppendImage(i_Direction, imageToAppend);
                    //        updatePixelMatrix(widthOffset, i);
                    //        updateIndexMatrix(i, x, m_Image.Height, m_Image.Size.Width - x, m_Image.Height);
                    //    }
                    //}
                    m_Image = ConvertCharToImage(i_Str[i_Str.Length - 2], i_Fontname, i_Fontsize);
                    updatePixelMatrix(0, 0);
                    updateIndexMatrix(i_Str.Length - 2, m_Image.Size.Width - 1, m_Image.Size.Height, m_Image.Size.Width, m_Image.Size.Height);
                    for (int i = i_Str.Length - 3; i >= 0; i--)
                    {
                        widthOffset = m_Image.Size.Width;
                        x = widthOffset;
                        imageToAppend = ConvertCharToImage(i_Str[i], i_Fontname, i_Fontsize);
                        AppendImage(i_Direction, imageToAppend);
                        updatePixelMatrix(widthOffset, i);
                        updateIndexMatrix(i, x, m_Image.Height, m_Image.Size.Width - x, m_Image.Height);
                    }
                    
                    
                }
                

                //m_Image = res;
                //return res;
            }

            private void updateIndexMatrix(int i_Index, int i_X, int i_Y, int i_W, int i_H)
            {
                m_IndexMatrix[i_Index, 0] = i_X;
                m_IndexMatrix[i_Index, 1] = i_Y;
                m_IndexMatrix[i_Index, 2] = i_W;
                m_IndexMatrix[i_Index, 3] = i_H;
            }

            private void initPixelAndIndexMatrix(string str, string fontname, int fontsize)
            {
                m_IndexMatrix = new double[str.Length-1,4];
                m_IndexMatrixRows = str.Length;
                m_IndexMatrixCols = 4;
                Bitmap retBmp = new Bitmap(1, 1);
                Graphics graphics = Graphics.FromImage(retBmp);
                Font font = new Font(fontname, fontsize);
                SizeF stringSize = graphics.MeasureString(str[0].ToString(), font);
                for (int i = 1; i < str.Length; i++)
                {
                    stringSize.Width += (graphics.MeasureString(str[i].ToString(), font)).Width;
                    stringSize.Width = Math.Max((graphics.MeasureString(str[i].ToString(), font)).Height, stringSize.Width);
                }

                m_PixelMatrix = new Int32[(int)stringSize.Height, (int)stringSize.Width];
            }

            private void updatePixelMatrix(int widthOffset, Int32 i)
            {
                for (int y = 0; y < Image.Size.Height; y++)
                    for (int x = widthOffset; x < Image.Size.Width; x++)
                        m_PixelMatrix[y, x] = i;
            }
        }

        private void clickableLetters()
        {
            char ch;
            m_LetterLabel.Visible = false;
            if (manuPicBox.Image != null)
            {
                Point pos = manuPicBox.PointToClient(Cursor.Position);
                if (pos.X >= 0 && pos.Y >= 0)
                    ch = m_Indexes[pos.Y][pos.X];
                else
                    ch = '\0';
                if (ch != '\0')
                {
                    m_LetterLabel.Location = new Point(pos.X, pos.Y - 13);
                    m_LetterLabel.Text = ch.ToString();
                    m_LetterLabel.Visible = true;
                    m_LetterLabel.BackColor = Color.LightYellow;
                    m_LetterLabel.Size = new Size(14, 13);
                }
            }
        }

        private void InsertInfoPoint(Point i_Location)
        {
            // insert actual "info point"
            Label lbl = new Label()
            {
                Text = "new label",
                BorderStyle = BorderStyle.FixedSingle,
                Left = i_Location.X,
                Top = i_Location.Y
            };
            this.Controls.Add(lbl);
        }

        private MWArray getCroppedImageForFindLines()
        {
            MWArray imageMat = null;
            Bitmap bmpImage;
            int x = 0;
            int y = 0;
            //if (m_LinesRectangles.Count > 0)
            if (m_CurrentTranscriptLineIndex > 0)
            {
                //y = m_LinesRectangles[m_LinesRectangles.Count() - 1].Y + m_LinesRectangles[m_LinesRectangles.Count() - 1].H;
                y = m_ButtomLeftLinePoint.Y+1;
            }

            if(manuPicBox.Image.Size.Height - y > 0)
            {
                Rectangle rec = new Rectangle(x, y, manuPicBox.Image.Size.Width, manuPicBox.Image.Size.Height - y);
                bmpImage = cropImage(manuPicBox.Image, rec);
            }
            else
            {
                bmpImage = null;
            }
            imageMat = Bmp2MWArr.Bitmap2MWArray(bmpImage);
            return imageMat;
        }

        private Bitmap cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }

        private void drawResizbleRectangle()
        {
            if (m_ManuPicBoxContainAPic == true)//seinity check
            {
                initManuImage(m_ManuFileName);
                Bitmap map = (Bitmap)manuPicBox.Image;
                Graphics g = Graphics.FromImage(map);
                using (Pen p = new Pen(Color.Black, 2))
                {
                    p.DashStyle = DashStyle.Dash;
                    g.DrawLine(p, m_ButtomRightLinePoint, m_ButtomLeftLinePoint);
                    if (m_SetLineWasPressedForTheFirstTime == true)
                    {
                        p.DashStyle = DashStyle.Solid;
                    }
                    g.DrawLine(p, m_TopRightLinePoint, m_TopLeftLinePoint);
                }
                manuPicBox.Refresh();
                g.Dispose();
            }
        }

        private void setLineButton_Click(object sender, EventArgs e)
        {
            setLines();
        }

        private void setLines()
        {
            m_LinesRectangles.Add(new AlignmentRectangle(m_TopLeftLinePoint.X, m_TopLeftLinePoint.Y,
                m_TopRightLinePoint.X - m_TopLeftLinePoint.X, m_ButtomLeftLinePoint.Y - m_ButtomRightLinePoint.Y));
        }        

        private void onclick(object sender, EventArgs e)
        {
            
        }

        private void onFindLineClick(object sender, EventArgs e)
        {
            findLine();
        }

        private void findLine()
        {            
            initManuImage(m_ManuFileName);
            if (m_FindLineClicked == false)
            {
                m_FindLineClicked = true;
            }
            initLineBordersPoints();
            initCorrectionLinePoints();
            drawResizbleRectangle();
            selectCurrLineInTextBox();

            leftDirButton.Visible = false;
            rightDirButton.Visible = false;
        }

        private void initLineBordersPoints()
        {
            int topHorizonPixel, buttomHorizonPixel;
            getHorizlePixelOfTheRectangle(out topHorizonPixel, out buttomHorizonPixel, true);
            m_ButtomLeftLinePoint.X = m_TopLeftLinePoint.X = 0;
            m_ButtomRightLinePoint.X = m_TopRightLinePoint.X = manuPicBox.Size.Width - 1;
            m_ButtomRightLinePoint.Y = m_ButtomLeftLinePoint.Y = m_CurrentStartEndLine.Value;
            m_TopRightLinePoint.Y = m_TopLeftLinePoint.Y = m_CurrentStartEndLine.Key;
        }


        private void initCorrectionLinePoints()
        {
            //int topHorizonPixel, buttomHorizonPixel;
            //getHorizlePixelOfTheRectangle(out topHorizonPixel, out buttomHorizonPixel, false);
            m_TopCorrectionPoint.X = m_ButtomCorrectionPoint.X = manuPicBox.Size.Width / 2;
            m_ButtomCorrectionPoint.Y = m_CurrentStartEndLine.Value;
            m_TopCorrectionPoint.Y = m_CurrentStartEndLine.Key;
        }

        private void drawResizbleVerticalLine()
        {
            if (m_ManuPicBoxContainAPic == true)//seinity check
            {
                //initManuImage(m_ManuFileName);
                Bitmap map = (Bitmap)manuPicBox.Image;
                Graphics g = Graphics.FromImage(map);
                using (Pen p = new Pen(Color.LightYellow, 4))
                {
                    p.DashStyle = DashStyle.Dash;
                    g.DrawLine(p, m_ButtomCorrectionPoint, m_TopCorrectionPoint);
                    if (m_SetLineWasPressedForTheFirstTime == true)
                    {
                        p.DashStyle = DashStyle.Solid;
                    }
                }
                manuPicBox.Refresh();
                g.Dispose();
                leftDirButton.Visible = true;
                rightDirButton.Visible = true;
                rightDirButton.BackColor = Color.Transparent;
                leftDirButton.BackColor = m_FixButtonChosenColor;
            }
        }

        private void selectCurrLineInTextBox()
        {
            int ? start = null;
            richTextBox1.SelectAll();
            richTextBox1.SelectionBackColor = Color.White;
            while (start == null || richTextBox1.Text[(int)start] == '\n')
            {
                if(start != null)
                {
                    m_CurrentTranscriptLineIndex++;
                }
                start = richTextBox1.GetFirstCharIndexFromLine(m_CurrentTranscriptLineIndex);
            }
            int length = richTextBox1.Lines[m_CurrentTranscriptLineIndex].Length;
            //paintTarnscriptFromCurrOffset(length, start);
            richTextBox1.Select((int)start, length);            
            richTextBox1.SelectionBackColor = SystemColors.Highlight;
            m_CurrentTranscriptLineIndex++;
            
        }

        private void manuPicBox_MouseMove(object sender, MouseEventArgs e)
        {
            updateWidthHight(e);
            int y = Math.Max(e.Y, 0);
            int x = Math.Max(e.X, 0);
            y = Math.Min(manuPicBox.Size.Height - 1, e.Y);
            x = Math.Min(manuPicBox.Size.Width - 1, e.X);

            if (m_PointMoveInProgress == ELineChangeType.topLineMove) // If moving first point
            {
                m_TopLeftLinePoint.Y = y;
                m_TopRightLinePoint.Y = y;
                m_CurrentStartEndLine = new KeyValuePair<int, int>(y, m_CurrentStartEndLine.Value);
                //changeStartLineInterger(y);                
                Refresh();
            }
            else if (m_PointMoveInProgress == ELineChangeType.buttomLineMove) // If moving second point
            {
                m_ButtomRightLinePoint.Y = y;
                m_ButtomLeftLinePoint.Y = y;
                //m_StartEndOfLinesPixels[m_StartEndOfLinesPixels.ElementAt(m_CurrentManuscriptLineIndex).Key] = y;
                m_CurrentStartEndLine = new KeyValuePair<int,int>(m_CurrentStartEndLine.Key,y);
                Refresh();
            }
            else if (m_PointMoveInProgress == ELineChangeType.verticalLineMove)// if moving is vertical line
            {
                m_ButtomCorrectionPoint.X = x;
                m_TopCorrectionPoint.X = x;
                Refresh();
            }
            else // If moving in the PictureBox: change cursor to hand if above a handle
            {
                //line borders
                if (m_CurrMode == EMoveMode.line &&((Math.Abs(e.Y - m_ButtomLeftLinePoint.Y) < m_HandleRadius) ||
                        (m_SetLineWasPressedForTheFirstTime == false &&(Math.Abs(e.Y - m_TopLeftLinePoint.Y) < m_HandleRadius))))
                {
                    Cursor.Current = Cursors.HSplit;
                }
                //correction line
                else if (m_CurrMode == EMoveMode.col && (((e.Y - m_ButtomCorrectionPoint.Y) < m_HandleRadius &&
                            (m_TopCorrectionPoint.Y - m_HandleRadius) < e.Y) &&
                         Math.Abs(e.X - m_ButtomCorrectionPoint.X) < m_HandleRadius))//make sure it's also in x borders
                {                        
                    Cursor.Current = Cursors.VSplit;
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void updateWidthHight(MouseEventArgs e)
        {
            widthLabel.Text = e.X.ToString();
            hightLabel.Text = e.Y.ToString();
        }

        //private void changeStartLineInterger(int i_NewStartPoint)
        //{
        //    int i = 0;
        //    Dictionary <int, int> newStartEndOfLinesPixels = new Dictionary<int,int>();
        //    foreach (KeyValuePair<int, int> pair in m_StartEndOfLinesPixels)
        //    {
        //        if(i == m_CurrentManuscriptLineIndex)
        //        {
        //            newStartEndOfLinesPixels.Add(i_NewStartPoint, pair.Value);
        //        }
        //        else
        //        {
        //            newStartEndOfLinesPixels.Add(pair.Key, pair.Value);
        //        }
        //        i++;
        //    }
        //    m_StartEndOfLinesPixels = newStartEndOfLinesPixels;
        //}        

        private void manuPicBox_MouseDown(object sender, MouseEventArgs e)
        {
            // Determine if a point is under the cursor. If so, declare that a move is in progress
            if (m_CurrMode == EMoveMode.line)
            {
                identityLineMoveProgress(e);
            }
            else if (m_CurrMode == EMoveMode.col)
            {
                identifyColMoveProgress(e);
            }

        }

        private void identifyColMoveProgress(MouseEventArgs e)
        {
            if ((((e.Y - m_ButtomCorrectionPoint.Y) < m_HandleRadius &&
                            (m_TopCorrectionPoint.Y - m_HandleRadius) < e.Y) &&
                         Math.Abs(e.X - m_ButtomCorrectionPoint.X) < m_HandleRadius))
            {
                m_PointMoveInProgress = ELineChangeType.verticalLineMove;
            }
            else
            {
                m_PointMoveInProgress = ELineChangeType.none;
            }
        }

        private void identityLineMoveProgress(MouseEventArgs e)
        {
            if (Math.Abs(e.Y - m_ButtomLeftLinePoint.Y) <= m_HandleRadius)
            {
                m_PointMoveInProgress = ELineChangeType.buttomLineMove;
            }
            else if (m_SetLineWasPressedForTheFirstTime == false && Math.Abs(e.Y - m_TopLeftLinePoint.Y) <= m_HandleRadius)
            {
                m_PointMoveInProgress = ELineChangeType.topLineMove;
            }
            else
            {
                m_PointMoveInProgress = ELineChangeType.none;
            }
        }       

        private void manuPicBox_MouseUp(object sender, MouseEventArgs e)
        {
            m_PointMoveInProgress = ELineChangeType.none ;
            if (m_CurrMode == EMoveMode.line)
            {
                drawResizbleRectangle();
                initCorrectionLinePoints();
            }
            else if (m_CurrMode == EMoveMode.col)
            {
                drawVerticalLineAfterCorrection();
            }
        }

        private void drawVerticalLineAfterCorrection()
        {
            if (m_ManuPicBoxContainAPic == true)//seinity check
            {
                loadBackUpImage();
                
                //initManuImage(m_ManuFileName);
                Bitmap map = (Bitmap)manuPicBox.Image;
                Graphics g = Graphics.FromImage(map);
                using (Pen p = new Pen(Color.LightYellow, 4))
                {
                    p.DashStyle = DashStyle.Dash;
                    g.DrawLine(p, m_ButtomCorrectionPoint, m_TopCorrectionPoint);
                    if (m_SetLineWasPressedForTheFirstTime == true)
                    {
                        p.DashStyle = DashStyle.Solid;
                    }
                }
                manuPicBox.Refresh();
                g.Dispose();
            }
        }

        private void loadBackUpImage()
        {
            manuPicBox.Image = new Bitmap(m_BackupPath);
            setManuPicBoxSizeAndRefresh();
        }

        private void transcriptionFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setParams();
        }

        private void setParams()
        {
            using (var dlg = new FontDialog())
            {
                dlg.Font = new Font("Arial Black", 80);
                DialogResult result = dlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    // Get Font.
                    Font font = dlg.Font;
                    // Set richTextBox properties.                    
                    richTextBox1.Font = font;
                    richTextBox1.SelectionFont = new Font(font.FontFamily, font.Size);
                }
            }
        }


        private void PlayButton_Click(object sender, EventArgs e)
        {
            bool dataIsSet = wasDataInitlized();
            if (m_LineWasDetected == false)
            {
                MessageBox.Show("Line wasn't detected");
            }
            if (richTextBox1.SelectedText.Length <= 1)
            {
                MessageBox.Show("Please choose a text from the transcript");
            }
            else if (dataIsSet)
            {
                setOffsetInLine();
                alignLine();
            }            
        }

        private void setOffsetInLine()
        {
            int selectionIndex = richTextBox1.SelectionStart;
            int lineNum = richTextBox1.GetLineFromCharIndex(selectionIndex);
            m_OffsetInLine = (richTextBox1.Lines[lineNum]+"\n").IndexOf(richTextBox1.SelectedText);
            m_OffsetInLine = Math.Max(m_OffsetInLine, 0);
            m_NumOfCharsInCurrLine = richTextBox1.Lines[lineNum].Length;
        }

        private void alignLine()
        {
            initManuImage(m_ManuFileName);
            drawResizbleRectangle();

            updateRectanglePointsAndAddLineIfNessecary();
            string text = richTextBox1.SelectedText;
            if (text[text.Length - 1] != '\n')
            {
                text += '\n';
            }
            CMR cmr = new CMR(m_CurrMode, m_FixDirection);
            m_CurrMode = EMoveMode.col;
            
            
            cmr.StringToImage(0, richTextBox1.SelectedText, m_FontName, m_FontSize);
            cmr.Image.Save(Directory.GetCurrentDirectory() + "\\myBitmap.bmp");
            //Rectangle srcRect = new Rectangle(0, m_TopRightLinePoint.Y, m_TopRightLinePoint.X - 1, m_ButtomRightLinePoint.Y - m_TopRightLinePoint.Y);
            Rectangle srcRect = new Rectangle(m_TopLeftLinePoint.X, m_TopRightLinePoint.Y, m_TopRightLinePoint.X - 1- m_TopLeftLinePoint.X, m_ButtomRightLinePoint.Y - m_TopRightLinePoint.Y);
            System.Drawing.Imaging.PixelFormat format = manuPicBox.Image.PixelFormat;
            m_Cropped = ((Bitmap)manuPicBox.Image).Clone(srcRect, format);
            MWArray ancientTextMatrix = Bmp2MWArr.Bitmap2MWArray(m_Cropped); // ancient text
            MWArray translateTextMatrix = Bmp2MWArr.Bitmap2MWArray(cmr.Image); // translated text

            MWNumericArray matrix = new MWNumericArray(MWArrayComplexity.Real, MWNumericType.Double,
                                                            2, cmr.IndexMatrixRows, cmr.IndexMatrixCols);
            matrix = cmr.IndexMatrix;
            try
            {
                AlignText align = new AlignText();
                MWArray[] indexedImg = align.alignLine(1, ancientTextMatrix, translateTextMatrix, matrix);
                deleteMatrixFile();
                File.Move(Directory.GetCurrentDirectory() + "\\indext_matrix.txt", Path.ChangeExtension(Directory.GetCurrentDirectory() + "\\indext_img.txt", ".csv"));
                paintManuScriptLineAndSaveResultsInMatrix();
                paintTarnscriptLine();
                saveBackupManu();
                drawResizbleVerticalLine();
                
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message);
            }
        }

        private void updateRectanglePointsAndAddLineIfNessecary()
        {
            if (m_CurrMode == EMoveMode.col)
            {
                if (m_FixDirection == EDirection.left)
                {
                    m_TopRightLinePoint.X = m_TopCorrectionPoint.X;
                    m_ButtomRightLinePoint.X = m_ButtomCorrectionPoint.X;
                }
                else if (m_FixDirection == EDirection.right)
                {
                    m_TopLeftLinePoint.X = m_TopCorrectionPoint.X;
                    m_ButtomLeftLinePoint.X = m_ButtomCorrectionPoint.X;
                }
            }
            else
            {
                m_FinalStartEndOfLinesPixels.Add(m_CurrentStartEndLine.Key, m_CurrentStartEndLine.Value);
                m_NumOfRecognizedLinesInTranscript++;
            }
        }


        private void paintTarnscriptLine()
        {
            int start = richTextBox1.GetFirstCharIndexFromLine(m_CurrentTranscriptLineIndex-1);
            int length = richTextBox1.Lines[m_CurrentTranscriptLineIndex-1].Length;
            paintTarnscriptFromCurrOffset(length, start);
        }

        private void paintManuScriptLineAndSaveResultsInMatrix()// reads matrix from m_AlignReturnedMatrixFilePath and paint the letters
        {
            CsvFileReader matrixCsv = new CsvFileReader(m_AlignReturnedMatrixFilePath);
            CsvRow currRow = new CsvRow();
            int x, y = 1;
            int toAdd = 0;
             int toAdd2 = 0;
             if (m_FixDirection == EDirection.right)
             {
                 toAdd = m_OriginalImage.Width-  m_ButtomCorrectionPoint.X;
                 toAdd2 = m_OffsetInLine;
             }
            while (matrixCsv.ReadRow(currRow))
            {
                for (x = 1; x < m_Cropped.Size.Width - 1; x++)// the alignline bring as back a matrix in the size of m_cropped minus one line from each side and one coloum from each side
                {
                    int currentLetterIndexInLine = Int32.Parse(currRow[x - 1])+toAdd2;
                    m_PixelDetailsMatrix[y + m_FinalStartEndOfLinesPixels.ElementAt(
                        m_CurrentManuscriptLineIndex).Key, x+toAdd].SetPixelDetails
                        (m_CurrentManuscriptLineIndex, currentLetterIndexInLine);
                    if (currentLetterIndexInLine > 0)
                    {
                        drawRectangle(x, y + m_FinalStartEndOfLinesPixels.ElementAt(m_CurrentManuscriptLineIndex).Key, 1, 1, EType.Filled,
                            m_ManuscriptColorsArray[(currentLetterIndexInLine - 1) % 2]);
                    }
                }
                y++;
            }

            matrixCsv.Close();
            deleteMatrixFile();
        }

        private void saveBackupManu()
        {
            m_ManuPicBoxBackUp = (Bitmap)manuPicBox.Image;
            m_ManuPicBoxBackUp.Save(Directory.GetCurrentDirectory() + "\\backup.bmp");
            m_BackupPath = Directory.GetCurrentDirectory() + "\\backup.bmp";
        }

        private void deleteMatrixFile()
        {
            if (File.Exists(m_AlignReturnedMatrixFilePath))
            {
                File.Delete(m_AlignReturnedMatrixFilePath);
            }
        }


        private void findNextLine_Click(object sender, EventArgs e)
        {
           m_CurrMode = EMoveMode.line;
            bool dataIsSet = wasDataInitlized();

            if(dataIsSet)
            {
                findLine();
                m_LineWasDetected = true;
            }
        }

        private bool wasDataInitlized()
        {
            bool res = true;

            if (m_ManuFileName == null)
            {
                if (richTextBox1.Text == "")
                    MessageBox.Show("Please load the manuscript and the transcript files");
                else
                    MessageBox.Show("Please load the manuscript file");
                res = false;

            }

            else if (richTextBox1.Text == "")
            {
                MessageBox.Show("Please load the transcript file");
                res = false;
            }
            return res;
        }

        private void loadManuscriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadManuscript();
        }

        private void loadTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadTranscript();
        }
                
        private void searchTextBox_Click(object sender, EventArgs e)
        {
            searchTextBox.Text = "";
            searchTextBox.ForeColor = Color.Black;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            onGoClick();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"©Tal Hassner website:
                            http://www.openu.ac.il/home/hassner/");
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            searchTextBox.Focus();
            searchTextBox_Click(sender, e);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void leftDirButton_Click(object sender, EventArgs e)
        {
            leftDirButton.BackColor = m_FixButtonChosenColor;
            rightDirButton.BackColor = Color.Transparent;
            m_FixDirection = EDirection.left;
        }

        private void rightDirButton_Click(object sender, EventArgs e)
        {
            rightDirButton.BackColor = m_FixButtonChosenColor;
            leftDirButton.BackColor = Color.Transparent;
            m_FixDirection = EDirection.right;
        }

    }
}
