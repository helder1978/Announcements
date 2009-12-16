'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by DotNetNuke Corporation
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE.
'

Imports System.IO

Imports DotNetNuke
Imports DotNetNuke.Entities.Modules
Imports System.Collections.Generic

Imports System.Xml
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Entities.Tabs

Imports DotNetNuke.Entities.Modules.Definitions
Imports DotNetNuke.UI.Utilities

Namespace DotNetNuke.Modules.Announcements

  ''' -----------------------------------------------------------------------------
  ''' <summary>
	''' The EditAnnouncements PortalModuleBase is used to manage Announcements
	''' </summary>
    ''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[cnurse]	9/20/2004	Moved Announcements to a separate Project
	''' 	[cnurse]	9/20/2004	Updated to reflect design changes for Help, 508 support
	'''                       and localisation
    ''' 	[erikvb]	11/20/2007	updated for version 4.0.0 of the module
    ''' </history>
	''' -----------------------------------------------------------------------------
	Partial  Class EditAnnouncements

		Inherits Entities.Modules.PortalModuleBase

#Region "Controls"

        ' controls
        Protected WithEvents plTitle As UI.UserControls.LabelControl
        Protected WithEvents plImage As UI.UserControls.LabelControl
        Protected WithEvents urlImage As UI.UserControls.UrlControl
        Protected WithEvents plDescription As UI.UserControls.LabelControl
        Protected WithEvents teDescription As UI.UserControls.TextEditor
        Protected WithEvents plURL As UI.UserControls.LabelControl
        Protected WithEvents ctlURL As UI.UserControls.UrlControl
        Protected WithEvents plViewOrder As UI.UserControls.LabelControl
        Protected WithEvents plPublishDate As UI.UserControls.LabelControl
        Protected WithEvents plExpireDate As UI.UserControls.LabelControl

        ' actions

        ' footer
        Protected WithEvents ctlAudit As UI.UserControls.ModuleAuditControl
        Protected WithEvents ctlTracking As UI.UserControls.URLTrackingControl

#End Region

#Region "Private Members"

        Private itemId As Integer

        Private filesAllowed As String = "jpg,jpeg,jpe,gif,bmp,png,swf,pdf,gif,xls,doc"

#End Region

#Region "Event Handlers"
        'Private Sub createPageTemplate2()
        '    Dim ctrTab As New DotNetNuke.Entities.Tabs.TabController
        '    Dim objTab As New DotNetNuke.Entities.Tabs.TabInfo
        '    Dim objActiveTab As New DotNetNuke.Entities.Tabs.TabInfo

        '    objActiveTab = Me.PortalSettings.ActiveTab
        '    objTab.IsVisible = False

        '    objTab.PortalID = Me.PortalId
        '    objTab.TabID = Null.NullInteger

        '    objTab.TabName = "titulo"
        '    objTab.Title = "titulo"
        '    objTab.Description = ""
        '    objTab.KeyWords = ""
        '    objTab.DisableLink = False

        '    objTab.ParentId = -1
        '    objTab.IconFile = ""
        '    objTab.IsDeleted = False
        '    objTab.Url = "N"
        '    objTab.TabPermissions = objActiveTab.TabPermissions

        '    objTab.SkinSrc = objActiveTab.SkinSrc
        '    objTab.ContainerSrc = objActiveTab.ContainerSrc
        '    objTab.TabPath = GenerateTabPath(objTab.ParentId, objTab.TabName)
        '    objTab.StartDate = Null.NullDate
        '    objTab.EndDate = Null.NullDate

        '    objTab.PageHeadText = ""
        '    objTab.TabID = ctrTab.AddTab(objTab)

        'End Sub


        Private Function CreatePageTemplate(ByVal title As String, ByVal Description As String) As Integer

            Dim ctrTab As New DotNetNuke.Entities.Tabs.TabController
            Dim objTab As New DotNetNuke.Entities.Tabs.TabInfo

            Dim objActiveTab As New DotNetNuke.Entities.Tabs.TabInfo

            objActiveTab = Me.PortalSettings.ActiveTab
            objTab.IsVisible = True

            objTab.PortalID = Me.PortalId
            objTab.TabID = Null.NullInteger

            objTab.TabName = title
            objTab.Title = title
            objTab.Description = Description
            objTab.KeyWords = ""
            objTab.DisableLink = False

            objTab.ParentId = 193
            objTab.IconFile = ""
            objTab.IsDeleted = False
            title = title.Replace(" ", "")
            objTab.Url = title
            objTab.TabPermissions = objActiveTab.TabPermissions

            'objTab.SkinSrc = objActiveTab.SkinSrc
            objTab.SkinSrc = "/Portals/_default/Skins/spwidthmaincontainer/secondPages-container.ascx"
            objTab.ContainerSrc = objActiveTab.ContainerSrc
            objTab.TabPath = GenerateTabPath(objTab.ParentId, objTab.TabName)

            'TODO: USE THE DATES FROM THE ANNOUNCEMENT HERE!
            objTab.StartDate = Null.NullDate
            objTab.EndDate = Null.NullDate

            objTab.TabID = ctrTab.AddTab(objTab)

            Dim xmlDoc As New System.Xml.XmlDocument
            xmlDoc.Load(Server.MapPath(Me.Page.TemplateSourceDirectory & "/Portals/_default/templatenews2.page.template"))

            ' Dim xmlDoc As New XmlDocument
            'xmlDoc.Load(cboTemplate.SelectedItem.Value)
            Dim hs As New Hashtable
            hs.Add(657, 657)
            hs.Add(413, 413)
            hs.Add(437, 437)
            hs.Add(408, 408)
            hs.Add(440, 440)
            hs.Add(616, 616)
            hs.Add(382, 382)
            hs.Add(441, 441)
            hs.Add(662, 662)

            Dim objPortals As New Entities.Portals.PortalController
            objPortals.ParsePanes(xmlDoc.SelectSingleNode("//portal/tabs/tab/panes"), objTab.PortalID, objTab.TabID, DotNetNuke.Entities.Portals.PortalTemplateModuleAction.Merge, hs)

            Return objTab.TabID

        End Function



        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_Load runs when the control is loaded
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                ' get itemid

                'Response.Write(ModuleId)
                If Not (Request.QueryString("ItemId") Is Nothing) Then
                    itemId = Int32.Parse(Request.QueryString("ItemId"))
                Else
                    itemId = Convert.ToInt32(Common.Utilities.Null.NullInteger)
                End If
                cmdCalendar.NavigateUrl = Calendar.InvokePopupCal(txtPublishDate)
                cmdCalendar2.NavigateUrl = Calendar.InvokePopupCal(txtExpireDate)

                If Page.IsPostBack = False Then
                    urlImage.FileFilter = filesAllowed
                    'urlImage.ShowNewWindow = False

                    urlImage.ShowNone = True

                    ctlURL.Visible = False

                    ctlURL.ShowLog = True
                    ctlURL.ShowNewWindow = True
                    ctlURL.ShowTrack = True
                    ctlURL.ShowUsers = True

                    urlImage.ShowUpLoad = True
                    urlImage.ShowFiles = True



                    ' delete confirmation
                    DotNetNuke.UI.Utilities.ClientAPI.AddButtonConfirm(cmdDelete, Localization.GetString("DeleteItem"))

                    If Not Common.Utilities.Null.IsNull(itemId) Then

                        ' get object
                        Dim objAnnouncements As New AnnouncementsController
                        Dim objAnnouncement As AnnouncementInfo = objAnnouncements.GetAnnouncement(itemId, ModuleId)

                        If Not objAnnouncement Is Nothing Then
                            'If (ModuleId <> 660) Then
                            ' populate controls
                            txtTitle.Text = objAnnouncement.Title.ToString
                            urlImage.Url = objAnnouncement.ImageSource
                            teDescription.Text = objAnnouncement.Description
                            ctlURL.Url = objAnnouncement.Url

                            If Not Common.Utilities.Null.IsNull(objAnnouncement.ViewOrder) Then
                                txtViewOrder.Text = Convert.ToString(objAnnouncement.ViewOrder)
                            End If
                            If Not Null.IsNull(objAnnouncement.PublishDate) Then
                                txtPublishDate.Text = objAnnouncement.PublishDate.ToShortDateString()
                            End If
                            If Not Null.IsNull(objAnnouncement.ExpireDate) Then
                                txtExpireDate.Text = objAnnouncement.ExpireDate.ToShortDateString()
                            End If

                            ctlAudit.CreatedDate = objAnnouncement.CreatedDate.ToString
                            ctlAudit.CreatedByUser = objAnnouncement.CreatedByUser.ToString

                            ctlTracking.URL = objAnnouncement.Url
                            ctlTracking.ModuleID = ModuleId
                            'Else
                            'Dim newTitle As String = ""
                            'newTitle = objAnnouncement.Title.Replace(" ", "")

                            'Response.Redirect(String.Concat("/news/PressReleases/pressreleasespages/", newTitle, "/tabid", objAnnouncement.Url, "/Default.aspx"))
                        'End If
                    Else ' security violation attempt to access item not related to this Module
                        Response.Redirect(NavigateURL(), True)
                    End If
                Else ' new item
                    txtPublishDate.Text = DateTime.Today.ToShortDateString
                    cmdDelete.Visible = False
                    ctlAudit.Visible = False
                    ctlTracking.Visible = False
                End If

                End If
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdCancel_Click runs when the cancel button is clicked
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
            Try
                Response.Redirect(NavigateURL(), True)
            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdDelete_Click runs when the delete button is clicked
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdDelete.Click
            Try
                If Not Common.Utilities.Null.IsNull(itemId) Then
                    ' delete item
                    Dim objAnnouncements As New AnnouncementsController
                    objAnnouncements.DeleteAnnouncement(ModuleId, itemId)
                    ' refresh cache
                    ModuleController.SynchronizeModule(ModuleId)
                End If

                ' redirect back to page
                Response.Redirect(NavigateURL(), True)
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdUpdate_Click runs when the update button is clicked
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdUpdate.Click
            Try
                ' verify data
                Response.Write("112")

                If Page.IsValid = True Then
                    Response.Write("113")
                    Dim newPageUrl As Integer = 0
                    Dim objAnnouncement As New AnnouncementInfo

                    objAnnouncement = CType(CBO.InitializeObject(objAnnouncement, GetType(AnnouncementInfo)), AnnouncementInfo)

                    'only creates a new page if this isn't an already created itemid
                    If (CType(ctlURL.Url, String) = "") And ModuleId = 660 And Common.Utilities.Null.IsNull(itemId) Then
                        newPageUrl = CreatePageTemplate(txtTitle.Text.ToString, teDescription.Text.ToString)
                        Response.Write("114")
                        'Dim objParent As New Entities.Tabs.TabInfo
                        'Dim objTabController As Entities.Tabs.TabController = New Entities.Tabs.TabController
                        'objParent = objTabController.GetTab(newPageUrl)
                        'ctlURL.Url = objParent.ToString()

                        ctlURL.Url = newPageUrl.ToString
                        ctlURL.UrlType = "T"
                        objAnnouncement.Url = newPageUrl.ToString
                    Else

                        If (ModuleId <> 660) Then
                            objAnnouncement.Url = ctlURL.Url
                        End If
                    End If


                    ' populate object
                    objAnnouncement.ItemId = itemId
                    objAnnouncement.ModuleId = ModuleId
                    objAnnouncement.CreatedByUser = UserInfo.UserID
                    objAnnouncement.CreatedDate = Now
                    objAnnouncement.Title = txtTitle.Text
                    objAnnouncement.ImageSource = urlImage.Url
                    objAnnouncement.Description = teDescription.Text

                    objAnnouncement.PublishDate = Convert.ToDateTime(txtPublishDate.Text, Threading.Thread.CurrentThread.CurrentUICulture)

                    objAnnouncement.ExpireDate = Null.NullDate
                    If txtExpireDate.Text.Trim <> "" Then
                        Try
                            objAnnouncement.ExpireDate = Convert.ToDateTime(txtExpireDate.Text, Threading.Thread.CurrentThread.CurrentUICulture)
                        Catch
                        End Try
                    End If

                    If txtViewOrder.Text <> "" Then
                        objAnnouncement.ViewOrder = Convert.ToInt32(txtViewOrder.Text)
                    End If

                    ' add or update
                    Dim objAnnouncements As New AnnouncementsController
                    If Common.Utilities.Null.IsNull(itemId) Then
                        objAnnouncements.AddAnnouncement(objAnnouncement)
                    Else
                        If (ModuleId <> 660) Then
                            objAnnouncements.UpdateAnnouncement(objAnnouncement)
                        Else
                            objAnnouncements.UpdateAnnouncement2(objAnnouncement)
                        End If
                    End If

                    Dim newTitle As String = objAnnouncement.Title.Replace(" ", "")

                    ' refresh cache
                    ModuleController.SynchronizeModule(ModuleId)

                    If (CType(ctlURL.Url, String) = "") And ModuleId = 660 And Common.Utilities.Null.IsNull(itemId) Then
                        Response.Redirect(String.Concat("/news/PressReleases/pressreleasespages/", newTitle, "/tabid/", objAnnouncement.Url, "/Default.aspx"), True)
                    Else
                        Dim objUrls As New UrlController
                        objUrls.UpdateUrl(PortalId, ctlURL.Url, ctlURL.UrlType, ctlURL.Log, ctlURL.Track, ModuleId, ctlURL.NewWindow)

                        Response.Redirect(NavigateURL(), True)
                    End If

                End If
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

#End Region

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub

#End Region

    End Class

End Namespace