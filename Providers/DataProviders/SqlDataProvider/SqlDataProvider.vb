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
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Imports Microsoft.ApplicationBlocks.Data

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Framework.Providers

Namespace DotNetNuke.Modules.Announcements

	''' -----------------------------------------------------------------------------
	''' <summary>
	''' The SqlDataProvider Class is an SQL Server implementation of the DataProvider Abstract
	''' class that provides the DataLayer for the Announcments Module.
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[cnurse]	9/20/2004	Moved Announcements to a separate Project
	''' </history>
	''' -----------------------------------------------------------------------------
	Public Class SqlDataProvider

		Inherits DataProvider

#Region "Private Members"

		Private Const ProviderType As String = "data"
        Private Const ModuleQualifier As String = "dnnAnnouncements_"

        Private _providerConfiguration As ProviderConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType)
        Private _connectionString As String
        Private _providerPath As String
        Private _objectQualifier As String
        Private _databaseOwner As String

#End Region

#Region "Constructors"

        Public Sub New()

            ' Read the configuration specific information for this provider
            Dim objProvider As Provider = CType(_providerConfiguration.Providers(_providerConfiguration.DefaultProvider), Provider)

            'Get Connection string from web.config
            _connectionString = Config.GetConnectionString()

            If _connectionString = "" Then
                ' Use connection string specified in provider
                _connectionString = objProvider.Attributes("connectionString")
            End If

            _providerPath = objProvider.Attributes("providerPath")

            _objectQualifier = objProvider.Attributes("objectQualifier")
            If _objectQualifier <> "" And _objectQualifier.EndsWith("_") = False Then
                _objectQualifier += "_"
            End If

            _databaseOwner = objProvider.Attributes("databaseOwner")
            If _databaseOwner <> "" And _databaseOwner.EndsWith(".") = False Then
                _databaseOwner += "."
            End If

        End Sub

#End Region

#Region "Properties"

		Public ReadOnly Property ConnectionString() As String
			Get
				Return _connectionString
			End Get
		End Property

		Public ReadOnly Property ProviderPath() As String
			Get
				Return _providerPath
			End Get
		End Property

		Public ReadOnly Property ObjectQualifier() As String
			Get
				Return _objectQualifier
			End Get
		End Property

		Public ReadOnly Property DatabaseOwner() As String
			Get
				Return _databaseOwner
			End Get
		End Property

#End Region

#Region "Private Methods"

        Private Function GetFullyQualifiedName(ByVal name As String) As String
            Return DatabaseOwner & ObjectQualifier & ModuleQualifier & name
        End Function

        Private Function GetNull(ByVal Field As Object) As Object
            Return DotNetNuke.Common.Utilities.Null.GetNull(Field, DBNull.Value)
        End Function

#End Region

