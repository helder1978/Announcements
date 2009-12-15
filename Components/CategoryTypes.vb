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
Imports System.Xml.Serialization

Namespace DotNetNuke.Modules.Announcements

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The CategoryTypes Enumeration the types of announcements
    ''' </summary>
    ''' <history>
    ''' 	[cnurse]	08/15/2007  Converted to a WAP project for Demo
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Enum CategoryTypes
        'TODO: WHEN USING BEVERAGE_PACKAGING REPLACE _ WITH A BLANK SPACE
        General = 1
        Soft_Drinks = 2
        Beer = 3
        Beverage_Packaging = 4
        Dairy_Drinks = 5
    End Enum

End Namespace

