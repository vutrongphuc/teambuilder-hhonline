﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HHOnline.Framework.Web;

public partial class Pages_ShopCart_AskPrice : HHPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public override void OnPageLoaded()
    {
        this.ShortTitle = "我的询价单";
        this.SetTitle();
    }
}
