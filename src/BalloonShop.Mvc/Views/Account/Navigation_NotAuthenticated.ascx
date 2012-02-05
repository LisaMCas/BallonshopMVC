﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<table cellspacing="0" border="0" width="200px" class="UserInfoContent">
<thead>
  <tr>
    <th class="UserInfoHead">
      User Info</th>
  </tr>
</thead>
<tbody>
<tr>
        <td>
          <span class="UserInfoText">You are not logged in.</span>
        </td>
      </tr>
      <tr>
        <td>
          &nbsp;&raquo;
          <a href="<%= Url.Action("Login") %>" class="UserInfoLink">Login</a>
          &nbsp;&laquo;
        </td>
      </tr>
      <tr>
        <td>
          &nbsp;&raquo;
		  <a href="<%= Url.Action("Register") %>" class="UserInfoLink" title="Go to the registration page">Register</a>
          &nbsp;&laquo;
        </td>
      </tr>
</tbody>
</table>