﻿<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="TagDetail.aspx.cs" Inherits="Web.Admin.TagManagement.TagDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <% if (enableShowDetail)
        { %>
    <div class="row grid-responsive">
        <div class="column page-heading">
            <div class="large-card">
                <h1>Chi tiết thẻ tag: <% = tagInfo.name %></h1>
                <p>ID của thẻ tag: <% = tagInfo.ID %></p>
                <p>Tên của thẻ tag: <% = tagInfo.name %></p>
                <p>Mô tả của thẻ tag: <% = tagInfo.description %></p>
                <p>Ngày tạo của thẻ tag: <% = tagInfo.createAt %></p>
                <p>Ngày cập nhật của thẻ tag: <% = tagInfo.updateAt %></p>
                <asp:HyperLink ID="hyplnkList" CssClass="button button-blue" runat="server">Quay về trang danh sách</asp:HyperLink>
                <asp:HyperLink ID="hyplnkEdit" CssClass="button button-green" runat="server">Chỉnh sửa</asp:HyperLink>
                <asp:HyperLink ID="hyplnkDelete" CssClass="button button-red" runat="server">Xóa</asp:HyperLink>
            </div>
        </div>
    </div>
    <%} %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
