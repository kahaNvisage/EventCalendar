<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="dashboard.ascx.cs" Inherits="EventCalendar.dashboard" %>
<style type="text/css">
    .style1
    {
        font-size: large;
        color: #FF0000;
    }
</style>

<asp:Panel ID="Panel1" runat="server" BorderColor="Black" BorderStyle="None" 
    Height="45px" HorizontalAlign="Center">
    <h1>
        <asp:Label ID="Label1" runat="server" 
            Text="Welcome to your personal EventCalendar"></asp:Label>
    </h1>
</asp:Panel>
<hr />
<div>
    The Package tries to help you to manage and maintain one or more calendars with 
    specific events on it.<br />
    <br />
    Features:<br />
&nbsp;1. Multiple Calendar<br />
&nbsp;2. Multiple Events on each claendar<br />
&nbsp;3. Multiple Locations where you&#39;re events will take place - reusable<br />
&nbsp;3. Events can span multiple days<br />
&nbsp;4. You can use Google Calendar instead of adding the events directly to the 
    calendar<br />
    <br />
    <span class="style1"><strong>Attention:</strong></span> This package is fully 
    working, but it&#39;s used as an proof of concept!<br />
    It uses MVC Views in the backend to display the data and also mvc controllers - 
    no usercontrols.<br />
    Because of that there are some minor bugs: if you change the name of the 
    calendar, the tree will not auto refresh.<br />
    <br />
    Future Features:<br />
    Enable also participants for each event.<br />
    Make a search function to search for an event.<br />
    <br />
    Bug reports, feature request and everything else&nbsp; welcome in the plugin 
    support forum on our.umbraco.org</div>