#Region "Public Methods"
        Public Overrides Function Add(ByVal ModuleId As Integer, ByVal CreatedByUser As Integer, ByVal CreatedDate As Date, ByVal Title As String, ByVal imageSource As String, ByVal URL As String, ByVal Description As String, ByVal ViewOrder As Integer, ByVal PublishDate As DateTime, ByVal ExpireDate As DateTime) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Add"), ModuleId, CreatedByUser, CreatedDate, Title, imageSource, URL, Description, GetNull(ViewOrder), GetNull(PublishDate), GetNull(ExpireDate)), Integer)
        End Function

        Public Overrides Sub Delete(ByVal ModuleID As Integer, ByVal ItemId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Delete"), ModuleID, ItemId)
        End Sub

        Public Overrides Function [Get](ByVal ItemId As Integer, ByVal ModuleId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Get"), ItemId, ModuleId), IDataReader)
        End Function

        Public Overrides Function GetAll(ByVal ModuleId As Integer, ByVal StartDate As Date, ByVal EndDate As Date) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("GetAll"), ModuleId, GetNull(""), GetNull(StartDate), GetNull(EndDate)), IDataReader)
        End Function
        'Canadean
        Public Overrides Function GetAll(ByVal ModuleId As Integer, ByVal StartDate As Date, ByVal EndDate As Date, ByVal Category As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("GetAllByCategory"), ModuleId, GetNull(StartDate), GetNull(EndDate), GetNull(Category)), IDataReader)
        End Function

        'With a certain category
        Public Overrides Function GetCurrentMonthYears(ByVal ModuleId As Integer, ByVal Category As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("GetCurrentMonthYearsByCategory"), ModuleId, Category), IDataReader)
        End Function
        'With a certain category
        Public Overrides Function GetExpiredMonthYears(ByVal ModuleId As Integer, ByVal Category As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("GetExpiredMonthYearsByCategory"), ModuleId, Category), IDataReader)
        End Function


        Public Overrides Function GetExpired(ByVal ModuleId As Integer, ByVal Category As String, ByVal StartDate As Date, ByVal EndDate As Date) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("GetExpired"), ModuleId, Category, GetNull(StartDate), GetNull(EndDate)), IDataReader)
        End Function

        Public Overrides Function GetExpired(ByVal ModuleId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("GetExpired"), ModuleId), IDataReader)
        End Function

        Public Overrides Function GetExpired(ByVal ModuleId As Integer, ByVal Category As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("GetExpiredByCategory"), ModuleId, Category), IDataReader)
        End Function

        Public Overrides Sub Update(ByVal ItemId As Integer, ByVal ModuleID As Integer, ByVal CreatedByUser As Integer, ByVal CreatedDate As Date, ByVal Title As String, ByVal imageSource As String, ByVal URL As String, ByVal Description As String, ByVal ViewOrder As Integer, ByVal PublishDate As DateTime, ByVal ExpireDate As DateTime)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Update"), ItemId, ModuleID, CreatedByUser, CreatedDate, Title, GetNull(imageSource), URL, Description, GetNull(ViewOrder), GetNull(PublishDate), GetNull(ExpireDate))
        End Sub

        Public Overrides Function GetCurrent(ByVal ModuleId As Integer, ByVal Category As String, ByVal StartDate As Date, ByVal EndDate As Date) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("GetCurrentByCategory"), ModuleId, Category, GetNull(StartDate), GetNull(EndDate)), IDataReader)
        End Function

        Public Overrides Function GetCurrent(ByVal ModuleId As Integer, ByVal StartDate As Date) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("GetAll"), ModuleId, GetNull(""), GetNull(StartDate)), IDataReader)
        End Function

        Public Overrides Function GetCurrent(ByVal ModuleId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("GetCurrentByCategory"), ModuleId), IDataReader)
        End Function

        'Canadean
        Public Overrides Function GetCurrent(ByVal ModuleId As Integer, ByVal StartDate As Date, ByVal Category As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("GetCurrentByCategory"), ModuleId, GetNull(Category), GetNull(StartDate), GetNull(Null.NullDate)), IDataReader)
        End Function
        'Canadean
        Public Overrides Function GetCurrent(ByVal ModuleId As Integer, ByVal Category As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("GetCurrentByCategory"), ModuleId, GetNull(Category)), IDataReader)
        End Function

        Public Overrides Sub Update(ByVal ItemId As Integer, ByVal ModuleID As Integer, ByVal CreatedByUser As Integer, ByVal CreatedDate As Date, ByVal Title As String, ByVal imageSource As String, ByVal Description As String, ByVal ViewOrder As Integer, ByVal PublishDate As DateTime, ByVal ExpireDate As DateTime)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("UpdateWithNoUrl"), ItemId, ModuleID, CreatedByUser, CreatedDate, Title, GetNull(imageSource), Description, GetNull(ViewOrder), GetNull(PublishDate), GetNull(ExpireDate))
        End Sub


#End Region

    End Class

End Namespace