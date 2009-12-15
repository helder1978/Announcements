<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="Portal" TagName="Tracking" Src="~/controls/URLTrackingControl.ascx" %>
<%@ Register TagPrefix="Portal" TagName="Audit" Src="~/controls/ModuleAuditControl.ascx" %>
<%@ Register TagPrefix="Portal" TagName="URL" Src="~/controls/URLControl.ascx" %>
<%@ Control language="vb" CodeBehind="EditAnnouncements.ascx.vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.Modules.Announcements.EditAnnouncements" %>
<table cellSpacing="0" cellPadding="0" width="600" summary="Edit Announcements Design Table">
	<tr vAlign="top">
		<td class="SubHead" width="150">
			<dnn:label id="plTitle" runat="server" controlname="txtTitle" suffix=":"></dnn:label>
		</td>
		<td width="450">
			<asp:textbox id="txtTitle" runat="server" maxlength="100" Columns="30" width="400px" cssclass="NormalTextBox"></asp:textbox>
			<br>
			<asp:requiredfieldvalidator id="valTitle" resourcekey="Title.ErrorMessage" runat="server" CssClass="NormalRed"
				ControlToValidate="txtTitle" ErrorMessage="You Must Enter A Title For The Announcement" Display="Dynamic"></asp:requiredfieldvalidator>
		</td>
	</tr>
	<tr>
		<td class="SubHead">
			<dnn:label id="plImage" runat="server" controlname="txtImage" suffix=":"></dnn:label>
		</td>
		<td>
			<portal:url id="urlImage" runat="server" width="300" required="False" showtabs="False" showfiles="True"
				showUrls="True" showlog="False" shownone="true" shownewwindow="False" showtrack="False" />
		</td>
	</tr>
	<tr vAlign="top">
		<td class="SubHead" width="150"><dnn:label id="plDescription" runat="server" controlname="txtDescription" suffix=":"></dnn:label></td>
		<td width="450">
			<br>
		</td>
	</tr>
	<TR>
		<TD class="SubHead" width="450" colSpan="2">
			<dnn:texteditor id="teDescription" runat="server" width="550" height="300"></dnn:texteditor>
			<asp:requiredfieldvalidator id="valDescription" resourcekey="Description.ErrorMessage" runat="server" CssClass="NormalRed"
				ControlToValidate="teDescription" ErrorMessage="You Must Enter A Description Of The Announcement" Display="Dynamic"></asp:requiredfieldvalidator></TD>
	</TR>
	<tr>
		<td colspan="2" width="450">&nbsp;</td>
	</tr>
	<tr>
		<td class="SubHead" width="150"><dnn:label id="plURL" runat="server" controlname="ctlURL" suffix=":"></dnn:label></td>
		<td width="450">
			<portal:url id="ctlURL" runat="server" width="225" shownone="true" />
		</td>
	</tr>
	<tr>
		<td colspan="2" width="450">&nbsp;</td>
	</tr>
	<TR>
		<TD class="SubHead" width="150">
			<dnn:label id="plPublishDate" suffix=":" controlname="txtPublishDate" runat="server"></dnn:label></TD>
		<TD width="450">
			<asp:TextBox id="txtPublishDate" runat="server" CssClass="NormalTextBox" Width="72px"></asp:TextBox>&nbsp;
			<asp:hyperlink id="cmdCalendar" resourcekey="Calendar" CssClass="CommandButton" Runat="server">Calendar</asp:hyperlink>
			<asp:comparevalidator id="valPublishDate" resourcekey="PublishDate.ErrorMessage" runat="server" CssClass="NormalRed"
				ControlToValidate="txtPublishDate" ErrorMessage="<br>You have entered an invalid date!" Display="Dynamic"
				Type="Date" Operator="DataTypeCheck"></asp:comparevalidator>
			<asp:requiredfieldvalidator id="valPublishDateRequired" runat="server" Display="Dynamic" ErrorMessage="You must enter a date"
				ControlToValidate="txtPublishDate" CssClass="NormalRed" resourcekey="PublishDateRequired.ErrorMessage"></asp:requiredfieldvalidator></TD>
	</TR>
	<TR>
		<TD class="SubHead" width="150">
			<dnn:label id="plExpireDate" suffix=":" controlname="txtPublishDate" runat="server"></dnn:label></TD>
		<TD width="450">
			<asp:TextBox id="txtExpireDate" runat="server" CssClass="NormalTextBox" Width="72px"></asp:TextBox>&nbsp;
			<asp:hyperlink id="cmdCalendar2" CssClass="CommandButton" resourcekey="Calendar" Runat="server">Calendar</asp:hyperlink>
			<asp:comparevalidator id="valExpireDate" runat="server" Display="Dynamic" ErrorMessage="<br>You have entered an invalid date!"
				ControlToValidate="txtExpireDate" CssClass="NormalRed" resourcekey="PublishDate.ErrorMessage" Operator="DataTypeCheck"
				Type="Date"></asp:comparevalidator></TD>
	</TR>
	<tr>
		<td class="SubHead" width="150"><dnn:label id="plViewOrder" runat="server" controlname="txtViewOrder" suffix=":"></dnn:label></td>
		<td width="450">
			<asp:textbox id="txtViewOrder" runat="server" maxlength="3" Columns="20" width="72px" CssClass="NormalTextBox"></asp:textbox>
			<asp:comparevalidator id="valViewOrder" resourcekey="ViewOrder.ErrorMessage" runat="server" CssClass="NormalRed"
				ControlToValidate="txtViewOrder" ErrorMessage="<br>View order must be an integer value." Display="Dynamic"
				Type="Integer" Operator="DataTypeCheck"></asp:comparevalidator>
		</td>
	</tr>
</table>
<p>
	<asp:linkbutton id="cmdUpdate" resourcekey="cmdUpdate" runat="server" CssClass="CommandButton" Text="Update"
		BorderStyle="none"></asp:linkbutton>&nbsp;
	<asp:linkbutton id="cmdCancel" resourcekey="cmdCancel" runat="server" CssClass="CommandButton" Text="Cancel"
		BorderStyle="none" CausesValidation="False"></asp:linkbutton>&nbsp;
	<asp:linkbutton id="cmdDelete" resourcekey="cmdDelete" runat="server" CssClass="CommandButton" Text="Delete"
		BorderStyle="none" CausesValidation="False"></asp:linkbutton>
</p>
<portal:Audit id="ctlAudit" runat="server" />
<br>
<br>
<portal:Tracking id="ctlTracking" runat="server" />
