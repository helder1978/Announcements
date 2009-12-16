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

Imports DotNetNuke
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Modules.Actions
Imports DotNetNuke.Services.Exceptions

Namespace DotNetNuke.Modules.Announcements


    Public Class AnnouncementsBase
        Inherits PortalModuleBase

        Private m_Template As Template

        Protected ReadOnly Property Template() As Template
            Get
                Dim m_TemplateCachKey As String = "dnnAnnouncements_Template_" + Me.TabModuleId.ToString
                If m_Template Is Nothing Then
                    Dim obj As Object = DataCache.GetCache(m_TemplateCachKey)
                    If obj Is Nothing Then
                        obj = New Template(ModuleId, TabModuleId, Settings)
                        DataCache.SetCache(m_TemplateCachKey, obj)
                    End If
                    m_Template = CType(obj, Template)
                End If
                Return m_Template
            End Get
        End Property

    End Class

End Namespace