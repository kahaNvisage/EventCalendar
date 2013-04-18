using System;
using System.Collections.Generic;
using System.Web;
using umbraco.cms.presentation.Trees;
using umbraco.BusinessLogic.Actions;
using umbraco.interfaces;
using System.Text;
using umbraco.businesslogic;
using Umbraco.Core.Persistence;
using Umbraco.Core;
using EventCalendar.Models;

namespace EventCalendar.Trees
{
    [Tree("eventCalendar", "eventCalendarTree", "Event Calendar")]
    public class LoadCustomTree : BaseTree
    {
        private UmbracoDatabase _db = null;

        public LoadCustomTree(string application) : base(application) {
            _db = ApplicationContext.Current.DatabaseContext.Database;
        }



        protected override void CreateRootNode(ref XmlTreeNode rootNode)
        {
            rootNode.Icon = FolderIcon;
            rootNode.OpenIcon = FolderIconOpen;
            rootNode.NodeType = "init" + TreeAlias;
            rootNode.NodeID = "-1";
        }

        protected override void CreateRootNodeActions(ref List<IAction> actions)
        {
            actions.Clear();
            //actions.Add(ActionNew.Instance);
            //actions.Add(ActionRefresh.Instance);
        }

        protected override void CreateAllowedActions(ref List<IAction> actions)
        {
            actions.Clear();
            //actions.Add(ActionRefresh.Instance);
            actions.Add(ActionDelete.Instance);
            
        }

        public override void Render(ref XmlTree tree)
        {
            int id = 0;

            if (this.NodeKey == string.Empty)
            {

                PopulateRootNodes(ref tree);

            }
            else
            {
                string keyType = this.NodeKey.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries)[0];
                if(this.NodeKey.Contains("-"))
                    id = Convert.ToInt32(this.NodeKey.Split(new string[] {"-"}, StringSplitOptions.RemoveEmptyEntries)[1]);

                switch (keyType)
                {
                    case "CalendarBase": this.RenderCalendar(ref tree); break;
                    case "LocationBase": RenderLocations(ref tree); break;
                    case "CalendarEntry": RenderCalendarEntryNodes(ref tree, id); break;
                }
            }
        }

        private void RenderCalendarEntryNodes(ref XmlTree tree, int id)
        {
            XmlTreeNode node = XmlTreeNode.Create(this);                       
            node.NodeType = "CalendarSettings";
            node.Text = "Settings";
            node.Icon = "developerMacro.gif";
            node.Action = "javascript:openCalendar(" + id.ToString() + ")";
            node.Menu.Clear();            
            tree.Add(node);

            node = XmlTreeNode.Create(this);
            node.NodeID = (1 + id).ToString();
            node.NodeType = "CalendarEvents";
            node.Text = "Events";
            node.Icon = "cog.png";
            node.Action = "javascript:showEvents(" + id.ToString() + ")";
            node.Menu.Clear();
            node.Menu.Add(ActionNew.Instance);
            tree.Add(node);

            node = XmlTreeNode.Create(this);
            node.NodeID = (2 + id).ToString();
            node.NodeType = "CalendarRecurringEvents";
            node.Text = "Recurring Events";
            node.Icon = "cog.png";
            node.Action = "javascript:showRecurringEvents(" + id.ToString() + ")";
            node.Menu.Clear();
            node.Menu.Add(ActionNew.Instance);
            tree.Add(node);
        }

        private void RenderLocations(ref XmlTree tree)
        {
            if (null != this._db)
            {
                var locations = this._db.Query<EventLocation>("SELECT * FROM ec_locations");

                foreach (var c in locations)
                {
                    XmlTreeNode node = XmlTreeNode.Create(this);

                    node.NodeID = c.Id.ToString();
                    node.NodeType = "LocationEntry";
                    node.Text = c.LocationName;
                    node.Icon = "map.png";
                    node.Action = "javascript:openLocation(" + c.Id.ToString() + ")";

                    tree.Add(node);
                }
            }
        }

