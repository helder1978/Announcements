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

Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Services.Tokens

Namespace DotNetNuke.Modules.Announcements

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The AnnouncementsTokenReplace Class is a custom token replace class for parsing
    ''' Announcment templates
    ''' </summary>
    ''' <history>
    ''' 	[cnurse]	08/272007  Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Class AnnouncementsTokenReplace
        Inherits TokenReplace


        Public Sub New(ByVal announcement As AnnouncementInfo)
            MyBase.new(Services.Tokens.Scope.DefaultSettings)
            Me.UseObjectLessExpression = True
            Me.PropertySource(ObjectLessToken) = announcement
        End Sub

        Protected Overrides Function replacedTokenValue(ByVal strObjectName As String, ByVal strPropertyName As String, ByVal strFormat As String) As String
            Return MyBase.replacedTokenValue(strObjectName, strPropertyName, strFormat)
        End Function

        Public Function ReplaceAnnouncmentTokens(ByVal strSourceText As String) As String
            Return MyBase.ReplaceTokens(strSourceText)
        End Function

    End Class

End Namespace