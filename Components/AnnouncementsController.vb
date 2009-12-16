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
Imports System.IO
Imports System.XML
Imports System.Text

Imports DotNetNuke.Common
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Search

Namespace DotNetNuke.Modules.Announcements

    ''' -----------------------------------------------------------------------------
    ''' Namespace:  DotNetNuke.Modules.Announcements
    ''' Project:    DotNetNuke
    ''' Class:      AnnouncementsController
    ''' -----------------------------------------------------------------------------
    ''' <summary>
	''' The AnnouncementsController Class represents the Announcments Business Layer
	''' Methods in this class call methods in the Data Layer
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[cnurse]	9/20/2004	Moved Announcements to a separate Project
	''' </history>
	''' -----------------------------------------------------------------------------
	Public Class AnnouncementsController
        Implements Entities.Modules.ISearchable
		Implements Entities.Modules.IPortable

#Region "Public Methods"

        Public Sub AddAnnouncement(ByVal objAnnouncement As AnnouncementInfo)

            'DotNetNuke.Modules.Announcements
            DataProvider.Instance().Add(objAnnouncement.ModuleId, objAnnouncement.CreatedByUser, objAnnouncement.CreatedDate, objAnnouncement.Title, objAnnouncement.ImageSource, objAnnouncement.Url, objAnnouncement.Description, objAnnouncement.ViewOrder, objAnnouncement.PublishDate, objAnnouncement.ExpireDate)

        End Sub

        'Canadean
        Public Sub AddAnnouncement(ByVal objAnnouncement As AnnouncementInfo, ByVal descriptionBody As String)

            DataProvider.Instance().Add(objAnnouncement.ModuleId, objAnnouncement.CreatedByUser, objAnnouncement.CreatedDate, objAnnouncement.Title, objAnnouncement.ImageSource, objAnnouncement.Url, String.Concat(objAnnouncement.Description, descriptionbody), objAnnouncement.ViewOrder, objAnnouncement.PublishDate, objAnnouncement.ExpireDate)

        End Sub

        Public Sub DeleteAnnouncement(ByVal ModuleID As Integer, ByVal ItemID As Integer)

            DataProvider.Instance().Delete(ModuleID, ItemID)

        End Sub

        Public Function GetAnnouncement(ByVal ItemId As Integer, ByVal ModuleId As Integer) As AnnouncementInfo

            Return CType(CBO.FillObject(DataProvider.Instance().Get(ItemId, ModuleId), GetType(AnnouncementInfo)), AnnouncementInfo)

        End Function

        Public Function GetAnnouncements(ByVal ModuleId As Integer, ByVal StartDate As Date, ByVal EndDate As Date) As ArrayList

            Return CBO.FillCollection(DataProvider.Instance().GetAll(ModuleId, StartDate, EndDate), GetType(AnnouncementInfo))

        End Function

        'Canadean
        Public Function GetAnnouncements(ByVal ModuleId As Integer, ByVal StartDate As Date, ByVal EndDate As Date, ByVal Category as String) As ArrayList

            Return CBO.FillCollection(DataProvider.Instance().GetAll(ModuleId, StartDate, EndDate, Category), GetType(AnnouncementInfo))

        End Function

        Public Function GetCurrentAnnouncements(ByVal ModuleId As Integer, ByVal Category As String, ByVal StartDate As Date, ByVal EndDate As Date) As ArrayList

            Return CBO.FillCollection(DataProvider.Instance().GetCurrent(ModuleId, Category, StartDate, EndDate), GetType(AnnouncementInfo))

        End Function

        Public Function GetCurrentAnnouncements(ByVal ModuleId As Integer) As ArrayList

            Return CBO.FillCollection(DataProvider.Instance().GetCurrent(ModuleId), GetType(AnnouncementInfo))

        End Function


        Public Function GetCurrentAnnouncements(ByVal ModuleId As Integer, ByVal StartDate As Date) As ArrayList

            Return CBO.FillCollection(DataProvider.Instance().GetCurrent(ModuleId, StartDate), GetType(AnnouncementInfo))

        End Function

        Public Function GetCurrentAnnouncements(ByVal ModuleId As Integer, ByVal Category As String) As ArrayList

            Return CBO.FillCollection(DataProvider.Instance().GetCurrent(ModuleId, Category), GetType(AnnouncementInfo))

        End Function


        Public Function GetCurrentMonthYears(ByVal ModuleId As Integer, ByVal Category As String) As ArrayList

            'Dim array As System.Collections.ArrayList = (DataProvider.Instance().GetCurrentMonthYears(ModuleId, Category))
            'Dim writer As StreamWriter = File.CreateText("c:\temp\myfile.txt")
            'writer.WriteLine("Out to file array2222.")
            'writer.Close()
            'Return array
            Return CBO.FillCollection(DataProvider.Instance().GetCurrentMonthYears(ModuleId, Category), GetType(PopulatedDatesInfo))

        End Function

        Public Function GetExpiredMonthYears(ByVal ModuleId As Integer, ByVal Category As String) As ArrayList

            Return CBO.FillCollection(DataProvider.Instance().GetExpiredMonthYears(ModuleId, Category), GetType(AnnouncementInfo))

        End Function

        'Canadean
        Public Function GetCurrentAnnouncements(ByVal ModuleId As Integer, ByVal StartDate As Date, ByVal Category As String) As ArrayList

            Return CBO.FillCollection(DataProvider.Instance().GetCurrent(ModuleId, StartDate, Category), GetType(AnnouncementInfo))

        End Function

        Public Function GetExpiredAnnouncements(ByVal ModuleId As Integer) As ArrayList

            Return CBO.FillCollection(DataProvider.Instance().GetExpired(ModuleId), GetType(AnnouncementInfo))

        End Function


        Public Function GetExpiredAnnouncements(ByVal ModuleId As Integer, ByVal Category As String, ByVal StartDate As Date, ByVal EndDate As Date) As ArrayList

            Return CBO.FillCollection(DataProvider.Instance().GetExpired(ModuleId, Category, StartDate, EndDate), GetType(AnnouncementInfo))

        End Function

        Public Function GetExpiredAnnouncements(ByVal ModuleId As Integer, ByVal Category As String) As ArrayList

            Return CBO.FillCollection(DataProvider.Instance().GetExpired(ModuleId, Category), GetType(AnnouncementInfo))

        End Function

        'GetMonthYears

        Public Function GetMonthYears(ByVal ModuleId As Integer) As ArrayList

            Return CBO.FillCollection(DataProvider.Instance().GetExpired(ModuleId), GetType(AnnouncementInfo))

        End Function

        Public Sub UpdateAnnouncement(ByVal objAnnouncement As AnnouncementInfo)

            DataProvider.Instance().Update(objAnnouncement.ItemId, objAnnouncement.ModuleId, objAnnouncement.CreatedByUser, objAnnouncement.CreatedDate, objAnnouncement.Title, objAnnouncement.ImageSource, objAnnouncement.Url, objAnnouncement.Description, objAnnouncement.ViewOrder, objAnnouncement.PublishDate, objAnnouncement.ExpireDate)

        End Sub

        Public Sub UpdateAnnouncement2(ByVal objAnnouncement As AnnouncementInfo)

            DataProvider.Instance().Update(objAnnouncement.ItemId, objAnnouncement.ModuleId, objAnnouncement.CreatedByUser, objAnnouncement.CreatedDate, objAnnouncement.Title, objAnnouncement.ImageSource, objAnnouncement.Description, objAnnouncement.ViewOrder, objAnnouncement.PublishDate, objAnnouncement.ExpireDate)

        End Sub

