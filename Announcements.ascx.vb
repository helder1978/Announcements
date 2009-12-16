'438013701 'Soft drinks
'438013702 'Beer
'438013703 'Beverage Packaging
'438013704 'Dairy Drinks
'438013705 'General

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


Imports System.text
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Reflection
Imports System.Text.RegularExpressions
Imports System.Globalization
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.Collections
Imports System.Web
Imports System.Math
Imports DotNetNuke.Services.Mail

Imports DotNetNuke
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Modules.Actions
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.FileSystem
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Services.Tokens

Imports DotNetNuke.Modules.Store.Catalog
Imports DotNetNuke.Modules.Store.WebControls

Imports DotNetNuke.Entities.Tabs


Namespace DotNetNuke.Modules.Announcements

    Partial Public Class Announcements
        Inherits AnnouncementsBase
        Implements IActionable

#Region "Class Variables"

        Private intNumberOfData As Integer
        Private intNumberPerPage As Integer
        Private intCurrentPageNumber As Integer
        Private strPagingCssClass As String

        'Changed 02012008 by Canadean

        Dim monthTitle As String = ""
        Dim categoryTitle As String = ""

        Dim newsType As Int16 = 0
        Dim curCatId As Integer = -1

        Dim scriptToken As String = "<script type="
        Dim imgToken As String = "<img alt="
        Dim removeAfterHeadingToken As String = "<br/>"

        Dim imageToken As String = "<img src="

        Dim pressReleasesToken2 As String = "PressReleases"
        Dim pressReleasesToken As String = "Press_Releases"
        Dim newsToken2 As String = "IndustryNews"
        Dim newsToken As String = "Industry_News"
        Dim eventsToken2 As String = "CanadeanEvents"
        Dim eventsToken As String = "Canadean_Events"

        Dim tokenseparator As String = "#separator#"
        Dim readToken As String = "Read >"

        Dim showOnlyArchived As String = "Archive"

        Dim section As String = ""

        Dim iPageSize As Integer = 8    'How big our pages are
        Dim iPageCount As Integer       'The number of pages we get back
        Dim iPageCurrent As Integer     'The page we want to show
        'Dim strOrderBy As Integer       'A fake parameter used to illustrate passing them
        'Dim strSQL As Integer           'SQL command to execute
        'Dim objPagingConn As Integer    'The ADODB connection object
        'Dim objPagingRS As Integer      'The ADODB recordset object
        'Dim iRecordsShown As Integer    'Loop controller for displaying just iPageSize records
        Dim I As Integer                'Standard looping var

#End Region

#Region "Private Members"

        Private arrAnnouncements As ArrayList
        Private monthYear As ArrayList
        Public sbPager As StringBuilder

#End Region

