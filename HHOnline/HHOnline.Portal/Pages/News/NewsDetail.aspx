﻿<%@ Page Language="C#" MasterPageFile="~/Masters/HomeMasterPage.master" AutoEventWireup="true"
	CodeFile="NewsDetail.aspx.cs" Inherits="News_NewsDetail" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHeader" runat="Server">
<script type="text/javascript">
    var activeTab = 'news';
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderSearch" runat="Server">
 <huc:ArticleSearch ID="asNews" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderContent" runat="Server">
	<div id="content">
		<div id="newsBox">
			<div id="news">
				<h3 id="news_title">
					<asp:Label ID="lblTitle" runat="server" Text=""></asp:Label></h3>
				<h4 id="news_subtitle">
					<asp:Label ID="lblSubTitle" runat="server" Text=""></asp:Label></h4>
				<p id="news_font">
				</p>
				<p id="news_author">
					<span></span>
				</p>
				<div id="news_content">
					<div class="articleimg">
						<asp:Image ID="imgAttachment" Width="100px" Height="100px" runat="server" />
					</div>
					<div class="post_item_foot">
						发布于:
						<asp:Label ID="lblDate" runat="server" Text=""></asp:Label>
						作者: <a class="lightblue" href='#'>
							<asp:Label ID="lblAuthor" runat="server" Text=""></asp:Label></a> <span class="article_view">
								阅读次数: (<span id="viewcount_1560804"><asp:Label ID="lblHitTimes" runat="server" Text="Label"></asp:Label></span>)</span>
						关键字:
						<asp:Label ID="lblKeywords" runat="server" Text="Label"></asp:Label> 字体: <a href="javascript:fontZoomB();">大</a> <a href="javascript:fontZoomA();">小</a> 所属分类: 
						<asp:LinkButton ID="btnCategory" runat="server"></asp:LinkButton>
						<a href="javascript:{}" id="anchorAddNews" class="addnews">+ 收藏</a>
					</div>
					<div class="post_item_sep_detail">
					</div>
					<div id="news_content_detail">
					<p class="post_item_summary">
						<div ID="lblAbstract" runat="server"></div></p>
					<p>
						<div ID="lblContent" runat="server"></div></p>
					<p id="copy_form">
						<asp:Label ID="lblCopyForm" runat="server" Text=""></asp:Label></p></div>
				</div>
			</div>
		</div>
	</div>
</asp:Content>
