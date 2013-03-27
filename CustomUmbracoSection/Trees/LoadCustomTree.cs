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
            rootNode.NodeID = "init";
        }

        protected override void CreateRootNodeActions(ref List<IAction> actions)
        {
            actions.Clear();
            actions.Add(ActionNew.Instance);
            actions.Add(ActionRefresh.Instance);
        }

        protected override void CreateAllowedActions(ref List<IAction> actions)
        {
            actions.Clear();
            //actions.Add(ActionRefresh.Instance);
            actions.Add(ActionDelete.Instance);
            
        }

        public override void Render(ref XmlTree tree)
        {
            if (null != this._db)
            {
               var calendars = this._db.Query<ECalendar>("SELECT * FROM ec_calendars");

                foreach (var c in calendars)
                {
                    XmlTreeNode node = XmlTreeNode.Create(this);

                    node.NodeID = c.Id.ToString();
                    node.NodeType = TreeAlias;
                    node.Text = c.Calendarname;
                    node.Icon = "folder.gif";
                    node.Action = "javascript:openCalendar(" + c.Id.ToString() + ")";
                    
                    tree.Add(node);
                }
            }

            //XmlTreeNode xNode = XmlTreeNode.Create(this);
            //xNode.NodeID = "1";
            //xNode.Text = "Custom node";
            //xNode.Action = "javascript:openCustom('" + "1" + "');";
            //xNode.Icon = "doc.gif";
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
                ");
        }
    }
}
