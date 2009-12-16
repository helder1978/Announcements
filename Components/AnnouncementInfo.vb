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

Imports System
Imports System.Configuration
Imports System.Data
Imports System.Web
Imports System.Xml
Imports System.Xml.Schema
Imports System.Xml.Serialization

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Services.FileSystem
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Services.Tokens
Imports DotNetNuke.Security.PortalSecurity

Namespace DotNetNuke.Modules.Announcements

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The AnnouncementInfo Class provides the Announcements Business Object
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[cnurse]	9/20/2004	Moved Announcements to a separate Project
    ''' </history>
    ''' -----------------------------------------------------------------------------
    <Serializable(), XmlRoot("Announcement")> _
      Public Class AnnouncementInfo
        Implements IHydratable
        Implements IXmlSerializable
        Implements IPropertyAccess

#Region "Private Members"

        Private _ItemId As Integer
        Private _ModuleId As Integer
        Private _Title As String
        Private _Url As String
        Private _Description As String
        Private _LongHeading As String
        Private _Summary As String
        Private _imageSource As String
        Private _ViewOrder As Integer
        Private _CreatedByUser As Integer
        Private _CreatedDate As Date
        Private _TrackClicks As Boolean
        Private _NewWindow As Boolean
        Private _PublishDate As DateTime
        Private _ExpireDate As DateTime
        Private _isEditable As Boolean

#End Region

#Region "Constructors"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Constructs a new AnnouncementInfo instance
        ''' </summary>
        ''' <history>
        ''' 	[erikvb]	11/19/2007  Described
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Sub New()
        End Sub

#End Region

#Region "Properties"

        Public Property IsEditable() As Boolean
            Get
                Return _isEditable
            End Get
            Set(ByVal value As Boolean)
                _isEditable = value
            End Set
        End Property
        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets and sets the Id of the announcment
        ''' </summary>
        ''' <history>
        ''' 	[erikvb]	11/19/2007  Described
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property ItemId() As Integer
            Get
                Return _ItemId
            End Get
            Set(ByVal Value As Integer)
                _ItemId = Value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets and sets the Id of the module to which the annoucement belongs
        ''' </summary>
        ''' <history>
        ''' 	[erikvb]	11/19/2007  Described
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property ModuleId() As Integer
            Get
                Return _ModuleId
            End Get
            Set(ByVal Value As Integer)
                _ModuleId = Value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets and sets the Title of the announcment
        ''' </summary>
        ''' <history>
        ''' 	[erikvb]	11/19/2007  Described
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property Title() As String
            Get
                Return _Title
            End Get
            Set(ByVal Value As String)
                _Title = Value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets and sets the Url of the announcment
        ''' </summary>
        ''' <history>
        ''' 	[erikvb]	11/19/2007  Described
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property Url() As String
            Get
                Return _Url
            End Get
            Set(ByVal Value As String)
                _Url = Value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets and sets the View Order of the announcment
        ''' </summary>
        ''' <history>
        ''' 	[erikvb]	11/19/2007  Described
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property ViewOrder() As Integer
            Get
                Return _ViewOrder
            End Get
            Set(ByVal Value As Integer)
                _ViewOrder = Value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets and sets the Description of the announcment
        ''' </summary>
        ''' <history>
        ''' 	[erikvb]	11/19/2007  Described
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal Value As String)
                _Description = Value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets and sets the Summary of the announcment
        ''' </summary>
        ''' <history>
        ''' 	[erikvb]	11/19/2007  Described
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property Summary() As String
            Get
                Return _Summary
            End Get
            Set(ByVal Value As String)
                _Summary = Value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets and sets the LongHeading of the announcment
        ''' </summary>
        ''' <history>
        ''' 	[erikvb]	11/19/2007  Described
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property LongHeading() As String
            Get
                Return _LongHeading
            End Get
            Set(ByVal Value As String)
                _LongHeading = Value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets and sets the Image Source of the announcment
        ''' </summary>
        ''' <history>
        ''' 	[erikvb]	11/19/2007  Described
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property ImageSource() As String
            Get
                Return _imageSource
            End Get
            Set(ByVal value As String)
                _imageSource = value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets and sets the Id of the user that last updated the announcment
        ''' </summary>
        ''' <history>
        ''' 	[erikvb]	11/19/2007  Described
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property CreatedByUser() As Integer
            Get
                Return _CreatedByUser
            End Get
            Set(ByVal Value As Integer)
                _CreatedByUser = Value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets and sets the date the annoucnement was last changed
        ''' </summary>
        ''' <history>
        ''' 	[erikvb]	11/19/2007  Described
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property CreatedDate() As Date
            Get
                Return _CreatedDate
            End Get
            Set(ByVal Value As Date)
                _CreatedDate = Value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets and sets a flag that determines whether each time the annoucnement is clicked is tracked
        ''' </summary>
        ''' <history>
        ''' 	[erikvb]	11/19/2007  Described
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property TrackClicks() As Boolean
            Get
                Return _TrackClicks
            End Get
            Set(ByVal Value As Boolean)
                _TrackClicks = Value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets and sets a flag that determines whether the link to the annoucnement is opened in a new window
        ''' </summary>
        ''' <history>
        ''' 	[erikvb]	11/19/2007  Described
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property NewWindow() As Boolean
            Get
                Return _NewWindow
            End Get
            Set(ByVal Value As Boolean)
                _NewWindow = Value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets and sets a the Publish Date of the annoucement
        ''' </summary>
        ''' <history>
        ''' 	[erikvb]	11/19/2007  Described
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property PublishDate() As Date
            Get
                Return _PublishDate
            End Get
            Set(ByVal Value As Date)
                _PublishDate = Value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets and sets a the Expiry Date of the annoucement
        ''' </summary>
        ''' <history>
        ''' 	[erikvb]	11/19/2007  Described
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property ExpireDate() As Date
            Get
                Return _ExpireDate
            End Get
            Set(ByVal Value As Date)
                _ExpireDate = Value
            End Set
        End Property


