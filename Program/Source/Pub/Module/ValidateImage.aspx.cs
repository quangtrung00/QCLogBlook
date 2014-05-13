﻿using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing.Imaging;
using System.Drawing;

public partial class Pub_Module_ValidateImage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GenImage();
        }

    }

    //取得亂數值0~99999
    private Random GetRandom(int seed)
    {
        Random r = (Random)HttpContext.Current.Cache.Get("RandomNumber");
        if (r == null)
        {
            if (seed == 0)
                r = new Random();
            else
                r = new Random(seed);
            HttpContext.Current.Cache.Insert("RandomNumber", r);
        }
        return r;
    }

    private Random GetRandom()
    {
        return GetRandom(0);
    }

    //產生的亂數值寫入到圖檔中
    private void GenImage()
    {

        Bitmap newBitmap = null;
        Graphics g = null;

        try
        {
            Random r = GetRandom(0);

            Session["ValidNumber"] = r.Next(0, 99999).ToString("00000");			//產生亂數值
            Font fontCounter = new Font("Times New Roman", 18);

            // calculate size of the string.
            newBitmap = new Bitmap(1, 1, PixelFormat.Format32bppArgb);				//開啟一個32bits的彩色圖
            g = Graphics.FromImage(newBitmap);
            SizeF stringSize = g.MeasureString(Session["ValidNumber"].ToString(), fontCounter);
            int nWidth = (int)stringSize.Width;										//利用輸入的字串測量圖的長寬
            int nHeight = (int)stringSize.Height;
            g.Dispose();
            newBitmap.Dispose();

            newBitmap = new Bitmap(nWidth, nHeight, PixelFormat.Format32bppArgb);
            g = Graphics.FromImage(newBitmap);
            g.FillRectangle(new SolidBrush(Color.Silver), new Rectangle(0, 0, nWidth, nHeight));//設定圖的顏色和長寬

            g.DrawString(Session["ValidNumber"].ToString(), fontCounter, new SolidBrush(Color.Black), 0, 0);

            //newBitmap.Save(Server.MapPath(Session["PageLayer"] + "images/Verify.jpg"), ImageFormat.Png); //將圖形存檔
            //imgVerify.ImageUrl=("../../images/Verify.jpg");										 //呼叫圖形	
            Response.ContentType = "Image/jpeg";
            newBitmap.Save(Response.OutputStream, ImageFormat.Jpeg); //將圖形存檔

        }
        catch (Exception e)
        {
            e.Message.ToString();
        }
        finally
        {
            if (null != g) g.Dispose();
            if (null != newBitmap) newBitmap.Dispose();
        }
    }


}
