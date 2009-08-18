﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using HHOnline.Shops;
using HHOnline.Framework;
using System.IO;

namespace HHOnline.Controls
{
    public class VarietyList : UserControl
    {
        static VarietyList()
        {
            ProductBrands.Updated += delegate { _Html = string.Empty; };
        }
        private int _Num = 10;
        /// <summary>
        /// 显示的列表数
        /// </summary>
        public int Num
        {
            get { return _Num; }
            set { _Num = value; }
        }

        public static object _lock = new object();
        private static string _Html;
        public string HTML
        {
            get
            {
                if (string.IsNullOrEmpty(_Html))
                {
                    lock (_lock)
                    {
                        if (string.IsNullOrEmpty(_Html))
                        {
                            HtmlGenericControl ul = BindVarietyList();
                            StringWriter sw = new StringWriter();
                            ul.RenderControl(new HtmlTextWriter(sw));
                            _Html = sw.ToString();
                        }
                    }
                }
                return _Html;
            }
        }
        HtmlGenericControl BindVarietyList()
        {
            List<ProductBrand> brandsTemp = ProductBrands.GetProductBrands();
            List<ProductBrand> brands = brandsTemp.GetRange(0, Math.Min(_Num, brandsTemp.Count));
           
            if (brands.Count == 0)
            {
                HtmlGenericControl p = new HtmlGenericControl("P");
                p.InnerText = "没有品牌信息！";
                return p;
            }

            HtmlGenericControl ul = new HtmlGenericControl("ul");
            ul.ID = "ulVarietyList";

            HtmlGenericControl li = null;
            HtmlAnchor anchor = null;
            foreach (var b in brands)
            {
                li = new HtmlGenericControl("LI");
                anchor = new HtmlAnchor();
                anchor.HRef = GlobalSettings.RelativeWebRoot + "pages/product/variety-" + b.BrandID + ".aspx";
                anchor.InnerText = b.BrandName;
                anchor.Title = b.BrandTitle;
                li.Controls.Add(anchor);

                ul.Controls.Add(li);
            }
            return ul;
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            writer.Write(HTML);
            writer.Write(Environment.NewLine);
        }
    }
}