﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HHOnline.Framework.Web;
using HHOnline.Shops;
using HHOnline.Framework;
using System.Text;

public partial class Pages_Product_Product : HHPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BindPicture();
        if (!IsPostBack)
        {
            BindProduct();
        }
    }
    Product p = null;
    void BindPicture()
    {
        if (p == null)
        {
            p = GetProduct();
        }
        List<ProductPicture> pics = ProductPictures.GetPictures(p.ProductID);
        string picStr = "''";
        if (pics != null && pics.Count > 0)
        {
            picStr = Newtonsoft.Json.JavaScriptConvert.SerializeObject(pics);
        }
        base.ExecuteJs("var pictures=" + picStr + ";var _infos={l:" + User.Identity.IsAuthenticated.ToString().ToLower() + ",d:" + p.ProductID + "}", true);
    }
    void BindProduct()
    {
        if (p == null)
        {
            p = GetProduct();
        }
        ltProductName.Text = p.ProductName;
        ltDescription.Text = "最后更新：" + p.UpdateTime.ToShortDateString() + "  关键字：" + p.ProductKeywords;
        ltProductCode.Text = GetString(p.ProductCode);
        ltProductAbstract.Text = GetString(p.ProductAbstract);
        BindCategory(p.ProductID);
        BindIndustry(p.ProductID);
        BindBrand(p);
        BindPrice(p.ProductID);
        ltDescription1.Text = p.ProductContent;
        BindProperty(p.ProductID);
        List<ProductModel> models = ProductModels.GetModelsByProductID(p.ProductID);
        if (models.Count == 0)
        {
            rbModel.Visible = false;
            ltModel.Text = "——";
        }
        else
        {
            ListItem li = null;
            foreach (ProductModel m in models)
            {
                li = new ListItem(m.ModelName, m.ModelID.ToString());
                li.Attributes.Add("title", m.ModelDesc);
                rbModel.Items.Add(li);
            }
            rbModel.SelectedIndex = 0;
        }
    }
    List<ProductCategory> cat = null;
    List<ProductCategory> GetCategories(int pId)
    {
        return ProductCategories.GetCategoreisByProductID(pId);
    }
    void BindCategory(int pId)
    {
        if (cat == null)
            cat = GetCategories(pId);
        if (cat.Count == 0)
        {
            ltCategory.Text = "——";
        }
        else
        {
            string cats = string.Empty;
            foreach (ProductCategory pc in cat)
            {
                cats += "<a target=\"_blank\" href=\"" + GlobalSettings.RelativeWebRoot +
                             "pages/view.aspx?product-category&ID=" + GlobalSettings.Encrypt(pc.CategoryID.ToString()) + "\">" + pc.CategoryName + "</a>";
            }
            ltCategory.Text = cats;
        }
    }
    void BindIndustry(int pId)
    {
        List<ProductIndustry> pi = ProductIndustries.GetIndustriesByProductID(pId);
        if (pi.Count == 0)
        {
            ltIndustry.Text = "——";
        }
        else
        {
            string pis = string.Empty;
            foreach (ProductIndustry p in pi)
            {
                pis += "<a target=\"_blank\" href=\"" + GlobalSettings.RelativeWebRoot +
                            "pages/view.aspx?product-industry&ID=" + GlobalSettings.Encrypt(p.IndustryID.ToString()) + "\">" + p.IndustryName + "</a>";
            }
            ltIndustry.Text = pis;
        }
    }
    void BindBrand(Product p)
    {
        if (p.BrandID == 0 || string.IsNullOrEmpty(p.BrandName))
        {
            ltBrand.Text = "——";
        }
        else
        {
            ltBrand.Text = "<a target=\"_blank\"  href=\"" + GlobalSettings.RelativeWebRoot +
                                    "pages/view.aspx?product-brand&ID=" + GlobalSettings.Encrypt(p.BrandID.ToString()) + "\">" + p.BrandName + "</a>"; ;
        }
    }
    void BindPrice(int pId)
    {
        decimal? p = null, p1 = null, p2 = null;
        string priceText = string.Empty;
        if (!Context.User.Identity.IsAuthenticated)
        {
            p1 = ProductPrices.GetPriceDefault(pId);
            p2 = ProductPrices.GetPricePromote(0, pId);
            priceText = GlobalSettings.GetPrice(p1, p2);
        }
        else
        {
            p1 = ProductPrices.GetPriceMarket(Profile.AccountInfo.UserID, pId);
            p2 = ProductPrices.GetPriceMember(Profile.AccountInfo.UserID, pId);
            p = ProductPrices.GetPricePromote(Profile.AccountInfo.UserID, pId);
            priceText = GlobalSettings.GetPrice(true, p, GlobalSettings.GetMinPrice(p1, p2));
        }
        ltPrice.Text = priceText;
    }
    void BindProperty(int pId)
    {
        List<ProductProperty> props = ProductProperties.GetPropertiesByProductID(pId);
        if (props == null || props.Count == 0)
        {
            msgBox.ShowMsg("此产品没有设置任何属性！", System.Drawing.Color.Gray);
        }
        else
        {
            msgBox.HideMsg();
            rpProperties.DataSource = props;
            rpProperties.DataBind();
        }
    }
    string GetString(string str)
    {
        if (string.IsNullOrEmpty(str)) return "——";
        else return str;
    }
    Product GetProduct()
    {
        int pid = int.Parse(GlobalSettings.Decrypt(Request.QueryString["ID"]));
        return Products.GetProduct(pid);
    }
    public override void OnPageLoaded()
    {
        p = GetProduct();
        cat = GetCategories(p.ProductID);
        StringBuilder sb = new StringBuilder();
        sb.Append(p.ProductName);
        sb.Append(",");
        if (cat != null && cat.Count > 0)
        {
            foreach (ProductCategory pc in cat)
            {
                sb.Append(pc.CategoryName);
                sb.Append(",");
            }
        }
        sb.Append(p.BrandName);
        this.AddKeywords(sb.ToString());
        this.AddDescription(string.IsNullOrEmpty(p.ProductAbstract) ? p.ProductName : p.ProductAbstract + " 关键字: " + sb.ToString());
        this.ShortTitle = p.ProductName + " - " + sb.ToString();
        SetTitle();
        AddJavaScriptInclude("scripts/jquery.accordion.js", true, false);
        AddJavaScriptInclude("scripts/jquery.lightbox-0.5.js", true, false);
        AddJavaScriptInclude("scripts/pages/product.aspx.js", true, false);
    }
}
