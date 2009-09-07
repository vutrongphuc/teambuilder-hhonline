﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HHOnline.Framework.Web;
using HHOnline.Framework;
using HHOnline.Shops;

public partial class Pages_Product_Industry : HHPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
        }

        base.ExecuteJs("var _nativeUrl = '" + GetUrl() + "'", true);
    }

    #region -Events-
    protected void dlProduct_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Product product = e.Item.DataItem as Product;
            Image productPicture = e.Item.FindControl("imgProduct") as Image;
            if (productPicture != null)
            {
                productPicture.ImageUrl = GlobalSettings.RelativeWebRoot + product.GetDefaultImageUrl(100, 100);
            }
            Literal ltPrice = e.Item.FindControl("ltPrice") as Literal;
            if (ltPrice != null)
            {
                decimal? price = null;
                if (!User.Identity.IsAuthenticated)
                {
                    price = ProductPrices.GetPriceDefault(product.ProductID);
                }
                else
                {
                    price = ProductPrices.GetPriceDefault(product.ProductID);
                }
                ltPrice.Text = (price == null ? "需询价" : price.Value.ToString("c"));
            }
        }
    }
    #endregion

    #region -Common-

    string GetUrl()
    {
        if (Request.QueryString["sortby"] == null)
        {
            return Request.RawUrl.ToString();
        }
        else
        {
            string url = Request.RawUrl.ToString().Split('&')[0];
            foreach (string k in Request.QueryString.AllKeys)
            {
                if (k != "sortby")
                {
                    url += "&" + k + "=" + Request.QueryString[k];
                }
            }
            return url;
        }
    }

    void GetData(out ProductOrderBy sortBy, out SortOrder sortOrder, string sort)
    {
        sortBy = ProductOrderBy.DisplayOrder;
        sortOrder = SortOrder.Descending;
        switch (sort)
        {
            case "PruductName":
                sortBy = ProductOrderBy.ProductName;
                break;
            case "Date":
                sortBy = ProductOrderBy.DataCreated;
                break;
            case "Variety":
                sortBy = ProductOrderBy.BrandName;
                break;
            case "PriceDesc":
                sortBy = ProductOrderBy.Price;
                sortOrder = SortOrder.Descending;
                break;
            case "PriceAsc":
                sortBy = ProductOrderBy.Price;
                sortOrder = SortOrder.Ascending;
                break;
            case "None":
            default:
                sortBy = ProductOrderBy.DisplayOrder;
                sortOrder = SortOrder.Descending;
                break;
        }
    }
    #endregion

    #region -BindData-
    void BindData()
    {
        lnkGrid.PostBackUrl = Request.RawUrl;
        lnkList.PostBackUrl = Request.RawUrl;
        lnkGrid.Attributes.Add("rel", "grid");
        lnkList.Attributes.Add("rel", "list");
        string id = Request.QueryString["ID"];
        if (string.IsNullOrEmpty(id))
        {
            hpilProduct.Visible = true;
            pnlSort.Visible = false;
        }
        else
        {
            #region -Adapt Show-
            string s = Request.QueryString["s"];
            if (!string.IsNullOrEmpty(s))
            {
                switch (s)
                {
                    case "grid":
                        lnkGrid.CssClass = "showByGrid showBy showByGridActive";
                        lnkList.CssClass = "showByList showBy";
                        cpProduct.PageSize = 20;
                        break;
                    case "list":
                        lnkList.CssClass = "showByList showBy showByListActive";
                        lnkGrid.CssClass = "showByGrid showBy";
                        cpProduct.PageSize = 100;
                        break;
                }
            }
            else
            {
                cpProduct.PageSize = 20;
                s = "grid";
                lnkGrid.CssClass = "showByGrid showBy showByGridActive";
                lnkList.CssClass = "showByList showBy";
            }
            #endregion

            hpilProduct.Visible = false;
            int pid = int.Parse(GlobalSettings.Decrypt(id));
            inProduct.IndustryID = pid;
            illProduct.IndustryID = pid;
            islProduct.IndustryID = pid;

            #region -BindData-
            ProductQuery query = new ProductQuery();
            query.IndustryID = pid;
            string sortBy = Request.QueryString["sortby"];
            ProductOrderBy orderBy = ProductOrderBy.DisplayOrder;
            SortOrder sortOrder = SortOrder.Descending;
            if (!string.IsNullOrEmpty(sortBy))
            {
                try
                {
                    ddlSortBy.Items.FindByValue(sortBy).Selected = true;
                }
                catch { ddlSortBy.SelectedIndex = 0; }
                GetData(out orderBy, out sortOrder, sortBy);
            }
            query.ProductOrderBy = orderBy;
            query.SortOrder = sortOrder;
            List<Product> prods = Products.GetProducts(query).Records;
            if (prods == null || prods.Count == 0)
            {
                msgBox.ShowMsg("没有符合条件的产品存在！", System.Drawing.Color.Gray);
                return;
            }
            msgBox.HideMsg();
            if (s == "grid")
            {
                dlProduct2.Visible = false;
                dlProduct.Visible = true;
                cpProduct.DataSource = prods;
                cpProduct.BindToControl = dlProduct;
                dlProduct.DataSource = cpProduct.DataSourcePaged;
                dlProduct.DataBind();
            }
            else
            {
                dlProduct.Visible = false;
                dlProduct2.Visible = true;
                cpProduct.DataSource = prods;
                cpProduct.BindToControl = dlProduct2;
                dlProduct2.DataSource = cpProduct.DataSourcePaged;
                dlProduct2.DataBind();

            }
            #endregion
        }
    }
    #endregion

    #region -Event-
    protected void linkButton_Click(object obj, EventArgs e)
    {
        LinkButton lnk = obj as LinkButton;
        if (Request.QueryString["s"] == null)
        {
            Response.Redirect(Request.RawUrl.ToString() + "&s=" + lnk.Attributes["rel"]);
        }
        else
        {
            string url = Request.RawUrl.ToString().Split('&')[0];
            foreach (string k in Request.QueryString.AllKeys)
            {
                if (k != "s")
                {
                    url += "&" + k + "=" + Request.QueryString[k];
                }
                else
                {
                    url += "&s=" + lnk.Attributes["rel"];
                }
            }
            Response.Redirect(url);
        }
    }

    #endregion

    #region -Override-
    public override void OnPageLoaded()
    {
        string id = Request.QueryString["ID"];
        if (!string.IsNullOrEmpty(id))
        {
            int pid = int.Parse(GlobalSettings.Decrypt(id));
            ProductIndustry pi = ProductIndustries.GetProductIndustry(pid);
            this.ShortTitle = pi.IndustryName;
        }
        else
        {
            this.ShortTitle = "所有行业";
        }
        this.SetTitle();
        this.AddJavaScriptInclude("scripts/pages/sortby.aspx.js", false, false);
    }
    #endregion
}
