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
Imports System.Web.UI.WebControls
Imports DotNetNuke
Imports DotNetNuke.Entities.Modules

Namespace DotNetNuke.Modules.Announcements

    ''' -----------------------------------------------------------------------------
    ''' <summary>
	''' The Settings ModuleSettingsBase is used to manage the 
	''' settings for the Links Module
	''' </summary>
    ''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[cnurse]	9/23/2004	Moved Links to a separate Project
	''' 	[cnurse]	9/23/2004	Updated to reflect design changes for Help, 508 support
	'''                       and localisation
	'''		[cnurse]	10/20/2004	Converted to a ModuleSettingsBase class
	''' </history>
	''' -----------------------------------------------------------------------------
	Partial  Class Settings
		Inherits Entities.Modules.ModuleSettingsBase

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

#Region "Base Method Implementations"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' LoadSettings loads the settings from the Databas and displays them
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        '''		[cnurse]	10/20/2004	created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overrides Sub LoadSettings()
            Try
                If Not Page.IsPostBack Then
                    If CType(TabModuleSettings("history"), String) <> "" Then
                        txtHistory.Text = CType(TabModuleSettings("history"), String)
                    End If
                    If CType(ModuleSettings("descriptionLength"), String) <> "" Then
                        txtDescriptionLength.Text = CType(ModuleSettings("descriptionLength"), String)
                    Else
                        txtDescriptionLength.Text = "100"
                    End If

                    txtTemplate.Text = Template.ItemTemplate
                    txtHeaderTemplate.Text = Template.HeaderTemplate
                    txtAltItemTemplate.Text = Template.AltItemTemplate
                    txtSeparator.Text = Template.Separator
                    txtFooterTemplate.Text = Template.FooterTemplate

                End If


            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' UpdateSettings saves the modified settings to the Database
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        '''		[cnurse]	10/20/2004	created
        '''		[cnurse]	10/25/2004	upated to use TabModuleId rather than TabId/ModuleId
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overrides Sub UpdateSettings()
            Try
                Dim objModules As New Entities.Modules.ModuleController

                objModules.UpdateTabModuleSetting(TabModuleId, "history", txtHistory.Text)
                'If String.IsNullOrEmpty(txtTemplate.Text.Trim) Then
                '    objModules.DeleteTabModuleSetting(TabModuleId, "template")
                'Else
                '    objModules.UpdateTabModuleSetting(TabModuleId, "template", txtTemplate.Text)

                'End If
                objModules.UpdateModuleSetting(ModuleId, "descriptionLength", txtDescriptionLength.Text)

                Template.ItemTemplate = txtTemplate.Text.Trim
                Template.HeaderTemplate = txtHeaderTemplate.Text
                Template.AltItemTemplate = txtAltItemTemplate.Text
                Template.Separator = txtSeparator.Text
                Template.FooterTemplate = txtFooterTemplate.Text

                Template.UpdateTemplate()

                ModuleController.SynchronizeModule(ModuleId)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

#End Region


    End Class

End Namespace
