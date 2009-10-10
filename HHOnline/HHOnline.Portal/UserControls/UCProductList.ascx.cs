﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HHOnline.Shops;
using HHOnline.Framework;
using HHOnline.SearchBarrel;

public partial class UserControls_UCProductList : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack&&Visible)
        {
            BindData();
        }
    }

    private ProductQuery _Query;
    public ProductQuery Query
    {
        get { return _Query; }
        set { _Query = value; }
    }
    private bool _IsSearch = false;
    public bool IsSearch
    {
        get { return _IsSearch; }
        set { _IsSearch = value; }
    }

    #region -Events-
    protected void dlProduct_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Product product = e.Item.DataItem as Product;
            Literal ltImage = e.Item.FindControl("ltImage") as Literal;
            if (ltImage != null)
            {
                ltImage.Text = "<div class=\"productImage\" style=\"background-image:url(" + GlobalSettings.RelativeWebRoot + product.GetDefaultImageUrl(100, 100) + ")\"></div>";
            }
            Literal ltPrice = e.Item.FindControl("ltPrice") as Literal;
            if (ltPrice != null)
            {
                decimal? price = null;
                string priceText = string.Empty;
                if (!Context.User.Identity.IsAuthenticated)
                {
                    price = ProductPrices.GetPriceDefault(product.ProductID);
                    priceText = GlobalSettings.GetPrice(price);
                }
                else
                {
                    price = ProductPrices.GetPriceMarket(Profile.AccountInfo.UserID, product.ProductID);
                    decimal? price1 = ProductPrices.GetPriceMember(Profile.AccountInfo.UserID, product.ProductID);
                    priceText = GlobalSettings.GetPrice(true, price, price1);
                }
                ltPrice.Text = priceText;
            }
        }
    }
    #endregion

    #region -Common-


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
        ProductQuery query = this.Query;
        lnkGrid.PostBackUrl = Request.RawUrl;
        lnkList.PostBackUrl = Request.RawUrl;
        lnkGrid.Attributes.Add("rel", "grid");
        lnkList.Attributes.Add("rel", "list");

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

        #region -BindData-
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
        List<Product> prods = null;
        if (!IsSearch) prods = Products.GetProducts(query).Records;
        else
        {
            if (string.IsNullOrEmpty(query.ProductNameFilter))
            {
                prods = Products.GetProducts(query).Records;
            }
            else
            {
                SearchResultDataSet<Product> _pros = ProductSearchManager.Search(query);
                prods = _pros.Records;
                pnlSearch.Visible = true;
                ltSearchDuration.Text = "搜索用时：" + _pros.SearchDuration.ToString() + "秒。";
            }
        }
        if (prods == null || prods.Count == 0)
        {
            msgBox.ShowMsg("没有符合条件的产品存在！", System.Drawing.Color.Gray);
            return;
        }
        msgBox.HideMsg();
        bool islogin = Context.User.Identity.IsAuthenticated;
        if (orderBy == ProductOrderBy.Price)
        {
            prods.Sort(new SortProductByPrice(sortOrder,
                (islogin ? Profile.AccountInfo.UserID : 0),
                islogin
                ));
        }

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
}