#End Region

#Region "IHydratable Implementation"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Fill hydrates the object from a Datareader
        ''' </summary>
        ''' <remarks>The Fill method is used by the CBO method to hydrtae the object
        ''' rather than using the more expensive Refection  methods.</remarks>
        ''' <history>
        ''' 	[cnurse]	08/17/2007  Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Sub Fill(ByVal dr As IDataReader) Implements IHydratable.Fill

            ItemId = Convert.ToInt32(Null.SetNull(dr.Item("ItemID"), ItemId))
            ModuleId = Convert.ToInt32(Null.SetNull(dr.Item("ModuleID"), ModuleId))
            Title = Convert.ToString(Null.SetNull(dr.Item("Title"), Title))
            Url = Convert.ToString(Null.SetNull(dr.Item("Url"), Url))
            ViewOrder = Convert.ToInt32(Null.SetNull(dr.Item("ViewOrder"), ViewOrder))
            Description = Convert.ToString(Null.SetNull(dr.Item("Description"), Description))
            Summary = Convert.ToString(Null.SetNull(dr.Item("Summary"), Description))
            LongHeading = Convert.ToString(Null.SetNull(dr.Item("LongHeading"), Description))
            ImageSource = Convert.ToString(Null.SetNull(dr.Item("ImageSource"), ImageSource))
            CreatedByUser = Convert.ToInt32(Null.SetNull(dr.Item("CreatedByUser"), CreatedByUser))
            CreatedDate = Convert.ToDateTime(Null.SetNull(dr.Item("CreatedDate"), CreatedDate))
            TrackClicks = Convert.ToBoolean(Null.SetNull(dr.Item("TrackClicks"), TrackClicks))
            NewWindow = Convert.ToBoolean(Null.SetNull(dr.Item("NewWindow"), NewWindow))
            PublishDate = Convert.ToDateTime(Null.SetNull(dr.Item("PublishDate"), PublishDate))
            ExpireDate = Convert.ToDateTime(Null.SetNull(dr.Item("ExpireDate"), ExpireDate))

        End Sub
        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets and sets the Key ID
        ''' </summary>
        ''' <remarks>The KeyID property is part of the IHydratble interface.  It is used
        ''' as the key property when creating a Dictionary</remarks>
        ''' <history>
        ''' 	[cnurse]	08/17/2007  Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property KeyID() As Integer Implements Entities.Modules.IHydratable.KeyID
            Get
                Return ItemId
            End Get
            Set(ByVal value As Integer)
                ItemId = value
            End Set
        End Property
