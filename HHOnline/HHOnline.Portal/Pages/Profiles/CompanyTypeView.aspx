﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/AdminMasterPage.master" AutoEventWireup="true" CodeFile="CompanyTypeView.aspx.cs" Inherits="Pages_Profiles_CompanyTypeView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphOpts" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">

<hc:MsgBox ID="mbNC" runat="server" SkinID="msgBox"></hc:MsgBox>
<asp:Panel ID="pnlManager" runat="server">
<table cellpadding="10" cellspacing="10" class="postform">
    <tr>
        <th style="width:100px;">当前类型</th>
        <td><asp:Literal ID="ltComType" runat="server"></asp:Literal></td>
    </tr>
    <tr>
        <th>申请状态</th>
        <td>
            <asp:Literal ID="ltPendingCom" runat="server"></asp:Literal>            
        </td>
    </tr>
    <tr>
        <th>&nbsp;</th>
        <td>
            <asp:Button ID="btnAgent" runat="server" Text="申请代理商" />
            <asp:Button ID="btnProvider" runat="server" Text="申请供应商" />
        </td>
    </tr>
</table>
</asp:Panel>
</asp:Content>