        private void RenderCalendar(ref XmlTree tree)
        {
            if (null != this._db)
            {
                var calendars = this._db.Query<ECalendar>("SELECT * FROM ec_calendars");

                foreach (var c in calendars)
                {
                    XmlTreeNode node = XmlTreeNode.Create(this);

                    node.NodeID = c.Id.ToString();
                    node.NodeType = "CalendarEntry";
                    node.Text = c.Calendarname;
                    node.Icon = "calendar.png";
                    node.Action = "javascript:openCalendar(" + c.Id.ToString() + ")";

                    var treeService = new TreeService(c.Id, TreeAlias, ShowContextMenu, IsDialog, DialogMode, app, "CalendarEntry-"+c.Id.ToString());
                    node.Source = treeService.GetServiceUrl();

                    node.Menu.Clear();
                    //node.Menu.Add(ActionNew.Instance);
                    node.Menu.Add(ActionDelete.Instance);

                    tree.Add(node);
                }
            }
        }

        /// <summary>
        /// Render the top Root Nodes.
        /// - Calendar -> The Root of all Calendars
        /// - Locations -> The Root of all Locations
        /// </summary>
        /// <param name="tree">The current tree</param>
        private void PopulateRootNodes(ref XmlTree tree)
        {
            XmlTreeNode xNode = XmlTreeNode.Create(this);
            xNode.NodeID = "1";
            xNode.Text = "Calendar";
            //xNode.Action = "javascript:openCustom('" + "1" + "');";
            xNode.Icon = "calendar.png";
            xNode.OpenIcon = "folder_o.gif";
            xNode.NodeType = "EventCalendarBase";

            var treeService = new TreeService(-1, TreeAlias, ShowContextMenu, IsDialog, DialogMode, app, "CalendarBase");
            xNode.Source = treeService.GetServiceUrl();

            xNode.Menu.Clear();
            xNode.Menu.Add(ActionNew.Instance);
            xNode.Menu.Add(ActionRefresh.Instance);            

            tree.Add(xNode);

            xNode = XmlTreeNode.Create(this);
            xNode.NodeID = "2";
            xNode.Text = "Locations";
            //xNode.Action = "javascript:openCustom('" + "1" + "');";
            xNode.Icon = "map.png";
            xNode.OpenIcon = "folder_o.gif";
            xNode.NodeType = "EventLocationBase";

            treeService = new TreeService(-1, TreeAlias, ShowContextMenu, IsDialog, DialogMode, app, "LocationBase");
            xNode.Source = treeService.GetServiceUrl();

            xNode.Menu.Clear();            
            xNode.Menu.Add(ActionNew.Instance);
            xNode.Menu.Add(ActionRefresh.Instance);

            tree.Add(xNode);

            //xNode = XmlTreeNode.Create(this);
            //xNode.NodeID = "3";
            //xNode.Text = "Settings";
            //xNode.Action = "javascript:openSettings();";
            //xNode.Icon = "cog.png";
            //xNode.NodeType = "GeneralSettings";
            //xNode.Menu.Clear();

            //tree.Add(xNode);
        }

        public override void RenderJS(ref StringBuilder Javascript)
        {
            Javascript.Append(
               @"
                    function openCalendar(id) {
                        var url = '/EventCalendar/ECBackendSurface?id=' + id;
                        UmbClientMgr.contentFrame(url);
                    }
                    function openLocation(id) {
                        var url = '/EventCalendar/ECBackendSurface/EditLocation/?id=' + id;
                        UmbClientMgr.contentFrame(url);
                    }
                    function showEvents(id) {
                        var url = '/EventCalendar/ECBackendSurface/ShowEvents/?id=' + id;
                        UmbClientMgr.contentFrame(url);
                    }
                    function showRecurringEvents(id) {
                        var url = '/EventCalendar/ECBackendSurface/ShowRecurringEvents/?id=' + id;
                        UmbClientMgr.contentFrame(url);
                    }
                    function openSettings() {
                        var url = '/EventCalendar/ECBackendSurface/GeneralSettings/';
                        UmbClientMgr.contentFrame(url);
                    }
                ");
        }
    }
}
