<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // 應用程式啟動時執行的程式碼
        Application["EDPNET"] = "Source";
        Application["CodePage"] = "CP950";

        Application["OnlineCount"] = "0";
        Application["OnlineUser"] = new Hashtable();
        Application["CodeLang"] = "TC";

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  應用程式關閉時執行的程式碼

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // 發生未處理錯誤時執行的程式碼

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // 啟動新工作階段時執行的程式碼
        Session["kind"] = null;
    }

    void Session_End(object sender, EventArgs e) 
    {
        // 工作階段結束時執行的程式碼。 
        // 注意: 只有在 Web.config 檔將 sessionstate 模式設定為 InProc 時，
        // 才會引發 Session_End 事件。如果將工作階段模式設定為 StateServer 
        // 或 SQLServer，就不會引發這個事件。
        if (Session["UserName"] != null)
        {
            try
            {
                ((Hashtable)Application["OnlineUser"]).Remove(Session["UserName"] + Session.SessionID.ToString());
            }
            catch { }
        }
        Session["kind"] = null;
    }
       
</script>
