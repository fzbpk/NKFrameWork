<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/default.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="UIWeb" Namespace="UIWeb.Entity" TagPrefix="cc1" %>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="DefaultMain">
   <cc1:NetSetUI ID="NetSetUI1" runat='server'></cc1:NetSetUI>
        <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
    
</asp:Content>
