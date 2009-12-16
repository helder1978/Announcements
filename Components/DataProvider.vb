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
Imports DotNetNuke

Namespace DotNetNuke.Modules.Announcements

  ''' -----------------------------------------------------------------------------
  ''' <summary>
  ''' The DataProvider Class Is an abstract class that provides the DataLayer
  ''' for the Announcments Module.
  ''' </summary>
  ''' <remarks>
  ''' </remarks>
  ''' <history>
  ''' 	[cnurse]	9/20/2004	Moved Announcements to a separate Project
  ''' </history>
  ''' -----------------------------------------------------------------------------
  Public MustInherit Class DataProvider

#Region "Shared/Static Methods"

        ' singleton reference to the instantiated object 
        Private Shared objProvider As DataProvider = Nothing

        ' constructor
        Shared Sub New()
            CreateProvider()
        End Sub

        ' dynamically create provider
        Private Shared Sub CreateProvider()
			objProvider = CType(Framework.Reflection.CreateObject("data", "DotNetNuke.Modules.Announcements", "DotNetNuke.Modules.Announcements"), DataProvider)
        End Sub

        ' return the provider
        Public Shared Shadows Function Instance() As DataProvider
            Return objProvider
        End Function

#End Region

#Region "Abstract methods"

        Public MustOverride Function GetCurrentMonthYears(ByVal ModuleId As Integer, ByVal Category As String) As IDataReader
        Public MustOverride Function GetExpiredMonthYears(ByVal ModuleId As Integer, ByVal Category As String) As IDataReader
        Public MustOverride Function GetAll(ByVal ModuleId As Integer, ByVal StartDate As Date, ByVal EndDate As Date) As IDataReader
        Public MustOverride Function GetAll(ByVal ModuleId As Integer, ByVal StartDate As Date, ByVal EndDate As Date, ByVal Category as String) As IDataReader
        Public MustOverride Function GetCurrent(ByVal ModuleId As Integer, ByVal Category As String, ByVal StartDate As Date, ByVal EndDate As Date) As IDataReader
        Public MustOverride Function GetCurrent(ByVal ModuleId As Integer) As IDataReader
        Public MustOverride Function GetCurrent(ByVal ModuleId As Integer, ByVal StartDate As Date, ByVal Category As String) As IDataReader
        Public MustOverride Function GetCurrent(ByVal ModuleId As Integer, ByVal StartDate As Date) As IDataReader
        Public MustOverride Function GetCurrent(ByVal ModuleId As Integer, ByVal Category As String) As IDataReader
        Public MustOverride Function GetExpired(ByVal ModuleId As Integer) As IDataReader
        Public MustOverride Function GetExpired(ByVal ModuleId As Integer, ByVal Category As String) As IDataReader
        Public MustOverride Function GetExpired(ByVal ModuleId As Integer, ByVal Category As String, ByVal StartDate As Date, ByVal EndDate As Date) As IDataReader
        Public MustOverride Function [Get](ByVal ItemId As Integer, ByVal ModuleId As Integer) As IDataReader
        Public MustOverride Sub Delete(ByVal ModuleID As Integer, ByVal ItemID As Integer)
        Public MustOverride Function Add(ByVal ModuleId As Integer, ByVal CreatedByUser As Integer, ByVal CreatedDate As Date, ByVal Title As String, ByVal imageSource As String, ByVal URL As String, ByVal Description As String, ByVal ViewOrder As Integer, ByVal PublishDate As DateTime, ByVal ExpireDate As DateTime) As Integer
        Public MustOverride Sub Update(ByVal ItemId As Integer, ByVal ModuleID As Integer, ByVal CreatedByUser As Integer, ByVal CreatedDate As Date, ByVal Title As String, ByVal imageSource As String, ByVal URL As String, ByVal Description As String, ByVal ViewOrder As Integer, ByVal PublishDate As DateTime, ByVal ExpireDate As DateTime)
        Public MustOverride Sub Update(ByVal ItemId As Integer, ByVal ModuleID As Integer, ByVal CreatedByUser As Integer, ByVal CreatedDate As Date, ByVal Title As String, ByVal imageSource As String, ByVal Description As String, ByVal ViewOrder As Integer, ByVal PublishDate As DateTime, ByVal ExpireDate As DateTime)

#End Region

    End Class

End Namespace