#End Region

#Region "ISearchable Implementation"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' GetSearchItems implements the ISearchable Interface
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <param name="ModInfo">The ModuleInfo for the module to be Indexed</param>
        ''' <history>
        '''		[cnurse]	11/17/2004	documented
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function GetSearchItems(ByVal ModInfo As Entities.Modules.ModuleInfo) As Services.Search.SearchItemInfoCollection Implements Entities.Modules.ISearchable.GetSearchItems
            Dim moduleSettings As Hashtable = DotNetNuke.Entities.Portals.PortalSettings.GetModuleSettings(ModInfo.ModuleID)
            Dim descriptionLength As Integer = 100
            If CType(moduleSettings("descriptionLength"), String) <> "" Then
                descriptionLength = Integer.Parse(CType(moduleSettings("descriptionLength"), String))
                If descriptionLength < 1 Then
                    descriptionLength = 1950 'max length of description is 2000 char, take a bit less to make sure it fits...
                End If
            End If

            Dim SearchItemCollection As New SearchItemInfoCollection

            Dim dt As New Date(1970, 1, 1)

            Dim Announcements As ArrayList = GetCurrentAnnouncements(ModInfo.ModuleID, dt, "")

            Dim objAnnouncement As Object
            For Each objAnnouncement In Announcements
                Dim SearchItem As SearchItemInfo
                With CType(objAnnouncement, AnnouncementInfo)
                    Dim strContent As String = System.Web.HttpUtility.HtmlDecode(.Title & " " & .Description)
                    Dim strDescription As String = HtmlUtils.Shorten(HtmlUtils.Clean(System.Web.HttpUtility.HtmlDecode(.Description), False), descriptionLength, "...")
                    SearchItem = New SearchItemInfo(ModInfo.ModuleTitle & " - " & .Title, strDescription, .CreatedByUser, .PublishDate, ModInfo.ModuleID, .ItemId.ToString, strContent, "ItemId=" & .ItemId.ToString)
                    SearchItemCollection.Add(SearchItem)
                End With
            Next

            Return SearchItemCollection
        End Function
#End Region

