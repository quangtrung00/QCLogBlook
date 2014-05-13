using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PccCommonForC;
using PccBsSystemForC;
using System.Xml;

public partial class SysManager_FactGroupManage_FactGroupManage104 : System.Web.UI.Page
{
    private DataTable m_Table;
    private string m_CurrentNodeIndex;

    private const string MYURL = "FactGroupManage104.aspx";
    private const string FACTGROUPDETAIL = "FactGroupDetail104.aspx";

    private string m_ApID;

    protected void Page_Load(object sender, EventArgs e)
    {
        // 將使用者程式碼置於此以初始化網頁
        if (Session["UserID"] == null) return;

        m_ApID = Request.Params["ApID"];

        if (!IsPostBack)
        {
            GenTree(ref TreeView1);
            if ((Request.Params["SelectedNode"] != null) && (Request.Params["SelectedNode"].ToString() != ""))
            {
                string SelectedNode = Request.Params["SelectedNode"].ToString();
                string[] a;
                if (SelectedNode.IndexOf("/") < 0)
                {
                    string ParentIndex = GetNodeIndex(TreeView1.Nodes, SelectedNode);
                    if (m_CurrentNodeIndex != null)
                    {
                        a = m_CurrentNodeIndex.Split('/');
                        TreeView1.ExpandDepth = a.Length - 1;
                        //TreeView1.ExpandLevel = a.Length - 1;
                        TreeView1.FindNode(m_CurrentNodeIndex).Selected = true;
                        TreeView1.FindNode(m_CurrentNodeIndex).Expand();
                        
                        //TreeView1.SelectedValue = m_CurrentNodeIndex;
                        //TreeView1.SelectedNodeIndex = m_CurrentNodeIndex;
                    }
                }
                else
                {
                    a = SelectedNode.Split('/');
                    TreeView1.ExpandDepth = a.Length - 1;
                    //TreeView1.ExpandLevel = a.Length - 1;
                    TreeView1.FindNode(SelectedNode).Selected = true;
                    TreeView1.FindNode(SelectedNode).Expand();
                    //TreeView1.SelectedValue = SelectedNode;
                    //TreeView1.SelectedNodeIndex = SelectedNode;
                }
            }
            else
            {
                TreeView1.ExpandDepth = 2;
                //TreeView1.ExpandLevel = 2;
            }
        }
    }

    private string GetNodeIndex(TreeNodeCollection Nodes, string NodeID)
    {
        string strReturn = "";

        foreach (TreeNode node in Nodes)
        {
            if (node.Value == NodeID)
            {
                strReturn = node.ValuePath;
                //strReturn = node.GetNodeIndex();
                
                m_CurrentNodeIndex = strReturn;
                break;
            }

            if (node.ChildNodes.Count > 0)
            {
                strReturn = GetNodeIndex(node.ChildNodes, NodeID);
            }
        }

        return strReturn;
    }

    #region "建立基本的Tree架構"

