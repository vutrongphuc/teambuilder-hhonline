﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using HHOnline.Shops;


namespace HHOnline.Controls
{
    public class FocusTypeList : DropDownList
    {
        public FocusTypeList()
        {
            this.Items.AddRange(new ListItem[] { 
                new ListItem("新品上架",((int)FocusType.New).ToString()),
                new ListItem("热卖产品",((int)FocusType.Hot).ToString()),
                new ListItem("推荐产品",((int)FocusType.Recommend).ToString()),
                new ListItem("促销产品",((int)FocusType.Promotion).ToString())
            });
        }

        /// <summary>
        /// 选择值
        /// </summary>
        public new FocusType SelectedValue
        {
            get
            {
                return (FocusType)(Convert.ToInt32(base.SelectedValue));
            }
            set
            {
                base.SelectedValue = ((int)value).ToString();
            }
        }
    }
}