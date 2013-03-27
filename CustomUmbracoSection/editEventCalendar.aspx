<%@ Page Title="" Language="C#" MasterPageFile="../../masterpages/umbracoPage.Master" AutoEventWireup="true" CodeBehind="editEventCalendar.aspx.cs" Inherits="EventCalendar.editEventCalendar" %>
<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="umb" %>

<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
<umb:TabView ID="TabView1" runat="server" Width="552px" Height="692px" />
 <umb:Pane ID="SettingsPane" runat="server" Height="600px" Width="330px">
 <umb:PropertyPanel runat="server" ID="ppcalendarName" Text="Calendar Name">
<asp:TextBox ID="CalendarName" runat="server"></asp:TextBox>
 </umb:PropertyPanel>
 <umb:PropertyPanel runat="server" ID="ppisGCal" Text="Test">
     <asp:CheckBox ID="IsGCal" runat="server" />
 </umb:PropertyPanel>
 <umb:PropertyPanel ID="ppDisplayOnSite" runat="server" Text="DisplayOnSite">
     <asp:CheckBox ID="DisplayOnSite" runat="server" />
 </umb:PropertyPanel>
 </umb:Pane>

</asp:Content>