<%@ Page language="c#"  MasterPageFile="../../masterpages/umbracoPage.Master" validateRequest="false" Codebehind="editCustom.aspx.cs" AutoEventWireup="True" Inherits="EventCalendar.editCustom" %>
<%@ Register Namespace="umbraco.uicontrols" Assembly="controls" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="body" runat="server">
    <cc1:TabView runat="server" ID="tabControl" Width="552px" Height="692px"/>
        <cc1:Pane ID="SettingsPane" runat="server" Height="600px" Width="330px">
             <cc1:PropertyPanel runat="server" ID="ppcalendarName">
                <asp:TextBox runat="server" ID="CalendarName"></asp:TextBox>
             </cc1:PropertyPanel>
             <cc1:PropertyPanel runat="server" ID="ppisGCal" Text="Test">
                <asp:CheckBox runat="server" ID="IsGCal"></asp:CheckBox>
             </cc1:PropertyPanel>
             <cc1:PropertyPanel runat="server" ID="ppdisplayOnSite" Text="Test">
                <asp:CheckBox runat="server" ID="DisplayOnSite"></asp:CheckBox>
             </cc1:PropertyPanel>
        </cc1:Pane>
           
		<cc1:Pane ID="CreateEventPane" runat="server" Height="600px" Width="330px">
        </cc1:Pane>

        <cc1:Pane ID="ShowEventsPane" runat="server" Height="600px" Width="330px">
		    <cc1:PropertyPanel runat="server" ID="ppeventsTable">
                <asp:Table ID="EventsTable" runat="server">
                    <asp:TableHeaderRow ID="EventsTableHeader" runat="server">
                        <asp:TableHeaderCell>Event Name</asp:TableHeaderCell>
                        <asp:TableHeaderCell>Start Date</asp:TableHeaderCell>
                        <asp:TableHeaderCell>End Date</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </cc1:PropertyPanel>
        </cc1:Pane>

</asp:Content>