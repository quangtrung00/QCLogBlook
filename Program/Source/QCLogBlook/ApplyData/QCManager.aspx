<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QCManager.aspx.cs" Inherits="QCLogBlook_ApplyData_QCManager" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Pub/Css/cssHome.css" rel="stylesheet" type="text/css" />
    <script src="../../Pub/Js/jquery.js" type="text/javascript"></script>
    <link href="../../Pub/Css/ControlStyles.css" rel="styleshe  et" type="text/css" />
</head>
<body style="margin:0px;">
    <form id="frmQCmanager" runat="server">
    <div id="Master"> 
        <div class="cssQCTitle" style="width:100%;height:50px;float:left;font-weight:bold;text-align:center;line-height:2;font-size:21px;">
            <asp:Label ID="lblTitle" runat="server" Text=""></asp:Label>
        </div>
        <div id="TopMaster">
            <table border="0" style="width:100%;border:0px solid #A4B8C3;height:50px;border-collapse:collapse">
                <tr>
                    <td style="width:15%;text-align:center;border-right:1px solid #A4B8C3;border-left:1px solid #A4B8C3;border-top:1px solid #A4B8C3"><asp:Label ID="lblFactNo" Text="X??ng" runat="server"></asp:Label> : <asp:Label ID="lblFactNo1" runat="server"></asp:Label></td>
                    <td style="width:13%;text-align:center;border-right:1px solid #A4B8C3;border-top:1px solid #A4B8C3"><asp:Label ID="lblDeptNo" Text="B? ph?n" runat="server"></asp:Label> : 
                        <asp:DropDownList ID="ddlDepNo" runat="server" AutoPostBack="true"
                            onselectedindexchanged="ddlDepNo_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                    <td style="width:17%;text-align:center;border-right:1px solid #A4B8C3;border-top:1px solid #A4B8C3"><asp:Label ID="lblSecNo" Text="T?" runat="server"></asp:Label> : 
                        <asp:DropDownList ID="ddlSec_no" runat="server" AutoPostBack="true"
                            onselectedindexchanged="ddlSec_no_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                    <td style="width:15%;text-align:center;border-right:1px solid #A4B8C3;border-top:1px solid #A4B8C3"><asp:Label ID="lblDateTimeNow" Text="Ngay thang" runat="server"></asp:Label> : 
                        <asp:Label ID="lblDateTimeNow1" runat="server"></asp:Label>
                    </td>
                    <td style="width:20%;text-align:center;border-right:1px solid #A4B8C3;border-top:1px solid #A4B8C3" id="tdQC" runat="server">
                        <asp:Label ID="lblQC" Text="QC c? ??nh" runat="server"></asp:Label> :
                        <asp:DropDownList ID="ddlQC" runat="server" AutoPostBack="true"
                            onselectedindexchanged="ddlQC_SelectedIndexChanged"></asp:DropDownList> 
                    </td> 
                    <td style="width:20%;text-align:center;border-top:1px solid #A4B8C3;border-right:1px solid #A4B8C3" id="tdNV" runat="server">
                        <asp:Label ID="lblNVKT" Text="NV ki?m tra" runat="server"></asp:Label> : 
                        <asp:Label ID="lbllogin_user" runat="server"></asp:Label>
                    </td>                   
                </tr>
            </table>
        </div>        
        <div id="MiddleMaster">
             <table border="1" style="width:100%;border:0px solid #A4B8C3;height:80px;border-collapse:collapse">
                <tr>
                    <td  style="width:15%;text-align:center">訂單編號<br />Mã đơn đặt</td>
                    <td style="width:85%;text-align:center">
                        <table border="0" style="width:100%;border:0px solid #A4B8C3;border-collapse:collapse">
                            <tr>
                                <td id="idIcon" style="width:20%;text-align:center;display:none; white-space:nowrap">
                                    <%--<img style="cursor:pointer;" onclick="AddSLOK();" width="60px" src="../../Images/Icon/iconAdd.png" />&nbsp;&nbsp;<img style="cursor:pointer;" onclick="AddSLErr();"  width="60px" src="../../Images/Icon/IconRemove.png" />--%>
                                    <asp:ImageButton ID="btnAddSLOK" ImageUrl="../../Images/Icon/iconAdd.png"  OnClientClick="return AddSLOK();" Width="50px"
                                        runat="server" onclick="btnAddSLOK_Click" />&nbsp;&nbsp;&nbsp;&nbsp;<asp:ImageButton 
                                        ID="btnAddSLErr" Width="50px" ImageUrl="../../Images/Icon/IconRemove.png" 
                                        runat="server" onclick="btnAddSLErr_Click" />
                                </td>
                                <td style="width:20%;text-align:center; white-space:nowrap">
                                    <asp:Label ID="llbSLDat" Text="SL ??t" runat="server"></asp:Label> : <asp:Label ID="lblSLDat1" Text="0" runat="server"></asp:Label>&nbsp;                                    
                                    <asp:ImageButton ID="imgAddSLOKQC01" ImageAlign="AbsMiddle" Width="50px" Visible="false" 
                                        ImageUrl="../../Images/Icon/iconAdd.png" runat="server"  
                                        onclick="imgAddSLOKQC01_Click"/>&nbsp;&nbsp;
                                    <asp:ImageButton ImageAlign="AbsMiddle" ID="imgAddSLERRQC01" Width="50px" Visible="false" 
                                        ImageUrl="../../Images/Icon/IconRemove.png" runat="server" OnClientClick="return checkSLKhongDat();"
                                        onclick="imgAddSLERRQC01_Click" />
                                </td>
                                <td style="width:20%;text-align:center; white-space:nowrap">
                                    <asp:Label ID="lblSLKhongDat" Text="SL khong ??t" runat="server"></asp:Label> : <asp:Label ID="lblSLKhongDat1" Text="0" runat="server"></asp:Label>&nbsp;<asp:ImageButton 
                                        ID="imgAddSLOKQC02" runat="server" ImageAlign="AbsMiddle" Visible="false"
                                        ImageUrl="../../Images/Icon/iconAdd.png" Width="50px"  OnClientClick="return AddSLOKQC();" 
                                        onclick="imgAddSLOKQC02_Click" />
                                    &nbsp;&nbsp;<asp:ImageButton ID="imgAddSLERRQC02" runat="server" 
                                        ImageAlign="AbsMiddle" Visible="false"
                                        ImageUrl="../../Images/Icon/IconRemove.png" OnClientClick="return AddSLErrQC();" 
                                       Width="50px" onclick="imgAddSLERRQC02_Click" />
                                 </td>
                                <td style="width:20%;text-align:center; white-space:nowrap">
                                    <asp:Label ID="lblSumRutKiem" Text="T?ng s? rut ki?m" runat="server"></asp:Label> : <asp:Label ID="lblSumRutKiem1" Text="0" runat="server"></asp:Label>
                                </td>
                                <td style="width:20%;text-align:center; white-space:nowrap">
                                    <asp:Label ID="lblTyLeDat" Text="T? l? ??t" runat="server"></asp:Label> : <asp:Label ID="lblTyLeDat1" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>        
                </tr>
             </table>
        </div>   
        <div id="ContentMaster">
             <table border="0" style="width:100%;border:0px solid #A4B8C3;border-collapse:collapse">
                <tr>
                    <td class="cssQCVou_No" style="width:15%;text-align:left;padding-left:10px;padding-top:10px;border-left:1px solid #A4B8C3;border-bottom:1px solid #A4B8C3;border-right:1px solid #A4B8C3" valign="top">
                        <asp:Literal ID="ltrVouNo" runat="server"></asp:Literal>
                    </td>
                    <td style="width:80%;text-align:left;border-bottom:1px solid #A4B8C3;" valign="top" class="cssAlpha" id="tdAl">
                       <div style="width:100%;margin-top:0px;margin-bottom:0px">                             
                           <asp:Literal ID="ltrBadReason" runat="server"></asp:Literal>                                               
                         </div>
                    </td>
                    <td class="cssTitleReson"  style="width:5%;text-align:center;border-bottom:1px solid #A4B8C3;border-right:1px solid #A4B8C3">
                        不<br />良<br />原<br />因
                    </td>
                </tr>
             </table>
        </div>  
    </div>
    <input id="hOdrNo" type="hidden" name="hOdrNo" value="" runat="server" />
     <input id="hBadNo" type="hidden" name="hBadNo" value="" runat="server" />
     <input id="hBad_cn" type="hidden" name="hBad_cn" value="" runat="server" />
     <input id="hBad_vn" type="hidden" name="hBad_vn" value="" runat="server" />
     <input id="hQty" type="hidden" name="hQty" value="" runat="server" />
     <input id="hTypeCheck" type="hidden" name="hTypeCheck" value="" runat="server" />
    </form>
   <script type="text/javascript">
       GetTyLeDat();

       function GetTyLeDat() {           
           var sl = parseFloat($("#lblSLKhongDat1").html());
           var slok = parseFloat($("#lblSLDat1").html());
           var allSL = parseFloat($("#lblSumRutKiem1").html());
           if (slok != 0 && allSL != 0) {
               var Total = (parseFloat(slok) / parseFloat(allSL) * 100).toFixed(2) + "%";
               $("#lblTyLeDat1").html(Total);
               //$("#lblTyLeDat1").html(() * 100 + "%");
           }
           else
               $("#lblTyLeDat1").html(0 + "%");
       }
       function AddSLOK() {
           var sl = parseFloat($("#lblSLKhongDat1").html());
           var slok = parseFloat($("#lblSLDat1").html());
           var allSL = parseFloat($("#lblSumRutKiem1").html());
           if (sl == allSL) {
               alert("<%=sNumBer2%>");
               return false;
           }

       }
       function AddSLErr() {
           var sl = parseFloat($("#lblSLKhongDat1").html());
           var slok = parseFloat($("#lblSLDat1").html());
           var allSL = parseFloat($("#lblSumRutKiem1").html());
           if (slok == allSL) {
               alert('sooo');
               return false;
           }
           if ($("#hQty").val() == "") {
               alert("<%=sNumBer%>");
               return false;
           }

       }

       //QC 02
       function checkSLKhongDat() {
           var sl = parseFloat($("#lblSLDat1").html());
           if (sl == "0") {
               alert("<%=sNumBer%>");
               return false;
           }
       }

       function submitOdrnoQC01(odr_no) {
           $("#hOdrNo").val(odr_no);
           $("#hBadNo").val("");
           $("#frmQCmanager").submit();
       }
       setActive();
       function setActive() {

           if ($("#hOdrNo").val() != "") {
               $("#" + $("#hOdrNo").val() + "").addClass("cssAAAActive");
               $("#tdAl").removeClass("cssAlpha");
           }
           if ($("#hBadNo").val() != "") {
               $("#" + $("#hBadNo").val() + "").addClass("cssTDIconActive");
               //$("#idIcon").css("display", "");
               var Type = '<%= Request["TypeCheck"] %>'
               if (Type == "1")
                   $("#idIcon").css("display", "");
               else {
                   $("#idIcon").css("display", "none");
               }
           }
       }

       function submitBadReason(bad_no, bad_cn, bad_vn, qty) {

           if ($("#hOdrNo").val() == "") {
               alert("<%=sCode %>");
               return;
           }
           $("#hBadNo").val(bad_no);
           $("#hBad_cn").val(bad_cn);
           $("#hBad_vn").val(bad_vn);
           $("#hQty").val(qty);

           $(".cssTDIconActive").removeClass("cssTDIconActive");
           $("#" + $("#hBadNo").val() + "").addClass("cssTDIconActive");

           var Type = '<%= Request["TypeCheck"] %>'
           if (Type == "1")
               $("#idIcon").css("display", "");
           else {
               $("#idIcon").css("display", "none");
           }
       }

       function AddSLOKQC() {
           if ($("#hBadNo").val() == "") {
               alert("<%=sBadNo%>");
               return false;
           }
       }
       function AddSLErrQC() {
           if ($("#hBadNo").val() == "") {
               alert("<%=sBadNo%>");
               return false;
           }
           var sl = parseFloat($("#lblSLKhongDat1").html());
           if (sl == "0") {
               alert("<%=sNumBer%>");
               return false;
           }
       }
   </script>
</body>
</html>
