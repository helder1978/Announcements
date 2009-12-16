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

Imports DotNetNuke.Entities.Portals
Namespace DotNetNuke.Modules.Announcements

    Public Class Template

#Region " Private Members "
        Private m_TemplateCachKey As String = "dnnAnnouncements_Template_" + m_tabModuleId.ToString
        Private LocalResourceFile As String = ApplicationPath + "/DesktopModules/Announcements/App_LocalResources/Announcements.ascx"
        Private m_settings As Hashtable = Nothing

        Private m_name As String = "default"
        Private m_itemTemplate As String = ""
        Private m_altItemTemplate As String = ""
        Private m_separator As String = ""
        Private m_headerTemplate As String = ""
        Private m_footerTemplate As String = ""

        Private m_moduleId As Integer = -1
        Private m_tabModuleId As Integer = -1
#End Region


#Region " Public Properties "

        ''' <summary>
        ''' Gets and sets the name of the template
        ''' </summary>
        ''' <history>
        '''    [erikvb]   11/24/2007    created
        ''' </history>
        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(ByVal value As String)
                m_name = value
            End Set
        End Property

        ''' <summary>
        ''' Gets and sets the Item Template
        ''' </summary>
        ''' <history>
        '''    [erikvb]   11/24/2007    created
        ''' </history>
        Public Property ItemTemplate() As String
            Get
                Return m_itemTemplate
            End Get
            Set(ByVal value As String)
                m_itemTemplate = value
            End Set
        End Property

        ''' <summary>
        ''' Gets and sets the alternate item template
        ''' </summary>
        ''' <history>
        '''    [erikvb]   11/24/2007    created
        ''' </history>
        Public Property AltItemTemplate() As String
            Get
                Return m_altItemTemplate
            End Get
            Set(ByVal value As String)
                m_altItemTemplate = value
            End Set
        End Property

        ''' <summary>
        ''' Gets and sets the Separator
        ''' </summary>
        ''' <history>
        '''    [erikvb]   11/24/2007    created
        ''' </history>
        Public Property Separator() As String
            Get
                Return m_separator
            End Get
            Set(ByVal value As String)
                m_separator = value
            End Set
        End Property

        ''' <summary>
        ''' Gets and sets the HeaderTemplate
        ''' </summary>
        ''' <history>
        '''    [erikvb]   11/24/2007    created
        ''' </history>
        Public Property HeaderTemplate() As String
            Get
                Return m_headerTemplate
            End Get
            Set(ByVal value As String)
                m_headerTemplate = value
            End Set
        End Property

        ''' <summary>
        ''' Gets and sets the FooterTemplate
        ''' </summary>
        ''' <history>
        '''    [erikvb]   11/24/2007    created
        ''' </history>
        Public Property FooterTemplate() As String
            Get
                Return m_footerTemplate
            End Get
            Set(ByVal value As String)
                m_footerTemplate = value
            End Set
        End Property

        Public ReadOnly Property TokenReplaceNeeded() As Boolean
            Get
                Return (Not String.IsNullOrEmpty(HeaderTemplate)) Or (Not String.IsNullOrEmpty(FooterTemplate)) Or (Not String.IsNullOrEmpty(Separator))
            End Get
        End Property
#End Region


        Private Function getTemplate(ByVal settingsName As String) As String
            If CType(m_settings(settingsName.ToLower), String) <> "" Then
                Return CType(m_settings(settingsName.ToLower), String)
            Else
                Return Localization.GetString(settingsName + ".Text", LocalResourceFile)
            End If

        End Function

        Public Sub New(ByVal ModuleId As Integer, ByVal TabModuleId As Integer, ByVal Settings As Hashtable)
            m_settings = Settings
            m_moduleId = ModuleId
            m_tabModuleId = TabModuleId

            ItemTemplate = getTemplate("Template")
            AltItemTemplate = getTemplate("AltItemTemplate")
            Separator = getTemplate("Separator")
            HeaderTemplate = getTemplate("HeaderTemplate")
            FooterTemplate = getTemplate("FooterTemplate")
        End Sub

        Public Sub UpdateTemplate()
            Dim moduleController As New DotNetNuke.Entities.Modules.ModuleController
            moduleController.UpdateTabModuleSetting(m_tabModuleId, "template", ItemTemplate)
            moduleController.UpdateTabModuleSetting(m_tabModuleId, "altitemtemplate", AltItemTemplate)
            moduleController.UpdateTabModuleSetting(m_tabModuleId, "separator", Separator)
            moduleController.UpdateTabModuleSetting(m_tabModuleId, "headertemplate", HeaderTemplate)
            moduleController.UpdateTabModuleSetting(m_tabModuleId, "footertemplate", FooterTemplate)

            DataCache.RemoveCache(m_TemplateCachKey)

        End Sub

    End Class
End Namespace
