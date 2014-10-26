using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MathWorks.MATLAB.NET.Arrays;

namespace text_alignment
{
    static class Bmp2MWArr
    {
        public static MWNumericArray Bitmap2MWArray(Bitmap bmp)
        {
            MWNumericArray matrix = null;
            byte[, ,] rgbImage = new byte[3, bmp.Height, bmp.Width];
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    if (bmp.GetPixel(i, j).R != 0)
                    {
                        
                    }
                    rgbImage[0, j, i] = bmp.GetPixel(i, j).R;
                    rgbImage[1, j, i] = bmp.GetPixel(i, j).G;
                    rgbImage[2, j, i] = bmp.GetPixel(i, j).B;
                }
            }

            try
            {
                matrix = new MWNumericArray();
                matrix = rgbImage;
            }
            catch (Exception ex)
            {
                string s = ex.InnerException.Message;
            }            

            return matrix;
        }


        public static double[,] MWArray2Matrix(MWArray[] i_MwArray, int i_Rows, int i_Cols)
        {
            // convert 1-dim array (vector) to 2-dim array
            double[,] vec = (double[,])((MWNumericArray)i_MwArray).ToArray(MWArrayComponent.Real);

            return vec;
             
        }
   }
}