    private void GenTree(ref TreeView tree)
    {
        //判斷是否為系統超級管理者
        string superAdmin = "N";
        if (ConfigurationManager.AppSettings["superAdminEmail"].ToLower().Equals(Session["UserAccount"].ToString().ToLower()))
        {
            superAdmin = "Y";
        }

        //先把Tree設為空白
        tree.Nodes.Clear();

        //先建立廠別樹的根(Root)寶成寶成國際集團
        TreeNode node = new TreeNode();
        node.ImageUrl = "../../Images/TreeRoot.gif";
        node.Value = "0";
        //node.ID = "0";
        if (superAdmin.Equals("Y"))
        {
            node.Text = "<B>寶成國際集團</B>";
            node.NavigateUrl = FACTGROUPDETAIL + "?ApID=" + m_ApID + "&SrcUp_Id=0";
            node.Target = "doc";
        }
        else
        {
            node.Text = "<Font color=gray>寶成國際集團</Font>";
        }
        tree.Nodes.Add(node);

        //取得這顆樹的所有資料
        bs_FactGroupManage mybs = new bs_FactGroupManage(ConfigurationManager.AppSettings["ConnectionType"], ConfigurationManager.AppSettings["ConnectionServer"], ConfigurationManager.AppSettings["ConnectionDB"], ConfigurationManager.AppSettings["ConnectionUser"], ConfigurationManager.AppSettings["ConnectionPwd"], Session["UserIDAndName"].ToString(), ConfigurationManager.AppSettings["EventLogPath"]);
        PccMsg myMsg = new PccMsg();
        myMsg.CreateFirstNode("Ap_Id", Request.Params["ApID"]);
        myMsg.CreateFirstNode("LoginUser_Id", Session["UserID"].ToString());
        myMsg.CreateFirstNode("SuperAdmin", superAdmin);

        DataSet ds = mybs.DoReturnDataSet("GetFgrpTree", myMsg.GetXmlStr, "");
        m_Table = ds.Tables["FgrpTree"];

        //取得這顆樹的第一層資料，條件為Up_ID = 0
        DataRow[] rows;
        rows = m_Table.Select("up_id = 0");

        //設定所要跑的迴圈變數 i
        int i;
        //設定這顆樹的根節點的集合，所有的節點由此往下長
        TreeNodeCollection RootNodes;
        RootNodes = tree.Nodes[0].ChildNodes;
        //RootNodes = tree.Nodes[0].Nodes;

        //跑第一層的迴圈
        for (i = 0; i < rows.Length; i++)
        {
            AddNode(ref RootNodes, int.Parse(rows[i]["Fgrp_Id"].ToString()), i);
        }
    }

    //建立日期： 2004/01/08
    //建立者  ： LemorYen
    //建立目的： 示範如何撰寫不規則樹的建立，Recurceive的實做方法
    //輸入資料： ParentNodes為所要加入節點的父節點，Tree_ID為這個節點的基本資料ID，Level表示現在是在這個層的第幾個節點
    //輸出資料： Boolean，若已結束，則為False，否則就為True
    private bool AddNode(ref TreeNodeCollection ParentNodes, int Branch_ID, int Level)
    {
        bool bReturn = true;
        DataRow[] rows;
        int i;
        TreeNode Node;

        //首先取得要加入的這一筆Node的資料，判斷如果沒有資料則Return False
        rows = m_Table.Select("Fgrp_Id = " + Branch_ID);

        if (rows.Length > 0)
        {
            Node = new TreeNode();
            Node.Value = rows[0]["Fgrp_Id"].ToString();
            //Node.ID = rows[0]["Fgrp_Id"].ToString();

            if (rows[0]["IsAuth"].ToString().Equals("Y"))
            {
                Node.Text = "<B>" + rows[0]["Fgrp_Nm"].ToString() + "</B>";

                Node.NavigateUrl = FACTGROUPDETAIL + "?ApID=" + m_ApID + "&SrcUp_Id=" + rows[0]["Fgrp_Id"].ToString();
                Node.Target = "doc";
            }
            else
            {
                Node.Text = "<Font color=gray>" + rows[0]["Fgrp_Nm"].ToString() + "</Font>";
            }
            ParentNodes.Add(Node);

        }

        //取得屬於這個Node的所有(Child)子節點
        rows = m_Table.Select("Up_ID = " + Branch_ID);

        //判斷如果有子節點，就進入迴圈做加入節點的工作，如果沒有子節點了，則跳出這個Function並Return False
        if (rows.Length > 0)
        {
            //設定這顆樹的根節點的集合，所有的節點由此往下長
            TreeNodeCollection ParentNodes1;

            for (i = 0; i < rows.Length; i++)
            {
                //利用Recurceive的方式，使每一個節點都能新增其子節點，其結束點即是若已沒有子節點了，表示自已本身已
                //是葉子了，所以這個Recurceive就可結束了。
                ParentNodes1 = ParentNodes[Level].ChildNodes;
                //ParentNodes1 = ParentNodes[Level].Nodes;
                bReturn = AddNode(ref ParentNodes1, int.Parse(rows[i]["Fgrp_Id"].ToString()), i);
            }
        }

        return bReturn;

    }

    #endregion


}
