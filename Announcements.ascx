<%@ Control Language="vb" Inherits="DotNetNuke.Modules.Announcements.Announcements"
    Codebehind="Announcements.ascx.vb" AutoEventWireup="false" Explicit="True" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<asp:panel id="canadeanNews" runat="server" Visible="true">
<table width="100%">
    <tr width="100%" style="width:100%;margin-bottom:10px;">
        <td align=left class="newsMenuOff">
            <asp:Hyperlink CssClass="newsMenuOff" ID="hlAll" runat="server" Text="All"></asp:Hyperlink>|&nbsp;
            <asp:Hyperlink CssClass="newsMenuOff" ID="hlGeneral" runat="server" Text="General"></asp:Hyperlink>|&nbsp;
            <asp:Hyperlink CssClass="newsMenuOff" ID="hlSoftDrinks" runat="server" Text="Soft Drinks"></asp:Hyperlink>|&nbsp;
            <asp:Hyperlink CssClass="newsMenuOff" ID="hlBeer" runat="server" Text="Beer"></asp:Hyperlink>|&nbsp;
            <asp:Hyperlink CssClass="newsMenuOff" ID="hlBeveragePackaging" runat="server" Text="Beverage Packaging"></asp:Hyperlink>|&nbsp;
            <asp:Hyperlink CssClass="newsMenuOff" ID="hlDairyDrinks" runat="server" Text="Dairy Drinks"></asp:Hyperlink>
        </td>
    </tr>
    <tr><td><hr /></td></tr>
    <tr width="100%" style="width:100%">
        <td align=left id="trFilterMonth" runat="server">
            <asp:Label CssClass="newsGreenTitle" ID="lblMonthYear" runat="server" Text="Filter by Month/Year"></asp:Label>
            <asp:DropDownList ID="ddMonthYear" runat="server" AutoPostBack="True"></asp:DropDownList>
        </td>
    </tr>
</table>
</asp:panel>

<asp:Panel ID="canadeanEvents" runat="server" Visible="false">
</asp:Panel>

<asp:Panel ID="canadeanPressReleases" runat="server" Visible="false">
<table>
<tr width="556px" nowrap>
    <td align=left nowrap>
        <asp:Label CssClass="newsBlueTitle" ID="lblCanadeanPressReleases" runat="server" Text="Press Releases"></asp:Label>
    </td>
    <td width="100%"></td>
    <td align="right" nowrap>
        <asp:Label CssClass="newsGreenTitle" ID="lblSelection" runat="server" Text="Categories"></asp:Label>
        <asp:DropDownList ID="ddlPressReleases" runat="server"  AutoPostBack="True">
            <asp:ListItem Text="Select a Press Release Category" Value="0"></asp:ListItem> 
            <asp:ListItem Text="General" Value="1"></asp:ListItem>
            <asp:ListItem Text="Soft Drinks" Value="2"></asp:ListItem>
            <asp:ListItem Text="Beer" Value="3"></asp:ListItem>
            <asp:ListItem Text="Beverage Packaging" Value="4"></asp:ListItem>
            <asp:ListItem Text="Dairy Drinks" Value="5"></asp:ListItem>
        </asp:DropDownList>
    </td>
</tr>
</table>
</asp:Panel>
<div class="DNN_ANN_viewtypeSelector" id="viewtypeSelector" runat="server" visible=false>
    <div class="DNN_ANN_viewtypeSelectorLabel"><dnn:Label ID="plSelectView" runat="server" ControlName="ddlViewType" Suffix=":" CssClass="SubHead" visible=false></dnn:Label></div>
    <div class="DNN_ANN_viewtypeSelectorDDL"><asp:DropDownList ID="ddlViewType" CssClass="NormalTextBox" runat="server" AutoPostBack="True" visible=false></asp:DropDownList></div>
    <br />
</div>
<asp:Literal ID="litAnnouncements" runat="server"></asp:Literal>

<br /><!-- -->

