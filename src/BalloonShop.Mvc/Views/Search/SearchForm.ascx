﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<table border="0" cellpadding="0" cellspacing="0" width="200px">
    <tr>
        <td class="SearchBoxHead">
            Search the Catalog
        </td>
    </tr>
    <tr>
        <td class="SearchBoxContent">
            <% using (Html.BeginForm("Show","Search", FormMethod.Get)) { %>
                <input type="text" name="search" id="search" class="SearchBox" style="border: dotted 1px #C0C0C0;height: 16px; width: 128px;" maxlength="100" />
                <input type="submit" value="Go!" class="SearchBox" style="width: 36px; height: 21px;" />
                <%=Html.CheckBox("allWords") %>
                <label for="allWords">Search for all words</label>
            <% } %>
        </td>
    </tr>
</table>