#Region "IPortable Implementation "

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' ExportModule implements the IPortable ExportModule Interface using an XmlWriter
        ''' and the IXmlSerializable Interface on the AnnoucnementInfo object
        ''' </summary>
        ''' <param name="ModuleID">The Id of the module to be exported</param>
        ''' <history>
        ''' 	[cnurse]	08/15/2007  Converted to a WAP project for Demo
        ''' 	[cnurse]	08/18/2007  Updated to use the IXmlSerializable Interface
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function ExportModule(ByVal ModuleID As Integer) As String Implements IPortable.ExportModule
            Dim sb As New StringBuilder()
            Dim settings As New XmlWriterSettings()
            settings.ConformanceLevel = ConformanceLevel.Fragment
            settings.OmitXmlDeclaration = True

            Dim arrAnnouncements As ArrayList = GetAnnouncements(ModuleID, Null.NullDate, Null.NullDate)
            If arrAnnouncements.Count <> 0 Then
                Dim writer As XmlWriter = XmlWriter.Create(sb, settings)

                'Write start of Annoucements Node
                writer.WriteStartElement("Announcements")

                For Each announcement As AnnouncementInfo In arrAnnouncements
                    announcement.WriteXml(writer)
                Next

                'Write end of Annoucements Node
                writer.WriteEndElement()

                writer.Close()
            End If

            Return sb.ToString()
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' ImportModule implements the IPortable ImportModule Interface using an XmlReader
        ''' and the IXmlSerializable Interface
        ''' </summary>
        ''' <param name="ModuleID">The Id of the module to be imported</param>
        ''' <history>
        ''' 	[cnurse]	08/15/2007  Converted to a WAP project for Demo
        ''' 	[cnurse]	08/18/2007  Updated to use the IXmlSerializable Interface
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Sub ImportModule(ByVal ModuleID As Integer, ByVal Content As String, ByVal Version As String, ByVal UserId As Integer) Implements IPortable.ImportModule

            If Version.StartsWith("03.04") Then
                ' this is the legacy import function for version 03.04.00
                Dim xmlAnnouncement As XmlNode
                Dim xmlAnnouncements As XmlNode = GetContent(Content, "Announcements")
                For Each xmlAnnouncement In xmlAnnouncements
                    Dim objAnnouncement As AnnouncementInfo = ImportAnnouncement(xmlAnnouncement)
                    If Not objAnnouncement Is Nothing Then
                        objAnnouncement.ModuleId = ModuleID
                        objAnnouncement.CreatedByUser = UserId
                        objAnnouncement.CreatedDate = Date.Now()

                        AddAnnouncement(objAnnouncement)

                    End If
                Next
            ElseIf Version.StartsWith("03") Then
                ' this is the legacy import function for all versions prior to version 03.04
                Dim xmlAnnouncement As XmlNode
                Dim xmlAnnouncements As XmlNode = GetContent(Content, "announcements")
                For Each xmlAnnouncement In xmlAnnouncements
                    Dim objAnnouncement As New AnnouncementInfo
                    objAnnouncement.ModuleId = ModuleID
                    objAnnouncement.Title = XmlUtils.GetNodeValue(xmlAnnouncement, "title")
                    objAnnouncement.Url = ImportUrl(ModuleID, XmlUtils.GetNodeValue(xmlAnnouncement, "url"))
                    objAnnouncement.Description = XmlUtils.GetNodeValue(xmlAnnouncement, "description")
                    objAnnouncement.ViewOrder = XmlUtils.GetNodeValueInt(xmlAnnouncement, "vieworder")
                    objAnnouncement.CreatedDate = XmlUtils.GetNodeValueDate(xmlAnnouncement, "createddate", Now)
                    objAnnouncement.PublishDate = objAnnouncement.CreatedDate
                    objAnnouncement.CreatedByUser = UserId
                    AddAnnouncement(objAnnouncement)
                Next
            Else
                ' this is the current import function
                Using reader As XmlReader = XmlReader.Create(New StringReader(Content))
                    If reader.Read() Then
                        reader.ReadStartElement("Announcements")
                        If reader.ReadState <> ReadState.EndOfFile And reader.NodeType <> XmlNodeType.None And reader.LocalName <> "" Then
                            Do
                                reader.ReadStartElement("Announcement")
                                Dim announcement As New AnnouncementInfo

                                'Deserialize announcement
                                announcement.ReadXml(reader)

                                announcement.ModuleId = ModuleID
                                announcement.CreatedByUser = UserId
                                announcement.CreatedDate = Date.Now()

                                'Save announcement
                                AddAnnouncement(announcement)
                            Loop While reader.ReadToNextSibling("Announcement")
                        End If
                    End If

                    reader.Close()
                End Using
            End If
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Deserializes announcementInfo xml into new AnnouncementInfo instance.
        ''' If an error occurs, Nothing is returned
        ''' 
        ''' deprecated
        ''' </summary>
        ''' <param name="xmlAnnouncement">the xml to be deserialized</param>
        ''' <returns>AnnouncementInfo instance</returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	12/07/2006	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function ImportAnnouncement(ByVal xmlAnnouncement As Xml.XmlNode) As AnnouncementInfo

            Try
                Dim objAnnouncement As New AnnouncementInfo
                Dim xSer As Xml.Serialization.XmlSerializer
                xSer = New Xml.Serialization.XmlSerializer(GetType(AnnouncementInfo))

                objAnnouncement = CType(xSer.Deserialize(New IO.StringReader(xmlAnnouncement.OuterXml)), AnnouncementInfo)

                Return objAnnouncement

            Catch
                Return Nothing
            End Try
        End Function

#End Region



    End Class


End Namespace