<table cellspacing="0" cellpadding="0" width="100%" border="0">
  <tr id="trRelatedProd" runat="server">
    <td width="33%" valign="top">
        <asp:PlaceHolder ID="phDE" runat="server" Visible="false">
            <table cellpadding="2" cellspacing="0" border="0">
                <tr>
                    <td colspan="2" class="SubHeadNews">Related Data <hr /></td>
                </tr>
                <tr style="height:80px">
                    <td><img src="/Portals/0/images/prods_dataextracts.gif" border="0" alt="Data Extracts" title="Data Extracts" /></td>
                    <td>
                        <table cellpadding="2" cellspacing="0" border="0">
                            <tr style="height:50px">
                                <td valign="top">
                                    <asp:HyperLink id="hlDataExtract" runat="server" NavigateUrl="/tabid/97/Shop/Buy_Data_Extracts.aspx" CssClass="NormalBold"></asp:HyperLink>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HyperLink ID="hlDEBuyNow" runat="server" ImageUrl="/Portals/0/images/buy_now_just_50.gif" NavigateUrl="/tabid/97/Shop/Buy_Data_Extracts.aspx" CssClass="NormalBold" />
                                </td>
                            </tr>
                         </table>
                    </td>
                </tr>
                <tr><td colspan="2"><hr /></td></tr>
            </table>
        </asp:PlaceHolder>
    </td>
    <td>&nbsp;</td>
    <td width="33%" valign="top">
        <asp:PlaceHolder ID="phReport" runat="server" Visible="false">
            <table cellpadding="2" cellspacing="0" border="0">
                <tr>
                    <td colspan="2" class="SubHeadNews">Related Report <hr /></td>
                </tr>
                <tr style="height:80px">
                    <td><img src="/Portals/0/reports-images/img-reports-2.gif" border="0" width="50" alt="Wisdom" title="Wisdom" /></td>
                    <td>
                        <table cellpadding="2" cellspacing="0" border="0">
                            <tr style="height:50px">
                                <td valign="top">
                        			<asp:HyperLink id="hlReport" runat="server" CssClass="NormalBold"></asp:HyperLink>
                        			<asp:Literal id="litReport" runat="server"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HyperLink ID="hlBuyNow" runat="server" ImageUrl="/Portals/0/images/find_out_more.gif" CssClass="NormalBold" />
                                </td>
                            </tr>
                         </table>
                    </td>
                </tr>
                <tr><td colspan="2"><hr /></td></tr>
            </table>
        </asp:PlaceHolder>
    </td>
    <td>&nbsp;</td>
    <td width="33%" valign="top">
        <asp:PlaceHolder ID="phWisdom" runat="server" Visible="false">
            <table cellpadding="2" cellspacing="0" border="0">
                <tr>
                    <td colspan="2" class="SubHeadNews">Database Subscription<hr /></td>
                </tr>
                <tr style="height:80px">
                    <td><img src="/Portals/0/images/products-wisdom-image.gif" border="0" width="50" alt="Wisdom" title="Wisdom" /></td>
                    <td>
                        <table cellpadding="2" cellspacing="0" border="0">
                            <tr style="height:50px">
                                <td valign="top" class="Normal">
                        					<b>Web-based access</b> <br />
                        					Easy use and continuous updating
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <a href="/Products_Services/Wisdom_Database.aspx"><img src="/Portals/0/images/subscribe_now.gif" border="0" title="Subscribe now" /></a>
                                </td>
                            </tr>
                         </table>
                    </td>
                </tr>
                <tr><td colspan="2"><hr /></td></tr>
            </table>
        </asp:PlaceHolder>
    </td>
   </tr>
  <tr><td colspan="5">
    <asp:GridView ID="gvResults" Visible="false" AllowPaging="False" PageSize="5" AllowSorting="False" AutoGenerateColumns="False" runat="server" 
            Width="100%" HeaderStyle-CssClass="rowTableReports" RowStyle-CssClass="rowTableReports" CellPadding="5" GridLines="None" >
        <Columns>
            <asp:TemplateField HeaderText="Other Related Reports" HeaderStyle-CssClass="SubHeadNews">
             <ItemTemplate>
                <asp:HyperLink ID="hlTitle" Runat="Server" NavigateUrl='<%# FixHyperLink(Eval("ProductID").ToString())  %>' Text='<%# Eval("ModelName").ToString()  %>' CssClass="NormalBold" />
                <br />
                <asp:Label ID="lblDescriptionTag" runat="server" Text='<%# Eval("DescriptionTag").ToString()  %>' ></asp:Label>
              </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>    
  </td></tr>
  <tr><td colspan="5">&nbsp;</td></tr>
  <tr><td colspan="5">
    <table id="tableSendToFriend" visible="false" runat="server" cellspacing="3" cellpadding="0" width="100%" border="0">
        <tr><td colspan="5" class="SubHeadNews">Send this article to a friend</td></tr>
        <tr>
            <td width="12%" class="Normal" nowrap><b>Your Name</b></td>
            <td width="35%"><asp:TextBox ID="tbYourName" CssClass="Normal" runat="server" Width="190"></asp:TextBox></td>
            <td width="6%">&nbsp;</td>
            <td width="12%" class="Normal" nowrap><b>Their Name</b></td>
            <td width="35%"><asp:TextBox ID="tbTheirName" CssClass="Normal" runat="server" Width="190"></asp:TextBox></td>
        </tr>
        <tr>
            <td width="12%" class="Normal" nowrap><b>Your Email</b></td>
            <td width="35%"><asp:TextBox ID="tbYourEmail" CssClass="Normal" runat="server" Width="190"></asp:TextBox></td>
            <td width="6%">&nbsp;</td>
            <td width="12%" class="Normal" nowrap><b>Their Email</b></td>
            <td width="35%"><asp:TextBox ID="tbTheirEmail" CssClass="Normal" runat="server" Width="190"></asp:TextBox></td>
        </tr>
        <tr><td colspan="5"><asp:Label id="lblResult" runat="server" CssClass="Normal"></asp:Label></td></tr>
        <tr><td colspan="5" align="right"><asp:ImageButton id="imgBtnSend" runat="server" OnClick="imgBtnSend_Click" ImageUrl="/Portals/0/titles_text/send.jpg" CausesValidation="true"  /></td></tr>
    </table>
  </td></tr>
</table>

<p style="text-align:center" class="paging">
    <asp:Literal ID="paging" runat="server"></asp:Literal> 
</p>
<asp:Literal ID="litDebug" runat="server"></asp:Literal> 
<br />