#End Region

#Region "IXmlSerializable Implementation"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' GetSchema returns the XmlSchema for this class
        ''' </summary>
        ''' <remarks>GetSchema is implemented as a stub method as it is not required</remarks>
        ''' <history>
        ''' 	[cnurse]	08/17/2007  Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function GetSchema() As XmlSchema Implements IXmlSerializable.GetSchema
            Return Nothing
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' ReadXml fills the object (de-serializes it) from the XmlReader passed
        ''' </summary>
        ''' <remarks></remarks>
        ''' <param name="reader">The XmlReader that contains the xml for the object</param>
        ''' <history>
        ''' 	[cnurse]	08/17/2007  Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Sub ReadXml(ByVal reader As XmlReader) Implements IXmlSerializable.ReadXml
            While reader.Read()
                If reader.NodeType = XmlNodeType.EndElement Then
                    Exit While
                Else
                    Select Case reader.Name
                        Case "ItemId"
                            ItemId = reader.ReadContentAsInt()
                        Case "ModuleId"
                            ModuleId = reader.ReadContentAsInt()
                        Case "Title"
                            Title = reader.ReadContentAsString()
                        Case "Url"
                            Url = reader.ReadContentAsString()
                        Case "ViewOrder"
                            ViewOrder = reader.ReadContentAsInt()
                        Case "Description"
                            Description = reader.ReadContentAsString()
                        Case "Summary"
                            Summary = reader.ReadContentAsString()
                        Case "LongHeading"
                            LongHeading = reader.ReadContentAsString()
                        Case "ImageSource"
                            'some code needs to be added here to check whether the file is available in the file system, and is of type image
                            ImageSource = reader.ReadContentAsString()
                        Case "TrackClicks"
                            TrackClicks = reader.ReadContentAsBoolean()
                        Case "NewWindow"
                            NewWindow = reader.ReadContentAsBoolean()
                        Case "PublishDate"
                            PublishDate = reader.ReadContentAsDateTime()
                        Case "ExpireDate"
                            ExpireDate = reader.ReadContentAsDateTime()
                    End Select
                End If
            End While
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' WriteXml converts the object to Xml (serializes it) and writes it using the XmlWriter passed
        ''' </summary>
        ''' <remarks></remarks>
        ''' <param name="writer">The XmlWriter that contains the xml for the object</param>
        ''' <history>
        ''' 	[cnurse]	08/17/2007  Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Sub WriteXml(ByVal writer As XmlWriter) Implements IXmlSerializable.WriteXml
            writer.WriteStartElement("Announcement")
            writer.WriteElementString("ItemId", ItemId.ToString())
            writer.WriteElementString("ModuleId", ModuleId.ToString())
            writer.WriteElementString("Title", Title)
            writer.WriteElementString("Url", Url)
            writer.WriteElementString("ViewOrder", ViewOrder.ToString)
            writer.WriteElementString("Description", Description)
            writer.WriteElementString("Summary", Summary)
            writer.WriteElementString("LongHeading", LongHeading)
            writer.WriteElementString("ImageSource", ImageSource)
            writer.WriteElementString("TrackClicks", TrackClicks.ToString)
            writer.WriteElementString("NewWindow", NewWindow.ToString)
            writer.WriteElementString("PublishDate", PublishDate.ToString)
            writer.WriteElementString("ExpireDate", ExpireDate.ToString)
            writer.WriteEndElement()
        End Sub

