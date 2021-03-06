﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HHOnline.Framework.Web;
using HHOnline.Framework;

public partial class ControlPanel_Common_ContactInfo : HHPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) BindInfo();
    }
    void BindInfo()
    {
        FooterInfo fi = FooterInfos.FooterInfoGet();
        if (fi != null && !string.IsNullOrEmpty(fi.ContactInfo))
        {
            txtAbout.Text = fi.ContactInfo;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        FooterInfos.FooterInfoUpdate(FooterUpdateAction.ContactInfo, txtAbout.Text.Trim());
        BindInfo();
    }
    public override void OnPageLoaded()
    {
        this.ShortTitle = "联系我们";
        this.SetTitle();
        this.SetTabName(this.ShortTitle);
    }
}
