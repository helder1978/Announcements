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
      Public Class PopulatedDatesInfo
#Region "Private Members"

        Private _Month As Integer
        Private _Year As Integer
        Private _KeyId As Integer
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

        Public Sub Fill(ByVal dr As IDataReader)

            Month = Convert.ToInt32(Null.SetNull(dr.Item("Month"), Month))
            Year = Convert.ToInt32(Null.SetNull(dr.Item("Year"), Year))
            
        End Sub

#Region "Properties"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets and sets the Id of the announcment
        ''' </summary>
        ''' <history>
        ''' 	[erikvb]	11/19/2007  Described
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property Month() As Integer
            Get
                Return _Month
            End Get
            Set(ByVal Value As Integer)
                _Month = Value
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
        Public Property Year() As Integer
            Get
                Return _Year
            End Get
            Set(ByVal Value As Integer)
                _Year = Value
            End Set
        End Property

        Public Property KeyId() As Integer
            Get
                Return _KeyId
            End Get
            Set(ByVal Value As Integer)
                _KeyId = _Year * 100 + _Month
            End Set
        End Property


#End Region

    End Class


End Namespace