#End Region

#Region "IPropertyAccess Implementation"

        Private LocalResourceFile As String = ApplicationPath + "/DesktopModules/Announcements/App_LocalResources/Announcements.ascx"


        Public Function GetProperty(ByVal strPropertyName As String, ByVal strFormat As String, ByVal formatProvider As System.Globalization.CultureInfo, ByVal AccessingUser As Entities.Users.UserInfo, ByVal AccessLevel As Services.Tokens.Scope, ByRef PropertyNotFound As Boolean) As String Implements Services.Tokens.IPropertyAccess.GetProperty
            Dim OutputFormat As String = String.Empty
            Dim portalSettings As PortalSettings = PortalController.GetCurrentPortalSettings()
            If strFormat = String.Empty Then
                OutputFormat = "D"
            Else
                OutputFormat = strFormat
            End If
            Select Case strPropertyName.ToLower
                Case "addnew"
                    Dim userInfo As UserInfo = CType(HttpContext.Current.Items("UserInfo"), UserInfo)
                    Dim moduleConfiguration As ModuleInfo = New ModuleController().GetModule(ModuleId, portalSettings.ActiveTab.TabID)
                    'If DotNetNuke.Security.PortalSecurity.HasNecessaryPermission(Security.SecurityAccessLevel.Edit, portalSettings, moduleConfiguration, userInfo) Then
                    If IsEditable Or IsInRole("Administrators") Or IsInRole("Webmaster") Or IsInRole("ContentAdmin") Then
                        Return "<a href=""" + Common.Globals.NavigateURL(portalSettings.ActiveTab.TabID, False, portalSettings, "Edit", Globalization.CultureInfo.CurrentCulture.Name, "mid=" + ModuleId.ToString) + """><img border=""0"" src=""" + Common.Globals.ApplicationPath + "/images/edit.gif"" alt=""" + Localization.GetString("EditAnnouncement.Text", LocalResourceFile) + """ /> Add New News</a>"
                    Else
                        'it should show the add announcement button
                        Return String.Empty
                    End If
                Case "edit"
                    Dim userInfo As UserInfo = CType(HttpContext.Current.Items("UserInfo"), UserInfo)
                    Dim moduleConfiguration As ModuleInfo = New ModuleController().GetModule(ModuleId, portalSettings.ActiveTab.TabID)
                    'If DotNetNuke.Security.PortalSecurity.HasNecessaryPermission(Security.SecurityAccessLevel.Edit, portalSettings, moduleConfiguration, userInfo) Then
                    If IsEditable Or IsInRole("Administrators") Or IsInRole("Webmaster") Or IsInRole("ContentAdmin") Then
                        Return "<a href=""" + Common.Globals.NavigateURL(portalSettings.ActiveTab.TabID, False, portalSettings, "Edit", Globalization.CultureInfo.CurrentCulture.Name, "mid=" + ModuleId.ToString, "itemid=" + ItemId.ToString) + """><img border=""0"" src=""" + Common.Globals.ApplicationPath + "/images/edit.gif"" alt=""" + Localization.GetString("EditAnnouncement.Text", LocalResourceFile) + """ /></a>"
                    Else
                        'it should show the add announcement button
                        Return String.Empty
                    End If
                Case "itemid"
                    Return (Me.ItemId.ToString(OutputFormat, formatProvider))
                Case "moduleid"
                    Return (Me.ModuleId.ToString(OutputFormat, formatProvider))
                Case "title"
                    Return PropertyAccess.FormatString(Me.Title, strFormat)
                Case "url"
                    If String.IsNullOrEmpty(Url) Then
                        Return Url
                    Else
                        Return Common.Globals.LinkClick(Url, portalSettings.ActiveTab.TabID, ModuleId, TrackClicks)
                    End If
                Case "description"
                    Return HttpUtility.HtmlDecode(Me.Description)
                Case "summary"
                    Return HttpUtility.HtmlDecode(Me.Summary)
                Case "longheading"
                    Return HttpUtility.HtmlDecode(Me.LongHeading)
                Case "imagesource", "rawimage"
                    Dim strValue As String = Me.ImageSource
                    If strPropertyName.ToLower = "imagesource" AndAlso String.IsNullOrEmpty(strFormat) Then
                        strFormat = "<img src=""{0}"" alt=""" + Title + """/>"
                    End If

                    ''Retrieve the path to the imagefile
                    If strValue <> "" Then
                        'Get path from filesystem only when the image comes from within DNN.

                        If Me.ImageSource.StartsWith("FileID=") Then
                            Dim fileCnt As New FileController
                            Dim objFile As FileInfo = fileCnt.GetFileById(CInt(strValue.Substring(7)), portalSettings.PortalId)
                            If Not objFile Is Nothing Then
                                strValue = portalSettings.HomeDirectory & objFile.Folder & objFile.FileName
                            End If

                        End If
                        'strValue = PropertyAccess.FormatString(strValue, strFormat)
                    End If
                    Return strValue
                Case "vieworder"
                        Return (Me.ViewOrder.ToString(OutputFormat, formatProvider))
                Case "createdbyuser"
                        Dim tmpUser As UserInfo = UserController.GetUser(portalSettings.PortalId, Me.CreatedByUser, False)
                        If Not tmpUser Is Nothing Then
                            Return tmpUser.DisplayName
                        Else
                            Return Localization.GetString("userUnknown.Text", LocalResourceFile)
                        End If
                Case "trackclicks"
                        Return (PropertyAccess.Boolean2LocalizedYesNo(Me.TrackClicks, formatProvider))
                Case "newwindow"
                        If NewWindow Then
                            Return "_new"
                        Else
                            Return "_self"
                        End If
                Case "createddate"
                        Return (Me.CreatedDate.ToString(OutputFormat, formatProvider))
                Case "publishdate"
                        Return (Me.PublishDate.ToString(OutputFormat, formatProvider))
                Case "expiredate"
                        Return (Me.ExpireDate.ToString(OutputFormat, formatProvider))
                Case "more"
                        Return Localization.GetString("More.Text", LocalResourceFile)
                Case "readmore"
                        Dim strTarget As String
                        If NewWindow Then
                            strTarget = "_new"
                        Else
                            strTarget = "_self"
                        End If

                        If Not String.IsNullOrEmpty(Url) Then
                            Return "<a href=""" + Common.Globals.LinkClick(Url, portalSettings.ActiveTab.TabID, ModuleId, TrackClicks) + """ target=""" + strTarget + """>" + Localization.GetString("More.Text", LocalResourceFile) + "</a>"
                        Else
                            Return ""
                        End If
                Case "linkbutton"
                        Dim strTarget As String
                        If NewWindow Then
                            strTarget = "_new"
                        Else
                            strTarget = "_self"
                        End If

                        If Not String.IsNullOrEmpty(Url) Then
                            Return "<a href=""" + Common.Globals.LinkClick(Url, portalSettings.ActiveTab.TabID, ModuleId, TrackClicks) + """ target=""" + strTarget + """>" + Localization.GetString("More.Text", LocalResourceFile) + "</a>"
                        Else
                            Return ""
                        End If
                Case Else
                        PropertyNotFound = True
            End Select

            Return Null.NullString
        End Function

        Public ReadOnly Property Cacheability() As Services.Tokens.CacheLevel Implements Services.Tokens.IPropertyAccess.Cacheability
            Get
                Return CacheLevel.fullyCacheable
            End Get
        End Property

#End Region


    End Class


End Namespace