#Region "Protected Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' FormatURL formats a URL
        ''' </summary>
        ''' <param name="Link">The link to format</param>
        ''' <returns>The formatted URL</returns>
        ''' <history>
        ''' 	[cnurse]	08/15/2007  Converted to a WAP project for Demo
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Function FormatURL(ByVal Link As String, ByVal TrackClicks As Boolean) As String
            Return Common.Globals.LinkClick(Link, TabId, ModuleId, TrackClicks)
        End Function

        Protected Function GetCurrentCategory() As Integer
            Dim category As Integer = 0
            If curCatId = -1 Then
                If CType(Request.QueryString("cat"), String) <> "" Then
                    category = CInt(Request.QueryString("cat"))
                Else
                    Dim newsId As Integer = GetNewsId()
                    If newsId <> -1 Then
                        Dim objAnnouncements As New AnnouncementsController
                        Dim objAnnouncement As AnnouncementInfo = objAnnouncements.GetAnnouncement(newsId, ModuleId)
                        category = GetCategoryId(objAnnouncement.Title)
                    End If
                End If
                curCatId = category
            Else
                category = curCatId
            End If
            Return category
        End Function

        Private Function GetStoreCategoryId(ByVal categoryId As Integer) As Integer
            Select Case categoryId ' Me.ddCategories.SelectedIndex
                Case 0  'All
                    GetStoreCategoryId = 9      ' All Beverages
                Case 1  'General
                    GetStoreCategoryId = 9      ' All Beverages
                Case 2  'Soft Drinks
                    GetStoreCategoryId = 5
                Case 3  'Beer
                    GetStoreCategoryId = 6
                Case 4  'Beverage Packaging
                    GetStoreCategoryId = 8
                Case 5  'Dairy Drinks
                    GetStoreCategoryId = 7
            End Select
        End Function

        Protected Function GetCategoryName(ByVal categoryId As Integer) As String
            Dim category As String = ""
            Select Case categoryId ' Me.ddCategories.SelectedIndex
                Case 1  'General
                    category = "General"
                Case 2  'Soft Drinks
                    category = "Soft Drinks"
                Case 3  'Beer
                    category = "Beer"
                Case 4  'Beverage Packaging
                    category = "Beverage Packaging"
                Case 5  'Dairy Drinks
                    category = "Dairy Drinks"
            End Select
            Return category
        End Function

        Protected Function GetCategoryId(ByVal title As String) As Integer
            Dim categoryId As Integer = 0
            If title.ToLower().Contains(" - general - ") Then
                categoryId = 1
            End If
            If title.ToLower().Contains(" - soft drinks - ") Or title.ToLower().Contains(" - bottled water - ") Or _
                title.ToLower().Contains(" - carbonated soft drinks - ") Or title.ToLower().Contains(" - juices and nectars - ") Or _
                title.ToLower().Contains(" - still drinks - ") Or title.ToLower().Contains(" - sports drinks - ") Or _
                title.ToLower().Contains(" - energy drinks - ") Or title.ToLower().Contains(" - iced and rtd tea - ") Or _
                title.ToLower().Contains(" - dilutables - ") Or title.ToLower().Contains(" - iced and rtd coffee - ") Then
                categoryId = 2
            End If
            If title.ToLower().Contains(" - beer - ") Or title.ToLower().Contains(" - cider - ") Or _
                title.ToLower().Contains(" - perry - ") Then
                categoryId = 3
            End If
            If title.ToLower().Contains(" - beverage packaging - ") Then
                categoryId = 4
            End If
            If title.ToLower().Contains(" - dairy drinks - ") Then
                categoryId = 5
            End If
            Return categoryId
        End Function

        Protected Function GetUrlForAnotherCategory(ByVal url As String, ByVal newCat As Integer) As String
            If (url.Contains("&title=")) Then
                url = Regex.Replace(url, "&title=.*", "")
            End If

            If (url.Contains("&cat=")) Then
                url = Regex.Replace(url, "cat=\d{1}", "cat=" & newCat)
                'Response.Write("aaaa1")
            Else
                If (url.Contains("?")) Then
                    url = url & "&cat=" & newCat
                Else
                    url = url & "?cat=" & newCat
                End If
                If (Not url.Contains("section=")) Then
                    url &= "&section=" & newsToken
                End If
            End If

            If (url.Contains("&newsId=")) Then
                url = Regex.Replace(url, "&newsId=\d+", "")
            End If

            If (url.Contains("&ItemId=")) Then
                url = Regex.Replace(url, "&ItemId=\d+", "")
            End If

            Return url
        End Function

        Protected Function GetUrlForCurrentCategoryHome() As String
            Dim url As String = Request.Url.ToString()
            If (url.Contains("&newsId=")) Then
                url = Regex.Replace(url, "&newsId=\d+", "")
                'Response.Write("aaaa1")
            End If
            If (url.Contains("&ItemId=")) Then
                url = Regex.Replace(url, "&ItemId=\d+", "")
                'Response.Write("aaaa1")
            End If
            If (url.Contains("&title=")) Then
                url = Regex.Replace(url, "&title=.*", "")
            End If
            If (Not url.Contains("section=")) Then
                url &= "&section=" & returnNewsString(newsType)
            End If
            If (Not url.Contains("cat=")) Then
                url &= "&cat=" & GetCurrentCategory()
            End If

            Return url
        End Function


        Protected Function GetNewsTokenPosition(ByVal url As String) As Integer
            If CShort(url.IndexOf(newsToken, 0)) = -1 Then
                Return CShort(url.IndexOf(newsToken2, 0))
            Else
                Return CShort(url.IndexOf(newsToken, 0))
            End If
        End Function

        Protected Function GetPressReleasesTokenPosition(ByVal url As String) As Integer
            If CShort(url.IndexOf(pressReleasesToken, 0)) = -1 Then
                Return CShort(url.IndexOf(pressReleasesToken2, 0))
            Else
                Return CShort(url.IndexOf(pressReleasesToken, 0))
            End If
        End Function

        Protected Function GetEventsTokenPosition(ByVal url As String) As Integer
            If CShort(url.IndexOf(eventsToken, 0)) = -1 Then
                Return CShort(url.IndexOf(eventsToken2, 0))
            Else
                Return CShort(url.IndexOf(eventsToken, 0))
            End If
        End Function

        Protected Sub BindMenu()
            Dim url As String = Request.Url.ToString()
            hlAll.NavigateUrl = GetUrlForAnotherCategory(url, 0)
            hlGeneral.NavigateUrl = GetUrlForAnotherCategory(url, 1)
            hlSoftDrinks.NavigateUrl = GetUrlForAnotherCategory(url, 2)
            hlBeer.NavigateUrl = GetUrlForAnotherCategory(url, 3)
            hlBeveragePackaging.NavigateUrl = GetUrlForAnotherCategory(url, 4)
            hlDairyDrinks.NavigateUrl = GetUrlForAnotherCategory(url, 5)

            Dim currentCategory As Integer = GetCurrentCategory()
            Select Case currentCategory
                Case 0  'General
                    hlAll.CssClass = "newsMenuOn"
                    CType(Me.Page, DotNetNuke.Framework.CDefault).Title &= " - All"
                Case 1  'General
                    hlGeneral.CssClass = "newsMenuOn"
                    CType(Me.Page, DotNetNuke.Framework.CDefault).Title &= " - General"
                Case 2  'Soft Drinks
                    hlSoftDrinks.CssClass = "newsMenuOn"
                    CType(Me.Page, DotNetNuke.Framework.CDefault).Title &= " - Soft Drinks"
                Case 3  'Beer
                    hlBeer.CssClass = "newsMenuOn"
                    CType(Me.Page, DotNetNuke.Framework.CDefault).Title &= " - Beer"
                Case 4  'Beverage Packaging
                    hlBeveragePackaging.CssClass = "newsMenuOn"
                    CType(Me.Page, DotNetNuke.Framework.CDefault).Title &= " - Beverage Packaging"
                Case 5  'Dairy Drinks
                    hlDairyDrinks.CssClass = "newsMenuOn"
                    CType(Me.Page, DotNetNuke.Framework.CDefault).Title &= " - Dairy Drinks"
            End Select


        End Sub
#End Region

#Region "Protected Methods"
        Protected Sub populateNewsItems(ByVal Category As Integer, ByVal PostBack As Boolean)
            'populate the data for the month/year combobox
            'Clear other data previosly in the control

            Dim objAnnouncements As New AnnouncementsController
            Me.ddMonthYear.Items.Clear()

            'If Me.ddCategories.SelectedIndex <> 0 Then
            Dim currentCategory As Int32 = GetCurrentCategory()
            If currentCategory <> 0 Then
                Select Case currentCategory ' Me.ddCategories.SelectedIndex
                    Case 1  'General
                        monthYear = objAnnouncements.GetCurrentMonthYears(ModuleId, "General")
                    Case 2  'Soft Drinks
                        monthYear = objAnnouncements.GetCurrentMonthYears(ModuleId, "Soft Drinks")
                    Case 3  'Beer
                        monthYear = objAnnouncements.GetCurrentMonthYears(ModuleId, "Beer")
                    Case 4  'Beverage Packaging
                        monthYear = objAnnouncements.GetCurrentMonthYears(ModuleId, "Beverage Packaging")
                    Case 5  'Dairy Drinks
                        monthYear = objAnnouncements.GetCurrentMonthYears(ModuleId, "Dairy Drinks")
                End Select
            Else
                monthYear = objAnnouncements.GetCurrentMonthYears(ModuleId, "")
            End If

            Me.ddMonthYear.Items.Add(New ListItem("Please Select a Month", CStr(0)))

            If monthYear.Count > 1 Then
                Me.ddMonthYear.Enabled = True

                For Each monthYearTag As PopulatedDatesInfo In monthYear
                    Dim li As ListItem = New ListItem(String.Concat(GetMonthName(monthYearTag.Month), " ", monthYearTag.Year), CStr(monthYearTag.KeyId))
                    Me.ddMonthYear.Items.Add(li)
                Next

            Else
                Me.ddMonthYear.Enabled = False
            End If

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'check if previously checked items

            If (PostBack) Then
                categoryTitle = GetCategoryName(GetCurrentCategory())
                'If CType(Request.QueryString("cat"), String) <> "" Then
                '    Me.ddCategories.SelectedIndex = CInt(Request.QueryString("cat"))
                '    categoryTitle = Me.ddCategories.SelectedItem.Text
                'Else
                '    Me.ddCategories.SelectedIndex = 0
                'End If

                If CType(Request.QueryString("dat"), String) <> "" Then
                    Me.ddMonthYear.SelectedIndex = CInt(Request.QueryString("dat"))
                    monthTitle = Me.ddMonthYear.SelectedItem.Text
                Else
                    Me.ddMonthYear.SelectedIndex = 0
                End If

            End If

            'changeTitle(categoryTitle, monthTitle)

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        End Sub


        Protected Sub populatePressReleasesItems(ByVal PostBack As Boolean)

            'place the correct header in the label
            If (Not PostBack) Then
                If CType(Request.QueryString("cat"), String) <> "" Then
                    Me.ddlPressReleases.SelectedIndex = CInt(Request.QueryString("cat"))
                End If
            End If

            If (Me.ddlPressReleases.SelectedIndex < 1) Then
                Me.lblCanadeanPressReleases.Text = "All Press Releases"
            Else
                Me.lblCanadeanPressReleases.Text = Me.ddlPressReleases.SelectedItem.Text.ToString()
            End If

        End Sub

        Protected Sub populateCanadeanEventsItems()
            'place the correct header in the label

        End Sub


        Protected Sub bindData()
            Dim year As String = ""
            Dim month As String = ""

            Dim output As New StringBuilder
            Dim pagingOutput As New StringBuilder

            Dim announcements As New AnnouncementsController
            Dim newsId As Integer = -1

            'If (Me.ddCategories.SelectedIndex > 0) Then
            '    categoryTitle = Me.ddCategories.SelectedItem.Text
            If GetCurrentCategory() > 0 Then
                categoryTitle = GetCategoryName(GetCurrentCategory())
            Else
                categoryTitle = ""
            End If

            If (Me.ddMonthYear.SelectedIndex > 0) Then
                monthTitle = Me.ddMonthYear.SelectedItem.Text
            Else
                monthTitle = ""
            End If

            ' changeTitle(categoryTitle, monthTitle)


            ' bind data
            Dim datStartDate As Date = Null.NullDate
            Dim datEndDate As Date = Null.NullDate

            Dim url As String = Request.Url.ToString()

            Dim categoryBefore As Int32 = -1


            If CType(Request.QueryString("cat"), String) <> "" Then
                categoryBefore = CInt(Request.QueryString("cat"))
            Else
                categoryBefore = -1
            End If


            'Setup variables
            If CType(Request.QueryString("page"), String) <> "" Then
                iPageCurrent = CInt(Request.QueryString("page"))
            Else
                iPageCurrent = 1
            End If

            'If CType(Request.QueryString("newsId"), String) <> "" Then
            '    newsId = CInt(Request.QueryString("newsId"))
            'Else
            '    newsId = -1
            'End If
            newsId = GetNewsId()

            'Final changes

            If CType(Settings("history"), String) <> "" Then
                datStartDate = DateAdd(DateInterval.Day, -(CType(Settings("history"), Double)), Now)
            End If


            Select Case newsType
                Case 1
                    'News
                    If Me.ddMonthYear.SelectedIndex > 0 Then

                        Dim arrayDate As Array = Me.ddMonthYear.SelectedValue.ToCharArray()
                        Dim iterator As Integer = 0

                        For Each s As Char In arrayDate
                            If iterator < 4 Then    'first four digits are of the year
                                year = String.Concat(year, s)
                            Else
                                month = String.Concat(month, s)
                            End If
                            iterator += 1
                        Next

                        datStartDate = New DateTime(CInt(year), CInt(month), 1, 0, 0, 0)
                        datEndDate = New DateTime(CInt(year), CInt(month) + 1, 1, 0, 0, 0)
                    Else
                        'to get only until today
                        datEndDate = Date.Now
                    End If
                Case 2
                    'Press Release

                Case 3
                    'Events
            End Select

            'check if in default url with no parameters
            'Dim urlNew As String = Request.RawUrl.ToString()
            'Dim checkNoParameters As Int32 = 0
            'checkNoParameters = urlNew.IndexOf("&", 0)

            'archived should only work in events & press-release
            Dim urlArchived As String = Request.RawUrl.ToString()
            Dim checkArquivedStatement As Int32 = 0
            checkArquivedStatement = CShort(urlArchived.IndexOf(showOnlyArchived, 0))

            Dim typeSelected As Integer = 0
            'get items by Category in case of canadean news or canadean press releases

            Select Case newsType
                Case 1
                    'canadean industry news
                    'typeSelected = Me.ddCategories.SelectedIndex
                    typeSelected = GetCurrentCategory()
                Case 2
                    'canadean press releases
                    typeSelected = Me.ddlPressReleases.SelectedIndex
                Case 3
                    'canadean events
            End Select


            ' don't understand why this exists...???
            'Dim getSessionValue As Boolean = False
            'getSessionValue = CBool(Session("inline"))


            'If (getSessionValue) Then
            'Session("inline") = False
            '    If (url.Contains("&")) Then
            '      If (categoryBefore <> typeSelected) Then
            '            Response.Redirect(String.Concat(url.Substring(0, url.IndexOf("&")), "&section=", section, "&cat=", typeSelected))
            '       Else
            '            Response.Redirect(String.Concat(url.Substring(0, url.IndexOf("&")), "&section=", section, "&cat=", typeSelected, "&dat=", Me.ddMonthYear.SelectedIndex))
            '        End If
            'End If
            'End If

            ' get announcements, if isEditable decide based on dropdownlist selection, otherwise just get current items
            If checkArquivedStatement > 0 Then
                'show only the archived items
                If (ddlViewType.Visible) Then
                    ddlViewType.SelectedIndex = 1
                End If

                'which category
                Select Case typeSelected
                    Case 0
                        arrAnnouncements = announcements.GetExpiredAnnouncements(ModuleId)
                    Case 1  'General
                        arrAnnouncements = announcements.GetExpiredAnnouncements(ModuleId, "General")
                    Case 2  'Soft Drinks
                        arrAnnouncements = announcements.GetExpiredAnnouncements(ModuleId, "Soft Drinks")
                    Case 3  'Beer
                        arrAnnouncements = announcements.GetExpiredAnnouncements(ModuleId, "Beer")
                    Case 4  'Beverage Packaging
                        arrAnnouncements = announcements.GetExpiredAnnouncements(ModuleId, "Beverage Packaging")
                    Case 5  'Dairy Drinks
                        arrAnnouncements = announcements.GetExpiredAnnouncements(ModuleId, "Dairy Drinks")
                End Select
            Else
                Select Case typeSelected
                    Case 0
                        arrAnnouncements = announcements.GetCurrentAnnouncements(ModuleId, "", datStartDate, datEndDate)
                    Case 1  'General
                        arrAnnouncements = announcements.GetCurrentAnnouncements(ModuleId, "General", datStartDate, datEndDate)
                    Case 2  'Soft Drinks
                        arrAnnouncements = announcements.GetCurrentAnnouncements(ModuleId, "Soft Drinks", datStartDate, datEndDate)
                    Case 3  'Beer
                        arrAnnouncements = announcements.GetCurrentAnnouncements(ModuleId, "Beer", datStartDate, datEndDate)
                    Case 4  'Beverage Packaging
                        arrAnnouncements = announcements.GetCurrentAnnouncements(ModuleId, "Beverage Packaging", datStartDate, datEndDate)
                    Case 5  'Dairy Drinks
                        arrAnnouncements = announcements.GetCurrentAnnouncements(ModuleId, "Dairy Drinks", datStartDate, datEndDate)
                End Select
            End If

            'If (newsId < 0 Or Not urlNew.Contains("&")) Then
            If (newsId < 0) Then
                'Response.Write("Outside inline mode: " & newsId)

                Session("inline") = False

                'it isn't in inline mode
                Dim c As Double = Ceiling(arrAnnouncements.Count / iPageSize)
                iPageCount = CInt(c)

                If iPageCurrent > iPageCount Then iPageCurrent = iPageCount

                If iPageCount < 1 Then iPageCurrent = 1

                'Final Changes
                Dim dnnTokenReplace As Services.Tokens.TokenReplace = Nothing
                If Template.TokenReplaceNeeded Then
                    dnnTokenReplace = New Services.Tokens.TokenReplace(Scope.DefaultSettings, CultureInfo.CurrentCulture.Name, PortalSettings, UserInfo)
                End If

                Dim counter As Integer


                Dim newCounter As Integer = iPageCurrent * iPageSize - iPageSize

                Dim J As Integer = 0

                Dim F As Int16 = 0

                I = 0

                'check if we have any item with 

                If (arrAnnouncements.Count <= 0) Then
                    Dim altItemTemplateAvailable As Boolean = Not String.IsNullOrEmpty(Template.AltItemTemplate)
                    If dnnTokenReplace IsNot Nothing Then
                        output.Append(dnnTokenReplace.ReplaceEnvironmentTokens(Template.HeaderTemplate))
                    End If

                    output.Replace("[addnew]", "")

                End If

                'If CType(Request.QueryString("section"), String) <> "" Then
                '    'need to place in the url the correct parameters
                '    url = String.Concat(url.Substring(0, url.IndexOf("&")) + "&" + "section=" + returnNewsString(newsType))
                'Else
                '    url = String.Concat(url + "&" + "section=" + returnNewsString(newsType))

                '    'Else
                '    '    If (url.Contains("&")) Then
                '    '        url = String.Concat(String.Concat((url.Substring(0, url.IndexOf("&")) + "&" + "section=" + returnNewsString(newsType))))
                '    '    End If
                'End If

                If (url.Contains("&")) Then
                    'need to place in the url the correct parameters
                    url = String.Concat(url.Substring(0, url.IndexOf("&")) + "&" + "section=" + returnNewsString(newsType))
                Else
                    url = String.Concat(url + "&" + "section=" + returnNewsString(newsType))
                End If


                For Each announcement As AnnouncementInfo In arrAnnouncements

                    If Not CType(announcement.Url.ToString(), String) <> "" Then
                        'announcement.Url = url & "&" & "newsId=" & announcement.ItemId.ToString() & "&" & "cat=" & GetCurrentCategory()
                        'announcement.Url = url & "&" & "newsId=" & announcement.ItemId.ToString() & "&" & "cat=" & GetCategoryId(announcement.Title)
                        announcement.Url = url & "&" & "newsId=" & announcement.ItemId.ToString() & "&" & "cat=" & GetCategoryId(announcement.Title) & "&" & "title=" & GetNewsFriendlyTitle(announcement.Title)
                    End If

                    'Response.Write("<br>" & announcement.Summary)

                    If announcement.Description.Contains(tokenseparator) Then
                        announcement.Description = announcement.Description.Remove(announcement.Description.IndexOf(tokenseparator))
                    Else
                        Try
                            If announcement.Summary <> "" Then
                                announcement.Description = announcement.Summary
                            Else

                                Dim checkAfterHeadingToken As Integer = CShort(announcement.Description.IndexOf(removeAfterHeadingToken, 0))
                                If (checkAfterHeadingToken > 0) Then
                                    announcement.Description = announcement.Description.Substring(0, checkAfterHeadingToken)
                                End If
                            End If
                        Catch ex As Exception

                        End Try
                    End If

                    announcement.IsEditable = Me.IsEditable

                    'Create a Token Replace and replace the tokens for this template
                    Dim tokenReplace As New AnnouncementsTokenReplace(announcement)

                    If (F < 1) Then
                        F = CShort(F + 1)
                        output.Append(tokenReplace.ReplaceAnnouncmentTokens(Template.HeaderTemplate))
                    End If

                    If ((I < iPageSize) And (J >= newCounter)) Then
                        'If (counter Mod 2 = 0) Or (Not altItemTemplateAvailable) Then
                        output.Append(tokenReplace.ReplaceAnnouncmentTokens(Template.ItemTemplate))
                        'Else
                        '   output.Append(tokenReplace.ReplaceAnnouncmentTokens(Template.AltItemTemplate))
                        'End If

                        If (dnnTokenReplace IsNot Nothing) AndAlso (counter < arrAnnouncements.Count - 1) Then
                            output.Append(dnnTokenReplace.ReplaceEnvironmentTokens(Template.Separator))
                        End If

                        counter += 1
                        I += 1
                    Else
                        If (I > iPageSize) Then
                            Exit For
                        End If
                    End If
                    J += 1
                Next

                If dnnTokenReplace IsNot Nothing Then
                    output.Append(dnnTokenReplace.ReplaceEnvironmentTokens(Template.FooterTemplate))
                End If

                Me.litAnnouncements.Text = output.ToString

                Dim paramPlace As Int32 = 0

                Try
                    paramPlace = CShort(url.IndexOf("&", 0))
                    If (paramPlace > 0) Then
                        url = url.Substring(0, paramPlace)
                    End If
                Catch ex As Exception

                End Try


                Dim catOption As String = ""
                Dim datOption As String = ""
                catOption = String.Concat("&cat=" & typeSelected)

                'the cat option should only appear if we are in the industry news section
                If (newsType = 1) Then
                    'canadean industry news
                    'Response.Write("CNN")
                    datOption = String.Concat("&dat=" & Me.ddMonthYear.SelectedIndex.ToString)
                End If

                'Changed by mpedroto
                'Previous
                If iPageCurrent > 1 Then
                    pagingOutput.Append(String.Concat("<a href=", url, "&section=" & section, catOption, datOption, "&page=" & 1, ">«</a>"))
                    pagingOutput.Append("&nbsp;")
                    pagingOutput.Append(String.Concat("<a href=", url, "&section=" & section, catOption, datOption, "&page=" & iPageCurrent - 1, ">Prev</a>"))
                End If

                pagingOutput.Append("&nbsp;")
                J = 0

                I = 0

                If iPageCount > iPageSize Then
                    'The are enough pages for upper and lower bound
                    If CBool((iPageCurrent - 4 >= 0) And (iPageCurrent + 4 <= iPageCount)) Then
                        'starts at the lower bound
                        I = iPageCurrent - 4
                        J = 0

                        While J < iPageSize
                            I += 1
                            J += 1
                            If (I = iPageCurrent) Then
                                pagingOutput.Append(String.Concat(I))
                                pagingOutput.Append("&nbsp;")
                            Else
                                pagingOutput.Append(String.Concat("<a href=", url, "&section=" & section, catOption, datOption, "&page=" & I, ">", I, "</a>"))
                                pagingOutput.Append("&nbsp;")
                            End If


                        End While
                    Else
                        If CBool((iPageCurrent - 4 <= 0) And (iPageCurrent + 4 <= iPageCount)) Then
                            I = 0
                            J = 0

                            'Response.Write("7")
                            While J < iPageSize
                                I += 1
                                J += 1
                                If (I = iPageCurrent) Then
                                    pagingOutput.Append(String.Concat(I))
                                    pagingOutput.Append("&nbsp;")
                                Else
                                    pagingOutput.Append(String.Concat("<a href=", url, "&section=" & section, catOption, datOption, "&page=" & I, ">", I, "</a>"))
                                    pagingOutput.Append("&nbsp;")
                                End If

                            End While

                        Else
                            'No upper bound enough pages
                            J = 0

                            I = iPageCount - iPageSize + (iPageCount - iPageCurrent)
                            While J < iPageSize
                                J += 1
                                I += 1
                                If (I = iPageCurrent) Then
                                    pagingOutput.Append(String.Concat(I))
                                    pagingOutput.Append("&nbsp;")
                                Else
                                    pagingOutput.Append(String.Concat("<a href=", url, "&section=" & section, catOption, datOption, "&page=" & I, ">", I, "</a>"))
                                    pagingOutput.Append("&nbsp;")
                                End If

                            End While
                        End If
                    End If
                Else
                    'Not 8 pages to start with
                    I = 0
                    While I < iPageCount
                        I += 1
                        If (I = iPageCurrent) Then
                            pagingOutput.Append(String.Concat(I))
                            pagingOutput.Append("&nbsp;")
                        Else
                            pagingOutput.Append(String.Concat("<a href=", url, "&section=" & section, catOption, datOption, "&page=" & I, ">", I, "</a>"))
                            pagingOutput.Append("&nbsp;")
                        End If
                    End While
                End If

                If iPageCurrent < iPageCount Then
                    pagingOutput.Append(String.Concat("<a href=", url, "&section=" & section, catOption, datOption, "&page=" & iPageCurrent + 1, ">Next</a>"))
                    pagingOutput.Append("&nbsp;")
                    pagingOutput.Append(String.Concat("<a href=", url, "&section=" & section, catOption, datOption, "&page=" & iPageCount, ">»</a>"))
                End If

                Me.paging.Text = pagingOutput.ToString()
            Else    ' inline mode: news detail page

                'Response.Write("Inline mode")
                'Response.Write("<br>newsId: {" & newsId & "}")

                trFilterMonth.Visible = False

                'If (newsId > 0) Then
                '    Response.Redirect(String.Concat(url.Substring(0, url.IndexOf("&")), "&section=", section, "&cat=", typeSelected))
                'End If

                Session("inline") = True

                Dim objAnnouncements As New AnnouncementsController
                Dim objAnnouncement As AnnouncementInfo = objAnnouncements.GetAnnouncement(newsId, ModuleId)

                'Got the announcement
                'Place it in the output
                If Not objAnnouncement Is Nothing Then
                    'Response.Write("<br>" & objAnnouncement.Summary)

                    If objAnnouncement.Description.Contains(tokenseparator) Then
                        objAnnouncement.Description = objAnnouncement.Description.Replace(tokenseparator, " ")
                    Else
                        'Dim checkpressReleasesTitle As Int32 = 0
                        'Dim checkNewsTitle As Int32 = 0

                        'checkNewsTitle = url.IndexOf(newsToken, 0)
                        'checkNewsTitle = GetNewsTokenPosition(url)

                        'Response.Write("<br>url2: " & url)
                        'Response.Write("<br>checkNews2: " & checkNewsTitle)

                        'checkpressReleasesTitle = CShort(url.IndexOf(pressReleasesToken, 0))
                        'checkpressReleasesTitle = GetPressReleasesTokenPosition(url)

                        'for industry news
                        'IndustryNews
                        'If (checkNewsTitle > 0) Then
                        If (newsType = 1) Then  ' Industry News
                            Try
                                Me.ddMonthYear.Enabled = False

                                ' Change the html tags for SEO purposes
                                'CType(Me.Page, DotNetNuke.Framework.CDefault).Title &= " - " & objAnnouncement.Title
                                CType(Me.Page, DotNetNuke.Framework.CDefault).Title = "News: " & objAnnouncement.Title
                                'CType(Me.Page, DotNetNuke.Framework.CDefault).Description = GetStringPreview(objAnnouncement.Description)
                                If objAnnouncement.Summary <> "" Then
                                    CType(Me.Page, DotNetNuke.Framework.CDefault).Description = GetStringPreview(objAnnouncement.Summary)
                                Else
                                    CType(Me.Page, DotNetNuke.Framework.CDefault).Description = GetStringPreview(objAnnouncement.Description)
                                End If

                                loadRelatedProducts(objAnnouncement)

                                ' Show send to friend
                                tableSendToFriend.Visible = True

                                Dim checkScriptToken As Int32 = 0
                                checkScriptToken = objAnnouncement.Description.LastIndexOf(scriptToken, objAnnouncement.Description.Length)
                                If (checkScriptToken > 0) Then
                                    objAnnouncement.Description = objAnnouncement.Description.Substring(0, checkScriptToken)
                                End If

                                checkScriptToken = 0

                                checkScriptToken = objAnnouncement.Description.LastIndexOf(imgToken, objAnnouncement.Description.Length)
                                If (checkScriptToken > 0) Then
                                    objAnnouncement.Description = objAnnouncement.Description.Substring(0, checkScriptToken)
                                End If
                                'Response.Write("<br>before: {" & output.ToString & "}")
                                'Response.Write("<br>description: {" & objAnnouncement.Description & "}")

                                Dim tokenReplace As New AnnouncementsTokenReplace(objAnnouncement)
                                output.Append(tokenReplace.ReplaceAnnouncmentTokens(Template.AltItemTemplate))

                                output = output.Replace(readToken, "")

                                'Response.Write("<br>after: {" & output.ToString & "}")

                                Dim linkCurrentCategory As String = "<br /><div align='right'><a class='linkmore' href='" & GetUrlForCurrentCategoryHome() & "'>Read related stories &gt;</a></div>"
                                output.Append(linkCurrentCategory)

                            Catch ex As Exception
                                'Response.Redirect(NavigateURL(184), True)
                                Response.Write("<!--")
                                Response.Write(ex.InnerException)
                                Response.Write("<br>" & ex.Message)
                                Response.Write("<br>" & ex.ToString())
                                Response.Write("-->")

                            End Try
                        Else
                            'for press releases
                            'PressReleases

                            'If (checkpressReleasesTitle > 0) Then
                            If (newsType = 2) Then  ' Press Releases

                                ' Change the html tags for SEO purposes
                                'CType(Me.Page, DotNetNuke.Framework.CDefault).Title &= " - " & objAnnouncement.Title
                                CType(Me.Page, DotNetNuke.Framework.CDefault).Title = "News: " & objAnnouncement.Title
                                CType(Me.Page, DotNetNuke.Framework.CDefault).Description = GetStringPreview(objAnnouncement.Description)

                                Dim tokenReplace As New AnnouncementsTokenReplace(objAnnouncement)
                                output.Append(tokenReplace.ReplaceAnnouncmentTokens(Template.AltItemTemplate))
                            End If

                        End If
                    End If


                End If

                Me.litAnnouncements.Text = output.ToString

            End If
        End Sub

        Protected Function GetNewsFriendlyTitle(ByVal newsTitle As String) As String
            Dim separator As String = "_"
            newsTitle = newsTitle.Replace("«", "")
            newsTitle = newsTitle.Replace("»", "")
            newsTitle = newsTitle.Replace("'", "")
            newsTitle = newsTitle.Replace("""", "")
            newsTitle = newsTitle.Replace("\", " ")
            newsTitle = newsTitle.Replace("/", " ")
            newsTitle = newsTitle.Replace("(", " ")
            newsTitle = newsTitle.Replace(")", " ")
            newsTitle = newsTitle.Replace("[", " ")
            newsTitle = newsTitle.Replace("]", " ")
            newsTitle = newsTitle.Replace("  ", " ")
            newsTitle = newsTitle.Replace(" - ", separator)
            newsTitle = newsTitle.Replace(" ", separator)
            newsTitle = newsTitle.Replace("&", separator)
            newsTitle = newsTitle.Replace("=", separator)
            newsTitle = newsTitle.Replace("?", separator)
            newsTitle = newsTitle.Replace("!", separator)
            newsTitle = newsTitle.Replace("#", separator)
            newsTitle = newsTitle.Replace("+", separator)
            newsTitle = newsTitle.Replace(".", separator)
            newsTitle = newsTitle.Replace(":", separator)
            newsTitle = newsTitle.Replace(",", separator)
            newsTitle = newsTitle.Replace(";", separator)
            newsTitle = Server.UrlEncode(newsTitle)
            If (newsTitle.Length > 50) Then
                newsTitle = newsTitle.Substring(0, 50)
            End If
            Return newsTitle
        End Function

#End Region

#Region "UTILS METHODS"

        Function returnNewsString(ByVal newsType As Integer) As String
            Select Case newsType
                'Case 1 : Return "IndustryNews"
                'Case 2 : Return "PressReleases"
                'Case 3 : Return "CanadeanEvents"
                Case 1 : Return "Industry_News"
                Case 2 : Return "Press_Releases"
                Case 3 : Return "Canadean_Events"
            End Select
            Return ""
        End Function

        Function DaysInMonth(ByVal Month As Integer, ByVal Year As Integer) As Integer
            DaysInMonth = CInt(DateDiff("d", DateSerial(Year, Month, 1), DateSerial(Year, Month + 1, 1)))
        End Function

        Protected Function GetMonthName(ByVal month As Integer) As String
            Dim monthString As String = ""
            Select Case month
                Case 1
                    monthString = "January"
                Case 2
                    monthString = "February"
                Case 3
                    monthString = "March"
                Case 4
                    monthString = "April"
                Case 5
                    monthString = "May"
                Case 6
                    monthString = "June"
                Case 7
                    monthString = "July"
                Case 8
                    monthString = "August"
                Case 9
                    monthString = "September"
                Case 10
                    monthString = "October"
                Case 11
                    monthString = "November"
                Case 12
                    monthString = "December"
            End Select

            Return monthString

        End Function

        Protected Sub changeTitle(ByVal Category As String, ByVal Month As String)

            Dim title As String

            If (CType(Category, String) <> "") Then
                title = Category + " - "
            Else
                title = "All Categories - "
            End If

            If (CType(Month, String) <> "") Then
                title += Month
            Else
                title += "All Dates"
            End If

            'Me.lblTitle.Text = title

        End Sub

        Protected Sub getPanelToShow()

            Dim url As String = Request.RawUrl.ToString()

            'Response.Write("<br>url: " & url)

            Dim checkNews As Int32 = 0
            Dim checkpressReleases As Int32 = 0

            'checkNews = CShort(url.IndexOf(newsToken, 0))
            checkNews = GetNewsTokenPosition(url)

            'Response.Write("<br>checkNews: " & checkNews)
            'Response.Write("<br>newstoke1: " & CShort(url.IndexOf(newsToken, 0)))
            'Response.Write("<br>newstoke2: " & CShort(url.IndexOf(newsToken2, 0)))

            'checkpressReleases = CShort(url.IndexOf(pressReleasesToken, 0))
            checkpressReleases = GetPressReleasesTokenPosition(url)
            'Response.Write("<br>checkpressReleases: " & checkpressReleases)

            If (checkNews > 0) Then
                newsType = 1
                Me.canadeanNews.Visible = True
                Me.canadeanPressReleases.Visible = False
                Me.canadeanEvents.Visible = False
                section = newsToken
            Else
                If (checkpressReleases > 0) Then
                    newsType = 2
                    Me.canadeanNews.Visible = False
                    Me.canadeanPressReleases.Visible = True
                    Me.canadeanEvents.Visible = False
                    section = pressReleasesToken
                Else
                    newsType = 3
                    Me.canadeanNews.Visible = False
                    Me.canadeanPressReleases.Visible = False
                    Me.canadeanEvents.Visible = True
                    section = eventsToken
                End If

            End If

            'Response.Write("<br>section: " & section)


        End Sub


#End Region

#Region "Event Handlers"

        Sub Index_Changed(ByVal sender As Object, ByVal e As EventArgs)

            'Response.Write("Categoria diferente do que e seleccionado?")

            If (Me.ddMonthYear.SelectedIndex > 0) Then
                Me.ddMonthYear.Items.Clear()
                'populateNewsItems(Me.ddCategories.SelectedIndex, False)
                populateNewsItems(GetCurrentCategory(), False)

                'If (Me.ddCategories.SelectedIndex > 0) Then
                '    categoryTitle = Me.ddCategories.SelectedItem.Text
                If (GetCurrentCategory() > 0) Then
                    categoryTitle = GetCategoryName(GetCurrentCategory())
                Else
                    categoryTitle = ""
                End If

                monthTitle = ""
                ' changeTitle(categoryTitle, monthTitle)
            End If

            Me.ddMonthYear.SelectedIndex = 0
            bindData()

        End Sub


        Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

        End Sub
        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_Load runs when the control is loaded
        ''' </summary>
        ''' <history>
        ''' 	[cnurse]	08/15/2007  Converted to a WAP project for Demo
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try

                getPanelToShow()

                If (newsType = 1) Then BindMenu()

                'Response.Write("NEWS TYPE: ")
                'Response.Write(newsType)

                Dim objTab As New DotNetNuke.Entities.Tabs.TabInfo
                objTab = Me.PortalSettings.ActiveTab

                If Not Page.IsPostBack Then
                    'check if there is an item for every category
                    Me.viewtypeSelector.Visible = Me.IsEditable
                    If IsEditable Then
                        If Template.ItemTemplate.ToLower.IndexOf("[edit]") = -1 Then
                            Skins.Skin.AddModuleMessage(Me, Localization.GetString("NoEditToken.Error", LocalResourceFile), Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
                        End If
                        Dim i As [Enum]
                        For Each i In [Enum].GetValues(GetType(ViewTypes))
                            ddlViewType.Items.Add(New ListItem(Localization.GetString([Enum].GetName(GetType(ViewTypes), i), LocalResourceFile), [Enum].GetName(GetType(ViewTypes), i)))
                        Next i
                        Me.ddlViewType.SelectedValue = [Enum].GetName(GetType(ViewTypes), ViewTypes.Current)
                    End If

                    Select Case newsType
                        Case 1
                            Me.ddMonthYear.Items.Clear()
                            'populateNewsItems(Me.ddCategories.SelectedIndex, True)
                            populateNewsItems(GetCurrentCategory(), True)
                        Case 2
                            populatePressReleasesItems(Page.IsPostBack)
                        Case 3
                            populateCanadeanEventsItems()
                    End Select

                Else

                    Session("inline") = True

                    Select Case newsType
                        Case 1
                            'If Me.ddCategories.SelectedIndex <> 0 And Me.ddMonthYear.SelectedIndex <= 0 Then
                            If GetCurrentCategory() <> 0 And Me.ddMonthYear.SelectedIndex <= 0 Then
                                Me.ddMonthYear.Enabled = True
                                'populateNewsItems(Me.ddCategories.SelectedIndex, False)
                                populateNewsItems(GetCurrentCategory(), False)
                            End If
                        Case 2
                            populatePressReleasesItems(Page.IsPostBack)
                        Case 3
                            populateCanadeanEventsItems()
                    End Select

                End If
                bindData()

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Function RemoveHTML(ByVal strText As String) As String
            'Dim RegEx As Regex
            'RegEx = New Regex("<[^>]*>")
            'RegEx.Global = True
            'strText = Replace(LCase(strText), "<br>", Chr(10))
            'RemoveHTML = RegEx.Replace(strText, "")
            RemoveHTML = System.Text.RegularExpressions.Regex.Replace(Server.HtmlDecode(strText), "<[^>]*>", "")
        End Function

        Private Function GetStringPreview(ByVal strText As String) As String
            strText = RemoveHTML(strText)
            Dim posSpace As Integer = 0
            If (strText.Length > 160) Then
                posSpace = strText.Substring(160).IndexOf(" ")
                If (posSpace >= 0) Then
                    strText = strText.Substring(0, 160 + posSpace)
                End If
            End If
            GetStringPreview = strText
        End Function

        Private Sub loadRelatedProducts(ByVal objAnnouncement As AnnouncementInfo)

            Try
                Dim categoryID1 As Integer = GetStoreCategoryId(GetCurrentCategory())
                Dim categoryID3 As Integer = -1
                Dim productController As ProductController = New ProductController()
                'Dim productList As ArrayList = New ArrayList()
                Dim product As ProductInfo

                phDE.Visible = True
                phReport.Visible = True
                phWisdom.Visible = True

                Dim lh As String = objAnnouncement.LongHeading
                If lh IsNot Nothing Then
                    litDebug.Text = "<!-- Category: " & categoryID1 & " NewsTagging: " & objAnnouncement.LongHeading & "-->"

                    Dim pos2 As Integer = lh.IndexOf(":")
                    Dim posSp As Integer = lh.IndexOf(" ")
                    litDebug.Text &= "<!-- Pos2: " & pos2.ToString() & " posSp: " & posSp & " -->"
                    If pos2 = -1 Then pos2 = posSp
                    If posSp = -1 Then posSp = pos2

                    Dim pos As Integer = Min(pos2, posSp)
                    If pos > 0 Then
                        Dim catId3Str As String = lh.Substring(0, pos)
                        litDebug.Text &= "<!-- catId3Str = {" & catId3Str & "} -->"
                        If IsNumeric(catId3Str) Then
                            categoryID3 = Integer.Parse(catId3Str)

                            'phDE.Visible = True
                            'phReport.Visible = True
                            'phWisdom.Visible = True

                            ' Convert product id's to use on the reports catalog
                            categoryID3 = ConvertShopCat_DE2Report(categoryID3)
                            'If categoryID3 = 170 Then    ' Convert into USA (90), United States of America (170)
                            '    categoryID3 = 90
                            'End If
                            'If categoryID3 = 451 Then    ' Convert into United Kingdom (89), United Kingdom (National Research) - 451
                            '    categoryID3 = 89
                            'End If
                            'If categoryID3 = 169 Then    ' Convert into Ireland (59), Republic of Ireland (169)
                            '    categoryID3 = 59
                            'End If

                            ' categoryID1 => Type (Soft Drinks, ...)
                            ' categoryID2 => Continent
                            ' categoryID3 => Country
                            Dim relatedProds As ArrayList = productController.GetRelatedNewsProducts(-1, categoryID1, -1, categoryID3, -1, False)
                            If relatedProds.Count = 0 Then ' If there isn't a product that matches both subcategories
                                litDebug.Text &= "<!-- Couldn't match both categories, trying by type only: " & categoryID1 & "-->"
                                relatedProds = productController.GetRelatedNewsProducts(-1, categoryID1, -1, -1, -1, False) ' try to match only the categoryid1 (type)
                                If relatedProds.Count = 0 Then
                                    litDebug.Text &= "<!-- Trying by country only: " & categoryID3 & "-->"
                                    relatedProds = productController.GetRelatedNewsProducts(-1, -1, -1, categoryID3, -1, False) ' try to match only the categoryid3 (country)
                                End If
                            End If

                            ' Related products
                            For Each product In relatedProds
                                Dim url As String = FixHyperLinkProduct(product.ProductID & "", product.CategoryID1 & "", product.CategoryID2 & "", product.CategoryID3 & "", product.ProductTitle)

                                hlReport.Text = product.ModelName
                                hlReport.NavigateUrl = url ' FixHyperLink(product.ProductID & "")

                                hlBuyNow.Text = Decimal.ToInt32(product.UnitCost) & "£"
                                hlBuyNow.Attributes("title") = Decimal.ToInt32(product.UnitCost) & "£"
                                hlBuyNow.NavigateUrl = url ' FixHyperLink(product.ProductID & "")
                                Exit For
                            Next

                            ' Related Data Extracts 
                            Dim categoryController As CategoryController = New CategoryController()
                            Dim categoryInfo As CategoryInfo = categoryController.GetCategory(categoryID3)
                            'Response.Write(categoryInfo.CategoryName)

                            ' Convert product id's to use on the data extracts
                            categoryID3 = ConvertShopCat_Report2DE(categoryID3)
                            'If categoryID3 = 90 Then    ' Convert USA (90) into United States of America (170)
                            '    categoryID3 = 170
                            'End If
                            'If categoryID3 = 89 Then    ' Convert United Kingdom (89) into United Kingdom (National Research) - 451
                            '    categoryID3 = 451
                            'End If
                            'If categoryID3 = 59 Then    ' Convert Ireland (59) into Republic of Ireland (169)
                            '    categoryID3 = 169
                            'End If

                            Dim subCategories As ArrayList = categoryController.GetCategoriesFromProducts(0, 2, categoryID3, -1, 3)
                            Dim relatedDEs As ArrayList = New ArrayList()
                            Dim relatedDEIds As ArrayList = New ArrayList()
                            For Each cur_category As CategoryInfo In subCategories
                                'Response.Write("<br>" & cur_category.CategoryName)
                                relatedDEs.Add(cur_category.CategoryName)
                                relatedDEIds.Add(cur_category.CategoryID)
                            Next

                            Dim associatedDEs As ArrayList = New ArrayList()
                            associatedDEs = GetAssociatedDataExtract(objAnnouncement.Title, relatedDEs, relatedDEIds)
                            For Each cur_category As Integer In associatedDEs
                                'Response.Write("<br>id:" & cur_category)
                                hlDataExtract.Text = categoryInfo.CategoryName & " - " & categoryController.GetCategory(cur_category).CategoryName

                                'Session("selectedDE") = categoryID3 & ":" & cur_category
                                'Session("selectedDECost") = New Decimal(50)
                            Next
                            If hlDataExtract.Text = "" Then
                                hlDataExtract.Text = "Annual consumption volumes by country and category"
                                '    Session("selectedDE") = ""
                                '    Session("selectedDECost") = New Decimal(0)
                            End If
                        End If
                    Else
                        'phDE.Visible = True
                        'phReport.Visible = True
                        'phWisdom.Visible = True

                        'litReport.Text = "Latest developments and trends in <a href='http://www.canadean.com/Shop/Reports/CategoryID/6/CategoryID2/15/CategoryID3/426.aspx'>beer</a>, <a href='http://www.canadean.com/Shop/Reports/CategoryID/5/CategoryID2/15/CategoryID3/426.aspx'>soft drinks</a>, <a href='http://www.canadean.com/Shop/Reports/CategoryID/7/CategoryID2/15/CategoryID3/426.aspx'>dairy drinks</a>, <a href='http://www.canadean.com/Shop/Reports/CategoryID/8/CategoryID2/15/CategoryID3/426.aspx'>beverage packaging</a>"
                        'hlDataExtract.Text = "Annual consumption volumes by country and category"
                    End If
                End If

                If hlReport.Text = "" Then

                    Dim catId As Int32 = GetCurrentCategory()
                    Dim catName As String = GetCategoryName(catId)
                    If catId < 2 Then catName = "the Beverage Industry" ' For "General" and "All" 
                    'If catId < 2 Then catName = "Beverage Industry" ' For "General" and "All" 

                    Dim sCatID2 As Int32 = 15 ' Region "Global"
                    Dim sCatID3 As Int32 = 426 ' Country "Global"

                    Dim url As String = FixHyperLinkCats(categoryID1 & "", sCatID2 & "", sCatID3 & "")
                    'litReport.Text = "<div class='NormalBold'>Latest developments and trends in <a href='http://www.canadean.com/Shop/Reports/CategoryID/6/CategoryID2/15/CategoryID3/426.aspx'>beer</a>, <a href='http://www.canadean.com/Shop/Reports/CategoryID/5/CategoryID2/15/CategoryID3/426.aspx'>soft drinks</a>, <a href='http://www.canadean.com/Shop/Reports/CategoryID/7/CategoryID2/15/CategoryID3/426.aspx'>dairy drinks</a>, <a href='http://www.canadean.com/Shop/Reports/CategoryID/8/CategoryID2/15/CategoryID3/426.aspx'>beverage packaging</a></div>"
                    hlReport.Text = "Latest developments and trends in " & catName
                    'hlReport.Text = "Latest developments and trends in the " & catName & " market"
                    hlReport.NavigateUrl = url
                    hlBuyNow.NavigateUrl = url ' FixHyperLink(product.ProductID & "")
                    hlDataExtract.Text = "Annual consumption volumes by country and category"
                End If

                'For Each product In productController.GetRelatedProducts(-1, categoryID1, -1, -1, -1, False)
                'For Each product In productController.GetRelatedNewsProducts(-1, categoryID1, -1, categoryID3, -1, False)
                '    ProductList.Add(product)
                'Next


                'gvResults.Visible = True
                'gvResults.DataSource = productList
                'gvResults.DataBind()

            Catch ex As Exception
                Response.Write("<!-- error retrieving associated prods. ex: " & ex.ToString & " -->")

            End Try

        End Sub

        Protected Function ConvertShopCat_Report2DE(ByVal inCatId As Int32) As Int32
            Dim outCatId As Int32 = inCatId
            ' Convert product id's to use on the data extracts
            Select Case inCatId
                Case 90  ' Convert USA (90) into United States of America (170)
                    outCatId = 170
                Case 89  ' Convert United Kingdom (89) into United Kingdom (National Research) - 451
                    outCatId = 451
                Case 59  ' Convert Ireland (59) into Republic of Ireland (169)
                    outCatId = 169
                Case 163  ' China
                    outCatId = 438
                Case 164  ' East Europe
                    outCatId = 439
                Case 165  ' Middle East / North Africa
                    outCatId = 440
                Case 166  ' Rest of Africa
                    outCatId = 441
                Case 167  ' South America
                    outCatId = 442
                Case 168  ' West Europe
                    outCatId = 443
                Case 99  ' Caribbean
                    outCatId = 444
                    'Case 44  ' China
                    '    outCatId = 445
            End Select
            Return outCatId

        End Function

        Protected Function ConvertShopCat_DE2Report(ByVal inCatId As Int32) As Int32
            Dim outCatId As Int32 = inCatId
            ' Convert product id's to use on the reports catalog
            Select Case inCatId
                Case 170  ' Convert into USA (90), United States of America (170)
                    outCatId = 90
                Case 451  ' Convert into United Kingdom (89), United Kingdom (National Research) - 451
                    outCatId = 89
                Case 169  ' Convert into Ireland (59), Republic of Ireland (169)
                    outCatId = 59
                Case 438  ' China
                    outCatId = 163
                Case 439 ' East Europe
                    outCatId = 164
                Case 440 ' Middle East / North Africa
                    outCatId = 165
                Case 441  ' Rest of Africa
                    outCatId = 166
                Case 442  ' South America
                    outCatId = 167
                Case 443  ' West Europe
                    outCatId = 168
                Case 444  ' Caribbean
                    outCatId = 99
                    'Case 44  ' China
                    '    outCatId = 445            
            End Select
            Return outCatId
        End Function

        Protected Function GetAssociatedDataExtract(ByVal title As String, ByVal relatedDEs As ArrayList, ByVal relatedDEIds As ArrayList) As ArrayList
            Dim associatedDEs As ArrayList = New ArrayList()

            If title.ToLower().Contains(" - soft drinks - ") Then
                associatedDEs = CheckChoice("Carbonates", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Packaged Water", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Still Drinks", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Iced/RTD Tea Drinks", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Juice", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Nectars", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Sports Drinks", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Iced/RTD Coffee Drinks", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Bulk/HOD Water", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Energy Drinks", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Squash/Syrups", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Fruit Powders", relatedDEs, relatedDEIds, associatedDEs)
            End If

            If title.ToLower().Contains(" - beer - ") Then
                associatedDEs = CheckChoice("Beer", relatedDEs, relatedDEIds, associatedDEs)
            End If

            If title.ToLower().Contains(" - beverage packaging - ") Then
                associatedDEs = CheckChoice("Beer", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Carbonates", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Packaged Water", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Still Drinks", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Iced/RTD Tea Drinks", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Juice", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Nectars", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Sports Drinks", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Iced/RTD Coffee Drinks", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Bulk/HOD Water", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Energy Drinks", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Squash/Syrups", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Fruit Powders", relatedDEs, relatedDEIds, associatedDEs)
            End If

            If title.ToLower().Contains(" - dairy drinks - ") Then
                associatedDEs = CheckChoice("Dairy Drinks", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Milk/Milk Drinks", relatedDEs, relatedDEIds, associatedDEs)
            End If

            If title.ToLower().Contains(" - general - ") Then
                associatedDEs = CheckChoice("Beer", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Carbonates", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Packaged Water", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Still Drinks", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Iced/RTD Tea Drinks", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Juice", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Nectars", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Sports Drinks", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Iced/RTD Coffee Drinks", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Bulk/HOD Water", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Energy Drinks", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Squash/Syrups", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Fruit Powders", relatedDEs, relatedDEIds, associatedDEs)
            End If

            If title.ToLower().Contains(" - bottled water - ") Then
                associatedDEs = CheckChoice("Packaged Water", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Bulk/HOD Water", relatedDEs, relatedDEIds, associatedDEs)
            End If

            If title.ToLower().Contains(" - carbonated soft drinks - ") Then
                associatedDEs = CheckChoice("Carbonates", relatedDEs, relatedDEIds, associatedDEs)
            End If

            If title.ToLower().Contains(" - juices and nectars - ") Then
                associatedDEs = CheckChoice("Juice", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Nectars", relatedDEs, relatedDEIds, associatedDEs)
            End If

            If title.ToLower().Contains(" - still drinks - ") Then
                associatedDEs = CheckChoice("Still Drinks", relatedDEs, relatedDEIds, associatedDEs)
            End If

            If title.ToLower().Contains(" - sports drinks - ") Then
                associatedDEs = CheckChoice("Sports Drinks", relatedDEs, relatedDEIds, associatedDEs)
            End If

            If title.ToLower().Contains(" - energy drinks - ") Then
                associatedDEs = CheckChoice("Energy Drinks", relatedDEs, relatedDEIds, associatedDEs)
            End If

            If title.ToLower().Contains(" - iced and rtd tea - ") Then
                associatedDEs = CheckChoice("Iced/RTD Tea Drinks", relatedDEs, relatedDEIds, associatedDEs)
            End If

            If title.ToLower().Contains(" - iced and rtd coffee - ") Then
                associatedDEs = CheckChoice("Iced/RTD Coffee Drinks", relatedDEs, relatedDEIds, associatedDEs)
            End If

            If title.ToLower().Contains(" - dilutables - ") Then
                associatedDEs = CheckChoice("Squash/Syrups", relatedDEs, relatedDEIds, associatedDEs)
                associatedDEs = CheckChoice("Fruit Powders", relatedDEs, relatedDEIds, associatedDEs)
            End If

            If title.ToLower().Contains(" - cider - ") Or title.ToLower().Contains(" - perry - ") Then
                associatedDEs = CheckChoice("Cider", relatedDEs, relatedDEIds, associatedDEs)
            End If

            Return associatedDEs
        End Function

        Protected Function CheckChoice(ByVal DEStr As String, ByVal relatedDEs As ArrayList, ByVal relatedDEIds As ArrayList, ByVal associatedDEs As ArrayList) As ArrayList
            Dim pos As Integer = relatedDEs.IndexOf(DEStr)
            If pos <> -1 And associatedDEs.Count = 0 Then associatedDEs.Add(relatedDEIds.Item(pos))
            Return associatedDEs
        End Function

        Protected Function FixHyperLink(ByVal productId As String) As String
            Dim replaceParams As StringDictionary = New StringDictionary()
            'replaceParams("CategoryID") = Null.NullString
            replaceParams("ProductID") = productId
            'replaceParams("PageIndex") = Null.NullString
            Dim catalogNav As CatalogNavigation = New CatalogNavigation(Request.QueryString)

            Dim StorePageID As Integer = 109
            FixHyperLink = catalogNav.GetNavigationUrl(replaceParams, StorePageID)
        End Function

        Protected Function FixHyperLinkProduct(ByVal productId As String, ByVal cat1 As String, ByVal cat2 As String, ByVal cat3 As String, ByVal title As String) As String
            Dim replaceParams As StringDictionary = New StringDictionary()
            replaceParams("CategoryID") = cat1
            replaceParams("CategoryID2") = cat2
            replaceParams("CategoryID3") = cat3
            replaceParams("Title") = GetNewsFriendlyTitle(title)
            replaceParams("ProductID") = productId
            replaceParams("PageIndex") = Null.NullString
            Dim StorePageID As Integer = 109
            Dim catalogNav As CatalogNavigation = New CatalogNavigation(Request.QueryString)
            FixHyperLinkProduct = catalogNav.GetNavigationUrl(replaceParams, StorePageID)
        End Function

        Protected Function FixHyperLinkCats(ByVal cat1 As String, ByVal cat2 As String, ByVal cat3 As String) As String
            Dim replaceParams As StringDictionary = New StringDictionary()
            replaceParams("CategoryID") = cat1
            replaceParams("CategoryID2") = cat2
            replaceParams("CategoryID3") = cat3
            replaceParams("Title") = Null.NullString
            replaceParams("PageIndex") = Null.NullString
            Dim StorePageID As Integer = 109
            Dim catalogNav As CatalogNavigation = New CatalogNavigation(Request.QueryString)
            FixHyperLinkCats = catalogNav.GetNavigationUrl(replaceParams, StorePageID)
        End Function

        Private Function GetNewsId() As Integer
            Dim newsId As Integer = -1
            Dim newsIdStr As String = "-1"

            Try
                If CType(Request.QueryString("newsId"), String) <> "" Then
                    newsId = CInt(Request.QueryString("newsId"))
                    newsIdStr = Request.QueryString("newsId")
                Else
                    newsIdStr = GetNewsIdInUrl("newsId")
                    If newsIdStr = "-1" Then
                        newsIdStr = GetNewsIdInUrl("ItemId")
                    End If
                End If
                'Return newsId
                newsId = Int32.Parse(newsIdStr)
            Catch ex As Exception
                Response.Write("<!--<br>url: " & Request.RawUrl & "-->")
                Response.Write("<!--<br>newsId: " & newsIdStr & "-->")
            End Try
            Return newsId
        End Function

        Private Function GetNewsIdInUrl(ByVal keyword As String) As String
            Dim url As String = Request.RawUrl.ToLower()
            Dim pos As Integer = url.IndexOf(keyword.ToLower())
            If pos > 0 Then
                Dim pos1 As Integer = pos + keyword.Length + 1
                Dim pos2 As Integer = url.Substring(pos1).IndexOf("/")
                If pos2 < 1 Then    ' doesn't have a / after the news Id
                    pos2 = url.Substring(pos1).IndexOf("&")
                    If pos2 < 1 Then    ' doesn't have a & after the news Id
                        pos2 = url.Substring(pos1).IndexOf(".")
                        If pos2 < 1 Then    ' doesn't have a . after the news Id
                            pos2 = url.Length - pos1
                        End If
                    End If
                End If
                Return url.Substring(pos1, pos2)
            Else
                Return "-1"
            End If
        End Function
        Protected Sub imgBtnSend_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnSend.Click
            Dim newsId As Integer = -1
            'If CType(Request.QueryString("newsId"), String) <> "" Then
            '    newsId = CInt(Request.QueryString("newsId"))
            'End If
            newsId = GetNewsId()

            Dim objAnnouncements As New AnnouncementsController
            Dim objAnnouncement As AnnouncementInfo = objAnnouncements.GetAnnouncement(newsId, ModuleId)

            Dim checkScriptToken As Int32 = 0
            checkScriptToken = objAnnouncement.Description.LastIndexOf(scriptToken, objAnnouncement.Description.Length)
            If (checkScriptToken > 0) Then
                objAnnouncement.Description = objAnnouncement.Description.Substring(0, checkScriptToken)
            End If

            checkScriptToken = 0

            checkScriptToken = objAnnouncement.Description.LastIndexOf(imgToken, objAnnouncement.Description.Length)
            If (checkScriptToken > 0) Then
                objAnnouncement.Description = objAnnouncement.Description.Substring(0, checkScriptToken)
            End If

            If System.Text.RegularExpressions.Regex.IsMatch(tbYourEmail.Text, "\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*") = False Then
                lblResult.Text = "<b>Invalid From email address specified</b>"
                Return
            End If
            If System.Text.RegularExpressions.Regex.IsMatch(tbTheirEmail.Text, "\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*") = False Then
                lblResult.Text = "<b>Invalid Destination email address specified</b>"
                Return
            End If


            Dim mailfrom As String = tbYourName.Text & " <" & tbYourEmail.Text & ">"
            'Dim mailto As String = tbTheirEmail.Text
            Dim mailto As String = tbTheirName.Text & " <" & tbTheirEmail.Text & ">"
            Dim subject As String = tbTheirName.Text & ", news from your friend " & tbYourName.Text
            Dim body As String = "Dear " & tbTheirName.Text & ",<br><br>"
            body &= "Your friend, " & tbYourName.Text & ", saw this article and found it interesting for you: <br><br>"
            body &= objAnnouncement.Title & "<br><br>"
            body &= objAnnouncement.Description & "<br><br><br>"
            body &= "you can find the article <a href='" & Request.Url.ToString() & "'>here</a>"

            'Mail.SendMail(PortalSettings.Email, mailto, "helder1978@gmail.com", subject, body, "", "HTML", "", "", "", "")
            Mail.SendMail(mailfrom, mailto, "helder1978@gmail.com", subject, body, "", "HTML", "", "", "", "")

            lblResult.Text = "<b>The article was sent successfully</b>"

        End Sub

#End Region

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            'Response.Write("testeteste")

            InitializeComponent()
        End Sub


#End Region


#Region "IActionable Implementation"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the modules custom Actions
        ''' </summary>
        ''' <history>
        ''' 	[cnurse]	08/15/2007  Converted to a WAP project for Demo
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public ReadOnly Property ModuleActions() As Actions.ModuleActionCollection Implements IActionable.ModuleActions
            Get
                Dim Actions As New Actions.ModuleActionCollection
                Actions.Add(GetNextActionID, Localization.GetString(ModuleActionType.AddContent, LocalResourceFile), ModuleActionType.AddContent, "", "", EditUrl(), False, Security.SecurityAccessLevel.Edit, True, False)
                Return Actions
            End Get
        End Property

#End Region

    End Class

End Namespace
