﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BalloonShop.Infrastructure.PagedList<BalloonShop.Model.Balloon>>"  %>
<% if (Model.NumberOfPages > 1){ %>
    Page
    <%= Model.Page%>
    of
    <%= Model.NumberOfPages%>
    &nbsp;&nbsp;
    <% } %>
    <% if (Model.HasPreviousPage)
       {%><a href="?page=<%=Model.Page - 1%>">Prev</a> &nbsp;&nbsp;<%}%>
    <% if (Model.HasNextPage){ %><a href="?page=<%= Model.Page + 1 %>">Next</a><% } %>
    <table>
        <tr>
            <%
                var index = 1;
                foreach (var product in Model)
                { %>
            <td>
                <table cellpadding="0" align="left">
                    <tr height="105">
                        <td align="center" width="110">
                            <a href='<%= Url.Action("Show", "Balloon", new { id = product.Id }) %>'>
                                <img width="100" src='/Content/ProductImages/<%= product.Thumb %>' border="0" />
                            </a>
                        </td>
                        <td valign="top" width="250">
                            <a class="ProductName" href='<%= Url.Action("Show", "Balloon", new { id = product.Id }) %>'>
                                <%= product.Name%>
                            </a>
                            <br />
                            <span class="ProductDescription">
                                <%= product.Description%>
                                <br />
                                <br />
                                Price: </span><span class="ProductPrice">
                                    <%= product.Price.ToString("c") %>
                                </span>
                        </td>
                    </tr>
                </table>
            </td>
            <% if (index % 2 == 0)
               { %>
        </tr>
        <tr>
            <% }
               index++; %>
            <%} %>
        </tr>
    </table>